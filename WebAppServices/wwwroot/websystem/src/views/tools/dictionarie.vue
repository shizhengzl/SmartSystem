<template>
  <div>
    <div class="header" />
    <div class="navbar" style="overflow-x: hidden; overflow-y: hidden;">

      <el-tabs v-model="activeName" @tab-click="tabhandleClick" style="margin-left:10px"  >
        <el-tab-pane label="数据库字典" name="first" >
          <el-input v-model="filterText"
                    placeholder="输入关键字进行过滤" />

          <div style="overflow:scroll;height:800px;">
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
        </el-tab-pane>
        <el-tab-pane label="分类字典" name="second">
          <div style="overflow:scroll;height:800px;">
            <el-tree ref="treetype"
                     class="filter-tree"
            
            :load="gettablestype"
                     node-key="id"
             lazy
         
            :data="areatableData"
            :props="defaultProps"
            :filter-node-method="filterNode"
            @node-click="handleNodeClicktype">
            <span class="custom-tree-node" slot-scope="{ node, data }">
              <span style="color:orangered;font-weight:600;">{{ node.label }}</span>
              <!--<span>  {{data.description}}</span>-->
              <span> 
                <el-link type="warning" round
                         v-if="data.description"
                         size="mini">
                  {{data.description}}
                </el-link> &nbsp; &nbsp;
                <el-button type="text" v-if="data.level > 1"
                           size="mini"
                           @click="() => updatetabledescription(node, data)">
                  备注
                </el-button>
              </span>
            </span>
            </el-tree>
          </div>
        </el-tab-pane> 
      </el-tabs>

     
    </div>
    <div class="main">
      <el-tag>{{tablename}}</el-tag>
      <el-button type="primary" icon="el-icon-circle-plus-outline" @click="exports()">导出</el-button>
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
                         width="80">
          <template slot-scope="scope">
            <i v-if="scope.row.isRequire" class="el-icon-circle-check" style="color:blue;"></i>
            <i v-if="!scope.row.isRequire" class="el-icon-close" style="color:red;"></i>
          </template>
        </el-table-column>
        <el-table-column prop="isPrimarykey"
                         label="主键"
                         :formatter="formatterisPrimarykey"
                         width="80">
          <template slot-scope="scope">
            <i v-if="scope.row.isPrimarykey" class="el-icon-circle-check" style="color:blue;"></i>
            <i v-if="!scope.row.isPrimarykey" class="el-icon-close" style="color:red;"></i>
          </template>
        </el-table-column>
        <el-table-column prop="isIdentity"
                         label="自增"
                         :formatter="formatterisIdentity"
                         width="80">
          <template slot-scope="scope">
            <i v-if="scope.row.isIdentity" class="el-icon-circle-check" style="color:blue;"></i>
            <i v-if="!scope.row.isIdentity" class="el-icon-close" style="color:red"></i>
          </template>
        </el-table-column>
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
                         width="80" />
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
    background-color:white;
  }

  .el-table--medium th, .el-table--medium td {
    padding: 3px 0;
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
    , settabledescription, ExportTableColumnList
  } from '@/api/datadictionaries'
  import { getHeader, GetResult, Save, Remove } from '@/api/tablearea'

  import { GetResult as GetResultArea } from '@/api/tableareadata'
  import { debounce } from '@/utils';
import { Level } from 'chalk';
  export default {
    name: 'HelloWorld',

    data() {
      return {
        params: {},
        activeName:'first',
        tableData: [],
        areatableData: [],
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
    mounted() {
      this.initarea()
    },
    methods: {
      exports: function () {
        this.ExportTableColumnList(this.params);
      },
      tabhandleClick: function (tab, event) {
          
      },
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
    
        var requestdata = '' + (data.parentId || data.databaseid) + ',' + data.label + ',' + data.description;

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
        return row.isRequire ? '' : '否'
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
          this.params = params
          owner.getcolumns(params)
        }
      },

      handleNodeClicktype(data) {
       // debugger
        if (data.level == 2) {
        var ps = data.databaseid + ',' + data.label
          this.params = ps;
          this.getcolumns(ps)
        }
      },

      initarea() {
        const owner = this
        GetResult({
          PageSize: 1000,
          PageIndex: 1,
          TotalCount: 0,
          Sort: 'Id',
          Asc: false,
          Filter: '',
          Model: {
          }
        })
          .then(response => {
            owner.areatableData = response.data 
            owner.areatableData.forEach(function (item, index) {
              owner.areatableData[index].label = item.areaName;
              owner.setchild(owner.areatableData[index])
            }) 
          })
      },

      setchild(arr) {
        var owner = this;
        arr.children.forEach(function (item, index) {
          arr.children[index].label = item.areaName;
          owner.setchild(arr.children[index]);
        });
      },

      getdataare(node, resolve) {
        const owner = this 
        GetResult({
          PageSize: 1000,
          PageIndex: 1,
          TotalCount: 0,
          Sort: 'Id',
          Asc: false,
          Filter: '',
          Model: {
          }
        })
          .then(response => {
         
            if (node.level == 0) {

              let d = response.data
              var rs = []
              d.forEach(function (item, index) {
                rs.push({ id: item.id, label: item.areaName,level:1})
              })

              return resolve(rs)
            }
            else if (node.level == 1) {
              owner.gettablestype(node, resolve)
              return resolve([])
            }
            else if (node.level == 2) {
              var ps = node.data.databaseid +',' + node.label
             this.params = ps
              owner.getcolumns(ps)
              return resolve([])
            }
            else {
              resolve([])
            }
          })
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
      },

      gettablestype(node, resolve) {
        var owner = this;

        if (node.data.length ==0) {
          return resolve([]);
        }
        if (node.data && node.data.children && node.data.children.length > 0) {
          return resolve(node.data.children)
        }
        let p = {
          PageSize: 20,
          PageIndex: 1,
          TotalCount: 0,
          Sort: 'Id',
          Asc: false,
          Filter: '',
          Model: {
            TableAreaId:node.key
          }
        } 
        GetResultArea(p)
          .then(response => { 
            let d = response.data
            var rs = [] 
            d.forEach(function (item, index) {
              rs.push({ id: item.id, label: item.tableName, level: 2, databaseid: item.dabaBaseId, description : item.description })
            })  

            return resolve(rs)
          })
      },

      getcolumns(table) {
        const owner = this
        datadictionariesgetcolumns(table) 
          .then(response => {
            owner.tableData = response.data
          })
      },

      ExportTableColumnList(table) {
        const owner = this
        ExportTableColumnList(table)
          .then(response => {  
            let link = document.createElement('a')
            link.style.display = 'none'
            link.href = process.env.VUE_APP_BASE_API + response.data
 
            link.setAttribute('download', "a.xlsx")
            document.body.appendChild(link)

            link.click()
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
            this.params = getcolumn
            owner.getcolumns(getcolumn)
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
