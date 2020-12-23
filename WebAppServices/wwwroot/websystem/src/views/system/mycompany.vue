<template>
  <div style="margin-left:12px;margin-top:5px;">
    <el-button type="primary" icon="el-icon-circle-plus-outline" @click="create()">添加</el-button>
    <el-input v-model="filter"
              placeholder="请输入内容"
              style="width:220px;margin-left:5px;"
              prefix-icon="el-icon-search" />

    <el-table style="margin-top:5px; width: 100%" border :data="tableData" @sort-change="SortChange">
      <template v-for="(item,index) in tableHead">
        <el-table-column :key="index"
                         :prop="item.columnName"
                         :label="item.columnDescription || item.columnName"
                         v-if="hiddenColumn[item.columnName] !== true"
                         show-overflow-tooltip sortable="custom" />
      </template>

      <el-table-column label="操作" width="300">
        <template slot-scope="scope">
          <el-button type="success" size="small" @click="Grant(scope.row)">授权</el-button>
          <el-button type="success" size="small" @click="Modify(scope.row)">编辑</el-button>
          <el-button type="danger" size="small" @click="Remove(scope.row)">删除</el-button>
        </template>
      </el-table-column>
    </el-table>

    <el-pagination :current-page="paging.PageIndex"
                   :page-sizes="[5, 10, 20, 40]"
                   :page-size="paging.PageSize"
                   layout="total, sizes, prev, pager, next, jumper"
                   :total="paging.TotalCount"
                   @size-change="handleSizeChange"
                   @current-change="handleCurrentChange" />

    <el-dialog title="单位" :visible.sync="createdialog" :close-on-click-modal="false" :close-on-press-escape="false" @close="reset">
      <el-form id="#create" ref="create" :model="model" :rules="rules" label-width="130px">
        <template v-for="(item,index) in tableHead">

          <el-form-item v-if="item.sqlType == 'nvarchar' && item.maxLength > 0"
                        :visible.sync="item.columnName != 'Id'"
                        :label="item.columnDescription || item.columnName"
                        :prop="item.columnName">
            <el-input v-model="model[item.columnName]"
                      type="textarea"
                      clearable />
          </el-form-item>

          <el-form-item v-else-if="item.sqlType == 'nvarchar' && item.maxLength < 0"
                        :visible.sync="item.columnName != 'Id'"
                        :label="item.columnDescription || item.columnName"
                        :prop="item.columnName">
            <el-input v-model="model[item.columnName]"
                      :autosize="{ minRows: 2, maxRows: 4}"
                      type="textarea"
                      clearable />
          </el-form-item>

          <el-form-item v-else-if="item.sqlType == 'bit'" :label="item.columnDescription || item.columnName">
            <el-radio-group v-model="model[item.columnName]">
              <el-radio :label="true">是</el-radio>
              <el-radio :label="false">否</el-radio>
            </el-radio-group>
          </el-form-item>


          <el-form-item v-if="item.columnName=='grantMode'"
                        :label="item.columnDescription || item.columnName"
                        :prop="item.columnName">

            <el-select v-model="model.grantMode" placeholder="请选择">
              <el-option v-for="item in grantModes"
                         :key="item.keys"
                         :label="item.description"
                         :value="item.keys">
              </el-option>
            </el-select>

          </el-form-item>

        </template>
      </el-form>
      <div slot="footer" class="dialog-footer">
        <el-button @click="createdialog=false">取 消</el-button>
        <el-button type="primary" :loading="createLoading" @click="Save">确 定</el-button>
      </div>
    </el-dialog>

    <el-dialog title="单位授权" :visible.sync="grantdialog" :close-on-click-modal="false" :close-on-press-escape="false">
      <div>
        <el-tree :data="treedata"
                 show-checkbox
                 ref="tree"
                 :default-checked-keys="companymenus"
                 node-key="id"
                 :props="defaultProps">
        </el-tree>
      </div>
      <div slot="footer" class="dialog-footer">
        <el-button @click="grantdialog=false">取 消</el-button>
        <el-button type="primary" @click="SaveGrant">确 定</el-button>
      </div>
    </el-dialog>
  </div>
