<template>
  <div class="formcenter">
    
    <el-form :model="ruleForm"
             :label-width="ruleForm.labelWidth || '80px'"
             :label-position="ruleForm.labelPosition || 'left'">
      <el-form-item label="手机号：" prop="phone">
        <el-input v-model="ruleForm.phone"></el-input>
      </el-form-item>
      <el-form-item label="用户名：" prop="userName">
        <el-input v-model="ruleForm.userName"></el-input>
      </el-form-item>
      <el-form-item label="密码：" prop="password">
        <el-input v-model="ruleForm.password"></el-input>
      </el-form-item>
      <el-form-item label="确认密码：" prop="passwordconfirm">
        <el-input v-model="ruleForm.passwordconfirm"></el-input>
      </el-form-item>

       
      <el-button type="primary" style="width:80%;margin-bottom:30px;margin-left:100px;" @click.native.prevent="registeruser">注册</el-button>
    </el-form>
  </div>
</template>

<script>
  import { validUsername } from '@/utils/validate'
  import { register  } from '@/api/user'
export default {
  name: 'register', 
  data() {
    return {
      ruleForm: {
        labelWidth: '100px',
        labelPosition:'right'
      }
    }
  },
  watch: {
    
  },
  created() {
    // window.addEventListener('storage', this.afterQRScan)
  },
  mounted() { 
  },
  destroyed() {
    // window.removeEventListener('storage', this.afterQRScan)
  },
    methods: {
      registeruser: function () {
        const owner = this;
        register(owner.ruleForm) 
          .then(response => {
            console.log(response);
          })
          .catch(function (error) { 
            console.log(error)
          })
      }
  }
}
</script>
 <style>
   .formcenter {
     position: absolute; /*绝对定位*/
     width: 500px;
     height: 200px;
     text-align: center; /*(让div中的内容居中)*/
     top: 50%;
     left: 50%;
     margin-top: -300px;
     margin-left: -300px;
   }
 </style>
