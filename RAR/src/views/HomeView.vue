<script setup>
import TheWelcome from '../components/TheWelcome.vue'
</script>

<template>
  <div class="home">{{ msg }}</div>
</template>

<script>
  export default {
    name: 'home',
    components: {},
    data() {
      return {
        msg: 'Welcome to Your Vue.js App',
      }
    },
  };
</script>