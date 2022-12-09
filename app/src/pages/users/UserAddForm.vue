<template>
    <q-form @submit="onSubmit" @reset="onReset">
        <div class="row">
            <div class="col-5">
                <q-input filled v-model="name" label="Your name *" hint="Name and surname" lazy-rules
                    :rules="[val => val && val.length > 0 || 'Please type something']" />

                <q-input filled v-model="surname" label="Your surname *" hint="Surname" lazy-rules
                    :rules="[val => val && val.length > 0 || 'Please type something']" />

                <q-input filled v-model="email" label="Your email *" hint="Email" lazy-rules
                    :rules="[val => val && val.length > 0 || 'Please type something']" />

                <q-input filled v-model="phonenumber" label="Your Phone number *" hint="Phone number" lazy-rules
                    :rules="[val => val && val.length > 0 || 'Please type something']" />
                <q-toggle v-model="accept" label="I accept the license and terms" />
            </div>
            <div class="col-1" />

            <div class="col-5">
                <q-date filled v-model="age" title="Birthdate" subtitle lazy-rules />
            </div>

            <div>
                <q-btn label="Submit" type="submit" color="primary" />
                <q-btn label="Reset" type="reset" color="primary" flat class="q-ml-sm" />
            </div>
        </div>
    </q-form>
</template>
  
<script>
import { useQuasar } from 'quasar'
import { ref } from 'vue'

export default {
    setup() {
        const $q = useQuasar()

        const name = ref(null)
        const surname = ref(null)
        const email = ref(null)
        const phonenumber = ref(null)
        const age = ref('1980/01/01')
        const accept = ref(false)

        return {
            name,
            surname,
            email,
            phonenumber,
            age,
            accept,

            onSubmit() {
                if (accept.value !== true) {
                    $q.notify({
                        color: 'red-5',
                        textColor: 'white',
                        icon: 'warning',
                        message: 'You need to accept the license and terms first'
                    })
                }
                else {
                    $q.notify({
                        color: 'green-4',
                        textColor: 'white',
                        icon: 'cloud_done',
                        message: 'Submitted'
                    })
                }
            },

            onReset() {
                name.value = null
                age.value = null
                accept.value = false
            },

            setCalendarTo() {
                year = '1980'
                month = '1'
            }
        }
    }
}
</script>
  