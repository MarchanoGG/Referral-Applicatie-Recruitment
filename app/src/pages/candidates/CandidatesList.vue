<template>
  <div class="q-pa-md">
    <q-toolbar class="q-my-md shadow-2">
      <EssentialLink v-for="link in essentialLinks" :key="link.title" v-bind="link" />
      <q-separator vertical inset />
    </q-toolbar>
    <q-table title="Candidates" dense :rows="rows" :columns="columns" row-key="id" :loading="loading"
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
    link: '/Candidates/add'
  },
]

const columns = [
  {
    name: 'fk_profile',
    label: 'Profile key',
    field: 'fk_profile',
    align: 'left',
    sortable: true
  },
  {
    name: 'referred_at',
    label: 'Referred at',
    field: 'referred_at',
    align: 'left',
    sortable: true
  },
]

import { defineComponent } from 'vue'

export default defineComponent({
  name: 'CandidatesList'
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
    api.get('/Candidates')
      .then((response) => {
        this.rows = response.data
      })
      .catch(() => {
      })
  }
})
</script>
