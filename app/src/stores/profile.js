import { defineStore } from "pinia";
import { useRouter } from "vue-router";
import { api } from "boot/axios";

export const useProfileStore = defineStore("profile", {
  state: () => ({
    router: useRouter(),
    default_item: {
      initials: null,
      name: null,
      surname: null,
      email: null,
      phone_number: null,
      address: null,
    },
    selected_item: {
      initials: null,
      name: null,
      surname: null,
      email: null,
      phone_number: null,
      address: null,
    },
    items: [],
    has_errors: false,
    last_res: null,
  }),
  getters: {},
  actions: {
    focusItem(item) {
      this.selected_item = { ...this.selected_item, ...item };
    },
    formItem() {
      return this.selected_item;
    },
    async all() {
      this.last_res = await api.get("/Profiles");
      if (this.last_res.status == 200) {
        this.items = this.last_res?.data;
      } else {
        this.has_errors = true;
      }
    },
    async addItem() {
      this.last_res = await api.post("/Profiles", this.formItem());
      if (this.last_res.status == 200) {
        this.selected_item = this.last_res?.data[0];
        this.all();
        this.resetItem();
      } else {
        this.has_errors = true;
      }
    },
    async editItem() {
      this.last_res = await api.put("/Profiles", this.formItem());
      if (this.last_res.status == 200) {
        this.all();
        this.resetItem();
      } else {
        this.has_errors = true;
      }
    },
    async deleteItem() {
      this.last_res = await api.delete("/Profiles", {
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
      this.start_to_end = {
        from: new Date().toLocaleDateString(),
        to: new Date().toLocaleDateString(),
      };
    },
    dateRange() {
      return this.selected_item.start_dt;
    },
  },
});
