<template>
  <div style="margin-left:12px;margin-top:5px;">
    <el-button type="primary" icon="el-icon-circle-plus-outline">添加</el-button>
    <el-button type="primary" icon="el-icon-edit">编辑</el-button>
    <el-button type="primary" icon="el-icon-delete">删除</el-button>
    <el-input placeholder="请输入内容" style="width:220px;margin-left:5px;"
              prefix-icon="el-icon-search">
    </el-input>


    <el-table style="margin-top:5px; width: 100%" border :data="tableData"  >
      <template v-for="(item,index) in tableHead">
        <el-table-column :prop="capitalize(item.columnName)" :label="item.columnDescription || item.columnName" :key="index" show-overflow-tooltip></el-table-column>
      </template>
    </el-table>


  </div>
</template>

<script>
import { getHeader, GetResult } from '@/api/Intellisence'
import { debounce } from '@/utils';
  export default {
    name: 'Intellisence',
    data() {
      return {
        tableData: [],
        tableHead: []
      }
    },
    watch: {},
    mounted() {
      this.getHeader();
      this.GetResult();
    },

     
    methods: {

      capitalize: function (value) {
        if (!value)
          return ''
        value = value.toString()
        return value.charAt(0).toLowerCase() + value.slice(1)
      },

      getHeader: function (name) {
        const owner = this
        getHeader().then(response => {
          owner.tableHead = response.data
        }).catch(function (error) {
          console.log(error)
        })
      },
      GetResult: function (name) {
        const owner = this
        GetResult({}).then(response => {
          owner.tableData = response.data 
        }).catch(function (error) {
          console.log(error)
        })
      }
    }
  }</script>

<style scoped>
</style>
