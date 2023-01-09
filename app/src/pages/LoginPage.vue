<template>
  <q-page class="">
    <div class="row flex justify-center">
      <div class="col-6 q-mt-xl logo-section">
        <div class="col-4 text-center">
          <div>
            <img alt="Alten logo"
              src="https://www.alten.com/wp-content/uploads/2019/01/cropped-favicon-alten-1-180x180.png"
              style="width: 200px; height: 200px">
            <h3 class="text-h3">Alten</h3>
          </div>
        </div>
      </div>
      <div class="col-6 q-mt-xl">
        <div class="row flex justify-center">
          <div class="col-6">
            <q-form @submit="login" @reset="resetForm" class="">
              <h1 class="text-h3 text-center">Login</h1>
              <q-input filled label="Username *" v-model="userForm.username" hint="Username" lazy-rules
                :rules="[val => val && val.length > 0 || 'Please type something']" />

              <q-input filled label="Password *" v-model="userForm.password" :type="isPwd ? 'password' : 'text'"
                hint="Password" lazy-rules :rules="[val => val && val.length > 0 || 'Please type something']">
                <template v-slot:append>
                  <q-icon :name="isPwd ? 'visibility_off' : 'visibility'" class="cursor-pointer"
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
      </div>
    </div>
  </q-page>
</template>
<script>
import { useRouter } from 'vue-router'
import { defineComponent, ref, reactive, computed } from 'vue'
import { useUserStore } from "stores/user";

const router = useRouter()

export default defineComponent({
  name: 'LoginPage'
  , methods: {
    async login() {
      await this.userStore.signIn(this.userForm.username, this.userForm.password);
      // router.push("/");
    },
    resetForm() {
      this.userForm = this.defaultForm
    },
  }
  , data() {
    const defaultForm = {
      username: null,
      password: null,
    }
    return {
      isPwd: ref(true),
      defaultForm: defaultForm,
      userForm: defaultForm
    }
  }
  , setup() {
    const userStore = useUserStore();
    return { userStore };
  }
})
</script>
