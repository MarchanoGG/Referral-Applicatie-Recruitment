<template>
    <q-page class="q-pa-md">
        <div class="row">
            <div class="col-3 q-mx-md" v-for="item in scoreboardStore.items" :key="item.object_key">
                <div class="">
                    <q-toolbar class="bg-primary text-white shadow-2">
                        <q-toolbar-title>Scoreboard: {{ item.name }}</q-toolbar-title>
                    </q-toolbar>

                    <q-list bordered>
                        <q-item v-for="subitem in item.ranklist" :key="subitem.object_key" class="q-my-sm" clickable
                            v-ripple>
                            <q-item-section avatar>
                                <q-avatar color="primary" text-color="white">
                                    {{ subitem.totalPoints }}
                                </q-avatar>
                            </q-item-section>

                            <q-item-section>
                                <q-item-label>{{ subitem.username }}</q-item-label>
                                <!-- <q-item-label caption lines="1">{{ subitem.email }}</q-item-label> -->
                            </q-item-section>

                            <!-- <q-item-section side>
                                <q-icon name="chat_bubble" color="green" />
                            </q-item-section> -->
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
import { useScoreboardStore } from "stores/scoreboard";

export default defineComponent({
    setup() {
        const scoreboardStore = useScoreboardStore();
        const referralStore = useReferralStore();
        scoreboardStore.allByUser()
        return { scoreboardStore, referralStore };
    },
    data() {
        return {
        }
    }
})
</script>
