<template>
    <div class="q-pa-md">
        <q-form>
            <q-stepper v-model="step" ref="stepper" color="primary" animated>
                <q-step :name="1" title="Select Scoreboard" icon="settings" :done="step > 1">
                    <div class="row">
                        <div class="col-5">
                            <q-select v-model="selected_item.scoreboard" :options="scoreboardrows"
                                option-value="object_key" option-label="name" />
                        </div>
                    </div>
                </q-step>

                <q-step :name="2" title="Select Recruiter" icon="create_new_folder" :done="step > 2">
                    <div class="row">
                        <div class="col-5">
                            <q-select v-model="selected_item.recruiter" :options="recruiterrows"
                                option-value="object_key" option-label="username" :multiple="true" :use-chips="true" />
                        </div>
                    </div>
                </q-step>

                <q-step :name="3" title="Select Candidate" icon="add_comment" :done="step > 3">
                    <div class="row">
                        <div class="col-5">
                            <q-select v-model="selected_item.candidate" :options="candidaterows"
                                option-value="object_key" :option-label="profilefullname" :multiple="true"
                                :use-chips="true" />
                        </div>
                    </div>
                </q-step>
                <q-step :name="4" title="Select Tasks" icon="add_comment" :done="step > 4">
                    <div class="row">
                        <div class="col-5">
                            <q-select v-model="selected_item.task" :options="taskrows" option-value="object_key"
                                option-label="name" :multiple="true" :use-chips="true" />
                        </div>
                    </div>
                </q-step>
                <q-step :name="5" title="Finish" icon="add_comment">
                </q-step>
                <template v-slot:navigation>
                    <q-stepper-navigation>
                        <q-btn @click="$refs.stepper.next()" color="primary"
                            :label="step === 4 ? 'Finish' : 'Continue'" />
                        <q-btn v-if="step > 1" flat color="primary" @click="$refs.stepper.previous()" label="Back"
                            class="q-ml-sm" />
                    </q-stepper-navigation>
                </template>
            </q-stepper>
        </q-form>
    </div>

</template>
  
<script>
import { api } from 'boot/axios'
import { defineComponent, ref, computed } from 'vue'

export default {
    name: 'CampaignesAddForm',
    setup() {
        return {
        }
    },
    data() {
        const default_item = {
            scoreboard: null,
            recruiter: null,
            candidate: null,
            task: null,
            object_key: null,
        }
        return {
            step: ref(1),
            rows: [],
            recruiterrows: [],
            scoreboardrows: [],
            candidaterows: [],
            taskrows: [],
            isPwd: false,
            default_item: default_item,
            selected_item: default_item,
        }
    },
    methods: {
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
        getRecruiter() {
            api.get('/Users')
                .then((response) => {
                    if (response.data && response.data.length > 0) {
                        this.recruiterrows = response.data
                    }
                })
                .catch(() => {
                })
        },
        getCandidate() {
            api.get('/Candidates')
                .then((response) => {
                    if (response.data && response.data.length > 0) {
                        this.candidaterows = response.data
                    }
                })
                .catch(() => {
                })
        },
        getTask() {
            api.get('/Tasks')
                .then((response) => {
                    if (response.data && response.data.length > 0) {
                        this.taskrows = response.data
                    }
                })
                .catch(() => {
                })
        },
        profilefullname(item) {
            return item?.profile?.surname
        }
    },
    mounted() {
        this.getScoreboards()
        this.getRecruiter()
        this.getCandidate()
        this.getTask()
    },
}
</script>
  