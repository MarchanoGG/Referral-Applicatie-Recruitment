<template>
  <div class="q-px-lg q-pb-md">
    <q-timeline color="secondary">
      <q-timeline-entry heading>
        <!-- Total Points: 100 XP -->
      </q-timeline-entry>

      <q-timeline-entry v-for="item in referrals" :key="item.object_key" subtitle="February 22, 2022">
        <div>
          Award 1 received after completing Task {{ item.task.name }}
        </div>
      </q-timeline-entry>

      <!-- <q-timeline-entry title="Award 2" subtitle="February 21, 2022" color="green" icon="done_all">
        <div>
          Award 2 received after completing Task 2
        </div>
      </q-timeline-entry>

      <q-timeline-entry title="Award 3" subtitle="February 21, 2022" color="green" icon="done_all">
        <div>
          Award 3 received after completing Task 3
        </div>
      </q-timeline-entry>

      <q-timeline-entry title="Award 4" subtitle="February 21, 2022" color="green" icon="done_all">
        <div>
          Award 4 received after completing Task 4
        </div>
      </q-timeline-entry>

      <q-timeline-entry title="Award 5" subtitle="February 21, 2022" color="green" icon="done_all">
        <div>
          Award 5 received after completing Task 5
        </div>
      </q-timeline-entry> -->
    </q-timeline>
  </div>
</template>

<script>

import { defineComponent } from 'vue'
import { useReferralStore } from "stores/referral";

export default defineComponent({
  setup() {
    const referralStore = useReferralStore();
    return { referralStore };
  },
  data() {
    const referrals = this.referralStore.allReferralByUser();
    return {
      referrals: referrals
    }
  }
})
</script>
