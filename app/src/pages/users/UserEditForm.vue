<template>
  <q-form @submit="onSubmit" @reset="onReset">
    <div class="row">
      <div class="col-5">
        <q-input filled v-model="username" label="Your username *" hint="Userame" lazy-rules
          :rules="[val => val && val.length > 0 || 'Please type something']" />

        <q-input filled label="Password *" v-model="password" :type="isPwd ? 'password' : 'text'" hint="Password"
          lazy-rules :rules="[val => val && val.length > 0 || 'Please type something']">
          <template v-slot:append>
            <q-icon :name="isPwd ? 'visibility_off' : 'visibility'" class="cursor-pointer" @click="isPwd = !isPwd" />
          </template>
        </q-input>
        <q-toggle v-model="recruiter" label="Is a recruiter" />
      </div>
    </div>

    <div class="col-5">
      <q-btn label="Submit" type="submit" color="primary" />
      <q-btn label="Reset" type="reset" color="primary" flat class="q-ml-sm" />
      <p>{{ objectkey }}</p>
    </div>
  </q-form>
</template>
  
<script>
import { useQuasar } from 'quasar'
import { useRouter } from 'vue-router'
import { ref, reactive } from 'vue'
import { api } from 'boot/axios'
import { LocalStorage } from 'quasar'

export default {
  props: ['objectkey'],
  setup() {
    const router = useRouter()

    const username = ref(null)
    const password = ref(null)
    const recruiter = ref(true)
    var recruiterVal = ref(0)
    return {
      isPwd: ref(true),
      recruiter,
      username,
      password,
    }
  },
  methods() {
    return {
      onSubmit() {
        if (username.value !== null && password.value !== null) {

          if (recruiter.value == true) {
            recruiterVal = ref(1);
          }

          const userForm = reactive({
            username: username.value,
            password: password.value,
            recruiter: recruiterVal.value,
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
