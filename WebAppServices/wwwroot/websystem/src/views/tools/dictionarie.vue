<template>
  <div>
    <div class="header" />
    <div class="navbar" style="overflow-x: hidden; overflow-y: hidden;">

      <el-input v-model="filterText"
                placeholder="输入关键字进行过滤" />

      <div style="overflow:scroll;height:95%;">
        <el-tree ref="tree"
                 class="filter-tree"
                 :load="getdatabase"
                 node-key="id"
                 lazy
                 :props="defaultProps"
                 :filter-node-method="filterNode"
                 @node-click="handleNodeClick">
          <span class="custom-tree-node" slot-scope="{ node, data }">
            <span style="color:orangered;font-weight:600;">{{ node.label }}</span>
            <span>
              <el-link type="warning" round
                       v-if="data.description"
                       size="mini">

                {{data.description}}
              </el-link> &nbsp; &nbsp;
              <el-button type="text" v-if="data.parentId > 0"
                         size="mini"
                         @click="() => updatetabledescription(node, data)">
                备注
              </el-button>
            </span>
          </span>
        </el-tree>
      </div>
    </div>
    <div class="main">
      <el-tag>{{tablename}}</el-tag>

      <el-table :data="tableData"
                highlight-current-row
                height="97%"
                border
                :row-class-name="tableRowClassName"
                style="width: 100%">
        <el-table-column prop="columnName"
                         label="字段名称"
                         width="180" />
        <el-table-column prop="columnDescription"
                         label="字段描述"
                         width="250">

          <template slot-scope="scope">
            <span v-if="scope.row.isSet">
              <el-input v-model="scope.row.columnDescription" size="mini" placeholder="请输入字段描述" />
            </span>
            <span v-else>{{ scope.row.columnDescription }}</span>
          </template>
        </el-table-column>
        <el-table-column prop="isRequire"
                         label="必须"
                         :formatter="formatterisRequire"
                         width="80" />
        <el-table-column prop="isPrimarykey"
                         label="主键"
                         :formatter="formatterisPrimarykey"
                         width="80" />
        <el-table-column prop="isIdentity"
                         label="自增"
                         :formatter="formatterisIdentity"
                         width="80" />
        <el-table-column prop="sqlType"
                         label="数据库类型"
                         width="120" />
        <el-table-column prop="maxLength"
                         label="最大长度"
                         width="120" />
        <el-table-column prop="scale"
                         label="精度"
                         width="80" />
        <el-table-column prop="defaultValue"
                         label="默认值"
                         width="180" />
        <el-table-column label="操作">
          <template slot-scope="scope">
            <el-button size="small" type="success" round @click="pwdChange(scope.row,scope.$index,true)"> {{ scope.row.isSet?'保存':"编辑备注" }}</el-button>
          </template>
        </el-table-column>

      </el-table>
    </div>



    <el-dialog title="修改表备注" :visible.sync="createdialog" :close-on-click-modal="false" width="600px" :close-on-press-escape="false" @close="reset">
      <el-form id="#create" :model="tablemodel" label-width="100px" ref="create">

        <el-form-item label="表名">
          <el-input v-model="tablemodel.label" :disabled="true" clearable></el-input>
        </el-form-item>

        <el-form-item label="描述">
          <el-input v-model="tablemodel.description" :autosize="{ minRows: 2, maxRows: 4}"
                    type="textarea" clearable></el-input>
        </el-form-item>

      </el-form>
      <div slot="footer" class="dialog-footer">
        <el-button @click="createdialog=false">取 消</el-button>
        <el-button type="primary" @click="SaveTableDescription">确 定</el-button>
      </div>
    </el-dialog>

  </div>
</template>
<style>
  .el-table .warning-row {
    background: oldlace;
  }

  .el-table .success-row {
    background: #f0f9eb;
  }

  .el-table--medium th, .el-table--medium td {
    padding: 5px 0;
  }

  .custom-tree-node {
    flex: 1;
    display: flex;
    align-items: center;
    justify-content: space-between;
    font-size: 14px;
    padding-right: 8px;
  }
