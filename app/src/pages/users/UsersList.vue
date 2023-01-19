<template>
  <q-page class="q-pa-md">
    <q-table dense :rows="userStore.tablerows" :columns="columns" row-key="id" :pagination="pagination">
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
              <q-btn class="" color="secondary" dense @click="editform = true; userStore.selected_item = props.row;"
                :icon="'edit'" />
              <q-btn class="" color="secondary" dense @click="delform = true; userStore.selected_item = props.row;"
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

    <q-dialog v-model="addform" @hide="userStore.resetItem()">
      <q-card style="width: 700px; max-width: 80vw;">
        <q-card-section>
          <div class="flex">
            <div class="text-h6">Add User</div>
          </div>
        </q-card-section>

        <q-card-section class="q-pt-none">
          <q-form @submit="userStore.addItem(); addform = userStore.has_errors" @reset="userStore.resetItem()">
            <div class="row">
              <div class="col-5">
                <q-input filled v-model="userStore.selected_item.username" label="Your username *" hint="Userame"
                  lazy-rules :rules="[val => val && val.length > 0 || 'Please type something']" />

                <q-input filled label="Password *" v-model="userStore.selected_item.password"
                  :type="isPwd ? 'password' : 'text'" hint="Password" lazy-rules
                  :rules="[val => val && val.length > 0 || 'Please type something']">
                  <template v-slot:append>
                    <q-icon :name="isPwd ? 'visibility_off' : 'visibility'" class="cursor-pointer"
                      @click="isPwd = !isPwd" />
                  </template>
                </q-input>
                <q-toggle v-model="userStore.selected_item.recruiter_bool" label="Is a recruiter?" />
              </div>

              <div class="col-5">
                <q-input filled v-model="userStore.selected_item.profile.initials" label="Your initials" hint="Initials"
                  lazy-rules />

                <q-input filled v-model="userStore.selected_item.profile.name" label="Your firstname" hint="Firstname"
                  lazy-rules />

                <q-input filled v-model="userStore.selected_item.profile.surname" label="Your surname" hint="Surname"
                  lazy-rules />

                <q-input filled v-model="userStore.selected_item.profile.email" label="Your email" hint="Email"
                  lazy-rules />

                <q-input filled v-model="userStore.selected_item.profile.phone_number" label="Your phone number"
                  hint="Phone number" lazy-rules />

                <q-input filled v-model="userStore.selected_item.profile.address" label="Your address" hint="Address"
                  lazy-rules />
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
    <q-dialog v-model="editform" @hide="userStore.resetItem()">
      <q-card style="width: 700px; max-width: 80vw;">
        <q-card-section>
          <div class="flex">
            <div class="text-h6">Update User</div>
          </div>
        </q-card-section>

        <q-card-section class="q-pt-none">
          <q-form @submit="userStore.editItem(); editform = userStore.has_errors" @reset="userStore.resetItem()">
            <div class="row">
              <div class="col-5">
                <q-input filled v-model="userStore.selected_item.username" label="Your username *" hint="Userame"
                  lazy-rules :rules="[val => val && val.length > 0 || 'Please type something']" />

                <q-input filled label="Password *" v-model="userStore.selected_item.password"
                  :type="isPwd ? 'password' : 'text'" hint="Password" lazy-rules
                  :rules="[val => val && val.length > 0 || 'Please type something']">
                  <template v-slot:append>
                    <q-icon :name="isPwd ? 'visibility_off' : 'visibility'" class="cursor-pointer"
                      @click="isPwd = !isPwd" />
                  </template>
                </q-input>
                <q-toggle v-model="userStore.selected_item.recruiter_bool" label="Is a recruiter" />
              </div>

              <div class="col-5">
                <q-input filled v-model="userStore.selected_item.profile.initials" label="Your initials" hint="Initials"
                  lazy-rules />

                <q-input filled v-model="userStore.selected_item.profile.name" label="Your firstname" hint="Firstname"
                  lazy-rules />

                <q-input filled v-model="userStore.selected_item.profile.surname" label="Your surname" hint="Surname"
                  lazy-rules />

                <q-input filled v-model="userStore.selected_item.profile.email" label="Your email" hint="Email"
                  lazy-rules />

                <q-input filled v-model="userStore.selected_item.profile.phone_number" label="Your phone number"
                  hint="Phone number" lazy-rules />

                <q-input filled v-model="userStore.selected_item.profile.address" label="Your address" hint="Address"
                  lazy-rules />
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
    <q-dialog v-model="delform" @hide="userStore.resetItem()">
      <q-card style="width: 700px; max-width: 80vw;">
        <q-card-section>
          <div class="flex">
            <div class="text-h6">Delete User</div>
          </div>
        </q-card-section>

        <q-card-actions class="bg-white ">
          <q-btn @click="userStore.deleteItem(); delform = userStore.has_errors" label="Delete" type="submit"
            color="primary" />
          <q-btn label="Cancel" v-close-popup color="primary" flat class="float-right" />
        </q-card-actions>

      </q-card>
    </q-dialog>
  </q-page>
</template>

<script>
import { defineComponent, ref, reactive, computed } from 'vue'
import { useUserStore } from "stores/user";

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
    field: 'username',
    format: val => `${val}`,
    sortable: true
  },
  {
    name: 'recruiter',
    label: 'Is a recruiter',
    field: 'recruiter_str',
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
  name: 'UserList',
  setup() {
    const userStore = useUserStore();
    userStore.all()
    return {
      columns,
      pagination: { rowsPerPage: 10 },
      addform: ref(false),
      delform: ref(false),
      editform: ref(false),
      userStore,
    }
  },
  data() {
    return {
      isPwd: false,
    }
  },
})
</script>
