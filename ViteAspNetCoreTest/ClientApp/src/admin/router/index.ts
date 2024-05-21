import { createRouter, createWebHistory } from "vue-router";
import HomeView from "../views/HomeView.vue";
import AboutView from "../views/AboutView.vue";

function getPath(path?: string) {
  return "/admin" + path;
}

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: getPath("/"),
      name: "home",
      component: HomeView,
      meta: {
        title: "Home"
      }
    },
    {
      path: getPath("/about"),
      name: "about",
      component: AboutView,
      meta: {
        title: "About"
      }
    }
  ]
});

router.beforeEach((to, from, next) => {
  if (to.meta && to.meta.title) {
    document.title = "Admin - " + to.meta.title;
  } else {
    document.title = "Default Title";
  }
  next();
});

export default router;
