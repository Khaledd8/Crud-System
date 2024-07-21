<!-- src/App.vue -->
<template>
  <div id="app">
    <NavigationMenu />
    <router-view></router-view>
  </div>
</template>

<script>
import NavigationMenu from './components/NavigationMenu.vue';
export default {
  name: 'App',
  components:{
    NavigationMenu
  }
}
</script>

<style>
/* Add your styles here */
</style>
