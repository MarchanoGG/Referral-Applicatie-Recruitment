import { defineStore } from "pinia";
import { useRouter } from "vue-router";
import { api } from "boot/axios";
import { useUserStore } from "stores/user";

export const useTaskStore = defineStore("tasks", {
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
      start_dt: new Date().toLocaleDateString(),
      end_dt: new Date().toLocaleDateString(),
    },
    items: [],
    has_errors: false,
    last_res: null,
    addform: false,
    editform: false,
    delform: false,
  }),
  actions: {
    async all() {
      this.last_res = await api.get("/Tasks");
      if (this.last_res.status == 200) {
        this.items = this.last_res?.data;
      } else {
        errors = true;
      }
    },
    async addItem() {
      this.last_res = await api.post("/Tasks", this.selected_item);
      if (this.last_res.status == 200) {
        this.selected_item = this.last_res?.data[0];
        this.all();
        this.resetItem();
        this.addform = false;
      } else {
        errors = true;
      }
    },
    async editItem() {
      this.last_res = await api.put("/Tasks", this.selected_item);
      if (this.last_res.status == 200) {
        this.selected_item = this.last_res?.data[0];
        this.all();
        this.resetItem();
        this.editform = false;
      } else {
        errors = true;
      }
    },
    async deleteItem() {
      this.last_res = await api.delete("/Tasks?object_key=" + this.selected_item.object_key, this.selected_item);
      if (this.last_res.status == 200) {
        this.selected_item = this.last_res?.data[0];
        this.all();
        this.resetItem();
        this.delform = false;
      } else {
        errors = true;
      }
    },
    resetItem() {
      this.selected_item = this.default_item;
    },
  },
});
