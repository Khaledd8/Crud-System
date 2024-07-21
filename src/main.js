// src/main.js
import { createApp } from 'vue';
import App from './App.vue';
import { createRouter, createWebHistory } from 'vue-router';
import AdminPage from './views/AdminPage.vue';

import 'bootstrap/dist/css/bootstrap.css';
import 'bootstrap-vue/dist/bootstrap-vue.css';
import UserLogin from './components/UserLogin.vue';
import ItemList from './components/EmployeeList.vue';
import UserRegister from './components/UserRegister.vue';



const routes = [
    { path: '/', component: UserLogin },
    { path: '/login', component: UserLogin },
    { path: '/register', component: UserRegister },
    { path: '/admin', component: AdminPage },
    { path: '/items', component: ItemList },

];

const router = createRouter({
  history: createWebHistory(),
  routes,
});

const app = createApp(App);
app.use(router);
app.mount('#app');
