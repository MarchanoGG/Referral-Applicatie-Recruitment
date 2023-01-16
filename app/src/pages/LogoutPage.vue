<template>
  <q-page class="">
    <div class="row flex justify-center">
      <div class="col-6 q-mt-xl logo-section">
        <div class="col-4 text-center">
          <div>
            <img alt="Alten logo"
              src="https://www.alten.com/wp-content/uploads/2019/01/cropped-favicon-alten-1-180x180.png"
              style="width: 200px; height: 200px">
            <h3 class="text-h3">Alten</h3>
          </div>
        </div>
      </div>
      <div class="col-6 q-mt-xl">
        <div class="row flex justify-center">
          <div class="col-6">
            <!-- {{ userStore.user.profile.name }} Logging out -->
          </div>
        </div>
      </div>
    </div>
  </q-page>
</template>
<script>
import { defineComponent, ref, reactive, computed } from 'vue'
import { useUserStore } from "stores/user";

export default defineComponent({
  name: 'LogoutPage'
  , setup() {
    const userStore = useUserStore();
    userStore.logout()
  }
})
</script>
