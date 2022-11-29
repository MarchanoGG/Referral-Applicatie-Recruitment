<template>
  <div class="q-pa-md">
    <q-toolbar class="q-my-md shadow-2">
      <EssentialLink v-for="link in essentialLinks" :key="link.title" v-bind="link" />
      <q-separator vertical inset />
    </q-toolbar>
    <q-table title="Users" dense :rows="rows" :columns="columns" row-key="id" :loading="loading"
      :pagination="pagination">
    </q-table>
  </div>
</template>

<script>
  import EssentialLink from 'components/EssentialLink.vue'
  import { api } from 'boot/axios'
const linksList = [
  {
    title: '',
    // caption: '',
    icon: 'person_add',
    link: '/users/add'
  },
]

const columns = [
  {
    name: 'name',
    required: true,
    label: 'Name',
    align: 'left',
    field: 'username',
    format: val => `${val}`,
    sortable: true
  },
  {
    name: 'recruiter',
    label: 'Is a recruiter',
    field: 'recruiter',
    align: 'left',
    sortable: true
  },
  {
    name: 'modificationDate',
    label: 'Last modified',
    field: 'modification_dt',
    align: 'left',
    sortable: true
  }
]

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
      pagination: { rowsPerPage: 10 },
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
      })
      .catch(() => {
      })
  }
})
</script>
