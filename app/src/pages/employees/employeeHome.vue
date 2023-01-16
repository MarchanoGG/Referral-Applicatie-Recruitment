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
