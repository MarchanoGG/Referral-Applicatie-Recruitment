<template>
  <q-page class="q-pa-md">
    <q-table dense :rows="tablerows" :columns="columns" row-key="id" :pagination="pagination">
      <template v-slot:top>
        <q-toolbar>
          <q-toolbar-title :shrink="true">Candidates</q-toolbar-title>
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
            <div class="text-h6">Add Candidate</div>
          </div>
        </q-card-section>

        <q-card-section class="q-pt-none">
          <q-form @submit="addItem" @reset="resetForm">
            <div class="row">
              <div class="col-5">


                <q-input filled v-model="selected_item.profile.name" label="firstname" hint="Firstname" lazy-rules />

                <q-input filled v-model="selected_item.profile.surname" label="surname" hint="Surname" lazy-rules />

                <q-input filled v-model="selected_item.profile.initials" label="initials" hint="Initials" lazy-rules />
                <q-input filled v-model="selected_item.profile.email" label="email" hint="Email" lazy-rules />

                <q-input filled v-model="selected_item.profile.phone_number" label="phone number" hint="Phone number"
                  lazy-rules />

                <q-input filled v-model="selected_item.profile.address" label="address" hint="Address" lazy-rules />
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
            <div class="text-h6">Update Candidate</div>
          </div>
        </q-card-section>

        <q-card-section class="q-pt-none">
          <q-form @submit="editItem" @reset="resetForm">
            <div class="row">
              <div class="col-5">
                <q-input filled v-model="selected_item.profile.initials" label="initials" hint="Initials" lazy-rules />

                <q-input filled v-model="selected_item.profile.name" label="firstname" hint="Firstname" lazy-rules />

                <q-input filled v-model="selected_item.profile.surname" label="surname" hint="Surname" lazy-rules />

                <q-input filled v-model="selected_item.profile.email" label="email" hint="Email" lazy-rules />

                <q-input filled v-model="selected_item.profile.phone_number" label="phone number" hint="Phone number"
                  lazy-rules />

                <q-input filled v-model="selected_item.profile.address" label="address" hint="Address" lazy-rules />
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
            <div class="text-h6">Delete Candidate</div>
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
    name: 'initials',
    required: true,
    label: 'Initials',
    align: 'left',
    field: 'profile',
    format: val => `${val.initials}`,
    sortable: true
  },
  {
    name: 'name',
    required: true,
    label: 'Name',
    align: 'left',
    field: 'profile',
    format: val => `${val.fullname}`,
    sortable: true
  },
  {
    name: 'address',
    required: true,
    label: 'Address',
    align: 'left',
    field: 'profile',
    format: val => `${val.address}`,
    sortable: true
  },
  {
    name: 'email',
    required: true,
    label: 'Email',
    align: 'left',
    field: 'profile',
    format: val => `${val.email}`,
    sortable: true
  },
  {
    name: 'phone',
    required: true,
    label: 'Phone',
    align: 'left',
    field: 'profile',
    format: val => `${val.phone_number}`,
    sortable: true
  },
]
export default defineComponent({
  name: 'CandidatesList',
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
      fk_profile: null,
      object_key: null,
      profile: {
        initials: null,
        name: null,
        surname: null,
        email: null,
        phone_number: null,
        address: null,
      }
    }
    return {
      rows: [],
      isPwd: false,
      default_item: default_item,
      selected_item: default_item,
    }
  },
  computed: {
    tablerows() {
      return this.rows.map(this.readyRowItem)
    },
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
      api.post('/Candidates', this.selected_item)
        .then((response) => {
          if (response.status == 200) {
            this.addform = false
            this.getCandidates()
            this.resetForm()
          }
        })
        .catch(() => {
        })
    },
    editItem() {
      api.put('/Candidates', this.selected_item)
        .then((response) => {
          if (response.status == 200) {
            this.editform = false
            this.getCandidates()
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
      api.delete('/Candidates', params)
        .then((response) => {
          if (response.status == 200) {
            this.delform = false
            this.getCandidates()
            this.resetForm()
          }
        })
        .catch(() => {
        })
    },
    resetForm() {
      this.selected_item = this.readyRowItem(this.default_item)
    },
    getCandidates() {
      api.get('/Candidates')
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
    this.getCandidates()
  },
})
</script>
