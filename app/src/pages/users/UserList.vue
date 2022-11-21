<template>
  <div class="q-pa-md">
    <q-toolbar class="q-my-md shadow-2">
      <EssentialLink v-for="link in essentialLinks" :key="link.title" v-bind="link" />
      <q-separator vertical inset />
    </q-toolbar>
    <div v-if="rows">
      <q-table title="Users" dense :rows="rows" :columns="columns" row-key="id" :loading="loading">
      </q-table>
    </div>
  </div>
</template>

<script>
  import EssentialLink from 'components/EssentialLink.vue'
  import { ref } from 'vue'
  import { api } from 'boot/axios'
const linksList = [
  {
    title: '',
    // caption: '',
    icon: 'add',
    link: '/users/add'
  },
]

const columns = [
  {
    name: 'username',
    required: true,
    label: 'Name',
    align: 'left',
    field: 'username',
    format: val => `${val}`,
    sortable: true
  },
  {
    name: 'recruiter',
    label: 'Recruiter',
    field: 'recruiter',
    align: 'left',
    sortable: true
  },
  {
    name: 'creation_dt',
    label: 'Last modified',
    field: 'creation_dt',
    align: 'left',
    sortable: true
  }
]

  /*
  var rows = []
var rows = [
 {
    "fullname": "Tobias"
    , "email": "Tobias@gmail.com"
    , "phone": "1234567"
    , "birthday": "01-01-1996"
  }
  , {
    "fullname": "Marchano"
    , "email": "Marchano@gmail.com"
    , "phone": "1234567"
    , "birthday": "01-01-1996"
  }
  , {
    "fullname": "Jeremey"
    , "email": "Jeremey@gmail.com"
    , "phone": "1234567"
    , "birthday": "01-01-1996"
  }
  , {
    "fullname": "Michael"
    , "email": "Michael@gmail.com"
    , "phone": "1234567"
    , "birthday": "01-01-1996"
  }
  , {
    "fullname": "Mervin"
    , "email": "Mervin@gmail.com"
    , "phone": "1234567"
    , "birthday": "01-01-1996"
  }
  , {
    "fullname": "Jon Snow"
    , "email": "jonsnow@gmail.com"
    , "phone": "1234567"
    , "birthday": "01-01-1996"
  }
]
*/

import { defineComponent } from 'vue'

  export default defineComponent({
  name: 'UserList'
  , components: {
    EssentialLink
  }
    , setup() {
    return {
      essentialLinks: linksList,
      columns,
      
    }
    },
    data() {
      return {
        rows: []
      }
    },
    mounted() {
      api.get('/Users')
        .then((response) => {
          this.rows = response.data
          console.log(response)
        })
        .catch(() => {
        })
    }
})
</script>
