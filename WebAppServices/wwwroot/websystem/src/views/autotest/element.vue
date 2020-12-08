<template>
  <div>
    <div class="header" />
    <div class="navbar" style="overflow-x: hidden; overflow-y: hidden;">


      <el-input placeholder="输入关键字进行过滤"
                v-model="filterText">
      </el-input>

      <el-tree class="filter-tree"
               :data="treeData"
               node-key="id"
               :props="defaultProps"
               @node-click="nodeclick"
               default-expand-all
               :filter-node-method="filterNode"
               ref="tree">
      </el-tree>

    </div>
    <div class="main">
      <el-button type="primary" icon="el-icon-circle-plus-outline" @click="create()">添加</el-button>
      <el-input placeholder="请输入内容" style="width:220px;margin-left:5px;" v-model="filter"
                prefix-icon="el-icon-search">
      </el-input>


      <el-table style="margin-top:5px; width: 100%" border :data="tableData" @sort-change="SortChange">
        <template v-for="(item,index) in tableHead">
          <el-table-column :prop="item.columnName"
                           :label="item.columnDescription || item.columnName"
                           v-if="hiddenColumnHeader[item.columnName] !== true"
                           :key="index"
                           show-overflow-tooltip
                           sortable="custom"></el-table-column>
        </template>

        <el-table-column label="操作" width="200">
          <template slot-scope="scope">
            <el-button type="success" size="small" @click="Modify(scope.row)">编辑</el-button>
            <el-button type="danger" size="small" @click="Remove(scope.row)">删除</el-button>
          </template>
        </el-table-column>
      </el-table>

      <el-pagination @size-change="handleSizeChange"
                     @current-change="handleCurrentChange"
                     :current-page="paging.PageIndex"
                     :page-sizes="[5, 10, 20, 40]"
                     :page-size="paging.PageSize"
                     layout="total, sizes, prev, pager, next, jumper"
                     :total="paging.TotalCount">
      </el-pagination>
    </div>

    <el-dialog :title="title" :visible.sync="createdialog" :close-on-click-modal="false" :close-on-press-escape="false" @close="reset">
      <el-form id="#create" ref="create" :model="model" :rules="rules" label-width="130px">
        <template v-for="(item,index) in tableHead">



          <!-- 数字类型 -->
          <el-form-item v-if="(item.sqlType == 'int' || item.sqlType == 'bigint') && hiddenColumn[item.columnName] !== true
                        && item.columnName !== 'parentId'"
                        :label=" item.columnName"
                        :prop="item.columnName">
            <el-input v-model="model[item.columnName]"
                      clearable />
          </el-form-item>

          <!--树-->
          <el-form-item v-else-if="item.columnName == 'type' "
                        :label="item.columnDescription || item.columnName"
                        :prop="item.columnName">

            <el-select v-model="model.type" placeholder="请选择">
              <el-option v-for="item in types"
                         :key="item.value"
                         :label="item.label"
                         :value="item.value">
              </el-option>
            </el-select>

          </el-form-item>

          <!--树-->
          <el-form-item v-else-if="item.columnName == 'parentId'"
                        :label="item.columnDescription || item.columnName"
                        :prop="item.columnName">

            <wlTreeSelect width="240"
                          check-strictly="false"
                          multiple="true"
                          :props="props"
                          :data="treeData"
                          @change="hindleChanged"
                          v-model="selected"></wlTreeSelect>

          </el-form-item>


          <!--字符串 （不长）-->
          <el-form-item v-else-if="item.sqlType == 'nvarchar' && item.maxLength > 0  && hiddenColumn[item.columnName] !== true" 
                        :label="item.columnDescription || item.columnName"
                        :prop="item.columnName">
            <el-input v-model="model[item.columnName]"
                      type="textarea"
                      clearable />
          </el-form-item>

          <!--字符串 （非常长）-->
          <el-form-item v-else-if="item.sqlType == 'nvarchar' && item.maxLength < 0  && hiddenColumn[item.columnName] !== true" 
                        :label="item.columnDescription || item.columnName"
                        :prop="item.columnName">
            <el-input v-model="model[item.columnName]"
                      :autosize="{ minRows: 2, maxRows: 4}"
                      type="textarea"
                      clearable />
          </el-form-item>

          <!--boolean 类型-->
          <el-form-item v-else-if="item.sqlType == 'bit'  && hiddenColumn[item.columnName] !== true" :label="item.columnDescription || item.columnName">
            <el-radio-group v-model="model[item.columnName]">
              <el-radio :label="true">是</el-radio>
              <el-radio :label="false">否</el-radio>
            </el-radio-group>
          </el-form-item>

        </template>
      </el-form>
      <div slot="footer" class="dialog-footer">
        <el-button @click="createdialog=false">取 消</el-button>
        <el-button type="primary" :loading="createLoading" @click="Save">确 定</el-button>
      </div>
    </el-dialog>

  </div>
