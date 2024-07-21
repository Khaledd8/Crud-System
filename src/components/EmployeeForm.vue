<template>
  <div class="container mt-4">
    <h2>{{ localEmployee.id ? 'Edit Employee' : 'Add Employee' }}</h2>
    <form @submit.prevent="submitForm">
      <div class="mb-3">
        <label for="name" class="form-label">Name</label>
        <input v-model="localEmployee.name" id="name" class="form-control" placeholder="Name" required />
      </div>
      <div class="mb-3">
        <label for="email" class="form-label">Email</label>
        <input v-model="localEmployee.email" id="email" type="email" class="form-control" placeholder="Email" required />
      </div>
      <div class="mb-3">
        <label for="phone" class="form-label">Phone Number</label>
        <input v-model="localEmployee.phoneNumber" id="phone" type="tel" class="form-control" placeholder="Phone Number" required />
      </div>
      <div class="mb-3">
        <label for="gradUndergrad" class="form-label">Grad/Undergrad</label>
        <select v-model="localEmployee.gradUndergrad" id="gradUndergrad" class="form-control" required>
          <option value="Grad">Grad</option>
          <option value="Undergrad">Undergrad</option>
        </select>
      </div>
      <div class="mb-3">
        <label for="image" class="form-label">Upload Image</label>
        <input @change="handleFileUpload" type="file" id="image" class="form-control" />
      </div>
      <button type="submit" class="btn btn-primary me-2">{{ localEmployee.id ? 'Update' : 'Add' }}</button>
      <button type="button" class="btn btn-secondary" @click="$emit('save-employee')">Cancel</button>
    </form>
  </div>
</template>

<script>
import axios from 'axios';

export default {
  name:'EmployeeForm',
  props: {
    employee: {
      type: Object,
      default: () => ({})
    }
  },
  data() {
    return {
      localEmployee: { ...this.employee },
      selectedFile: null
    };
  },
  watch: {
    employee(newEmployee) {
      this.localEmployee = { ...newEmployee };
    }
  },
  methods: {
    handleFileUpload(event) {
      this.selectedFile = event.target.files[0];
    },
    submitForm() {
      const formData = new FormData();
      formData.append('name', this.localEmployee.name);
      formData.append('email', this.localEmployee.email);
      formData.append('phoneNumber', this.localEmployee.phoneNumber);
      formData.append('gradUndergrad', this.localEmployee.gradUndergrad);
      if (this.selectedFile) {
        formData.append('imageFile', this.selectedFile);
      }

      if (this.localEmployee.id) {
        // Update employee
        axios.put(`http://localhost:5001/api/employees/${this.localEmployee.id}`, formData, {
          headers: {
            'Content-Type': 'multipart/form-data'
          }
        })
        .then(response => {
          this.$emit('save-employee', response.data);
        })
        .catch(error => {
          console.log(error);
        });
      } else {
        // Add new employee
        axios.post('http://localhost:5001/api/employees', formData, {
          headers: {
            'Content-Type': 'multipart/form-data'
          }
        })
        .then(response => {
          this.$emit('save-employee', response.data);
        })
        .catch(error => {
          console.log(error);
        });
      }
    }
  }
}
</script>

<style scoped>
.container {
  max-width: 600px;
  margin: 0 auto;
}
</style>
