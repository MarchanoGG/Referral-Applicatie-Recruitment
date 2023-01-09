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

      const user = res.data[0];
      this.user = user;
    },
    async signIn(username, password) {
      const res = await api.post("/Authentication", {
        username: username,
        password: password,
      });
      const user = await res.data[0];
      // console.log(user);
      this.user = user;
      localStorage.setItem("token", user.sessiontoken);
    },
  },
});
