import { defineStore } from "pinia";
import { useRouter } from "vue-router";
import { api } from "boot/axios";
import { defineComponent, ref, reactive, computed } from "vue";

export const useUserStore = defineStore("user", {
  state: () => ({
    user: JSON.parse(localStorage.getItem("user")),
    router: useRouter(),
    default_item: {
      username: null,
      password: null,
      recruiter: 0,
      recruiter_bool: false,
      recruiter_str: "",
      fk_profile: null,
      object_key: null,
      profile: {
        initials: null,
        name: null,
        surname: null,
        email: null,
        phone_number: null,
        address: null,
      },
    },
    selected_item: {
      username: null,
      password: null,
      recruiter: 0,
      recruiter_bool: false,
      recruiter_str: "",
      fk_profile: null,
      object_key: null,
      profile: {
        initials: null,
        name: null,
        surname: null,
        email: null,
        phone_number: null,
        address: null,
      },
    },
    items: [],
    has_errors: false,
    last_res: null,
  }),
  getters: {
    currentUser: (state) => state.user,
    tablerows: (state) => state.items.map(state.readyRowItem),
  },
  actions: {
    async fetchUser() {
      const res = await await api.get("/Users", {
        object_key: this.user?.object_key,
      });

      const user = res?.data[0];
      this.user = user;
    },
    async signIn(username, password) {
      const res = await api.post("/Authentication", {
        username: username,
        password: password,
      });
      const user = await res?.data[0];
      localStorage.setItem("user", JSON.stringify(user));
      this.user = user;
      if (user?.recruiter == 1) {
        this.router.push("admin");
      } else {
        this.router.push("dashboard");
      }
      // this.router.push(this.returnUrl || "/");
    },
    logout() {
      this.user = null;
      localStorage.removeItem("user");
      this.router.push("/login");
    },
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
      this.last_res = await api.get("/Users");
      if (this.last_res.status == 200) {
        this.items = this.last_res?.data;
      } else {
        this.has_errors = true;
      }
    },
    async getUserByScoreboardId(id) {
      this.last_res = await api.get("/Users", {
        params: { fk_scoreboard: id },
      });
      if (this.last_res.status == 200) {
        this.items = this.last_res?.data
      } else {
        this.has_errors = true;
      }
    },
    async allByUser() {
      this.last_res = await api.get("/Users", {
        params: { fk_user: this.userStore.user.object_key },
      });
      if (this.last_res.status == 200) {
        this.items = this.last_res?.data;
      } else {
        this.has_errors = true;
      }
    },
    async addItem() {
      this.last_res = await api.post("/Users", this.formItem());
      if (this.last_res.status == 200) {
        this.selected_item = this.last_res?.data[0];
        this.all();
        this.resetItem();
      } else {
        this.has_errors = true;
      }
    },
    async editItem() {
      this.last_res = await api.put("/Users", this.formItem());
      if (this.last_res.status == 200) {
        this.all();
        this.resetItem();
      } else {
        this.has_errors = true;
      }
    },
    async deleteItem() {
      this.last_res = await api.delete("/Users", {
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
