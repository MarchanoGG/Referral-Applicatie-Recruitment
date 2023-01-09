import MainLayoutVue from "src/layouts/RecruiterLayout.vue";
import IndexPage from "src/pages/IndexPage.vue";
import LoginLayoutVue from "src/layouts/LoginLayout.vue";

const routes = [
  {
    path: "/",
    component: MainLayoutVue,
    b: [{ path: "", component: IndexPage }],
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
  },
  // Always leave this as last one,
  // but you can also remove it
  {
    path: "/:catchAll(.*)*",
    component: () => import("pages/ErrorNotFound.vue"),
  },
];

export default routes;