</template>


<script> 
  import { getHeader, GetResult, Save, Remove } from '@/api/element'
  import { GetTree as GetTree,} from '@/api/testmodule' 
  import { debounce } from '@/utils';
  import { MessageBox, Message } from 'element-ui'
  export default {
    name: 'elements',

    data() {
      return {
        props: {
          label: "moduleName",
          value: "id"
        }, // 配
        selected: [],
        types: [{
          value: 'xpath',
          label: 'xpath'
        }, {
            value: 'id',
            label: 'id'
        }, {
            value: 'linkText',
            label: 'linkText'
        }, {
            value: 'name',
            label: 'name'
        }, {
            value: 'className',
            label: 'className'
          }, {
            value: 'cssSelector',
            label: 'cssSelector'
          }, {
            value: 'partialLinkText',
            label: 'partialLinkText'
          }, {
            value: 'tagName',
            label: 'tagName'
          }], 
        model: {},
        rules: {},
        hiddenColumn: {
          id: true
          , parentId: true
          , createUserId: true
          , createUserName: true
          , createTime: false
          , modifyUserId: true
          , modifyUserName: true
          , modifyTime: true
          , parentName:true
        },
        hiddenColumnHeader: {
          id: true
          , parentId: true
          , createUserId: true
          , createUserName: true
          , createTime: false
          , modifyUserId: true
          , modifyUserName: true
          , modifyTime: true 
        },
        title: '新增页面元素',
        treeData: [], 
        tableHead: [],
        tableData: [],
        filterText: '',
        filter: '',
        createdialog: false,
        createLoading: false,
        tablemodel: {

        },
        paging: {
          PageSize: 20,
          PageIndex: 1,
          TotalCount: 0,
          Sort: 'Id',
          Asc: true,
          Filter: '',
          Model: {
          }
        },
        defaultProps: {
          children: 'children',
          label: 'moduleName'
        } 
      }
    },
    watch: {
      filterText(val) {
        this.$refs.tree.filter(val)
      }
    },
    mounted() {
      this.getHeader();
      this.GetResult();
      this.GetTree()

    },
    methods: {
      Modify: function (row) {
        this.createdialog = true
        this.model = row
        this.selected = !!row.parentId ? [row.parentId] : [];
      },
      nodeclick: function (data, node, t) {
        if (data.url) {
          this.selected = [data.id] 
        }
        // 加载元素
        this.paging.Model.parentId = data.id;
        this.GetResult()
      },
      hindleChanged(val) {
        this.model.parentId = val[0].id;
        this.model.parentName = val[0].moduleName;
      },
      Remove: function (row) {
        const owner = this
        Remove(row).then(response => {
          owner.GetResult()
        })
      }, 
      Save: function () {
        const owner = this
        if (!owner.model.parentId) {
          Message({
            message: '请选择页面',
            type: 'error',
            duration: 5 * 1000
          })
            return
        }
        Save(owner.model).then(response => {
          owner.createdialog = false
          owner.GetResult()
        })
      },
      create: function () {
        var owner = this
        this.createdialog = true;
     
      },
      getHeader: function () {
        const owner = this
        getHeader().then(response => {
          owner.tableHead = response.data
        })
      },
      handleCurrentChange: function (currentPage) {
        this.paging.PageIndex = currentPage;
        this.GetResult();
      },
      SortChange: function (column) {
        this.paging.Sort = column.prop;
        this.paging.Asc = column.order == "ascending";
        this.GetResult();
      },
      handleSizeChange: function (size) {
        this.paging.PageSize = size;
        this.GetResult();
      },
      GetResult: function (TableAreaId) {
        const owner = this  
        GetResult(owner.paging).then(response => {
          owner.tableData = response.data; 
          owner.paging.TotalCount = response.total;

        })
      },
      GetTree: function () {
        const owner = this
        GetTree(owner.paging).then(response => { 
          owner.treeData = response.data
          owner.paging.TotalCount = response.total
        })
      },

      // 重置表单
      reset() {
        this.$refs.create.resetFields();
        this.model = {};
        this.selected = [];
      },
      filterNode(value, data) {
        if (!value) return true
        return data.label.toLowerCase().indexOf(value.toLowerCase()) !== -1
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
