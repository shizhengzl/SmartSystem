<template>
  <div style="margin-left:12px;margin-top:5px;">
    <el-button type="primary" icon="el-icon-circle-plus-outline" @click="create()">添加</el-button>
    <el-button type="primary" icon="el-icon-circle-plus-outline" @click="GetYml({Id:0,moduleName:'所有YML'})">导出所有YML</el-button>
    <el-input v-model="filter"
              placeholder="请输入内容"
              style="width:220px;margin-left:5px;"
              prefix-icon="el-icon-search" />

    <vxe-table resizable
               row-id="id"
               :tree-config="{children: 'children'}"
               :data="treedata">

      <template v-for="(item,index) in tableHead">
        <vxe-table-column v-if="hiddenColumn[item.columnName] !== true"
                          :key="index"
                          :field="item.columnName"
                          :title="item.columnDescription || item.columnName"
                          :tree-node="item.columnName == 'moduleName'"
                          show-overflow-tooltip />
      </template>
      <vxe-table-column label="操作" width="300">
        <template slot-scope="scope">
          <el-button type="success" size="small" @click="GetYml(scope.row)">导出Yml</el-button>
          <el-button type="success" size="small" @click="Modify(scope.row)">编辑</el-button>
          <el-button type="danger" size="small" @click="Remove(scope.row)">删除</el-button>
        </template>
      </vxe-table-column>
    </vxe-table>

    <el-pagination :current-page="paging.PageIndex"
                   :page-sizes="[5, 10, 20, 40]"
                   :page-size="paging.PageSize"
                   layout="total, sizes, prev, pager, next, jumper"
                   :total="paging.TotalCount"
                   @size-change="handleSizeChange"
                   @current-change="handleCurrentChange" />

    <el-dialog title="测试模块" :visible.sync="createdialog" :close-on-click-modal="false" :close-on-press-escape="false" @close="reset">
      <el-form id="#create" ref="create" :model="model" :rules="rules" label-width="130px">
        <template v-for="(item,index) in tableHead">



          <!-- 数字类型 -->
          <el-form-item v-if="(item.sqlType == 'int' || item.sqlType == 'bigint') && hiddenColumn[item.columnName] !== true
                        && item.columnName !== 'parentId'"
                        :label="item.columnDescription || item.columnName"
                        :prop="item.columnName">
            <el-input v-model="model[item.columnName]"
                      clearable />
          </el-form-item>

          <!--树-->
          <el-form-item v-else-if="item.columnName == 'parentId'"
                        :label="item.columnDescription || item.columnName"
                        :prop="item.columnName">

            <wlTreeSelect width="240"
                          check-strictly="false"
                          multiple="true"
                          :props="props"
                          :data="treedata"
                          @change="hindleChanged"
                          v-model="selected"></wlTreeSelect>

          </el-form-item>

          <!--字符串 （不长）-->
          <el-form-item v-else-if="item.sqlType == 'nvarchar' && item.maxLength > 0"
                        :visible.sync="item.columnName != 'Id'"
                        :label="item.columnDescription || item.columnName"
                        :prop="item.columnName">
            <el-input v-model="model[item.columnName]"
                      type="textarea"
                      clearable />
          </el-form-item>

          <!--字符串 （非常长）-->
          <el-form-item v-else-if="item.sqlType == 'nvarchar' && item.maxLength < 0"
                        :visible.sync="item.columnName != 'Id'"
                        :label="item.columnDescription || item.columnName"
                        :prop="item.columnName">
            <el-input v-model="model[item.columnName]"
                      :autosize="{ minRows: 2, maxRows: 4}"
                      type="textarea"
                      clearable />
          </el-form-item>

          <!--boolean 类型-->
          <el-form-item v-else-if="item.sqlType == 'bit'" :label="item.columnDescription || item.columnName">
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
  import { getHeader, GetResult, Save, Remove, GetTree, GetYml} from '@/api/testmodule'
import { debounce } from '@/utils'
import { MessageBox, Message } from 'element-ui'
export default {
  name: 'testmodule', 
  data() {
    return {
      treedata: [],
      selected: [],
      props: {
        label: "moduleName",
        value: "id"
      }, // 配置
      hiddenColumn: {
        id: true
        , parentId:true
        , createUserId: true
        , createUserName: true
        , createTime: false
        , modifyUserId: true
        , modifyUserName: true
        , modifyTime: true
        , parentName: true
      },
      tableData: [],
      tableHead: [],
      model: { IsSql: false },
      createLoading: false,
      createdialog: false,
      rules: {},
      filter: '',
      paging: {
        PageSize: 20,
        PageIndex: 1,
        TotalCount: 0,
        Sort: 'Id',
        Asc: true,
        Filter: '',
        Model: {
        }
      }
    }
  },
  watch: {
    filter: function(searchvalue) {
      this.paging.Filter = searchvalue
      this.GetResult()
    }
  },
  mounted() {
    this.getHeader()
    this.GetResult()
    this.GetTree()
  },
    methods: {
      fakeClick(obj) {
        var ev = document.createEvent("MouseEvents");
        ev.initMouseEvent(
          "click",
          true,
          false,
          window,
          0,
          0,
          0,
          0,
          0,
          false,
          false,
          false,
          false,
          0,
          null
        );
        obj.dispatchEvent(ev);
      },
      hindleChanged(val) { 
        this.model.parentId = val[0].id;
        this.model.parentName = val[0].moduleName; 
      },
    SortChange: function(column) {
      this.paging.Sort = column.prop
      this.paging.Asc = column.order == 'ascending'
      this.GetResult()
    },
    handleSizeChange: function(size) {
      this.paging.PageSize = size
      this.GetResult()
    },

    handleCurrentChange: function(currentPage) {
      this.paging.PageIndex = currentPage
      this.GetResult()
    },

    Modify: function(row) {
      this.createdialog = true
      this.model = row
      this.selected = !!row.parentId ? [row.parentId] : []; 
    },
    Remove: function(row) {
      const owner = this
      Remove(row).then(response => {
        owner.GetResult()
      })
    }, 
    GetTree: function() {
      const owner = this
      GetTree().then(response => {
        owner.treedata = response.data
      })
    }, 
    getHeader: function() {
      const owner = this
      getHeader().then(response => {
        owner.tableHead = response.data
      })
      },
    GetYml: function (row) {
      const owner = this
      owner.paging.Model = row;
      GetYml(owner.paging).then(response => {
        var yml= response.data
        var export_blob = new Blob([yml]);
        var save_link = document.createElement("a");
        save_link.href = window.URL.createObjectURL(export_blob);
        save_link.download = row.moduleName + '.yml';
        this.fakeClick(save_link);
      })
     },
    GetResult: function() { 
      const owner = this
      GetTree().then(response => {
        owner.treedata = response.data
      })
    },

    Save: function() {
      const owner = this 
      if (owner.model.id && owner.model.id == owner.model.parentId) {

        Message({
          message: '不能选择自己',
          type: 'error',
          duration: 5 * 1000
        })
        return;
      }
      Save(owner.model).then(response => {
        owner.createdialog = false
        owner.GetTree()
      })
    },

    create: function () {
      this.model = {}
      this.createdialog = true
    },

    // 重置表单
    reset() {
      this.$refs.create.resetFields()
    }
  }
}</script>

<style scoped>
</style>
