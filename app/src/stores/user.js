import { defineStore } from "pinia";
import { useRouter } from "vue-router";
import { api } from "boot/axios";

const router = useRouter();

export const useUserStore = defineStore("user", {
  state: () => ({
    user: null,
  }),
  getters: {
    currentUser: (state) => state.user,
  },
  actions: {
    async fetchUser() {
      const res = await await api.get("/Users", {
        token: localStorage.getItem("token"),
      });

      this.user = await res.data[0];
    },
    retrieveUser() {
      return this.user;
    },
    async signIn(username, password) {
      const res = await api.post("/Authentication", {
        username: username,
        password: password,
      });
      this.user = await res.data[0];
      localStorage.setItem("username", username);
      localStorage.setItem("isAdmin", this.user.recruiter);
      return res;
    },
  },
});
