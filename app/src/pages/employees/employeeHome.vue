<template>
  <q-page class="q-pa-md">
    <div class="row">
      <div class="col-4 q-mx-md">
        <q-timeline color="secondary">
          <q-timeline-entry heading>
            <!-- Total Points: 100 XP -->
          </q-timeline-entry>

          <q-timeline-entry v-for="item in referralStore.items" :key="item.object_key" :subtitle="item.modification_dt">
            <!-- <q-timeline-entry v-for="item in referralStore.items" :key="item.object_key" subtitle="February 22, 2022"> -->
            <div>
              {{ item.task.name }}
            </div>
          </q-timeline-entry>
        </q-timeline>
      </div>
      <div class="col-3 q-mx-md">
      </div>
      <div class="col-4 q-mx-md float-right">
        <div class="">
          <q-toolbar class="bg-primary text-white shadow-2">
            <q-toolbar-title>Scoreboard: </q-toolbar-title>
          </q-toolbar>

          <q-list bordered>
            <q-item v-for="contact in contacts" :key="contact.id" class="q-my-sm" clickable v-ripple>
              <q-item-section avatar>
                <q-avatar color="primary" text-color="white">
                  {{ contact.letter }}
                </q-avatar>
              </q-item-section>

              <q-item-section>
                <q-item-label>{{ contact.name }}</q-item-label>
                <q-item-label caption lines="1">{{ contact.email }}</q-item-label>
              </q-item-section>

              <q-item-section side>
                <q-icon name="chat_bubble" color="green" />
              </q-item-section>
            </q-item>

            <q-separator />
            <q-item-label header>Offline</q-item-label>

            <q-item v-for="contact in offline" :key="contact.id" class="q-mb-sm" clickable v-ripple>
              <q-item-section avatar>
                <q-avatar>
                  <img :src="`https://cdn.quasar.dev/img/${contact.avatar}`">
                </q-avatar>
              </q-item-section>

              <q-item-section>
                <q-item-label>{{ contact.name }}</q-item-label>
                <q-item-label caption lines="1">{{ contact.email }}</q-item-label>
              </q-item-section>

              <q-item-section side>
                <q-icon name="chat_bubble" color="grey" />
              </q-item-section>
            </q-item>
          </q-list>
        </div>
      </div>
    </div>

  </q-page>
</template>

<script>

import { defineComponent } from 'vue'
import { useReferralStore } from "stores/referral";

export default defineComponent({
  setup() {
    const referralStore = useReferralStore();
    referralStore.allReferralByUser();
    return { referralStore };
  },
  data() {
    return {
    }
  }
})
</script>
