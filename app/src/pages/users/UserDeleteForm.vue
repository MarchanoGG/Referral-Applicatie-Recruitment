<template>
  <!-- <q-form @submit="onSubmit" @reset="onReset"> -->
  <!-- 
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
   -->
  <div class="row">
    <div class="col-5">
      {{ objectkey }}
      <q-btn @click="onSubmit" label="Delete" type="submit" color="primary" />
      <q-btn label="Cancel" type="reset" color="primary" flat class="q-ml-sm" />
    </div>
  </div>
  <!-- </q-form> -->
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
    return {
    }
  },
  methods: {
    onSubmit() {
      const userForm = {
        data: { object_key: this.objectkey, }
      }

      api.delete('/Users', userForm)
        .then((response) => {
          console.log(response);
          if (response.status == 200) {
            router.push("/Users");
          }
        })
        .catch(() => {
        })
    },
    onReset() {
      router.push("/Users");
    }
  }
}
</script>
