<template>
  <div class="q-pa-md">
    <q-toolbar class="q-my-md shadow-2">
      <EssentialLink v-for="link in essentialLinks" :key="link.title" v-bind="link" />
      <q-separator vertical inset />
    </q-toolbar>
    <q-table title="Tasks" dense :rows="rows" :columns="columns" row-key="id" :loading="loading"
      :pagination="pagination">
    </q-table>
  </div>
</template>

<script>
import EssentialLink from 'components/EssentialLink.vue'
const linksList = [
  {
    title: '',
    // caption: '',
    icon: 'add',
    link: '/tasks/add'
  },
]

const columns = [
  {
    name: 'name',
    required: true,
    label: 'Name',
    align: 'left',
    field: row => row.name,
    format: val => `${val}`,
    sortable: true
  },
  {
    name: 'description',
    label: 'Description',
    field: 'description',
    align: 'left',
    sortable: true
  },
  {
    name: 'points',
    label: 'Points',
    field: 'points',
    align: 'left',
    sortable: true
  },
]

const rows = [
  {
    "name": "Sollicitatie gesprek"
    , "description": ""

    , "points": 10
  }
  , {
    "name": "Telefonisch gesprek"
    , "description": ""

    , "points": 10
  }
  , {
    "name": "CV controlleren"
    , "description": ""

    , "points": 10
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
      rows,
      pagination: { rowsPerPage: 10 },
    }
  }
})
</script>
