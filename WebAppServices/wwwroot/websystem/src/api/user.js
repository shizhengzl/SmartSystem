import request from '@/utils/request'
import { debounce } from '@/utils';

export function login(data) {
  //return request({
  //  url: '/vue-element-admin/user/login',
  //  method: 'post',
  //  data
  //})
  return request({
    url: 'http://localhost:5000/api/Users/Login',
    method: 'post',
    data
  })
}

export function getInfo(token) {
 
  return request({
    url: '/vue-element-admin/user/info',
    method: 'get',
    params: { token }
  })
}

export function logout() {
  return request({
    url: '/vue-element-admin/user/logout',
    method: 'post'
  })
}


export function register(data) {
  return request({
    url: '/api/Users/Register',
    method: 'post',
    data
  })
}

export function getUserInfo() {
  return request({
    url: 'http://localhost:5000/api/Users/GetUserInfo',
    method: 'post'
  })

}



export function getHeader(data) {
  return request({
    url: 'http://localhost:5000/api/Users/GetHeader',
    method: 'post',
    data
  })

}

export function GetResult(data) {
  return request({
    url: 'http://localhost:5000/api/Users/GetResult',
    method: 'post',
    data
  })
}


export function Save(data) {
  return request({
    url: 'http://localhost:5000/api/Users/Save',
    method: 'post',
    data
  })
}


export function Remove(data) {
  return request({
    url: 'http://localhost:5000/api/Users/Remove',
    method: 'post',
    data
  })
}
