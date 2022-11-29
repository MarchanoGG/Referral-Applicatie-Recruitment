<template>
    <div class="q-pa-md" style="max-width: 400px">

        <q-form @submit="onSubmit" @reset="onReset" class="q-gutter-md">
            <q-input filled v-model="fk_user" label="Your user key *" hint="User key" lazy-rules
                :rules="[ val => val && val.length > 0 || 'Please type something']" />

            <q-input filled v-model="name" label="Reward name *" hint="Name" lazy-rules
                :rules="[ val => val && val.length > 0 || 'Please type something']" />

            <q-date filled v-model="award_dt" title="Award date" subtitle lazy-rules />

            <div>
                <q-btn label="Submit" type="submit" color="primary" />
                <q-btn label="Reset" type="reset" color="primary" flat class="q-ml-sm" />
            </div>
        </q-form>

    </div>
</template>
  
<script>
  import { useRouter } from 'vue-router'
  import { ref, reactive } from 'vue'
  import { api } from 'boot/axios'


export default {
    setup() {
        const router = useRouter()
        const fk_user = ref(null)
        const name = ref(null)
        const award_dt = ref(Date.now)

        return {
            fk_user,
            name,
            award_dt,
          onSubmit() {
            if (fk_user.value !== null && name.value !== null && award_dt.value !== null) {
              const userForm = reactive({
                fk_user: fk_user.value,
                name: name.value,
                award_dt: award_dt.value,
              });

              api.post('/Rewards', userForm)
                .then((response) => {
                  if (response.status == 200) {
                    router.push("/Rewards");
                  }
                })
                .catch(() => {
                })
            }
            },
            onReset() {
                fk_user.value = null
                name.value = null
                award_dt.value = false
            }
        }
    }
}
</script>
