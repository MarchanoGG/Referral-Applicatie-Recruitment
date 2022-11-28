<script setup>
import { dependencies } from '../../package.json'
import axios from 'axios';
</script>

<template>

  <ul v-if="posts && posts.length">
    <li v-for="post of posts">
      <p>{{post.email}}</p>
      <p>{{post.password}}</p>
    </li>
  </ul>

  <ul v-if="errors && errors.length">
    <li v-for="error of errors">
      {{error.message}}
    </li>
  </ul>

</template>

<script>
  export default { 
    data () {
      return {
        posts: [],
        errors: []
      }
    },
    created () {
      axios
        .get('http://127.0.0.1:5173/tests/api/users')
        .then(response => (this.posts = response.data))
        .catch(error => (this.errors = error))
    }
  };
</script>