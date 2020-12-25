<template>
  <div style="margin-left:12px;margin-top:5px;">
    <el-button type="primary" icon="el-icon-circle-plus-outline" @click="create()">添加</el-button>
    <el-input placeholder="请输入内容" style="width:220px;margin-left:5px;" v-model="filter"
              prefix-icon="el-icon-search">
    </el-input>


    <el-table style="margin-top:5px; width: 100%" border :data="tableData" @sort-change="SortChange">
      <template v-for="(item,index) in tableHead">
        <el-table-column :prop="item.columnName"
                         :label="item.columnDescription || item.columnName"
                         v-if="hiddenColumn[item.columnName] !== true && item.columnName!='password'"
                         :key="index"
                         show-overflow-tooltip
                         sortable="custom"></el-table-column>
      </template>

      <el-table-column label="操作" width="400">
        <template slot-scope="scope">
          <!--<el-button type="success" size="small" @click="Grant(scope.row)">用户角色</el-button>
          <el-button type="success" size="small" @click="Grant(scope.row)">用户部门</el-button>-->
          <el-button type="success" size="small" @click="Grant(scope.row)">授权</el-button>
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


    <el-dialog :title="title" :visible.sync="createdialog" :close-on-click-modal="false" :close-on-press-escape="false" @close="reset">
      <el-form id="#create" :model="model" :rules="rules" ref="create" label-width="130px">
        <template v-for="(item,index) in tableHead">

          <el-form-item v-if="item.sqlType == 'nvarchar' && item.maxLength > 0 && hiddenColumn[item.columnName] !== true"
                        :label="item.columnDescription || item.columnName" :prop="item.columnName">
            <el-input v-model="model[item.columnName]"
                      type="text" clearable></el-input>
          </el-form-item>

          <el-form-item v-else-if="item.sqlType == 'nvarchar' && item.maxLength < 0 && hiddenColumn[item.columnName] !== true"
                        :label="item.columnDescription || item.columnName" :prop="item.columnName">
            <el-input v-model="model[item.columnName]" :autosize="{ minRows: 2, maxRows: 4}"
                      type="textarea" clearable></el-input>
          </el-form-item>

          <el-form-item v-else-if="item.sqlType == 'bit'" :label="item.columnDescription || item.columnName">
            <el-radio-group v-model="model[item.columnName]">
              <el-radio :label="true">是</el-radio>
              <el-radio :label="false">否</el-radio>
            </el-radio-group>
          </el-form-item>

        </template>


        <!--树-->
        <el-form-item label="部门" 
                      prop="departmentId">

          <wlTreeSelect width="300"    checkbox checkStrictly="true"  
                        :props="departmentprops"
                        :data="departmenttreedata"
                        nodeKey="id"
                        @change="hindleChanged"
                        v-model="model.departmentId"></wlTreeSelect>

        </el-form-item>

        <el-form-item label="角色">
          <el-select v-model="model.roleId" collapse-tags placeholder="请选择角色" multiple style="width:300px">
            <div class="el-input" style="width:90%;margin-left:5%;">
              <input type="text" placeholder="请输入" class="el-input__inner" v-model="searchrole" @keyup="dropDownSearch">
            </div>
            <el-option v-for="item in roleShow" :key="item.id" :value="item.id" :label="item.roleName"  ></el-option>
          </el-select>
        </el-form-item>

      </el-form>
      <div slot="footer" class="dialog-footer">
        <el-button @click="createdialog=false">取 消</el-button>
        <el-button type="primary" :loading="createLoading" @click="Save">确 定</el-button>
      </div>
    </el-dialog>

    <el-dialog title="用户授权" :visible.sync="grantdialog" :close-on-click-modal="false" :close-on-press-escape="false">
      <div>
        <el-tree :data="treedata"
                 show-checkbox
                 ref="tree"
                 :default-checked-keys="usermenus"
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
  import { getHeader, GetResult, Save, Remove, SaveGrant, GetMenus } from '@/api/user'
  import { debounce } from '@/utils';
  import { GetCompanyTree as GetTree } from '@/api/menus'
  import { GetGrantMode } from '@/api/enum'
  import { GetResult as GetRoles } from '@/api/role'
  import {  GetTree as GetDepartmentTree } from '@/api/department'
  export default {
    name: 'Roles',
    data() {
      return {
        roleShow: [],
        roleShowAll:[],
        searchrole:'',
        departmentprops: {
          label: "departmentName",
          value: "id"
        },
        selecteddepartment:[],
        defaultProps: {
          children: 'children',
          label: 'menuName'
        },
        title: '新建用户',
        usermenus: [],
        selectuser: {},
        grantdialog: false,
        treedata: [],
        departmenttreedata:[],
        hiddenColumn: {
          id: true
          , parentId: true
          , createUserId: true
          , createUserName: true
          , createTime: false
          , modifyUserId: true
          , modifyUserName: true
          , modifyTime: true
          , parentName: true
          , companyId: true
          , password:true
        },
        tableData: [],
        tableHead: [],
        model: { roleId: [], departmentId:[]},
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
        },
      }
    },
    watch: {
      filter: function (searchvalue) {
        this.paging.Filter = searchvalue;
        this.GetResult();
      }
    },
    mounted() {
      this.getHeader();
      this.GetResult();
    },
    methods: {
      GetRoles: function (isall) {
        const owner = this
        GetRoles(owner.paging).then(response => {
          if (!isall) {
            owner.roleShow = response.data;
            owner.roleShowAll = response.data;
          }
          else {
            owner.roleShowAll = response.data;
          }
           
        })
      },
      dropDownSearch() {
        this.GetRoles(true)
        var owner = this; 
        owner.roleShow = owner.roleShowAll.filter(owner.filterSearch);
      },
      filterSearch(item) {
        return item.roleName.includes(this.searchrole);
      },
      GetDepartmentTree: function () {
        const owner = this
        GetDepartmentTree().then(response => {
          owner.departmenttreedata = response.data
        })
      },
      hindleChanged: function (val) {
        let departmentid = [];
        val.forEach(function (item, index) {
          departmentid.push(item.id);
        })
        this.model.departmentId = departmentid;
        //this.model.departmentName = val[0].departmentName;
      },
      GetTree: function () {
        const owner = this
        GetTree().then(response => {
          owner.treedata = response.data
        })
      },
      SaveGrant: function () {
        const owner = this

        let keys = owner.$refs.tree.getCheckedKeys()
        let userId = owner.selectuser.id
        let requestdata = []
        keys.forEach(function (item, index) {
          //item 就是当日按循环到的对象
          //index是循环的索引，从0开始
          requestdata.push({ userId: userId, menuId: item })
        })
        SaveGrant(requestdata).then(response => {
          owner.grantdialog = false

        })
      },
      GetMenus: function (row) {
        const owner = this
        GetMenus({ id: row.id }).then(response => {
          owner.usermenus = []
          response.data.forEach(function (item, index) {
            owner.usermenus.push(item.menuId);
          });
          this.GetTree()
        })
      },
      Grant: function (row) {
        this.GetMenus(row)
        this.grantdialog = true;
        this.selectuser = row
      },
      GetGrantMode: function () {
        const owner = this
        GetGrantMode().then(response => {
          owner.grantModes = response.data
        })
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

      handleCurrentChange: function (currentPage) {
        this.paging.PageIndex = currentPage;
        this.GetResult();
      },

      Modify: function (row) {
        this.GetRoles()
        this.GetDepartmentTree()
        this.createdialog = true
        this.title = "编辑用户"
        this.model = row;
      },
      Remove: function (row) {
        const owner = this
        Remove(row).then(response => {
          owner.GetResult();
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
          owner.tableData = response.data;
          owner.paging.TotalCount = response.total;
        })
      },

      Save: function () {
        const owner = this;
        Save(owner.model).then(response => {
          owner.createdialog = false;
          owner.reset();
          owner.GetResult();
        })
      },

      create: function () {
        this.GetRoles();
        this.GetDepartmentTree()
        this.createdialog = true;

      
      },
      // 重置表单
      reset() {
        this.$refs.create.resetFields();
        this.model = {};
      },
    }
  }</script>

<style scoped>
</style>
