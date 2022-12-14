import { createRouter, createWebHistory } from 'vue-router'
import HomeView from '../views/HomeView.vue'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'home',
      component: HomeView
    },
    {
      path: '/about',
      name: 'about',
      // route level code-splitting
      // this generates a separate chunk (About.[hash].js) for this route
      // which is lazy-loaded when the route is visited.
      component: () => import('../views/AboutView.vue')
    },
    {
      path: '/login',
      name: 'Login',
      component: function () {
        return import(/* webpackChunkName: "about" */ '../views/Login.vue')
      }
    },
    {
      path: '/users',
      name: 'Users',
      component: function () {
        return import(/* webpackChunkName: "about" */ '../views/users/Users.vue')
      }
    },
    {
      path: '/secure',
      name: 'Secure',
      component: function () {
        return import(/* webpackChunkName: "about" */ '../views/Secure.vue')
      }
    },
    {
      path: '/help',
      name: 'Help',
      component: function () {
        return import(/* webpackChunkName: "about" */ '../views/HelpView.vue')
      }
    },
    {
      path: '/leaderboard',
      name: 'Leaderboard',
      component: function () {
        return import(/* webpackChunkName: "about" */ '../views/LeaderboardView.vue')
      }
    },

  ]
})

export default router
