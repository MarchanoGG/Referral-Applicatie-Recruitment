<template>
    <q-page class="q-pa-md">
        <div class="row">
            <div class="col-3 q-mx-md" v-for="item in scoreboardStore.items" :key="item.object_key">
                <div class="">
                    <q-toolbar class="bg-primary text-white shadow-2">
                        <q-toolbar-title>Scoreboard: {{ item.scoreboard.name }}</q-toolbar-title>
                    </q-toolbar>

                    <q-list bordered>
                        <!-- <q-item v-for="item in referralStore.items" :key="item.object_key" class="q-my-sm" clickable
                            v-ripple>
                            <q-item-section avatar>
                                <q-avatar color="primary" text-color="white">
                                    {{ item.user.username }}
                                </q-avatar>
                            </q-item-section>

                            <q-item-section>
                                <q-item-label>{{ item.name }}</q-item-label>
                                <q-item-label caption lines="1">{{ item.email }}</q-item-label>
                            </q-item-section>

                            <q-item-section side>
                                <q-icon name="chat_bubble" color="green" />
                            </q-item-section>
                        </q-item> -->

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
import { useScoreboardStore } from "stores/scoreboard";

export default defineComponent({
    setup() {
        const scoreboardStore = useScoreboardStore();
        const referralStore = useReferralStore();
        // referralStore.allReferralByUser();
        // referralStore.allByScoreboard();
        scoreboardStore.allByUser()
        console.log(scoreboardStore.items)
        return { scoreboardStore, referralStore };
    },
    data() {
        return {
        }
    }
})
</script>
