<template>
  <q-page class="">
    <div class="row flex justify-center">
      <div class="col-3">
        <q-form @submit="onSubmit" @reset="onReset" class="">
          <h1 class="text-h3 text-center">Login</h1>
          <q-input filled label="Username or Email *" hint="Username or Email" lazy-rules
            :rules="[val => val && val.length > 0 || 'Please type something']" />

          <q-input filled label="Password *" hint="Password" lazy-rules
            :rules="[val => val && val.length > 0 || 'Please type something']" />

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
import { defineComponent } from 'vue'

export default defineComponent({
  name: 'LoginPage'
  , setup() {
    return {
    }
  }
})
</script>
