<template>
  <q-layout view="lHh Lpr lFf">
    <q-header elevated>
      <q-toolbar>
        <q-btn flat dense round icon="menu" aria-label="Menu" @click="toggleLeftDrawer" />

        <q-toolbar-title>
          Alten | {{ userStore.user.profile.name }}
        </q-toolbar-title>

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
import { useUserStore } from "stores/user";

const linksList = [
  {
    title: 'Home',
    // caption: '',
    icon: 'home',
    link: '/'
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
    title: 'Scoreboards',
    // caption: '',
    icon: 'leaderboard',
    link: '/scoreboards'
  },
  {
    title: 'Campaignes',
    // caption: '',
    icon: 'checklist',
    link: '/campaignes'
  },
  {
    title: 'Logout',
    // caption: '',
    icon: 'logout',
    link: '/logout'
  },
]

export default defineComponent({
  name: 'RecruiterLayout',

  components: {
    EssentialLink
  },

  setup() {
    const leftDrawerOpen = ref(false)
    const userStore = useUserStore();

    return {
      essentialLinks: linksList,
      leftDrawerOpen,
      userStore,
      toggleLeftDrawer() {
        leftDrawerOpen.value = !leftDrawerOpen.value
      }
    }
  }
})
</script>
