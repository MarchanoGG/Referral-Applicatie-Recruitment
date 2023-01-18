<template>
  <q-page class="q-pa-md">
    <q-table dense :rows="tablerows" :columns="columns" row-key="id" :pagination="pagination">
      <template v-slot:top>
        <q-toolbar>
          <q-toolbar-title :shrink="true">Campaigns</q-toolbar-title>
          <q-separator vertical inset />
          <q-btn type="a" href="/campaignes/add" class="q-ml-md" color="secondary" dense :icon="'person_add'" />
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
            <div class="text-h6">Add Campaign</div>
          </div>
        </q-card-section>

        <q-card-section class="q-pt-none">
          <q-form @submit="addItem" @reset="resetForm">
            <div class="row">
              <div class="col-5">
                <q-input filled v-model="selected_item.username" label="Your username *" hint="Userame" lazy-rules
                  :rules="[val => val && val.length > 0 || 'Please type something']" />

                <q-input filled label="Password *" v-model="selected_item.password" :type="isPwd ? 'password' : 'text'"
                  hint="Password" lazy-rules :rules="[val => val && val.length > 0 || 'Please type something']">
                  <template v-slot:append>
                    <q-icon :name="isPwd ? 'visibility_off' : 'visibility'" class="cursor-pointer"
                      @click="isPwd = !isPwd" />
                  </template>
                </q-input>
                <q-toggle v-model="selected_item.recruiter_bool" label="Is a recruiter?" />
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
                <q-input filled v-model="selected_item.username" label="Your username *" hint="Userame" lazy-rules
                  :rules="[val => val && val.length > 0 || 'Please type something']" />

                <q-input filled label="Password *" v-model="selected_item.password" :type="isPwd ? 'password' : 'text'"
                  hint="Password" lazy-rules :rules="[val => val && val.length > 0 || 'Please type something']">
                  <template v-slot:append>
                    <q-icon :name="isPwd ? 'visibility_off' : 'visibility'" class="cursor-pointer"
                      @click="isPwd = !isPwd" />
                  </template>
                </q-input>
                <q-toggle v-model="selected_item.recruiter_bool" label="Is a recruiter" />
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
            <div class="text-h6">Delete Campaign</div>
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
import { defineComponent, ref, computed } from 'vue'

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
    name: 'scoreboard',
    required: true,
    label: 'Scoreboard name',
    align: 'left',
    field: 'scoreboard',
    format: val => `${val.name}`,
    sortable: true
  },
  {
    name: 'username',
    required: true,
    label: 'Employee name',
    align: 'left',
    field: 'user',
    format: val => `${val.username}`,
    sortable: true
  },
  {
    name: 'candidate',
    required: true,
    label: 'Candidate name',
    align: 'left',
    field: 'user',
    format: val => `${val.profile.name}`,
    sortable: true
  },
]
export default defineComponent({
  name: 'CampaignesList',
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
      scoreboard: null,
      object_key: null,
    }
    return {
      rows: [],
      referralrows: [],
      isPwd: false,
      default_item: default_item,
      selected_item: default_item,
    }
  },
  computed: {
    tablerows() {
      return this.referralrows
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
      api.post('/Referrals', this.selected_item)
        .then((response) => {
          if (response.status == 200) {
            this.addform = false
            this.getUsers()
            this.resetForm()
          }
        })
        .catch(() => {
        })
    },
    editItem() {
      const params = {
        username: this.selected_item.username,
        password: this.selected_item.password,
        recruiter: this.selected_item.recruiter,
        object_key: this.selected_item.object_key,
      }
      api.put('/Referrals', params)
        .then((response) => {
          if (response.status == 200) {
            this.editform = false
            this.getUsers()
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
      api.delete('/Referrals', params)
        .then((response) => {
          if (response.status == 200) {
            this.delform = false
            this.getUsers()
            this.resetForm()
          }
        })
        .catch(() => {
        })
    },
    resetForm() {
      this.selected_item = this.readyRowItem(this.default_item)
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
    getReferrals() {
      api.get('/Referrals')
        .then((response) => {
          if (response.data && response.data.length > 0) {
            this.referralrows = response.data
          }
        })
        .catch(() => {
        })
    },
    getUsers() {
      api.get('/Users')
        .then((response) => {
          if (response.data && response.data.length > 0) {
            this.rows = response.data
          }
        })
        .catch(() => {
        })
    },
  },
  mounted() {
    this.getReferrals()
  },
})
</script>
