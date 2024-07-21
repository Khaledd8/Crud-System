// src/router/index.js
import Vue from 'vue';
import VueRouter from 'vue-router';
import store from '../store';
import Home from '../views/Home.vue';
import Admin from '../views/Admin.vue';

Vue.use(VueRouter);

const routes = [
  { path: '/', component: Home },
  { path: '/admin', component: Admin, meta: { requiresAdmin: true } },
  // other routes
];

const router = new VueRouter({
  routes
});

router.beforeEach((to, from, next) => {
  const requiresAdmin = to.matched.some(record => record.meta.requiresAdmin);
  const isAdmin = store.getters.isAdmin; // Assumes you have a Vuex store getter

  if (requiresAdmin && !isAdmin) {
    next('/');
  } else {
    next();
  }
});

export default router;
