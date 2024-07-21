<!-- src/components/UserLogin.vue -->
<template>
  <div class="container mt-5">
    <h2>User Login</h2>
    <form @submit.prevent="handleLogin">
      <div class="mb-3">
        <label for="username" class="form-label">Username</label>
        <input type="text" v-model="form.username" class="form-control" required>
      </div>
      <div class="mb-3">
        <label for="password" class="form-label">Password</label>
        <input type="password" v-model="form.password" class="form-control" required>
      </div>
      <button type="submit" class="btn btn-primary">Login</button>
      <router-link to="/register" class="btn btn-link">Register if you don't have an account</router-link>
    </form>
  </div>
</template>

<script>
import authService from '../services/authService';

export default {
  name: 'UserLogin',
  data() {
    return {
      form: {
        username: '',
        password: ''
      }
    };  
  },
  methods: {
    handleLogin() {
      // Add login logic here, e.g., call an authService
      authService.login(this.form).then(() => {
        // Handle login success
        this.$router.push('/admin'); // Navigate to the admin page on successful login
      }).catch(error => {
        // Handle login error
        console.error('Login failed', error);
      });
    }
  }
}
</script>

<style scoped>
.container {
  max-width: 500px;
  margin: 0 auto;
}
</style>
