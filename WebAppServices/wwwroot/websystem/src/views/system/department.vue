<template>
  <div style="margin-left:12px;margin-top:5px;">
    <el-button type="primary" icon="el-icon-circle-plus-outline" @click="create()">添加</el-button>
    <el-input v-model="filter"
              placeholder="请输入内容"
              style="width:220px;margin-left:5px;"
              prefix-icon="el-icon-search" />

    <vxe-table resizable
               row-id="id"
               :tree-config="{children: 'children'}"
               :data="tableData">

      <template v-for="(item,index) in tableHead">
        <vxe-table-column v-if="hiddenColumn[item.columnName] !== true"
                          :key="index"
                          :field="item.columnName"
                          :title="item.columnDescription || item.columnName"
                          :tree-node="item.columnName == 'departmentName'"
                          show-overflow-tooltip />
      </template>
      <vxe-table-column label="操作" width="400">
        <template slot-scope="scope">
          <el-button type="success" size="small" @click="ShowDepartmentUser(scope.row)">用户管理</el-button>
          <el-button type="success" size="small" @click="Grant(scope.row)">授权</el-button>
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

    <el-dialog :title="title" :visible.sync="createdialog" :close-on-click-modal="false" :close-on-press-escape="false" @close="reset">
      <el-form id="#create" ref="create" :model="model" :rules="rules" label-width="130px">
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
                            multiple="true"
                            :props="props"
                            :data="departtreedata"
                            @change="hindleChanged"
                            v-model="selected"></wlTreeSelect>

            </el-form-item>

            <!--字符串 （不长）-->
            <el-form-item v-else-if="item.sqlType == 'nvarchar' && item.maxLength > 0 && hiddenColumn[item.columnName] !== true"
                          :visible.sync="item.columnName != 'Id'"
                          :label="item.columnDescription || item.columnName"
                          :prop="item.columnName">
              <el-input v-model="model[item.columnName]"
                        type="textarea"
                        clearable />
            </el-form-item>

            <!--字符串 （非常长）-->
            <el-form-item v-else-if="item.sqlType == 'nvarchar' && item.maxLength < 0 && hiddenColumn[item.columnName] !== true"
                          :visible.sync="item.columnName != 'Id'"
                          :label="item.columnDescription || item.columnName"
                          :prop="item.columnName">
              <el-input v-model="model[item.columnName]"
                        :autosize="{ minRows: 2, maxRows: 4}"
                        type="textarea"
                        clearable />
            </el-form-item>

            <!--boolean 类型-->
            <el-form-item v-else-if="item.sqlType == 'bit' && hiddenColumn[item.columnName] !== true" :label="item.columnDescription || item.columnName">
              <el-radio-group v-model="model[item.columnName]">
                <el-radio :label="true">是</el-radio>
                <el-radio :label="false">否</el-radio>
              </el-radio-group>
            </el-form-item>

            <el-form-item v-else-if="item.columnName=='managerUserId'" :label="item.columnDescription || item.columnName">
              <el-select v-model="model.managerUserId" collapse-tags placeholder="请选择用户">
                <div class="el-input" style="width:90%;margin-left:5%;">
                  <input type="text" placeholder="请输入" class="el-input__inner" v-model="model.managerUserName" @keyup="dropDownSearch">
                </div>
                <el-option v-for="item in userShow" :key="item.id" :value="item.id" :label="item.userName"></el-option>
              </el-select>
            </el-form-item>

          </template>
        </el-form>
      </el-form>
      <div slot="footer" class="dialog-footer">
        <el-button @click="createdialog=false">取 消</el-button>
        <el-button type="primary" :loading="createLoading" @click="Save">确 定</el-button>
      </div>
    </el-dialog>


    <el-dialog title="部门授权" :visible.sync="grantdialog" :close-on-click-modal="false" :close-on-press-escape="false">
      <div>
        <el-tree :data="treedata"
                 show-checkbox
                 ref="tree"
                 :default-checked-keys="departmentmenus"
                 node-key="id"
                 :props="defaultgrantProps">
        </el-tree>
      </div>
      <div slot="footer" class="dialog-footer">
        <el-button @click="grantdialog=false">取 消</el-button>
        <el-button type="primary" @click="SaveGrant">确 定</el-button>
      </div>
    </el-dialog>

    <el-dialog title="部门用户管理" :visible.sync="departmentuserdialog" :close-on-click-modal="false" :close-on-press-escape="false">
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
                    <el-button type="danger" size="small" @click="RemoveDepartmentUser(scope.row)">删除</el-button>
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
                    <el-button type="danger" size="small" @click="SaveDepartmentUser(scope.row)">添加</el-button>
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
  </div>
</template>
<script>
  import { getHeader, GetResult, Save, Remove, GetTree, SaveGrant, GetMenus, GetDepartmentUser, GetDepartmentChoseUser, SaveDepartmentUser, RemoveDepartmentUser} from '@/api/department'
  import { GetResult as  GetUsers } from '@/api/user'
  import { debounce } from '@/utils'
  import { GetTree as GetMenuTree} from '@/api/menus'
