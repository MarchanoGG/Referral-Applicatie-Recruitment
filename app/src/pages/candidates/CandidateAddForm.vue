<template>
    <div class="q-pa-md" style="max-width: 400px">

        <q-form @submit="onSubmit" @reset="onReset" class="q-gutter-md">
            <q-input filled v-model="fk_profile" label="Your profile key *" hint="Profile key" lazy-rules
                :rules="[ val => val && val.length > 0 || 'Please type something']" />

            <q-date filled v-model="referred_at" title="Referred at" subtitle lazy-rules />

            <div>
                <q-btn label="Submit" type="submit" color="primary" />
                <q-btn label="Reset" type="reset" color="primary" flat class="q-ml-sm" />
            </div>
        </q-form>

    </div>
</template>
  
<script>
  import { useQuasar } from 'quasar'
  import { useRouter } from 'vue-router'
  import { ref, reactive } from 'vue'
  import { api } from 'boot/axios'


export default {
    setup() {
      const router = useRouter()

    const fk_profile = ref(null)
    const referred_at = ref(Date.now)

        return {
            fk_profile,
            referred_at,

            onSubmit() {
              if (fk_profile.value !== null && referred_at.value !== null) {
                const userForm = reactive({
                  fk_profile: fk_profile.value,
                  referred_at: referred_at.value,
                });

                api.post('/Candidates', userForm)
                  .then((response) => {
                    if (response.status == 200) {
                      console.log(1);
                      router.push("/Candidates");
                    }
                    else {
                      console.log(2);
                    }
                  })
                  .catch(() => {
                  })
              }
              else {
                console.log(3);
              }
            },

            onReset() {
                fk_profile.value = null
                referred_at.value = null
            }
        }
    }
}
</script>
