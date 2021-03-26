<template>
  <div style="margin-left:12px;margin-top:5px;height:800px">

    <el-row>
      <el-col :span="7" style="height:100%">
        <div style="height:100%">
          <el-button type="primary" icon="el-icon-circle-plus-outline" @click="create()">添加</el-button>
          <el-input placeholder="请输入内容" style="width:220px;margin-left:5px;" v-model="filter"
                    prefix-icon="el-icon-search">
          </el-input>

          <vxe-table resizable
                     row-id="id"
                     ref="vxetable"
                     @current-change="selects()"
                     highlight-current-row
                     :tree-config="{children: 'children'}"
                     :data="tableData">

            <!--<el-table style="margin-top:5px; width: 100%;" ref="singleTable"
          highlight-current-row
          @current-change="CurrentChange"
          @row-click="selectrow" border
          :data="tableData"
          @sort-change="SortChange">-->
            <!--<el-table-column type="selection"
                           width="55">
          </el-table-column>-->

            <template v-for="(item,index) in tableHead">
              <vxe-table-column v-if="!!!hiddenColumn[item.columnName]"
                                :key="index"
                                :field="item.columnName"
                                :title="item.columnDescription || item.columnName"
                                :tree-node="item.columnName == 'documentName'"
                                show-overflow-tooltip />
            </template>

            <vxe-table-column title="操作" width="180">
              <template slot-scope="scope">
                <el-button type="success" size="small" @click="Modify(scope.row)">编辑</el-button>
                <el-button type="danger" size="small" @click="Remove(scope.row)">删除</el-button>
              </template>
            </vxe-table-column>
          </vxe-table>

          <!--<el-pagination @size-change="handleSizeChange"
                       @current-change="handleCurrentChange"
                       :current-page="paging.PageIndex"
                       v-if="false"
                       :page-sizes="[5, 10, 20, 40]"
                       :page-size="paging.PageSize"
                       layout="total, sizes, prev, pager, next, jumper"
                       :total="paging.TotalCount">
        </el-pagination>-->
        </div>
      </el-col>
      <el-col :span="17">
        <input style="font-size:16px;" type="file" @change="uploadExcel" />
        <el-button type="primary" @click="saveexecl">保存</el-button>
        <!--<el-upload class="upload-demo"
             @on-change="uploadExcel" >
    <el-button size="small" type="primary">点击上传</el-button>
    <div slot="tip" class="el-upload__tip">只能上传jpg/png文件，且不超过500kb</div>
  </el-upload>-->

        <div id="luckysheet" style="height:800px"></div>
      </el-col>
    </el-row>

    <el-dialog title="文档" :visible.sync="createdialog" :close-on-click-modal="false" :close-on-press-escape="false" @close="reset">
      <el-form id="#create" :model="model" :rules="rules" ref="create" label-width="130px">
        <template v-for="(item,index) in tableHead">

          <el-form-item v-if="item.sqlType == 'nvarchar' && item.maxLength > 0  && !!!hiddenColumn[item.columnName]"
                        :label="item.columnDescription || item.columnName" :prop="item.columnName">
            <el-input v-model="model[item.columnName]"
                      type="textarea" clearable></el-input>
          </el-form-item>

          <el-form-item v-else-if="item.sqlType == 'nvarchar' && item.maxLength < 0  && !!!hiddenColumn[item.columnName]"
                        :label="item.columnDescription || item.columnName" :prop="item.columnName">
            <el-input v-model="model[item.columnName]" :autosize="{ minRows: 2, maxRows: 4}"
                      type="textarea" clearable></el-input>
          </el-form-item>

          <el-form-item v-else-if="item.sqlType == 'bit'  && !!!hiddenColumn[item.columnName]" :label="item.columnDescription || item.columnName">
            <el-radio-group v-model="model[item.columnName]">
              <el-radio :label="true">是</el-radio>
              <el-radio :label="false">否</el-radio>
            </el-radio-group>
          </el-form-item>

          <!--树-->
          <el-form-item v-else-if="item.columnName == 'parentId'"
                        :label="item.columnDescription || item.columnName"
                        :prop="item.columnName">

            <wlTreeSelect width="240"
                          multiple="true"
                          :props="props"
                          :data="tableData"
                          @change="hindleChanged"
                          v-model="selected"></wlTreeSelect>

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
  import { getHeader, GetResult, Save, Remove } from '@/api/document'
  import LuckyExcel from 'luckyexcel'
  import Cookies from 'js-cookie'
  import { debounce } from '@/utils';

  export default {
    name: 'tablearea',
    data() {
      return {
        selected: [],
        props: {
          label: "documentName",
          value: "id"
        },
        currentRow: {},
        hiddenColumn: {
          id: true
          , documentType: true
          , documentData: true
          , isFile: true
          , parentId: true
          , createUserId: true
          , createUserName: true
          , createTime: false
          , modifyUserId: true
          , modifyUserName: true
          , modifyTime: true
          , companyId: true
          , createTime:true
        },
        tableData: [],
        tableHead: [],
        model: { IsSql: false, flag: false },
        createLoading: false,
        createdialog: false,
        rules: {},
        filter: '',
        paging: {
          PageSize: 20,
          PageIndex: 1,
          TotalCount: 0,
          Sort: 'Id',
          Asc: false,
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

      var options = {
        container: 'luckysheet' //luckysheet为容器id
        , rh_height: 1000
        , title: 'ASB'
        , lang: 'zh'
        , showinfobar:false
      }
      luckysheet.create(options)
    },
    methods: {
      saveexecl: function () { 
        this.model.documentData = JSON.stringify(luckysheet.getAllSheets());
        this.Save()
      },
      uploadExcel: function(evt){
        const files = evt.target.files;
        if (files == null || files.length == 0) {
          alert("No files wait for import");
          return;
        }
        let name = files[0].name;
        let suffixArr = name.split("."), suffix = suffixArr[suffixArr.length - 1];
        if (suffix != "xlsx") {
          alert("Currently only supports the import of xlsx files");
          return;
        }
        LuckyExcel.transformExcelToLucky(files[0], function (exportJson, luckysheetfile) {

          if (exportJson.sheets == null || exportJson.sheets.length == 0) {
            alert("Failed to read the content of the excel file, currently does not support xls files!");
            return;
          }
          window.luckysheet.destroy();

          window.luckysheet.create({
            container: 'luckysheet', //luckysheet is the container id
            showinfobar: false,
            data: exportJson.sheets,
            title: exportJson.info.name,
            userInfo: exportJson.info.name.creator
          });
        });
      },
      selects: function (t, a) {
        this.model = this.$refs.vxetable.getCurrentRecord()

        var data = eval(this.model.documentData)
        if (data) {
          window.luckysheet.destroy();

          window.luckysheet.create({
            container: 'luckysheet', //luckysheet is the container id
            showinfobar: false,
            data: data
          });
        }
      },
      hindleChanged: function (val) {
        this.model.parentId = val[0].id;
      },
      CurrentChange(val) {
        this.currentRow = val;
      },
      selectrow: function (row, col, event) {
        const owner = this
        this.$refs.singleTable.clearSelection()
        this.$refs.singleTable.setCurrentRow(row);
        row.flag = true;
        this.$refs.singleTable.toggleRowSelection(row, row.flag);

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
        this.createdialog = true;
        this.model = row;
        this.selected = !!row.parentId ? [row.parentId] : [];
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
        createFile();
        this.createdialog = true;
      },
      // 重置表单
      reset() {
        //this.$refs.create.resetFields();
        this.model = {};
      },
    }
  }</script>

<style scoped>
</style>
