<template>
  <div class="q-pa-md">
    <q-toolbar class="q-my-md shadow-2">
      <EssentialLink v-for="link in essentialLinks" :key="link.title" v-bind="link" />
      <q-separator vertical inset />
    </q-toolbar>
    <q-table title="Rewards" dense :rows="rows" :columns="columns" row-key="id" :loading="loading"
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
    link: '/rewards/add'
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
    name: 'award_dt',
    label: 'Award Date',
    field: 'award_dt',
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
    "name": "Award 1"
    , "description": ""

    , "award_dt": "01-01-2023"
  }
  , {
    "name": "Award 2"
    , "description": ""

    , "award_dt": "01-01-2023"
  }
  , {
    "name": "Award 3"
    , "description": ""

    , "award_dt": "01-01-2023"
  }
  , {
    "name": "Award 4"
    , "description": ""

    , "award_dt": "01-01-2023"
  }
  , {
    "name": "Award 5"
    , "description": ""

    , "award_dt": "01-01-2023"
  }
  , {
    "name": "Award 6"
    , "description": ""

    , "award_dt": "01-01-2023"
  }
  ,
]


import { defineComponent } from 'vue'

export default defineComponent({
  name: 'RewardsList'
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
