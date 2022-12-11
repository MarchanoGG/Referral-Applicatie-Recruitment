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
              <q-btn class="" color="secondary" dense @click="addform = true; selected_id = props.row.object_key;"
                :icon="'edit'" />
              <q-btn class="" color="secondary" dense @click="delform = true; selected_id = props.row.object_key;"
                :icon="'delete'" />
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
            <div class="text-h6">Add User</div>
          </div>
        </q-card-section>

        <q-card-section class="q-pt-none">
          <q-form @submit="addItem" @reset="resetForm">
            <div class="row">
              <div class="col-5">
                <q-input filled v-model="username" label="Your username *" hint="Userame" lazy-rules
                  :rules="[val => val && val.length > 0 || 'Please type something']" />

                <q-input filled label="Password *" v-model="password" :type="isPwd ? 'password' : 'text'"
                  hint="Password" lazy-rules :rules="[val => val && val.length > 0 || 'Please type something']">
                  <template v-slot:append>
                    <q-icon :name="isPwd ? 'visibility_off' : 'visibility'" class="cursor-pointer"
                      @click="isPwd = !isPwd" />
                  </template>
                </q-input>
                <q-toggle v-model="recruiter" label="Is a recruiter" />
              </div>
            </div>

            <div class="col-5">
              <q-btn label="Submit" type="submit" color="primary" />
              <q-btn label="Reset" type="reset" color="primary" flat class="q-ml-sm" />
            </div>
          </q-form>
        </q-card-section>
      </q-card>
    </q-dialog>

    <!-- edit form -->
    <!-- <q-dialog v-model="editform">
      <q-card style="width: 700px; max-width: 80vw;">
        <q-card-section>
          <div class="flex">
            <div class="text-h6">Update User</div>
          </div>
        </q-card-section>

        <q-card-section class="q-pt-none">
          <UserEditFormVue :objectkey="selected_id" />
        </q-card-section>

        <q-card-actions align="right" class="bg-white text-teal">
          <q-btn flat label="OK" v-close-popup />
        </q-card-actions>

      </q-card>
    </q-dialog> -->

    <!-- delete form -->
    <q-dialog v-model="delform">
      <q-card style="width: 700px; max-width: 80vw;">
        <q-card-section>
          <div class="flex">
            <div class="text-h6">Delete User</div>
          </div>
        </q-card-section>

        <q-card-actions class="bg-white ">
          <q-btn @click="deleteItem" label="Delete" type="submit" color="primary" />
          <q-btn label="Cancel" v-close-popup color="primary" flat class="float-right" />
        </q-card-actions>

      </q-card>
    </q-dialog>
  </q-page>
</template>

<script>
import { api } from 'boot/axios'
import { defineComponent, ref, reactive } from 'vue'

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
export default defineComponent({
  name: 'UserList'
  , setup() {
    return {
      columns,
      pagination: { rowsPerPage: 10 },
      addform: ref(false),
      delform: ref(false),
    }
  },
  data() {
    return {
      username: null,
      password: null,
      recruiter: false,
      selected_id: null,
      rows: [],
    }
  },
  methods: {
    // also the edit
    addItem() {
      const params = reactive({
        username: this.username,
        password: this.password,
        recruiter: this.recruiter ? "1" : "0",
      });

      api.post('/Users', params)
        .then((response) => {
          if (response.status == 200) {
            this.addform = false
            this.getUsers()
            this.reset()
          }
        })
        .catch(() => {
        })
    },
    deleteItem() {
      const params = {
        params: { object_key: this.selected_id, }
      }
      api.delete('/Users', params)
        .then((response) => {
          if (response.status == 200) {
            this.delform = false
            this.getUsers()
            this.reset()
          }
        })
        .catch(() => {
        })
    },
    resetForm() {
      this.username = null
      this.password = null
      this.recruiter = false
      this.selected_id = null
    },
    getUsers() {
      api.get('/Users')
        .then((response) => {
          this.rows = response.data
        })
        .catch(() => {
        })
    },
  },
  mounted() {
    this.getUsers()
  },
})
</script>
