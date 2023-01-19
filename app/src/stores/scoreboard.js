import { defineStore } from "pinia";
import { useRouter } from "vue-router";
import { api } from "boot/axios";
import { useUserStore } from "stores/user";
import { useReferralStore } from "stores/referral";

export const useScoreboardStore = defineStore("scoreboard", {
  state: () => ({
    router: useRouter(),
    userStore: useUserStore(),
    referralStore: useReferralStore(),
    default_item: {
      name: null,
      object_key: null,
      fk_user: useUserStore().user.object_key,
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
    start_to_end: {
      from: new Date().toLocaleDateString(),
      to: new Date().toLocaleDateString(),
    },
    items: [],
    has_errors: false,
    last_res: null,
  }),
  getters: {},
  actions: {
    focusItem(item) {
      this.selected_item = { ...this.selected_item, ...item };
      this.start_to_end.from = this.selected_item.start_dt_str;
      this.start_to_end.to = this.selected_item.end_dt_str;
    },
    formItem() {
      this.selected_item.start_dt_str = this.start_to_end.from;
      this.selected_item.end_dt_str = this.start_to_end.to;
      return this.selected_item;
    },
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
      this.last_res = await api.post("/Scoreboards", this.formItem());
      if (this.last_res.status == 200) {
        this.selected_item = this.last_res?.data[0];
        this.all();
        this.resetItem();
      } else {
        this.has_errors = true;
      }
    },
    async editItem() {
      this.last_res = await api.put("/Scoreboards", this.formItem());
      if (this.last_res.status == 200) {
        this.all();
        this.resetItem();
      } else {
        this.has_errors = true;
      }
    },
    async deleteItem() {
      this.last_res = await api.delete("/Scoreboards", {
        params: { object_key: this.selected_item.object_key },
      });
      if (this.last_res.status == 200) {
        this.all();
        this.resetItem();
      } else {
        this.has_errors = true;
      }
    },
    resetItem() {
      this.selected_item = this.default_item;
      this.has_errors = false;
    },
    dateRange() {
      return this.selected_item.start_dt;
    },
  },
});
