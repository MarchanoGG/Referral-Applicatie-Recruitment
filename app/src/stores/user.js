import { defineStore } from "pinia";
import { useRouter } from "vue-router";
import { api } from "boot/axios";

export const useUserStore = defineStore("user", {
  state: () => ({
    user: JSON.parse(localStorage.getItem("user")),
    router: useRouter(),
  }),
  getters: {
    currentUser: (state) => state.user,
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
        this.router.push("/");
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
  },
});
