<template>
  <div style="margin-left:12px;margin-top:5px;">
    <el-button type="primary" icon="el-icon-circle-plus-outline" @click="create()">添加</el-button>
    <el-input placeholder="请输入内容" style="width:220px;margin-left:5px;" v-model="filter"
              prefix-icon="el-icon-search">
    </el-input>


    <el-table style="margin-top:5px; width: 100%" border :data="tableData"  @sort-change="SortChange">
      <template v-for="(item,index) in tableHead">
        <el-table-column :prop="capitalize(item.columnName)"
                         :label="item.columnDescription || item.columnName"
                         v-if="!hiddenColumn[item.columnName]"
                         :key="index"
                         show-overflow-tooltip
                         sortable="custom" ></el-table-column>
      </template>

      <el-table-column label="操作" width="180">
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


    <el-dialog title="创建角色" :visible.sync="createdialog" :close-on-click-modal="false" :close-on-press-escape="false" @close="reset">
      <el-form id="#create" :model="model" :rules="rules" ref="create" label-width="130px">
        <template v-for="(item,index) in tableHead">

          <el-form-item v-if="item.sqlType == 'nvarchar' && item.maxLength > 0"
                        :visible.sync="item.columnName != 'Id'"
                        :label="item.columnDescription || item.columnName" :prop="item.columnName">
            <el-input v-model="model[capitalize(item.columnName)]"
                      type="textarea" clearable></el-input>
          </el-form-item>

          <el-form-item v-else-if="item.sqlType == 'nvarchar' && item.maxLength < 0"
                        :visible.sync="item.columnName != 'Id'"
                        :label="item.columnDescription || item.columnName" :prop="item.columnName">
            <el-input v-model="model[capitalize(item.columnName)]" :autosize="{ minRows: 2, maxRows: 4}"
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
  </div>
</template>
<script>
  import { getHeader, GetResult, Save, Remove } from '@/api/requestresponselog'
  import { debounce } from '@/utils';
  export default {
    name: 'requestresponselog',
    data() {
      return {
        hiddenColumn: {
          id:true
          , createUserId: true
          , createUserName: true
          , createTime: false
          , modifyUserId: true
          , modifyUserName: true
          , modifyTime: true
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
          Asc:false,
          Filter:'',
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
        this.createdialog = true;  
        this.model = row; 
      },
      Remove: function (row) {
        const owner = this
        Remove(row).then(response => {
          owner.GetResult();
        })
      },

      capitalize: function (value) {
        if (!value)
          return value;
        value = value.toString()
        return value.charAt(0).toLowerCase() + value.slice(1)
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
