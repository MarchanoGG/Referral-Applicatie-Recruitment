import { defineStore } from "pinia";
import { useRouter } from "vue-router";
import { api } from "boot/axios";
import { useUserStore } from "stores/user";

export const useScoreboardStore = defineStore("scoreboard", {
  state: () => ({
    router: useRouter(),
    userStore: useUserStore(),
    default_item: {
      name: null,
      object_key: null,
      start_dt: new Date().toLocaleDateString(),
      end_dt: new Date().toLocaleDateString(),
    },
    selected_item: {
      name: null,
      object_key: null,
      fk_user: useUserStore().user.object_key,
      start_dt: new Date().toLocaleDateString(),
      end_dt: new Date().toLocaleDateString(),
    },
    items: [],
    has_errors: false,
    last_res: null,
  }),
  actions: {
    async all() {
      this.last_res = await api.get("/Scoreboards");
      if (this.last_res.status == 200) {
        this.items = this.last_res?.data;
      } else {
        this.has_errors = true;
      }
    },
    async allByUser() {
      this.last_res = await api.get("/Scoreboards", {
        params: { fk_user: this.userStore.user.object_key },
      });
      if (this.last_res.status == 200) {
        this.items = this.last_res?.data;
      } else {
        this.has_errors = true;
      }
    },
    async addItem() {
      this.last_res = await api.post("/Scoreboards", this.selected_item);
      if (this.last_res.status == 200) {
        this.selected_item = this.last_res?.data[0];
      } else {
        this.has_errors = true;
      }
    },
    resetItem() {
      this.selected_item = this.default_item;
    },
  },
});
