<template>
    <div class="q-pa-md">
        <q-form>
            <div class="row">
                <div class="col-12 q-mb-lg">
                    <div class="text-h6 q-mx-md">Edit Scoreboard {{ ScorboardId }}</div>
                </div>
              <div class="col-4 q-mx-md">
                <q-input filled v-model="referralStore.scoreboardStore.selected_item.name" label="Your username *" hint="Userame"
                  lazy-rules :rules="[val => val && val.length > 0 || 'Please type something']" />
                <q-date v-model="referralStore.scoreboardStore.start_to_end" subtitle="Start Date" range minimal />
              </div>
            </div>
            <div class="row">
                <div class="col-4 q-mx-md">
                    <q-select v-model="referralStore.userStore.items" :options="recruiterrows"
                        option-value="object_key" option-label="username" :multiple="true" :use-chips="true" label="Select Recruiter"/>
                </div>
                <div class="col-4 q-mx-md">
                    <q-select v-model="referralStore.candidates" :options="candidaterows"
                        option-value="object_key" :option-label="profilefullname" :multiple="true" :use-chips="true" label="Select Candidate"/>
                </div>
            </div>

            <div class="row">
                <div class="col-4 q-mx-md">
                    <q-select v-model="referralStore.taskStore.items" :options="taskrows" option-value="object_key"
                        option-label="name" :multiple="true" :use-chips="true" label="Select Tasks" />
                </div>
            </div>
            <div class="row">
                <q-btn @click="referralStore.updateReferral();" color="primary" label="Save" />
                <q-btn flat color="primary" href="/admin/scoreboards" label="Back" class="q-ml-sm" />
            </div>
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
    name: 'ScoreboardsEditForm',
    props: ['ScorboardId'],
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
        addScoreboard() {
            this.referralStore.scoreboardStore.addItemSync().then((response) => {
                if (response.data[0] != null) {
                    this.referralStore.scoreboardStore.selected_item = response.data[0];
                }
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
        this.referralStore.scoreboardStore.getById(this.ScorboardId)
        this.referralStore.getAllById(this.ScorboardId)
        this.referralStore.taskStore.getByScoreboardId(this.ScorboardId)
        this.referralStore.userStore.getUserByScoreboardId(this.ScorboardId)
        this.getScoreboards()
        this.getRecruiter()
        this.getCandidate()
        this.getTask()
    },
}
</script>
