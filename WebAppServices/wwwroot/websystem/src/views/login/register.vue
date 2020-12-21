<template>
  <div class="formcenter">
    
    <el-form :model="ruleForm" ref="ruleForm2" :rules=rules
             :label-width="ruleForm.labelWidth || '80px'"
             :label-position="ruleForm.labelPosition || 'left'">
      <el-form-item label="手机号：" prop="phone">
        <el-input v-model="ruleForm.phone"></el-input>
      </el-form-item>
      <el-form-item label="用户名：" prop="userName">
        <el-input v-model="ruleForm.userName"></el-input>
      </el-form-item>
      <el-form-item label="密码：" prop="password">
        <el-input v-model="ruleForm.password" show-password></el-input>
      </el-form-item>
      <el-form-item label="确认密码：" prop="passwordconfirm">
        <el-input v-model="ruleForm.passwordconfirm" show-password></el-input>
      </el-form-item>

      <el-form-item label="姓名：" prop="name">
        <el-input v-model="ruleForm.name" ></el-input>
      </el-form-item>

      <el-button type="primary" style="width:80%;margin-bottom:30px;margin-left:100px;" @click.native.prevent="registeruser">注册</el-button>
    </el-form>
  </div>
</template>

<script> 
import { validUsername } from '@/utils/validate'
import { register  } from '@/api/user'
import { debounce } from '@/utils';
export default {
  name: 'register', 
    data() {
     var validatePass = (rule, value, callback) => {
        if (value === '') {
            
          callback(new Error('请再次输入密码'))
        } else if (value !== this.ruleForm.password) {
          callback(new Error('两次输入密码不一致!'))
        } else {
          callback()
        }
      }
    return {
     
      ruleForm: {
        labelWidth: '100px',
        labelPosition: 'right',
        phone: '',
        password: '',
        passwordconfirm:''
      },
      rules: {
        phone: [{ required: true, message: '手机号不能为空' }, { pattern: /^1[12345789]\d{9}$/, message: '请输入正确的手机号码' }]
        , userName: [{ required: true, message: '用户名不能为空' }]
        , password: [{ required: true, message: '密码不能为空' },
          { pattern: /^(\w){6,20}$/, message: '只能输入6-20个字母、数字、下划线' }]
        , passwordconfirm: [{ required: true, message: '确认密码不能为空' }, { validator: validatePass, trigger: 'blur' }]
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

        this.$refs.ruleForm2.validate((valid) => { 
          if (valid) {
            register(owner.ruleForm)
              .then(response => {
                if (response.success) {
                  owner.$message({
                    message: '恭喜你，注册成功！',
                    type: 'success'
                  });
                  this.$router.push({ name: '/login', query: {} })
                }
                else {
                  owner.$message.error(response.message);
                }
              })
          }
        });
     
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
