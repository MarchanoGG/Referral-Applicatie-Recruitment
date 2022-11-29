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
  import { api } from 'boot/axios'
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
    name: 'award_dt',
    label: 'Award Date',
    field: 'award_dt',
    align: 'left',
    sortable: true
  },
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
      pagination: { rowsPerPage: 10 },
    }
  },
  data() {
    return {
      rows: []
    }
  },
  mounted() {
    api.get('/Rewards')
      .then((response) => {
        this.rows = response.data
      })
      .catch(() => {
      })
  }
})
</script>
