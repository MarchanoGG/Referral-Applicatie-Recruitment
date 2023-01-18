<template>
  <q-page class="q-pa-md">
    <q-table dense :rows="tablerows" :columns="columns" row-key="id" :pagination="pagination">
      <template v-slot:top>
        <q-toolbar>
          <q-toolbar-title :shrink="true">Scoreboards</q-toolbar-title>
          <q-separator vertical inset />
          <q-btn type="router-link" href="/admin/scoreboards/add" class="q-ml-md" color="secondary" dense
            :icon="'person_add'" />
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
              <q-btn class="" color="secondary" dense @click="editform = true; selected_item = props.row;"
                :icon="'edit'" />
              <q-btn class="" color="secondary" dense @click="delform = true; selected_item = props.row;"
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

    <q-dialog v-model="addform" @hide="resetForm">
      <q-card style="width: 700px; max-width: 80vw;">
        <q-card-section>
          <div class="flex">
            <div class="text-h6">Add Scoreboards</div>
          </div>
        </q-card-section>

        <q-card-section class="q-pt-none">
          <q-form @submit="addItem" @reset="resetForm">
            <div class="row">
              <div class="col-5">
                <q-input filled v-model="selected_item.name" label="Your username *" hint="Userame" lazy-rules
                  :rules="[val => val && val.length > 0 || 'Please type something']" />
                <q-select filled v-model="selected_item.fk_user" :options="userrows" option-value="object_key"
                  option-label="username" label="Standard" emit-value />
                <q-date v-model="selected_item.start_dt" subtitle="Start Date" />
                <q-date v-model="selected_item.end_dt" subtitle="End Date" />
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
    <q-dialog v-model="editform" @hide="resetForm">
      <q-card style="width: 700px; max-width: 80vw;">
        <q-card-section>
          <div class="flex">
            <div class="text-h6">Update User</div>
          </div>
        </q-card-section>

        <q-card-section class="q-pt-none">
          <q-form @submit="editItem" @reset="resetForm">
            <div class="row">
              <div class="col-5">
                <q-input filled v-model="selected_item.name" label="Your username *" hint="Userame" lazy-rules
                  :rules="[val => val && val.length > 0 || 'Please type something']" />
                <q-select filled v-model="selected_item.fk_user" :options="userrows" option-value="object_key"
                  option-label="username" label="Standard" emit-value />
                <q-date v-model="selected_item.start_dt" subtitle="Start Date" />
                <q-date v-model="selected_item.end_dt" subtitle="End Date" />
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

    <!-- delete form -->
    <q-dialog v-model="delform" @hide="resetForm">
      <q-card style="width: 700px; max-width: 80vw;">
        <q-card-section>
          <div class="flex">
            <div class="text-h6">Delete Scoreboard?</div>
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
import { defineComponent, ref, reactive, computed } from 'vue'

const columns = [
  {
    name: 'object_key',
    required: true,
    label: '#',
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
    field: 'name',
    format: val => `${val}`,
    sortable: true
  },
  {
    name: 'startDate',
    label: 'Start Date',
    field: 'start_dt',
    align: 'left',
    sortable: true
  },
  {
    name: 'endDate',
    label: 'End Date',
    field: 'end_dt',
    align: 'left',
    sortable: true
  },
]
export default defineComponent({
  name: 'ScoreboardsList',
  setup() {
    return {
      columns,
      pagination: { rowsPerPage: 10 },
      addform: ref(false),
      delform: ref(false),
      editform: ref(false),
    }
  },
  data() {
    const default_item = {
      name: null,
      object_key: null,
      start_dt: new Date().toLocaleDateString(),
      end_dt: new Date().toLocaleDateString(),
    }
    return {
      rows: [],
      isPwd: false,
      userrows: [],
      scoreboardrows: [],
      default_item: default_item,
      selected_item: default_item,
    }
  },
  computed: {
    tablerows() {
      return this.scoreboardrows
    }
  },
  methods: {
    readyRowItem(item) {
      item.recruiter_bool = computed({
        get: () => Boolean(item.recruiter),
        set: (val) => {
          item.recruiter = Number(val)
        }
      })
      item.recruiter_str = computed({
        get: () => item.recruiter ? 'Yes' : 'No',
        set: (val) => {
          item.recruiter = Number(val)
        }
      })
      return item
    },
    addItem() {
      api.post('/Scoreboards', this.selected_item)
        .then((response) => {
          if (response.status == 200) {
            this.addform = false
            this.getScoreboards()
            this.resetForm()
          }
        })
        .catch(() => {
        })
    },
    editItem() {
      api.put('/Scoreboards', this.selected_item)
        .then((response) => {
          if (response.status == 200) {
            this.editform = false
            this.getScoreboards()
            this.resetForm()
          }
        })
        .catch(() => {
        })
    },
    deleteItem() {
      const params = {
        params: { object_key: this.selected_item.object_key, }
      }
      api.delete('/Scoreboards', params)
        .then((response) => {
          if (response.status == 200) {
            this.delform = false
            this.getScoreboards()
            this.resetForm()
          }
        })
        .catch(() => {
        })
    },
    resetForm() {
      this.selected_item = this.default_item
    },
    getUsers() {
      api.get('/Users')
        .then((response) => {
          if (response.data && response.data.length > 0) {
            this.userrows = response.data
          }
        })
        .catch(() => {
        })
    },
    getScoreboards() {
      api.get('/Scoreboards')
        .then((response) => {
          if (response.data && response.data.length > 0) {
            this.scoreboardrows = response.data
          }
        })
        .catch(() => {
        })
    },
  },
  mounted() {
    this.getScoreboards()
    this.getUsers()
  },
})
</script>
