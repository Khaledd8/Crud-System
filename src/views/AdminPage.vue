<template>
  <div class="container">
    <h1 class="my-4">Admin Dashboard</h1>
    <div class="row mb-4">
      <div class="col-md-6">
        <div class="card">
          <div class="card-body">
            <h5 class="card-title">Employees List</h5>
            <p class="card-text">View and manage all employees.</p>
            <button class="btn btn-primary" @click="showEmployeeList">Go to Employees List</button>
          </div>
        </div>
      </div>
      <div class="col-md-6">
        <div class="card">
          <div class="card-body">
            <h5 class="card-title">Add Employee</h5>
            <p class="card-text">Add a new employee to the list.</p>
            <button class="btn btn-success" @click="showEmployeeForm">Go to Employee Form</button>
          </div>
        </div>
      </div>
    </div>
    <div v-if="currentView === 'list'">
      <EmployeeList :employees="employees" @edit-employee="editEmployee" @delete-employee="deleteEmployee" />
    </div>
    <div v-if="currentView === 'form'">
      <EmployeeForm :employee="currentEmployee" @save-employee="saveEmployee" />
    </div>
  </div>
</template>

<script>
import axios from 'axios';
import EmployeeList from '../components/EmployeeList.vue';
import EmployeeForm from '../components/EmployeeForm.vue';

export default {
  components: {
    EmployeeList,
    EmployeeForm
  },
  data() {
    return {
      currentView: 'list', // Default to show employee list
      currentEmployee: null,
      employees: [] // Ensure you load employees from your API or data source
    };
  },
  methods: {
    showEmployeeList() {
      this.currentView = 'list';
      this.currentEmployee = null;
    },
    showEmployeeForm() {
      this.currentView = 'form';
      this.currentEmployee = null;
    },
    editEmployee(employee) {
      this.currentEmployee = employee;
      this.currentView = 'form';
    },
    saveEmployee() {
      this.showEmployeeList();
    },
    deleteEmployee(employeeId) {
      axios.delete(`http://localhost:5001/api/employees/${employeeId}`)
        .then(() => {
          this.employees = this.employees.filter(employee => employee.id !== employeeId);
        })
        .catch(error => {
          console.error('There was an error deleting the employee:', error);
        });
    }
  },
  mounted() {
    // Load employees from your API
    axios.get('http://localhost:5001/api/employees')
      .then(response => {
        this.employees = response.data;
      })
      .catch(error => {
        console.error('There was an error loading the employees:', error);
      });
  }
}
</script>

<style scoped>
nav {
  margin-bottom: 20px;
}
.card {
  margin-bottom: 20px;
}
</style>
