<template>
  <div>
    <h2>Register</h2>
    <form @submit.prevent="handleRegister">
      <div class="mb-3">
        <label for="username" class="form-label">Username</label>
        <input type="text" v-model="form.username" class="form-control" required>
      </div>
      <div class="mb-3">
        <label for="email" class="form-label">Email</label>
        <input type="email" v-model="form.email" class="form-control" required>
      </div>
      <div class="mb-3">
        <label for="password" class="form-label">Password</label>
        <input type="password" v-model="form.password" class="form-control" required>
      </div>
      <div class="mb-3">
        <label for="role" class="form-label">Role</label>
        <select v-model="form.role" class="form-control" required>
          <option disabled value="">Select a role</option>
          <option value="User">User</option>
          <option value="Admin">Admin</option>
        </select>
      </div>
      <button type="submit" class="btn btn-primary">Register</button>
      <div v-if="errorMessage" class="alert alert-danger mt-3">{{ errorMessage }}</div>
    </form>
  </div>
</template>

<script>
import authService from '../services/authService';

export default {
  data() {
    return {
      form: {
        username: '',
        email: '',
        password: '',
        role: ''
      },
      errorMessage: ''
    };
  },
  methods: {
    handleRegister() {
      authService.register(this.form)
        .then(() => {
          this.$router.push('/login');
        })
        .catch(error => {
          console.error('Registration failed', error);
          this.errorMessage = 'Registration failed. Please check your input and try again.';
          if (error.response && error.response.data && error.response.data.error) {
            this.errorMessage = error.response.data.error;
          }
        });
    }
  }
}
</script>
