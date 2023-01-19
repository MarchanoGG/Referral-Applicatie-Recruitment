<template>
  <q-page class="q-pa-md">
    <q-table dense :rows="scoreboardStore.items" :columns="columns" row-key="id" :pagination="pagination">
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
              <q-btn class="" color="secondary" dense @click="editform = true; scoreboardStore.focusItem(props.row);"
                :icon="'edit'" />
              <q-btn class="" color="secondary" dense @click="delform = true; scoreboardStore.focusItem(props.row);"
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

    <q-dialog v-model="addform" @hide="scoreboardStore.resetItem">
      <q-card style="width: 700px; max-width: 80vw;">
        <q-card-section>
          <div class="flex">
            <div class="text-h6">Add Scoreboards</div>
          </div>
        </q-card-section>

        <q-card-section class="q-pt-none">
          <q-form @submit="scoreboardStore.addItem" @reset="scoreboardStore.resetItem">
            <div class="row">
              <div class="col-5">
                <q-input filled v-model="scoreboardStore.selected_item.name" label="Your username *" hint="Userame"
                  lazy-rules :rules="[val => val && val.length > 0 || 'Please type something']" />
                <q-select filled v-model="scoreboardStore.selected_item.fk_user" :options="userrows"
                  option-value="object_key" option-label="username" label="Standard" emit-value />
                <q-date v-model="scoreboardStore.start_to_end" subtitle="Start Date" mask="D-M-YYYY" />
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
    <q-dialog v-model="editform" @hide="scoreboardStore.resetItem">
      <q-card style="width: 700px; max-width: 80vw;">
        <q-card-section>
          <div class="flex">
            <div class="text-h6">Update User</div>
          </div>
        </q-card-section>

        <q-card-section class="q-pt-none">
          <q-form @submit="scoreboardStore.editItem" @reset="scoreboardStore.resetItem">
            <div class="row">
              <div class="col-5">
                <q-input filled v-model="scoreboardStore.selected_item.name" label="Your username *" hint="Userame"
                  lazy-rules :rules="[val => val && val.length > 0 || 'Please type something']" />
                <q-select filled v-model="scoreboardStore.selected_item.fk_user" :options="userrows"
                  option-value="object_key" option-label="username" label="Standard" emit-value />
                <q-date v-model="scoreboardStore.start_to_end" subtitle="Start Date" range />
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
    <q-dialog v-model="delform" @hide="scoreboardStore.resetItem">
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
import { defineComponent, ref, reactive, computed } from 'vue'
import { useScoreboardStore } from 'stores/scoreboard'

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
const scoreboardStore = useScoreboardStore()
scoreboardStore.all()

export default defineComponent({
  name: 'ScoreboardsList',
  setup() {
    return {
      columns,
      pagination: { rowsPerPage: 10 },
      addform: ref(false),
      delform: ref(false),
      editform: ref(false),
      scoreboardStore
    }
  },
  data() {
    return {
      userrows: [],
    }
  },
})
</script>
