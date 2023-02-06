<template>
  <q-page class="q-pa-md">
    <q-table dense :rows="tablerows" :columns="columns" row-key="id" :pagination="pagination">
      <template v-slot:top>
        <q-toolbar>
          <q-toolbar-title :shrink="true">Rewards</q-toolbar-title>
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

    <q-dialog v-model="addform">
      <q-card style="width: 700px; max-width: 80vw;">
        <q-card-section>
          <div class="flex">
            <div class="text-h6">Add Reward</div>
          </div>
        </q-card-section>

        <q-card-section class="q-pt-none">
          <q-form @submit="addItem">
            <div class="row">
              <div class="col-5">
                <q-input filled v-model="selected_item.name" label="Reward name *" hint="" lazy-rules
                  :rules="[val => val && val.length > 0 || 'Please type something']" />

                <q-select v-model="selected_item.user" :options="userrows" option-value="object_key"
                  option-label="username" label="Reward to user" lazy-rules
                  :rules="[val => val || 'Required to choose a user']" />
                <q-date v-model="selected_item.award_dt" subtitle="Award at" minimal lazy-rules
                  :rules="[val => val || 'Please type something']" />
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
    <q-dialog v-model="editform">
      <q-card style="width: 700px; max-width: 80vw;">
        <q-card-section>
          <div class="flex">
            <div class="text-h6">Update Reward</div>
          </div>
        </q-card-section>

        <q-card-section class="q-pt-none">
          <q-form @submit="editItem">
            <div class="row">
              <div class="col-5">
                <q-input filled v-model="selected_item.name" label="Reward name *" hint="" lazy-rules
                  :rules="[val => val && val.length > 0 || 'Please type something']" />

                <q-select v-model="selected_item.user" :options="userrows" option-value="object_key"
                  option-label="username" label="" />
                <q-date v-model="selected_item.award_dt" subtitle="Award at" minimal />
              </div>
            </div>

            <div class="col-5">
              <q-btn label="Submit" type="submit" color="primary" />
            </div>
          </q-form>
        </q-card-section>

      </q-card>
    </q-dialog>

    <!-- delete form -->
    <q-dialog v-model="delform">
      <q-card style="width: 700px; max-width: 80vw;">
        <q-card-section>
          <div class="flex">
            <div class="text-h6">Delete Reward</div>
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
    name: 'fullname',
    required: true,
    label: 'Awarded to',
    align: 'left',
    field: 'user',
    format: val => `${val.username}`,
    sortable: true
  },
  {
    name: 'awardAt',
    label: 'Awarded at',
    field: 'award_dt_str',
    align: 'left',
    sortable: true
  }
]
export default defineComponent({
  name: 'RewardsList',
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
      fk_user: null,
      user: null,
      object_key: null,
      award_dt: new Date().toLocaleDateString(),
    }
    return {
      rows: [],
      rewardRows: [],
      userrows: [],
      isPwd: false,
      default_item: default_item,
      selected_item: default_item,
    }
  },
  computed: {
    tablerows() {
      return this.rewardRows
    }

  },
  methods: {
    readyUserRowItem(item) {
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
      this.selected_item.fk_user = this.selected_item.user.object_key
      api.post('/Rewards', this.selected_item)
        .then((response) => {
          if (response.status == 200) {
            this.addform = false
            this.getRewards()
            this.resetForm()
          }
        })
        .catch(() => {
        })
    },
    editItem() {
      this.selected_item.fk_user = this.selected_item.user.object_key
      api.put('/Rewards', this.selected_item)
        .then((response) => {
          if (response.status == 200) {
            this.editform = false
            this.getRewards()
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
      api.delete('/Rewards', params)
        .then((response) => {
          if (response.status == 200) {
            this.delform = false
            this.getRewards()
            this.resetForm()
          }
        })
        .catch(() => {
        })
    },
    resetForm() {
      this.selected_item = this.default_item
    },
    getRewards() {
      api.get('/Rewards')
        .then((response) => {
          if (response.data && response.data.length > 0) {
            this.rewardRows = response.data
          }
        })
        .catch(() => {
        })
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
  },
  mounted() {
    this.getUsers()
    this.getRewards()
  },
})
</script>
