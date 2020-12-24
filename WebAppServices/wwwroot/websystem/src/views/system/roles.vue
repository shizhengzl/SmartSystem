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
                         v-if="hiddenColumn[item.columnName] !== true"
                         :key="index"
                         show-overflow-tooltip
                         sortable="custom"></el-table-column>
      </template>

      <el-table-column label="操作" width="400">
        <template slot-scope="scope">
          <el-button type="success" size="small" @click="ShowRoleUser(scope.row)">用户管理</el-button>
          <el-button type="success" size="small" @click="Grant(scope.row)">角色授权</el-button>
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


    <el-dialog title="创建角色" :visible.sync="createdialog" :close-on-click-modal="false" :close-on-press-escape="false" @close="reset">
      <el-form id="#create" :model="model" :rules="rules" ref="create" label-width="130px">
        <template v-for="(item,index) in tableHead">

          <el-form-item v-if="item.sqlType == 'nvarchar' && item.maxLength > 0"
                        :visible.sync="item.columnName != 'Id'"
                        :label="item.columnDescription || item.columnName" :prop="item.columnName">
            <el-input v-model="model[item.columnName]"
                      type="textarea" clearable></el-input>
          </el-form-item>

          <el-form-item v-else-if="item.sqlType == 'nvarchar' && item.maxLength < 0"
                        :visible.sync="item.columnName != 'Id'"
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
      </el-form>
      <div slot="footer" class="dialog-footer">
        <el-button @click="createdialog=false">取 消</el-button>
        <el-button type="primary" :loading="createLoading" @click="Save">确 定</el-button>
      </div>
    </el-dialog>



    <el-dialog title="角色用户管理" :visible.sync="roleuserdialog" :close-on-click-modal="false" :close-on-press-escape="false">
      <div>
        <el-row :gutter="20">
          <el-col :span="12">
            <div>
              <el-input placeholder="请输入内容" style="width:400px;margin-left:5px;" v-model="havefilter"
                        prefix-icon="el-icon-search">
              </el-input>

              <el-table border :data="haveUserData">
                <el-table-column prop="userName" label="用户名" show-overflow-tooltip></el-table-column>
                <el-table-column prop="name" label="姓名" show-overflow-tooltip></el-table-column>
                <el-table-column prop="phone" label="手机号码" show-overflow-tooltip></el-table-column>

                <el-table-column label="操作" width="100">
                  <template slot-scope="scope">
                    <el-button type="danger" size="small" @click="RemoveRoleUser(scope.row)">删除</el-button>
                  </template>
                </el-table-column>
              </el-table>
              <el-pagination @size-change="havehandleSizeChange"
                             @current-change="havehandleCurrentChange"
                             :current-page="havepaging.PageIndex"
                             :page-sizes="[5, 10, 20, 40]"
                             :page-size="havepaging.PageSize"
                             layout="total, sizes, prev, pager, next, jumper"
                             :total="havepaging.TotalCount">
              </el-pagination>


            </div>
          </el-col>
          <el-col :span="1"><div></div></el-col>
          <el-col :span="12">
            <div>
              <el-input placeholder="请输入内容" style="width:400px;margin-left:5px;" v-model="chosefilter"
                        prefix-icon="el-icon-search">
              </el-input>

              <el-table border :data="ChoseUserData">
                <el-table-column prop="userName" label="用户名" show-overflow-tooltip></el-table-column>
                <el-table-column prop="name" label="姓名" show-overflow-tooltip></el-table-column>
                <el-table-column prop="phone" label="手机号码" show-overflow-tooltip></el-table-column>

                <el-table-column label="操作" width="100">
                  <template slot-scope="scope">
                    <el-button type="danger" size="small" @click="SaveRoleUser(scope.row)">添加</el-button>
                  </template>
                </el-table-column>
              </el-table>
              <el-pagination @size-change="chosehandleSizeChange"
                             @current-change="chosehandleCurrentChange"
                             :current-page="chosepaging.PageIndex"
                             :page-sizes="[5, 10, 20, 40]"
                             :page-size="chosepaging.PageSize"
                             layout="total, sizes, prev, pager, next, jumper"
                             :total="chosepaging.TotalCount">
              </el-pagination>

            </div>
          </el-col>
        </el-row>
      </div>
    </el-dialog>

    <el-dialog title="角色授权" :visible.sync="grantdialog" :close-on-click-modal="false" :close-on-press-escape="false">
      <div>
        <el-tree :data="treedata"
                 show-checkbox
                 ref="tree"
                 :default-checked-keys="rolemenus"
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
  import {
    getHeader, GetResult, Save, Remove
    , GetRoleUser, GetRoleChoseUser, SaveRoleUser, RemoveRoleUser, GetRoleMenus, SaveGrant
  } from '@/api/role'
  import { debounce } from '@/utils';
  import { GetCompanyTree as GetTree } from '@/api/menus' 
  export default {
    name: 'Roles',
    data() {
      return {
        defaultProps: {
          children: 'children',
          label: 'menuName'
        },
        chosefilter: '',
        havefilter:'',
        treedata: [],
        selectrole: {},
        grantdialog: false,
        rolemenus: [], 
        roleuserdialog: false,
        ChoseUserData:[],
        haveUserData: [],
        hiddenColumn: {
          id: true
          , parentId: true
          , createUserId: true
          , createUserName: true
          , createTime: false
          , modifyUserId: true
          , modifyUserName: true
          , modifyTime: true
          , companyId: true
        },
        tableData: [],
        tableHead: [],
        model: { IsSql:false },
        createLoading:false,
        createdialog: false,
        rules: {},
        filter:'',
        paging: {
          PageSize: 20,
          PageIndex: 1,
          TotalCount: 0,
          Sort: 'Id',
          Asc:true,
          Filter:'',
          Model: { 
          }
        },
        havepaging: {
          PageSize: 20,
          PageIndex: 1,
          TotalCount: 0,
          Sort: 'Id',
          Asc: true,
          Filter: '',
          Model: {
          }
        },
        chosepaging: {
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
      filter: function (searchvalue) {
        this.paging.Filter = searchvalue;
        this.GetResult();
      },
      havefilter: function (searchvalue) {
        this.havepaging.Filter = searchvalue;
        this.GetRoleUser();
      },
      chosefilter: function (searchvalue) {
        this.chosepaging.Filter = searchvalue;
        this.GetRoleChoseUser();
      }

      
    },
    mounted() {
      this.getHeader();
      this.GetResult();
    },
    methods: {
      SaveGrant: function () {
        const owner = this

        let keys = owner.$refs.tree.getCheckedKeys()
        let roleid = owner.selectrole.id
        let requestdata = []
        keys.forEach(function (item, index) {
          //item 就是当日按循环到的对象
          //index是循环的索引，从0开始
          requestdata.push({ roleId: roleid, menuId: item })
        })
        SaveGrant(requestdata).then(response => {
          owner.grantdialog = false 
        })
      },
      GetTree: function () {
        const owner = this
        GetTree().then(response => {
          owner.treedata = response.data
        })
      }, 
      GetRoleMenus: function (row) {
        const owner = this
        GetRoleMenus({ id: row.id }).then(response => {
          owner.rolemenus = []
          response.data.forEach(function (item, index) {
            owner.rolemenus.push(item.menuId);
          }); 
          this.GetTree()

        })
      },
      Grant: function (row) {
        this.GetRoleMenus(row)
        this.grantdialog = true;
        this.selectrole = row
      },
      SaveRoleUser: function (row) {
        const owner = this; 
        var roleuser = [{ userId: row.id, roleId: owner.selectrole.id}]
        SaveRoleUser(roleuser).then(response => {  
          owner.GetRoleUser()
          owner.GetRoleChoseUser()
        })
      },
      RemoveRoleUser: function (row) {
        const owner = this;
        var roleuser = [{ userId: row.id, roleId: owner.selectrole.id }]
        RemoveRoleUser(roleuser).then(response => { 
          owner.GetRoleUser()
          owner.GetRoleChoseUser()
        })
      }, 
      GetRoleUser: function () {
        const owner = this
        owner.havepaging.Model.Id = owner.selectrole.id
        GetRoleUser(owner.havepaging).then(response => {
          owner.haveUserData = response.data;
          owner.havepaging.TotalCount = response.total
        })
      },
      GetRoleChoseUser: function () {
        const owner = this
        owner.chosepaging.Model.Id = owner.selectrole.id
        GetRoleChoseUser(owner.chosepaging).then(response => {
          owner.ChoseUserData = response.data;
          owner.chosepaging.TotalCount = response.total
        })
      }, 
      ShowRoleUser: function (row) {
        this.selectrole = row
        this.roleuserdialog = true;
        this.GetRoleUser()
        this.GetRoleChoseUser()
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

      havehandleSizeChange: function (size) {
        this.havepaging.PageSize = size;
        this.GetRoleUser();
      },

      chosehandleSizeChange: function (size) {
        this.chosepaging.PageSize = size;
        this.GetRoleChoseUser();
      },

      handleCurrentChange: function (currentPage) {
        this.paging.PageIndex = currentPage;
        this.GetResult();
      },

      havehandleCurrentChange: function (currentPage) {
        this.havepaging.PageIndex = currentPage;
        this.GetRoleChoseUser();
      },

      chosehandleCurrentChange: function (currentPage) {
        this.chosepaging.PageIndex = currentPage;
        this.GetResult();
      },

      Modify: function (row) {
        this.createdialog = true;  
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
