<template>
  <q-page class="q-pa-md">
    <q-table title="Users" dense :rows="rows" :columns="columns" row-key="id" :loading="loading"
      :pagination="pagination">

      <template v-slot:top>
        <q-toolbar>
          <q-toolbar-title :shrink="true">Users</q-toolbar-title>
          <q-separator vertical inset />
          <q-btn type="a" href="/users/add" class="q-ml-md" color="secondary" dense :icon="'person_add'" />
        </q-toolbar>
      </template>

      <template v-slot:header="props">
        <q-tr :props="props">
          <q-th>
            <q-btn class="float-left" color="secondary" dense :icon="'info'" />
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
              <q-btn class="" color="secondary" dense @click="props.expand = !props.expand" :icon="'info'" />
              <q-btn class="" color="secondary" dense @click="props.expand = !props.expand" :icon="'edit'" />
              <q-btn class="" color="secondary" dense @click="props.expand = !props.expand" :icon="'delete'" />
            </q-btn-group>
          </q-td>
          <q-td v-for="col in props.cols" :key="col.name" :props="props">
            {{ col.value }}
          </q-td>
        </q-tr>
        <q-tr v-show="props.expand" :props="props">
          <q-td colspan="100%">
            <div class="text-left">This is expand slot for row above: {{ props.row.name }}.</div>
          </q-td>
        </q-tr>
      </template>
    </q-table>
  </q-page>
</template>

<script>
const linksList = [
  {
    title: '',
    // caption: '',
    icon: 'person_add',
    link: '/users/add'
  },
]

const columns = [
  // {
  //   name: 'id',
  //   label: 'id',
  //   field: 'id',
  //   align: 'left',
  //   // sortable: true
  // },
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
    , "id": 1
  }
  , {
    "fullname": "Marchano"
    , "email": "Marchano@gmail.com"
    , "phone": "1234567"
    , "birthday": "01-01-1996"
    , "id": 2
  }
  , {
    "fullname": "Jeremey"
    , "email": "Jeremey@gmail.com"
    , "phone": "1234567"
    , "birthday": "01-01-1996"
    , "id": 3
  }
  , {
    "fullname": "Michael"
    , "email": "Michael@gmail.com"
    , "phone": "1234567"
    , "birthday": "01-01-1996"
    , "id": 4
  }
  , {
    "fullname": "Mervin"
    , "email": "Mervin@gmail.com"
    , "phone": "1234567"
    , "birthday": "01-01-1996"
    , "id": 5
  }
  , {
    "fullname": "Jon Snow"
    , "email": "jonsnow@gmail.com"
    , "phone": "1234567"
    , "birthday": "01-01-1996"
    , "id": 6
  }
  ,
]


import { defineComponent } from 'vue'

export default defineComponent({
  name: 'UserList'

  , setup() {
    return {
      columns,
      rows,
      pagination: { rowsPerPage: 10 },
    }
  }
})
</script>
