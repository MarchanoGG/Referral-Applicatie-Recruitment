using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.IO;
using System.Net;

namespace RAR
{
    public partial class Form1 : Form
    {

        private static Form1 d_instance = null;
        HttpClient d_client = new HttpClient();
        public String url = "";

        public static Form1 Instance()
        {
            if (d_instance == null)
            {
                d_instance = new Form1();
            }
            return d_instance;
        }

        public static void DestroyInstance()
        {
            d_instance = null;
        }

        public Form1()
        {
            TConnection con = new TConnection();
            d_instance = this;
            InitializeComponent();
        }
        // callbacks

        private void SetupServer(object sender, EventArgs e)
        {
            var port = textBox1.Text;
            var protocol = textBox3.Text;
            var server = textBox2.Text;
            url = protocol + "://" + server + ":" + port + "/";

            TServer.ThStartServer(url);

            richTextBox1.Text = "Server Started...";
            // Console.WriteLine(url);
        }
        // button functions

        public void Print(String msg)
        {
            richTextBox1.Text = msg;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            TServer.ThStopServer();
        }


        public async Task GetAsync()
        {
            d_client.DefaultRequestHeaders.Add("Forwarded", "true");
            try
            {
                HttpResponseMessage response = await d_client.GetAsync(textBox4.Text);
                Console.WriteLine((int)response.StatusCode);
                switch(response.StatusCode)
                {
                    case HttpStatusCode.OK:
                        break;
                }
                richTextBox2.Text = Convert.ToString(response.StatusCode);
                String resp = await response.Content.ReadAsStringAsync();
                richTextBox2.Text += resp;

            }
            catch
            {

            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            GetAsync();
        }

        public async Task PostAsync()
        {
            d_client.DefaultRequestHeaders.Add("Forwarded", "true");
            try
            {
                //String response = await d_client.PostAsync(url + "/POST");
                //richTextBox2.Text = response;
            }
            catch
            {
                richTextBox2.Text = "Tried to POST, but failed.";
            }
        }

        public async Task PutAsync()
        {
            d_client.DefaultRequestHeaders.Add("Forwarded", "true");
            try
            {
                //String response = await d_client.PutAsync(url + "/PUT");
                //richTextBox2.Text = response;
            }
            catch
            {
                richTextBox2.Text = "Tried to PUT, but failed.";
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            PutAsync();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            PostAsync();
        }
    }
}
