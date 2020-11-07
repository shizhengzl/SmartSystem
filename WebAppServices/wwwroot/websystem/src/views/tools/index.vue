<template>
  <div>
    <el-input
      v-model="textinput"
      type="textarea"
      :rows="10"
      placeholder="请输入内容"
    />
    <div class="divbut">
      <el-button type="primary" round @click.native="parsersql">SQL分析</el-button>

      <el-button type="primary" round @click.native="parserformatsql">SQLFormat分析</el-button>
    </div>
    <el-input
      v-model="textoutput"
      type="textarea"
      :rows="20"
      placeholder="请输入内容"
    />
  </div>

</template>

<script>
import { parsersql, parsersqlformat } from '@/api/datadictionaries'
export default {
  name: 'Tools',
  data() {
    return {
      textinput: '',
      textoutput: '',
      goableurl: 'http://localhost:5000'

    }
  },
  watch: {},
  methods: {
    parsersql: function(e) {
      const owner = this
      parsersql(
        { Input: owner.textinput }

      ).then(response => {
        owner.textoutput = response.data
      }).catch(function(error) { // 请求失败处理
        console.log(error)
      })
    },
    parserformatsql: function(e) {
      const owner = this
      parsersqlformat(
        { Input: owner.textinput }

      ).then(response => {
        owner.textoutput = response.data
      }).catch(function(error) { // 请求失败处理
        console.log(error)
      })
    }
  }
}</script>

<style scoped>

  .divbut {

      margin-top:10px;
      margin-bottom:10px;
  }
</style>
