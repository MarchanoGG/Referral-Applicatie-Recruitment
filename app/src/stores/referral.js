import { defineStore } from "pinia";
import { useRouter } from "vue-router";
import { api } from "boot/axios";
import { useUserStore } from "stores/user";
import { useScoreboardStore } from "stores/scoreboard";
import { useTaskStore } from "stores/tasks";
import { useCandidateStore } from "stores/candidates";

export const useReferralStore = defineStore("referral", {
  state: () => ({
    scoreboardStore: useScoreboardStore(),
    userStore: useUserStore(),
    taskStore: useTaskStore(),
    candidatesStore: useCandidateStore(),
    router: useRouter(),
    scoreboards: null,
    users: null,
    candidates: null,
    tasks: null,
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
    async allByUser() {
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
    DeleteByScoreboardId(id) {
      this.has_errors = false;
      this.last_res = api.get("/Referrals", {
        params: { fk_scoreboard: id },
      });
      if (this.last_res.status == 200) {
        this.items = this.last_res?.data;
        this.items.forEach((refobj) => {
          this.deleteById(refobj.object_key)
        })
      } else {
        this.has_errors = true;
      }
    },
    async addReferral() {
      this.scoreboardStore.addItemSync()
      this.users.forEach((user) => {
        this.candidates.forEach((candidate) => {
          this.tasks.forEach((task) => {
            this.selected_item.fk_scoreboard = this.scoreboardStore.selected_item.object_key;
            this.selected_item.fk_user = user.object_key;
            this.selected_item.fk_candidate = candidate.object_key;
            this.selected_item.fk_task = task.object_key;
            this.addItem();
          });
        });
      });
      this.router.push("/admin/scoreboards");
    },
    async updateReferral() {
      this.DeleteByScoreboardId(this.scoreboardStore.selected_item.object_key)
      this.userStore.items.forEach((user) => {
        this.candidatesStore.items.forEach((candidate) => {
          this.taskStore.items.forEach((task) => {
            this.selected_item.fk_scoreboard = this.scoreboardStore.selected_item.object_key;
            this.selected_item.fk_user = user.object_key;
            this.selected_item.fk_candidate = candidate.object_key;
            this.selected_item.fk_task = task.object_key;
            this.addItem();
          });
        });
      });
      // this.router.push("/admin/scoreboards");
    },
    deleteById(id) {
      this.last_res = api.delete("/Referrals", {
        params: { object_key: id },
      });
      if (this.last_res.status == 200) {
      } else {
        this.has_errors = true;
      }
    },
    async deleteItem() {
      this.last_res = await api.delete("/Referrals", {
        params: { object_key: this.selected_item.object_key },
      });
      if (this.last_res.status == 200) {
      } else {
        this.has_errors = true;
      }
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
