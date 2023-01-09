<template>
  <router-view />
</template>

<script>
import { defineComponent } from 'vue'
import { useUserStore } from "stores/user";

export default defineComponent({
  name: 'App',
  setup() {

  },
})
</script>
