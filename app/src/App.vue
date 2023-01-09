<template>
  <router-view />
</template>

<script>
import { defineComponent } from 'vue'
import { api } from 'boot/axios'

export default defineComponent({
  name: 'App',
  async created() {
    const response = await api.get('/Users', { token: localStorage.getItem('token') })
    console.log(response)
    this.user = response.data[0]
  },
})
</script>
