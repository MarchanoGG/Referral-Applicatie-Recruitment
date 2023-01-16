import { defineStore } from "pinia";
import { useRouter } from "vue-router";
import { api } from "boot/axios";
import { useUserStore } from "stores/user";
import { useScoreboardStore } from "stores/scoreboard";

export const useReferralStore = defineStore("referral", {
  state: () => ({
    scoreboardStore: useScoreboardStore(),
    userStore: useUserStore(),
    router: useRouter(),
    default_item: {
      object_key: null,
      fk_scoreboard: null,
      fk_user: null,
      fk_candidate: null,
      fk_task: null,
      creation_dt: new Date().toLocaleDateString(),
      modification_dt: new Date().toLocaleDateString(),
    },
    selected_item: {
      object_key: null,
      fk_scoreboard: null,
      fk_user: null,
      fk_candidate: null,
      fk_task: null,
      creation_dt: new Date().toLocaleDateString(),
      modification_dt: new Date().toLocaleDateString(),
    },
    current_item: {
      object_key: null,
      fk_scoreboard: null,
      fk_user: null,
      fk_candidate: null,
      fk_task: null,
      creation_dt: new Date().toLocaleDateString(),
      modification_dt: new Date().toLocaleDateString(),
    },
    items: [],
    has_errors: false,
    last_res: null,
  }),
  getters: {
    currentUser: (state) => state.user,
  },
  actions: {
    async all() {
      this.has_errors = false;
      this.last_res = await api.get("/Referrals");
      if (this.last_res.status == 200) {
        this.items = this.last_res?.data;
      } else {
        this.has_errors = true;
      }
    },
    async allReferralByUser() {
      this.has_errors = false;
      this.last_res = await api.get("/Referrals", {
        params: { fk_user: this.userStore.user.object_key },
      });
      if (this.last_res.status == 200) {
        this.items = this.last_res?.data;
      } else {
        has_errors = true;
      }
    },
    async addReferral() {
      this.current_item.fk_user.forEach((user) => {
        this.current_item.fk_candidate.forEach((candidate) => {
          this.current_item.fk_task.forEach((task) => {
            this.selected_item.fk_scoreboard =
              this.scoreboardStore.selected_item.object_key;
            this.selected_item.fk_user = user.object_key;
            this.selected_item.fk_candidate = candidate.object_key;
            this.selected_item.fk_task = task.object_key;
            this.addItem();
          });
        });
      });
    },
    async addItem() {
      this.has_errors = false;
      this.last_res = await api.post("/Referrals", this.selected_item);
      if (this.last_res.status == 200) {
      } else {
        this.has_errors = true;
      }
    },
  },
});