</template>
<script>
  import { getHeader, GetResult, Save, Remove, SaveGrant, GetCompanyMenus } from '@/api/company'
  import { GetGrantMode } from '@/api/enum'
  import { debounce } from '@/utils'
  import { GetTree } from '@/api/menus'
  export default {
    name: 'Company',
    data() {
      return {
        defaultProps: {
          children: 'children',
          label: 'menuName'
        },
        companymenus: [],
        grantdialog: false,
        treedata: [],
        hiddenColumn: {
          id: true
          , parentId: true
          , createUserId: true
          , createUserName: true
          , createTime: false
          , modifyUserId: true
          , modifyUserName: true
          , modifyTime: true
          , isMy: false
          , companyRegionCode: true
          , parentName: true
        },
        grantModes: [],
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
            isMy:true
          }
        }
      }
    },
    watch: {
      filter: function (searchvalue) {
        this.paging.Filter = searchvalue
        this.GetResult()
      }
    },
    mounted() {
      this.GetGrantMode()
      this.getHeader()
      this.GetResult()
    },
    methods: {
      GetTree: function () {
        const owner = this
        GetTree().then(response => {
          owner.treedata = response.data
        })
      },
      SaveGrant: function () {
        const owner = this

        let keys = owner.$refs.tree.getCheckedKeys()
        let companyId = owner.selectcompany.id
        let requestdata = []
        keys.forEach(function (item, index) {
          //item 就是当日按循环到的对象
          //index是循环的索引，从0开始
          requestdata.push({ companyId: companyId, menuId: item })
        })
        SaveGrant(requestdata).then(response => {
          owner.grantdialog = false

        })
      },
      GetCompanyMenus: function (row) {
        const owner = this
        GetCompanyMenus({ id: row.id }).then(response => {
          owner.companymenus = []
          response.data.forEach(function (item, index) {
            owner.companymenus.push(item.menuId);
          }); 
          this.GetTree() 
        })
      },
      Grant: function (row) {
        this.GetCompanyMenus(row)
        this.grantdialog = true;
        this.selectcompany = row
      },
      GetGrantMode: function () {
        const owner = this
        GetGrantMode().then(response => {
          owner.grantModes = response.data
        })
      },
      SortChange: function (column) {
        this.paging.Sort = column.prop
        this.paging.Asc = column.order == 'ascending'
        this.GetResult()
      },
      handleSizeChange: function (size) {
        this.paging.PageSize = size
        this.GetResult()
      },

      handleCurrentChange: function (currentPage) {
        this.paging.PageIndex = currentPage
        this.GetResult()
      },

      Modify: function (row) {
        this.createdialog = true
        this.model = row
      },
      Remove: function (row) {
        const owner = this
        Remove(row).then(response => {
          owner.GetResult()
        })
      },


      getHeader: function () {
        const owner = this
        getHeader().then(response => {
          owner.tableHead = response.data
        })
      },
      GetResult: function () {
        const owner = this
        GetResult(owner.paging).then(response => {

          response.data.forEach(function (item, index) {

            if (owner.grantModes)
                owner.GetGrantMode()
            let keys = item["grantMode"]
            let search = owner.grantModes.filter(item => { return item.keys == keys })
            item["grantMode"] = search[0].description
          });

          owner.tableData = response.data
          owner.paging.TotalCount = response.total
        })
      },

      Save: function () {
        const owner = this
        Save(owner.model).then(response => {
          owner.createdialog = false
          owner.GetResult()
        })
      },
      create: function () {
        this.createdialog = true
      },
    
      // 重置表单
      reset() {
        this.$refs.create.resetFields()
        this.model = {}
      }
    }
  }</script>

<style scoped>
</style>
