<template>
  <q-page class="q-pa-md">
    <div class="row">
      <div class="col-4 q-mx-md">

        <q-timeline color="secondary">
          <q-timeline-entry heading>
            Total Points: {{ referralStore.items }}
          </q-timeline-entry>
          <div v-if="referralStore.items.length == 0">
            <q-banner class="bg-orange text-white">
              No data found. Data may not be available yet. Contact admin for more info
            </q-banner>
          </div>
          <q-timeline-entry v-for="item in referralStore.items" :key="item.object_key" :subtitle="item.modification_dt">
            <!-- <q-timeline-entry v-for="item in referralStore.items" :key="item.object_key" subtitle="February 22, 2022"> -->
            <div>
              Campaign: {{ item.scoreboard.name }}
            </div>
            <div>
              Task: {{ item.task.name }}
            </div>
            <div>
              Candidate: {{ item.candidate.profile.fullname }}
            </div>
          </q-timeline-entry>
        </q-timeline>
      </div>
      <div class="col-3 q-mx-md">
      </div>
      <div class="col-4 q-mx-md float-right">

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
    referralStore.allByUser();
    return { referralStore };
  },
  data() {
    return {
    }
  }
})
</script>