export default {
  name: 'Menus',
  data() {
    return {
      havefilter: '',
      chosefilter:'',
      ChoseUserData: [],
      haveUserData: [],
      departmentuserdialog:false,
      defaultgrantProps: {
        children: 'children',
        label: 'menuName'
      },
      departmentmenus: [],
      selectdepartment: {},
      grantdialog: false,
      treegrantdata: [],

      selectUser: [],
      userShow: [],
      userShowAll:[],
      title:'创建部门',
      props: {
        label: "departmentName",
        value: "id"
      }, // 配置
      treedata: [],
      departtreedata:[],
      selected: [], 
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
        , managerUserId: true
      },
      tableData: [],
      tableHead: [],
      model: { managerUserName: '' },
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
    filter: function(searchvalue) {
      this.paging.Filter = searchvalue
      this.GetResult()
    },
    havefilter: function (searchvalue) {
      this.havepaging.Filter = searchvalue;
      this.GetDepartmentUser();
    },
    chosefilter: function (searchvalue) {
      this.chosepaging.Filter = searchvalue;
      this.GetDepartmentChoseUser();
    }
  },
  mounted() {
    
    this.getHeader()
    this.GetResult()
    this.GetTree()

    this.GetUsers()
  },
    methods: {
      RemoveDepartmentUser: function (row) {
        const owner = this;
        var departmentuser = [{ userId: row.id, departmentId: owner.selectdepartment.id }]
        RemoveDepartmentUser(departmentuser).then(response => {
          owner.GetDepartmentUser()
          owner.GetDepartmentChoseUser()
        })
      }, 
      SaveDepartmentUser: function (row) {
        const owner = this;
        var departmentuser = [{ userId: row.id, departmentId: owner.selectdepartment.id }]
        SaveDepartmentUser(departmentuser).then(response => {
          owner.GetDepartmentUser()
          owner.GetDepartmentChoseUser()
        })
      },
      havehandleCurrentChange: function (currentPage) {
        this.havepaging.PageIndex = currentPage;
        this.GetDepartmentUser();
      },

      chosehandleCurrentChange: function (currentPage) {
        this.chosepaging.PageIndex = currentPage;
        this.GetDepartmentChoseUser();
      },
      havehandleSizeChange: function (size) {
        this.havepaging.PageSize = size;
        this.GetDepartmentUser();
      },

      chosehandleSizeChange: function (size) {
        this.chosepaging.PageSize = size;
        this.GetDepartmentChoseUser();
      },

      GetDepartmentUser: function () {
        const owner = this
        owner.havepaging.Model.Id = owner.selectdepartment.id
        GetDepartmentUser(owner.havepaging).then(response => {
          owner.haveUserData = response.data;
          owner.havepaging.TotalCount = response.total
        })
      },
      GetDepartmentChoseUser: function () {
        const owner = this
        owner.chosepaging.Model.Id = owner.selectdepartment.id
        GetDepartmentChoseUser(owner.chosepaging).then(response => {
          owner.ChoseUserData = response.data;
          owner.chosepaging.TotalCount = response.total
        })
      }, 

      ShowDepartmentUser: function (row) {
        this.selectdepartment = row
        this.departmentuserdialog = true;
        this.GetDepartmentUser()
        this.GetDepartmentChoseUser()
      },
      GetMenuTree: function () {
        const owner = this
        GetMenuTree().then(response => {
          owner.treedata = response.data
        })
      },
      SaveGrant: function () {
        const owner = this

        let keys = owner.$refs.tree.getCheckedKeys()
        let departmentId = owner.selectdepartment.id
        let requestdata = []
        keys.forEach(function (item, index) {
          //item 就是当日按循环到的对象
          //index是循环的索引，从0开始
          requestdata.push({ departmentId: departmentId, menuId: item })
        })
        SaveGrant(requestdata).then(response => {
          owner.grantdialog = false

        })
      },
      GetMenus: function (row) {
        const owner = this
        GetMenus({ id: row.id }).then(response => {
          owner.departmentmenus = []
          response.data.forEach(function (item, index) {
            owner.departmentmenus.push(item.menuId);
          });
          this.GetMenuTree()
        })
      },
      Grant: function (row) {
        this.GetMenus(row)
        this.grantdialog = true;
        this.selectdepartment = row
      },

      GetUsers: function (isall) {
        const owner = this
        GetUsers(owner.paging).then(response => {
          if (!isall) {
            owner.userShow = response.data;
            owner.userShowAll = response.data;
          }
          else { 
            owner.userShowAll = response.data;
          } 
        })
      },
      dropDownSearch() {
        this.GetUsers(true)
        var owner = this;
        owner.selectUser = [];
        owner.userShow = owner.userShowAll.filter(owner.filterSearch);
      },
      filterSearch(item) { 
        return item.userName.includes(this.model.managerUserName);
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
      hindleChanged :function(val) {
        this.model.parentId = val[0].id;
        this.model.parentName = val[0].moduleName;
      },
      GetTree: function () {
        const owner = this
        GetTree().then(response => {
          owner.departtreedata = response.data 
        })
      }, 

    handleCurrentChange: function(currentPage) {
      this.paging.PageIndex = currentPage
      this.GetResult()
    },

      Modify: function (row) {
      this.title ="编辑部门"
        this.createdialog = true
        this.model = row 
        this.selected = !!row.parentId ?  [row.parentId] : []; 
    },
    Remove: function(row) {
      const owner = this
      Remove(row).then(response => {
        owner.GetResult()
      })
    },  
    getHeader: function() {
      const owner = this
      getHeader().then(response => {
        owner.tableHead = response.data
      })
    },
    GetResult: function() {
      const owner = this
      GetTree(owner.paging).then(response => {
        owner.tableData = response.data
        owner.paging.TotalCount = response.total
      })
    }, 
    Save: function() {
      const owner = this
      Save(owner.model).then(response => {
        owner.createdialog = false
        owner.reset()
        owner.GetResult()
      })
    }, 
    create: function() {
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
