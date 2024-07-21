// src/store/index.js
import Vue from 'vue';
import Vuex from 'vuex';

Vue.use(Vuex);

export default new Vuex.Store({
  state: {
    user: null // Assuming user object contains role info
  },
  getters: {
    isAdmin: state => {
      return state.user && state.user.role === 'Admin';
    }
  },
  mutations: {
    setUser(state, user) {
      state.user = user;
    }
  },
  actions: {
    login({ commit }, user) {
      // Handle login and set user
      commit('setUser', user);
    },
    logout({ commit }) {
      // Handle logout
      commit('setUser', null);
    }
  }
});
