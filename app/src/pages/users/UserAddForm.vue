<template>
    <q-form @submit="onSubmit" @reset="onReset">
        <div class="row">
            <div class="col-5">
                <q-input filled v-model="name" label="Your name *" hint="Name and surname" lazy-rules
                    :rules="[val => val && val.length > 0 || 'Please type something']" />

                <q-input filled v-model="surname" label="Your surname *" hint="Surname" lazy-rules
                    :rules="[val => val && val.length > 0 || 'Please type something']" />

                <q-input filled v-model="email" label="Your email *" hint="Email" lazy-rules
                    :rules="[val => val && val.length > 0 || 'Please type something']" />

                <q-input filled v-model="phonenumber" label="Your Phone number *" hint="Phone number" lazy-rules
                    :rules="[val => val && val.length > 0 || 'Please type something']" />

                <q-input filled v-model="password" label="Your password *" hint="Password" lazy-rules
                    :rules="[val => val && val.length > 0 || 'Please type something']" />
                <q-toggle v-model="accept" label="I accept the license and terms" />
            </div>

            <div class="col-1" />

            <div class="col-5">
                <q-date filled v-model="age" title="Birthdate" subtitle lazy-rules />
            </div>

            <div>
                <q-btn label="Submit" type="submit" color="primary" />
                <q-btn label="Reset" type="reset" color="primary" flat class="q-ml-sm" />
            </div>
        </div>
    </q-form>
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
