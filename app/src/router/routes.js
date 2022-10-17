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
      { path: "", component: () => import("src/pages/users/List.vue") },
      {
        path: "add",
        component: () => import("src/pages/users/UserAddForm.vue"),
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
