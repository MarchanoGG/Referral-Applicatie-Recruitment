<template>
  <q-page class="">
    <div class="row flex justify-center">
      <div class="col-3">
        <q-form @submit="onSubmit" @reset="onReset" class="">
          <h1 class="text-h3 text-center">Login</h1>
          <q-input filled label="Username *" v-model="username" hint="Username" lazy-rules
                   :rules="[val => val && val.length > 0 || 'Please type something']" />

          <q-input filled label="Password *" v-model="password" :type="isPwd ? 'password' : 'text'" hint="Password" lazy-rules
                   :rules="[val => val && val.length > 0 || 'Please type something']">
            <template v-slot:append>
              <q-icon :name="isPwd ? 'visibility_off' : 'visibility'"
                      class="cursor-pointer"
                      @click="isPwd = !isPwd" />
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

  export default defineComponent({
    name: 'LoginPage'
    , setup() {
      const router = useRouter()

      const username = ref(null)
      const password = ref(null)

      return {
        isPwd: ref(true),
        username,
        password,
        onSubmit() {
          if (username.value !== null && password.value !== null) {
            const userForm = reactive({
              username: username.value,
              password: password.value,
            });

            api.post('/Authentication', userForm)
              .then((response) => {
                if (response.status == 200) {
                  router.push("/");
                }
              })
              .catch(() => {
              })
          }
        },
      }
    }
  })
</script>
