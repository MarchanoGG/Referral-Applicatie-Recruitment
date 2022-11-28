<template>
  <div class="q-pa-md">
    <q-toolbar class="q-my-md shadow-2">
      <EssentialLink v-for="link in essentialLinks" :key="link.title" v-bind="link" />
      <q-separator vertical inset />
    </q-toolbar>
    <q-table title="Campagnes" dense :rows="rows" :columns="columns" row-key="id" :loading="loading"
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
    field: row => row.fullname,
    format: val => `${val}`,
    sortable: true
  },
  {
    name: 'email',
    label: 'Email',
    field: 'email',
    align: 'left',
    sortable: true
  },
  {
    name: 'phone',
    label: 'Phone',
    field: 'phone',
    align: 'left',
    sortable: true
  },
  {
    name: 'birthday',
    label: 'Birthday',
    field: 'birthday',
    align: 'left',
    sortable: true
  },
  // { name: 'carbs', label: 'Carbs (g)', field: 'carbs' },
  // { name: 'protein', label: 'Protein (g)', field: 'protein' },
  // { name: 'sodium', label: 'Sodium (mg)', field: 'sodium' },
  // { name: 'calcium', label: 'Calcium (%)', field: 'calcium', sortable: true, sort: (a, b) => parseInt(a, 10) - parseInt(b, 10) },
  // { name: 'iron', label: 'Iron (%)', field: 'iron', sortable: true, sort: (a, b) => parseInt(a, 10) - parseInt(b, 10) }
]

const rows = [
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
  ,
]


import { defineComponent } from 'vue'

export default defineComponent({
  name: 'CampagnesList'
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
