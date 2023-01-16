import MainLayoutVue from "src/layouts/MainLayout.vue";
import IndexPage from "src/pages/IndexPage.vue";
import LoginLayoutVue from "src/layouts/LoginLayout.vue";

const routes = [
  {
    path: "/",
    component: MainLayoutVue,
    children: [{ path: "", component: IndexPage }],
  },
  {
    path: "/login",
    component: LoginLayoutVue,
    children: [
      { path: "", component: () => import("src/pages/LoginPage.vue") },
    ],
  },
  {
    path: "/users",
    component: MainLayoutVue,
    children: [
      { path: "", component: () => import("src/pages/users/UsersList.vue") },
      {
        path: "add",
        component: () => import("src/pages/users/UserAddForm.vue"),
      },
    ],
    meta: {
      requiresAuth: true
    }
  },
  {
    path: "/candidates",
    component: MainLayoutVue,
    children: [
      {
        path: "",
        component: () => import("src/pages/candidates/CandidatesList.vue"),
      },
      {
        path: "add",
        component: () => import("src/pages/candidates/CandidateAddForm.vue"),
      },
    ],
    meta: {
      requiresAuth: true
    }
  },
  {
    path: "/rewards",
    component: MainLayoutVue,
    children: [
      {
        path: "",
        component: () => import("src/pages/rewards/RewardsList.vue"),
      },
      {
        path: "add",
        component: () => import("src/pages/rewards/RewardAddForm.vue"),
      },
    ],
    meta: {
      requiresAuth: true
    }
  },
  {
    path: "/tasks",
    component: MainLayoutVue,
    children: [
      { path: "", component: () => import("src/pages/tasks/TasksList.vue") },
      {
        path: "add",
        component: () => import("src/pages/tasks/TaskAddForm.vue"),
      },
    ],
    meta: {
      requiresAuth: true
    }
  },
  {
    path: "/campaignes",
    component: MainLayoutVue,
    children: [
      {
        path: "",
        component: () => import("src/pages/campaignes/CampaignesList.vue"),
      },
      {
        path: "add",
        component: () => import("src/pages/campaignes/CampaignesAddForm.vue"),
      },
    ],
    meta: {
      requiresAuth: true
    }
  },
  {
    path: "/scoreboards",
    component: MainLayoutVue,
    children: [
      {
        path: "",
        component: () => import("src/pages/scoreboards/ScoreboardsList.vue"),
      },
      {
        path: "add",
        component: () => import("src/pages/scoreboards/ScoreboardAddForm.vue"),
      },
    ],
    meta: {
      requiresAuth: true
    }
  },
  {
    path: "/employees",
    component: MainLayoutVue,
    children: [
      {
        path: "",
        component: () => import("src/pages/employees/employeeHome.vue"),
      },
    ],
    meta: {
      requiresAuth: true
    }
  },
  // Always leave this as last one,
  // but you can also remove it
  {
    path: "/:catchAll(.*)*",
    component: () => import("pages/ErrorNotFound.vue"),
  },
];

export default routes;
