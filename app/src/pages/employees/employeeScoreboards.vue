<template>
    <q-page class="q-pa-md">
        <div class="row">
            <div class="col-3 q-mx-md" v-if="referralStore.items.length == 0">
                <q-banner class="bg-orange text-white">
                No Campaignes joined. Contact admin for more info or to apply to Campaign
                </q-banner>
            </div>
            <div class="col-3 q-mx-md" v-for="item in scoreboardStore.items" :key="item.object_key">
                <div class="">
                    <q-toolbar class="bg-primary text-white shadow-2">
                        <q-toolbar-title>Scoreboard: {{ item.name }}</q-toolbar-title>
                    </q-toolbar>
                    <q-list bordered>
                        <q-item v-for="subitem in getRanklist(item)" :key="subitem.object_key" class="q-my-sm">
                            <q-item-section>
                                <q-item-label>{{ subitem.username }}</q-item-label>
                            </q-item-section>
                            <q-item-section side>
                                {{ subitem.totalPoints }}
                            </q-item-section>
                            <q-item-section side>
                                <q-icon name="star" color="green" />
                            </q-item-section>
                        </q-item>

                        <q-separator />

                    </q-list>
                </div>
            </div>
        </div>

    </q-page>
</template>

<script>

import { defineComponent } from 'vue'
import { useReferralStore } from "stores/referral";
import { useUserStore } from "stores/user";
import { useScoreboardStore } from "stores/scoreboard";
import { api } from "boot/axios";

export default defineComponent({
    setup() {
        const scoreboardStore = useScoreboardStore();
        const referralStore = useReferralStore();
        const userStore = useUserStore();
        scoreboardStore.allByUser()
        return { scoreboardStore, referralStore, userStore };
    },
    data() {
        return {
        }
    },
    methods: {
        getRanklist(item) {
            api.get("/Users", {
                    params: { fk_scoreboard: item.object_key },
                })
                .then((response) => {
                    item.ranklist = response?.data
                })
                .catch(() => { });
            return item.ranklist
        }
    }
})
</script>
