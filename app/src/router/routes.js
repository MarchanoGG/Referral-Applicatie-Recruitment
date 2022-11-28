import MainLayoutVue from "src/layouts/MainLayout.vue";
import IndexPage from "src/pages/IndexPage.vue";
const routes = [
  {
    path: "/",
    component: MainLayoutVue,
    children: [{ path: "", component: IndexPage }],
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
    path: "/campagnes",
    component: MainLayoutVue,
    children: [
      {
        path: "",
        component: () => import("src/pages/campagnes/CampagnesList.vue"),
      },
      {
        path: "add",
        component: () => import("src/pages/campagnes/CampagnesAddForm.vue"),
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
  // Always leave this as last one,
  // but you can also remove it
  {
    path: "/:catchAll(.*)*",
    component: () => import("pages/ErrorNotFound.vue"),
  },
];

export default routes;