</style>
<script>
  import {
    datadictionariesgetdatabase, datadictionariesgettables
    , datadictionariesgetcolumns, datadictionariessetextendedproperty
    , settabledescription
  } from '@/api/datadictionaries'
  import { debounce } from '@/utils';
  export default {
    name: 'HelloWorld',

    data() {
      return { 
        tableData: [],
        filterText: '',
        createdialog: false,
        tablename: 'TableName',
        tablemodel: {

        },
        defaultProps: {
          children: 'children',
          label: 'label',
          id: 'id'
        }
      }
    },
    watch: {
      filterText(val) {
        this.$refs.tree.filter(val)
      }
    },

    methods: {

      // 重置表单
      reset() {
        this.$refs.create.resetFields();
      },
      updatetabledescription: function (node, data) {
        this.createdialog = true;
        this.tablemodel = data;
      },
      SaveTableDescription: function () {
        var data = this.tablemodel;
        var requestdata = '' + data.parentId + ',' + data.label + ',' + data.description;

        settabledescription(requestdata)
          .then(response => {
            this.createdialog = false;
          })
          .catch(function (error) {
            console.log(error)
          })
      },

      filterNode(value, data) {
        if (!value) return true
        return data.label.toLowerCase().indexOf(value.toLowerCase()) !== -1
      },
      pwdChange(row, index, cg) {
        var owner = this
        // 点击修改 判断是否已经保存所有操作
        if (row.isSet) {
          var description = row.columnDescription
          var tablename = row.tableName
          var columnname = row.columnName
          var id = row.id
          owner.setextendedproperty(id, tablename, columnname, description)

          row.isSet = false
        } else {
          row.isSet = null
          row.isSet = true
        }
      },
      formatterisRequire(row, column) {
        return row.isRequire ? '是' : '否'
      },
      formatterisIdentity(row, column) {
        return row.isIdentity ? '是' : '否'
      },
      formatterisPrimarykey(row, column) {
        return row.isPrimarykey ? '是' : '否'
      },
      handleNodeClick(data) {
        var owner = this
        if (data.parentId) {
          var params = '' + data.parentId + ',' + data.label;
          owner.tablename = '表名:' + data.label;

          owner.getcolumns(params)
        }
      },

      getdatabase(node, resolve) {
        const owner = this
        datadictionariesgetdatabase()
          .then(response => {
            if (node.level == 0) {
              return resolve(response.data)
            } else if (node.level == 1) {
              owner.gettables(node, resolve)
            } else {
              resolve([])
            }
          })
          .catch(function (error) { // 请求失败处理
            console.log(error)
          })
      },
      gettables(node, resolve) {
        datadictionariesgettables(node.key) 
          .then(response => {
            return resolve(response.data)
          })
          .catch(function (error) { // 请求失败处理
            console.log(error)
          })
      },

      getcolumns(table) {
        const owner = this
        datadictionariesgetcolumns(table) 
          .then(response => {
            owner.tableData = response.data
          })
          .catch(function (error) { // 请求失败处理
            console.log(error)
          })
      },
      tableRowClassName({ row, rowIndex }) {
        if (rowIndex % 2 == 0) {
          return 'warning-row'
        } else {
          return 'success-row'
        }
      },

      setextendedproperty(id, table, column, description) {
        var owner = this
        var tables = '' + id + ',' + table + ',' + column + ',' + description
        const getcolumn = '' + id + ',' + table
        datadictionariessetextendedproperty(tables) 
          .then(response => {
            owner.getcolumns(getcolumn)
          })
          .catch(function (error) { // 请求失败处理
            console.log(error)
          })
      }

    }

  }</script>

<style scoped>

  /* 头部样式 */
  .header {
    position: absolute;
    line-height: 0px;
    top: 0px;
    left: 0px;
    right: 0px;
    background-color: #2d3a4b;
  }

  /* 左侧样式 */
  .navbar {
    position: absolute;
    width: 400px;
    top: 10px; /* 距离上面50像素 */
    left: 0px;
    bottom: 0px;
    overflow-y: auto; /* 当内容过多时y轴出现滚动条 */
  }

  /* 主区域 */
  .main {
    position: absolute;
    top: 0px;
    left: 400px;
    bottom: 0px;
    right: 0px; /* 距离右边0像素 */
    padding: 10px;
    overflow-y: auto; /* 当内容过多时y轴出现滚动条 */
    /* background-color: red; */
  }

  .el-tree-node {
    height: 62px;
  }
</style>
