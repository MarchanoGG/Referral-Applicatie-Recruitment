<template>
    <div class="q-pa-md" style="max-width: 400px">
        <q-form @submit="onSubmit" @reset="onReset" class="q-gutter-md">
            <q-input filled v-model="username" label="Your username *" hint="Username" lazy-rules
                :rules="[ val => val && val.length > 0 || 'Please type something']" />

            <q-input filled v-model="password" label="Your password *" hint="Password" lazy-rules
                :rules="[ val => val && val.length > 0 || 'Please type something']" />

            <div>
                <q-btn label="Submit" type="submit" color="primary"/>
                <q-btn label="Reset" type="reset" color="primary" flat class="q-ml-sm" />
            </div>
        </q-form>
    </div>
</template>
  
<script>
  import { useQuasar } from 'quasar'
  import { useRouter } from 'vue-router'
  import { ref, reactive } from 'vue'
  import { api } from 'boot/axios'

export default {
    setup() {
    const router = useRouter()

        const username = ref(null)
        const password = ref(null)

        return {
            username,
            password,
            onSubmit() {
              if (username.value !== null && password.value !== null) {
                const userForm = reactive({
                  username: username.value,
                  password: password.value,
                });

                api.post('/Users', userForm)
                  .then((response) => {
                    if (response.status == 200) {
                      console.log(1);
                      router.push("/Users");
                    }
                  })
                  .catch(() => {
                  })
                }
              else {
                }
            },

            onReset() {
                username.value = null
                password.value = null
            }
        }
    }
}
</script>
