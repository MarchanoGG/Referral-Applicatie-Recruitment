<script setup>
import { dependencies } from '../../package.json'
import axios from 'axios';
</script>

<template>

  <ul v-if="posts && posts.length">
    <li v-for="post of posts">
      <p><strong>{{post.title}}</strong></p>
      <p>{{post.body}}</p>
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
        .get('https://jsonplaceholder.typicode.com/posts')
        .then(response => (this.posts = response.data))
        .catch(error => (this.errors = error))
    }
  };
</script>