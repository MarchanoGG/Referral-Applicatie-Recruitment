<script setup>
import { dependencies } from '../../package.json'
</script>

<template>
  <div class="home">{{ applicationVersion }}</div>
</template>

<script>
  export default {
    data() {
      return {
        applicationVersion: dependencies
    }
    },
  };
</script>