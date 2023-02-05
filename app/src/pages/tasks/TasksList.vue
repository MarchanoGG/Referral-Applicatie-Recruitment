<template>
  <q-page class="q-pa-md">
    <q-table dense :rows="tablerows" :columns="columns" row-key="id" :pagination="pagination">
      <template v-slot:top>
        <q-toolbar>
          <q-toolbar-title :shrink="true">Tasks</q-toolbar-title>
          <q-separator vertical inset />
          <q-btn @click="taskStore.addform = true" class="q-ml-md" color="secondary" dense :icon="'person_add'" />
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
              <q-btn class="" color="secondary" dense
                @click="taskStore.editform = true; taskStore.selected_item = props.row;" :icon="'edit'" />
              <q-btn class="" color="secondary" dense
                @click="taskStore.delform = true; taskStore.selected_item = props.row;" :icon="'delete'" />
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

    <q-dialog v-model="taskStore.addform" @hide="taskStore.resetItem">
      <q-card style="width: 700px; max-width: 80vw;">
        <q-card-section>
          <div class="flex">
            <div class="text-h6">Add Task</div>
          </div>
        </q-card-section>

        <q-card-section class="q-pt-none">
          <q-form @submit="taskStore.addItem" @reset="taskStore.resetItem">
            <div class="row">
              <div class="col-5">
                <q-input filled v-model="taskStore.selected_item.name" label="Task name *" hint="" lazy-rules
                  :rules="[val => val && val.length > 0 || 'Please type something']" />

                <q-input filled v-model="taskStore.selected_item.points" label="Task points *" hint="" lazy-rules
                  :rules="[val => val && String(val).length > 0 || 'Please type something']" />
              </div>
              <div class="col-5">
                <q-input filled v-model="taskStore.selected_item.description" label="Task Description *" hint=""
                  type="textarea" lazy-rules :rules="[val => val && val.length > 0 || 'Please type something']" />
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
    <q-dialog v-model="taskStore.editform" @hide="taskStore.resetItem">
      <q-card style="width: 700px; max-width: 80vw;">
        <q-card-section>
          <div class="flex">
            <div class="text-h6">Update Task</div>
          </div>
        </q-card-section>

        <q-card-section class="q-pt-none">
          <q-form @submit="taskStore.editItem" @reset="taskStore.resetItem">
            <div class="row">
              <div class="col-5">
                <q-input filled v-model="taskStore.selected_item.name" label="Task name *" hint="" lazy-rules
                  :rules="[val => val && val.length > 0 || 'Please type something']" />

                <q-input filled v-model="taskStore.selected_item.points" label="Task points *" hint="" lazy-rules
                  :rules="[val => val && String(val).length > 0 || 'Please type something']" />
              </div>
              <div class="col-5">
                <q-input filled v-model="taskStore.selected_item.description" label="Task Description *" hint=""
                  type="textarea" lazy-rules :rules="[val => val && val.length > 0 || 'Please type something']" />
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
    <q-dialog v-model="taskStore.delform" @hide="taskStore.resetItem">
      <q-card style="width: 700px; max-width: 80vw;">
        <q-card-section>
          <div class="flex">
            <div class="text-h6">Delete Task</div>
          </div>
        </q-card-section>

        <q-card-actions class="bg-white ">
          <q-btn @click="taskStore.deleteItem" label="Delete" type="submit" color="primary" />
          <q-btn label="Cancel" v-close-popup color="primary" flat class="float-right" />
        </q-card-actions>

      </q-card>
    </q-dialog>
  </q-page>
</template>

<script>
import { defineComponent, ref, reactive, computed } from 'vue'
import { useTaskStore } from 'stores/tasks'

const taskStore = useTaskStore();
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
    name: 'points',
    label: 'Points',
    field: 'points',
    align: 'left',
    sortable: true
  },
  {
    name: 'description',
    required: true,
    label: 'Description',
    align: 'left',
    field: 'description',
    format: val => `${val}`,
    sortable: true
  },
]
export default defineComponent({
  name: 'TasksList',
  setup() {
    return {
      columns,
      taskStore,
      pagination: { rowsPerPage: 10 },
    }
  },
  computed: {
    tablerows() {
      return this.taskStore.items
    }
  },
  mounted() {
    this.taskStore.all()
  },
})
</script>
