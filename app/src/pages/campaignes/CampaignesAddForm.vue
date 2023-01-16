<template>
    <div class="q-pa-md">
        <q-form>
            <q-stepper v-model="step" ref="stepper" color="primary" animated>
                <q-step :name="1" title="Scoreboard info" icon="settings" :done="step > 1"
                    :error="referralStore.scoreboardStore.has_errors">
                    <div class="row">
                        <div class="col-12 q-mb-lg">
                            <div class="text-h6 q-mx-md">Create Scoreboards</div>
                        </div>
                        <div class="col-12">
                            <!-- <q-select v-model="selected_item.scoreboard" :options="scoreboardrows"
                            option-value="object_key" option-label="name" /> -->
                            <div class="row">
                                <div class="col-4 q-mx-md">
                                    <q-input filled v-model="referralStore.scoreboardStore.selected_item.name"
                                        label="Your username *" hint="Userame" lazy-rules
                                        :rules="[val => val && val.length > 0 || 'Please type something']" />
                                </div>
                                <div class="col-3 q-mx-md">
                                    <q-date v-model="referralStore.scoreboardStore.selected_item.start_dt"
                                        subtitle="Start Date" />
                                </div>
                                <div class="col-3 q-mx-md">
                                    <q-date v-model="referralStore.scoreboardStore.selected_item.end_dt"
                                        subtitle="End Date" />
                                </div>
                            </div>
                        </div>
                    </div>
                </q-step>

                <q-step :name="2" title="Recruiter | Candidate" icon="create_new_folder" :done="step > 2">
                    <div class="row">
                        <div class="col-4 q-mx-md">
                            <div class="text-h6 q-mx-md">Select Recruiter</div>
                            <q-select v-model="referralStore.current_item.fk_user" :options="recruiterrows"
                                option-value="object_key" option-label="username" :multiple="true" :use-chips="true" />
                        </div>
                        <div class="col-4 q-mx-md">
                            <div class="text-h6 q-mx-md">Select Candidate</div>
                            <q-select v-model="referralStore.current_item.fk_candidate" :options="candidaterows"
                                option-value="object_key" :option-label="profilefullname" :multiple="true"
                                :use-chips="true" />
                        </div>
                    </div>
                </q-step>

                <q-step :name="3" title="Tasks" icon="add_comment" :done="step > 3">
                    <div class="row">
                        <div class="col-4 q-mx-md">
                            <div class="text-h6 q-mx-md">Select Tasks</div>
                            <q-select v-model="referralStore.current_item.fk_task" :options="taskrows"
                                option-value="object_key" option-label="name" :multiple="true" :use-chips="true" />
                        </div>
                    </div>
                </q-step>
                <q-step :name="4" title="Finish" icon="add_comment">
                    <div class="text-h6 q-mx-md">Finished configuring scoreboard?</div>
                </q-step>
                <template v-slot:navigation>
                    <q-stepper-navigation>
                        <q-btn v-if="step == 1"
                            @click="referralStore.scoreboardStore.addItem(); test(); $refs.stepper.next()"
                            color="primary" label="Continue" />
                        <q-btn v-if="step == 2" @click="$refs.stepper.next()" color="primary" label="Continue" />
                        <q-btn v-if="step == 3" @click="$refs.stepper.next()" color="primary" label="Continue" />
                        <q-btn v-if="step == 4" @click="test(); referralStore.addReferral();" color="primary"
                            label="Finish" />

                        <q-btn v-if="step == 1" flat color="primary" href="/scoreboards" label="Back" class="q-ml-sm" />
                        <q-btn v-if="step == 2" flat color="primary" @click="$refs.stepper.previous()" label="Back"
                            class="q-ml-sm" />
                        <q-btn v-if="step == 3" flat color="primary" @click="$refs.stepper.previous()" label="Back"
                            class="q-ml-sm" />
                        <q-btn v-if="step == 4" flat color="primary" @click="$refs.stepper.previous()" label="Back"
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
import { useUserStore } from "stores/user";
import { useScoreboardStore } from "stores/scoreboard";
import { useReferralStore } from "stores/referral";

export default {
    name: 'CampaignesAddForm',
    setup() {
        const userStore = useUserStore();
        const scoreboardStore = useScoreboardStore();
        const referralStore = useReferralStore();
        return { userStore, scoreboardStore, referralStore };
    },
    data() {
        const default_item = {
            scoreboard: this.scoreboardStore.default_item,
            recruiter: null,
            candidate: null,
            task: null,
            object_key: null,
        }
        return {
            step: ref(1),
            rows: [],
            recruiterrows: [],
            scoreboardrows: this.scoreboardStore.all().items,
            candidaterows: [],
            taskrows: [],
            isPwd: false,
            default_item: default_item,
            selected_item: default_item,
        }
    },
    methods: {
        test() {
            console.log(this.referralStore.scoreboardStore.selected_item)
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
