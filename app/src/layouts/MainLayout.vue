<template>
  <q-layout view="lHh Lpr lFf">
    <q-header elevated>
      <q-toolbar>
        <q-btn flat dense round icon="menu" aria-label="Menu" @click="toggleLeftDrawer" />

        <q-toolbar-title>
          Alten
        </q-toolbar-title>

        <!-- <div>Quasar v{{ $q.version }}</div> -->
      </q-toolbar>
    </q-header>

    <q-drawer v-model="leftDrawerOpen" show-if-above bordered>
      <q-list>
        <q-item-label header>
          Menu
        </q-item-label>

        <EssentialLink v-for="link in essentialLinks" :key="link.title" v-bind="link" />
      </q-list>
    </q-drawer>

    <q-page-container>
      <router-view />
    </q-page-container>
  </q-layout>
</template>

<script>
import { defineComponent, ref } from 'vue'
import EssentialLink from 'components/EssentialLink.vue'

const linksList = [
  {
    title: 'Home',
    // caption: '',
    icon: 'home',
    link: '/'
  },
  {
    title: 'Login',
    // caption: '',
    icon: 'login',
    link: '/login'
  },
  {
    title: 'Users',
    // caption: '',
    icon: 'manage_accounts',
    link: '/users'
  },
  {
    title: 'Candidates',
    // caption: '',
    icon: 'people',
    link: '/candidates'
  },
  {
    title: 'Rewards',
    // caption: '',
    icon: 'emoji_events',
    link: '/rewards'
  },
  {
    title: 'Tasks',
    // caption: '',
    icon: 'list',
    link: '/tasks'
  },
  {
    title: 'Campaignes',
    // caption: '',
    icon: 'checklist',
    link: '/campaignes'
  },
  {
    title: 'Scoreboards',
    // caption: '',
    icon: 'leaderboard',
    link: '/scoreboards'
  },
  {
    title: 'Employees',
    // caption: '',
    icon: 'person',
    link: '/employees'
  },
]

export default defineComponent({
  name: 'MainLayout',

  components: {
    EssentialLink
  },

  setup() {
    const leftDrawerOpen = ref(false)

    return {
      essentialLinks: linksList,
      leftDrawerOpen,
      toggleLeftDrawer() {
        leftDrawerOpen.value = !leftDrawerOpen.value
      }
    }
  }
})
</script>
