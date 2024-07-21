<template>
  <div>
    <h2>Employees List</h2>
    <table class="table">
      <thead>
        <tr>
          <th>Name</th>
          <th>Email</th>
          <th>Phone Number</th>
          <th>Grad/Undergrad</th>
          <th>Actions</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="employee in employees" :key="employee.id">
          <td>{{ employee.name }}</td>
          <td>{{ employee.email }}</td>
          <td>{{ employee.phoneNumber }}</td>
          <td>{{ employee.gradUndergrad }}</td>
          <td>
            <button class="btn btn-primary" @click="$emit('edit-employee', employee)">Edit</button>
            <button class="btn btn-danger" @click="$emit('delete-employee', employee.id)">Delete</button>
          </td>
        </tr>
      </tbody>
    </table>
  </div>
</template>

<script>
export default {
  props: {
    employees: {
      type: Array,
      required: true
    }
  }
}
</script>
