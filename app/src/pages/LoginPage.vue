<template>
  <q-page class="">
    <div class="row flex justify-center">
      <div class="col-3">
        <q-form @submit="onSubmit" @reset="onReset" class="">
          <h1 class="text-h3 text-center">Login</h1>
          <q-input filled label="Username *" v-model="userForm.username" hint="Username" lazy-rules
            :rules="[val => val && val.length > 0 || 'Please type something']" />

          <q-input filled label="Password *" v-model="userForm.password" :type="isPwd ? 'password' : 'text'"
            hint="Password" lazy-rules :rules="[val => val && val.length > 0 || 'Please type something']">
            <template v-slot:append>
              <q-icon :name="isPwd ? 'visibility_off' : 'visibility'" class="cursor-pointer" @click="isPwd = !isPwd" />
            </template>
          </q-input>

          <div>
            <q-btn label="Submit" type="submit" color="primary" />
            <q-btn label="Reset" type="reset" color="primary" flat class="q-ml-sm" />
          </div>
        </q-form>
      </div>
    </div>
  </q-page>
</template>

<script>
import { useRouter } from 'vue-router'
import { defineComponent } from 'vue'
import { ref, reactive } from 'vue'
import { api } from 'boot/axios'

const router = useRouter()

export default defineComponent({
  name: 'LoginPage'
  , methods: {
    onSubmit() {
      console.log(this.userForm)
      if (this.userForm.username.value !== null && this.userForm.password.value !== null) {
        api.post('/Authentication', this.userForm)
          .then((response) => {
            console.log(response)
            if (response.status == 200) {
              router.push("/");
              localStorage.setItem("user", response);
            }
          })
          .catch(() => {
          })
      }
    },
  }
  , data() {
    return {
      isPwd: ref(true),
      userForm: {
        username: null,
        password: null,
      }
    }
  }
})
</script>
