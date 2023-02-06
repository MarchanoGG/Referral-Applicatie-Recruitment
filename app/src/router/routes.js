import MainLayoutVue from "src/layouts/RecruiterLayout.vue";
import EmployeeLayoutVue from "src/layouts/EmployeeLayout.vue";
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
    path: "/logout",
    component: LoginLayoutVue,
    children: [
      { path: "", component: () => import("src/pages/LogoutPage.vue") },
    ],
  },
  // recruiter pages
  {
    path: "/admin",
    component: MainLayoutVue,
    children: [{ path: "", component: IndexPage }],
  },
  {
    path: "/admin/users",
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
    path: "/admin/candidates",
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
    path: "/admin/rewards",
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
    path: "/admin/tasks",
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
    path: "/admin/campaignes",
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
    path: "/admin/scoreboards",
    component: MainLayoutVue,
    children: [
      {
        path: "",
        component: () => import("src/pages/scoreboards/ScoreboardsList.vue"),
      },
      {
        path: "add",
        component: () => import("src/pages/scoreboards/ScoreboardsAddForm.vue"),
      },
    ],
    meta: {
      requiresAuth: true
    }
  },
  // employee pages
  {
    path: "/dashboard",
    component: EmployeeLayoutVue,
    children: [
      {
        path: "",
        component: () => import("src/pages/employees/employeeHome.vue"),
      },
      {
        path: "scoreboards",
        component: () => import("src/pages/employees/employeeScoreboards.vue"),
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
