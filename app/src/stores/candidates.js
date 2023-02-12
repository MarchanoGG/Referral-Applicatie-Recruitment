import { defineStore } from "pinia";
import { useRouter } from "vue-router";
import { api } from "boot/axios";
import { defineComponent, ref, reactive, computed } from "vue";

export const useCandidateStore = defineStore("candidate", {
  state: () => ({
    router: useRouter(),
    default_item: {
      fk_profile: null,
      object_key: null,
      profile: {
        initials: null,
        name: null,
        surname: null,
        email: null,
        phone_number: null,
        address: null,
      }
    },
    selected_item: {
      fk_profile: null,
      object_key: null,
      profile: {
        initials: null,
        name: null,
        surname: null,
        email: null,
        phone_number: null,
        address: null,
      }
    },
    items: [],
    has_errors: false,
    last_res: null,
  }),
  getters: {
    tablerows: (state) => state.items.map(state.readyRowItem),
  },
  actions: {
    readyRowItem(item) {
      item.recruiter_bool = computed({
        get: () => Boolean(item.recruiter),
        set: (val) => {
          item.recruiter = Number(val);
        },
      });
      item.recruiter_str = computed({
        get: () => (item.recruiter ? "Yes" : "No"),
        set: (val) => {
          item.recruiter = Number(val);
        },
      });
      return item;
    },
    focusItem(item) {
      this.selected_item = { ...this.selected_item, ...item };
    },
    formItem() {
      return this.selected_item;
    },
    async all() {
      this.last_res = await api.get("/Candidates");
      if (this.last_res.status == 200) {
        this.items = this.last_res?.data;
      } else {
        this.has_errors = true;
      }
    },
    async getCandidateByScoreboardId(id) {
      this.last_res = await api.get("/Candidates", {
        params: { fk_scoreboard: id },
      });
      if (this.last_res.status == 200) {
        this.items = this.last_res?.data
      } else {
        this.has_errors = true;
      }
    },
    async addItem() {
      this.last_res = await api.post("/Candidates", this.formItem());
      if (this.last_res.status == 200) {
        this.selected_item = this.last_res?.data[0];
        this.all();
        this.resetItem();
      } else {
        this.has_errors = true;
      }
    },
    async editItem() {
      this.last_res = await api.put("/Candidates", this.formItem());
      if (this.last_res.status == 200) {
        this.all();
        this.resetItem();
      } else {
        this.has_errors = true;
      }
    },
    async deleteItem() {
      this.last_res = await api.delete("/Candidates", {
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
      this.selected_item = this.readyRowItem(this.default_item);
      this.has_errors = false;
    },
    dateRange() {
      return this.selected_item.start_dt;
    },
  },
});
