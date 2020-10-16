<template>
  <div id="app">
    
    <DataDictionarie/>
  </div>
</template>

<script>
  import DataDictionarie from './components/DataDictionarie'

export default {
  name: 'App',
  components: {
    DataDictionarie
  }
}
</script>

<style>
#app {
  font-family: 'Avenir', Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
  margin-top: 60px;
}
</style>
