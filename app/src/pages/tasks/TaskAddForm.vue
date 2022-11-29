<template>
    <div class="q-pa-md" style="max-width: 400px">

        <q-form @submit="onSubmit" @reset="onReset" class="q-gutter-md">
            <q-input filled v-model="name" label="Task name *" hint="Task name" lazy-rules
                :rules="[ val => val && val.length > 0 || 'Please type something']" />

            <q-input filled v-model="points" label="Points to earn *" hint="Points" lazy-rules
                :rules="[ val => val && val.length > 0 || 'Please type something']" />

            <q-input filled v-model="description" label="Task description" hint="Description" lazy-rules
                :rules="[ val => val && val.length > 0 || 'Please type something']" />

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
        const name = ref(null)
        const points = ref(null)
        const description = ref(null)

        return {
            name,
            points,
            description,

          onSubmit() {
            if (name.value !== null && points.value !== null && description.value !== null) {
              const userForm = reactive({
                name: name.value,
                points: points.value,
                description: description.value,
              });

              api.post('/Tasks', userForm)
                .then((response) => {
                  if (response.status == 200) {
                    router.push("/Tasks");
                  }
                })
                .catch(() => {
                })
            }
            },

            onReset() {
                name.value = null
                points.value = null
                description.value = false
            }
        }
    }
}
</script>
