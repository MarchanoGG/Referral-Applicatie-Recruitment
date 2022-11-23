using System.Net;
using System.Text;

namespace RarApiConsole.formData
{
    internal class FormData
    {
        static public Dictionary<string, string> GetFormData(HttpListenerRequest aRequest)
        {
            Dictionary<string, string> retVal = new();

            var stream = aRequest.InputStream;
            var type = aRequest.ContentType;

            if (type != null)
            { 
                string boundary = "";
                if (type.Contains('=') == true)
                {
                    boundary = type.Substring(type.IndexOf('=') + 1);
                }

                var encoding = aRequest.ContentEncoding.BodyName;
                Encoding decoder = Encoding.GetEncoding(encoding);

                if ((stream != null) && (decoder != null))
                {
                    MemoryStream ms = new MemoryStream();
                    stream.CopyTo(ms);

                    var aVal = decoder.GetString(ms.ToArray());
                    aVal = aVal.Replace(boundary, "");
                    aVal = aVal.Replace("\r\n", "");

                    while (aVal.Length > 0)
                    {
                        if (aVal.IndexOf("=") >= 0)
                        {
                            aVal = aVal.Substring(aVal.IndexOf("=") + 1);
                            if (aVal.IndexOf("--Content-Disposition: form-data; name") >= 0)
                            {
                                var sub = aVal.Substring(0, aVal.IndexOf("--Content-Disposition: form-data; name"));
                                string aKey = sub.Substring(aVal.IndexOf("\"") + 1);
                                string aValue = aKey.Substring(aKey.IndexOf("\"") + 1);
                                aKey = aKey.Substring(0, aKey.IndexOf("\""));

                                retVal[aKey] = aValue;
                            }
                            else if (aVal.IndexOf("----") >= 0)
                            {
                                aVal = aVal.Substring(aVal.IndexOf("=") + 1);
                                var sub = aVal.Substring(0, aVal.IndexOf("----"));
                                string aKey = sub.Substring(aVal.IndexOf("\"") + 1);
                                string aValue = aKey.Substring(aKey.IndexOf("\"") + 1);
                                aKey = aKey.Substring(0, aKey.IndexOf("\""));

                                retVal[aKey] = aValue;
                            }
                        }
                        else
                        {
                            aVal = "";
                        }
                    }
                }
            }
            return retVal;
        }
    }
}
