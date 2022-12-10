<template>
  <q-page class="q-pa-md">
    <q-table title="Users" dense :rows="rows" :columns="columns" row-key="id" :loading="loading"
      :pagination="pagination">
      <template v-slot:top>
        <q-toolbar>
          <q-toolbar-title :shrink="true">Users</q-toolbar-title>
          <q-separator vertical inset />
          <q-btn @click="addform = true" class="q-ml-md" color="secondary" dense :icon="'person_add'" />
        </q-toolbar>
      </template>

      <template v-slot:header="props">
        <q-tr :props="props">
          <q-th>
            <!-- <q-btn class="float-left" color="secondary" dense :icon="'info'" /> -->
          </q-th>
          <q-th v-for="col in props.cols" :key="col.name" :props="props">
            {{ col.label }}
          </q-th>
        </q-tr>
      </template>

      <template v-slot:body="props">
        <q-tr :props="props">
          <q-td auto-width>
            <q-btn-group>
              <!-- <q-btn class="" color="secondary" dense @click="props.expand = !props.expand" :icon="'info'" /> -->
              <q-btn class="" color="secondary" dense @click="addform = true; selected_item = props.row.object_key;" :icon="'edit'" />
              <q-btn class="" color="secondary" dense @click="props.expand = !props.expand" :icon="'delete'" />
            </q-btn-group>
          </q-td>
          <q-td v-for="col in props.cols" :key="col.name" :props="props">
            {{ col.value }}
          </q-td>
        </q-tr>
        <q-tr v-show="props.expand" :props="props">
          <q-td colspan="100%">
            <div class="text-left">This is expand slot for row above: {{ props.row.object_key }}.</div>
          </q-td>
        </q-tr>
      </template>

    </q-table>

    <q-dialog v-model="addform">
      <q-card style="width: 700px; max-width: 80vw;">
        <q-card-section>
          <div class="flex">
            <div class="text-h6">Users</div>
          </div>
        </q-card-section>

        <q-card-section class="q-pt-none">  
          <UserAddFormVue :objectkey="selected_item"/>
        </q-card-section>

        <q-card-actions align="right" class="bg-white text-teal">
          <q-btn flat label="OK" v-close-popup />
        </q-card-actions>

      </q-card>
    </q-dialog>
  </q-page>
</template>

<script>
import { api } from 'boot/axios'
import UserAddFormVue from './UserAddForm.vue'


  const columns = [
  {
    name: 'object_key',
    required: true,
    label: 'Key',
    align: 'left',
    field: 'object_key',
    format: val => `${val}`,
    sortable: true
  },
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

import { defineComponent, ref } from 'vue'

export default defineComponent({
  name: 'UserList'

  , components: {
    UserAddFormVue
  }
  , setup() {
    const selected_item = ref(null)
    return {
      columns,
      selected_item,
      pagination: { rowsPerPage: 10 },
      addform: ref(false),
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
