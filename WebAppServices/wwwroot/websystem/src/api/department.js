import request from '@/utils/request'
import { debounce } from '@/utils';


export function getHeader(data) {
  return request({
    url: '/api/Department/GetHeader',
    method: 'post',
    data
  })

}

  export function GetResult(data) {
    return request({
      url: '/api/Department/GetResult',
      method: 'post',
      data
    }) 
  }


export function Save(data) {
  return request({
    url: '/api/Department/Save',
    method: 'post',
    data
  })
}


export function Remove(data) {
  return request({
    url: '/api/Department/Remove',
    method: 'post',
    data
  })
}

export function GetTree(data) {
  return request({
    url: '/api/Department/GetTree',
    method: 'post',
    data
  })
}


export function SaveGrant(data) {
  return request({
    url: '/api/Department/SaveGrant',
    method: 'post',
    data
  })
}

export function GetMenus(data) {
  return request({
    url: '/api/Department/GetMenus',
    method: 'post',
    data
  })
}


export function GetDepartmentUser(data) {
  return request({
    url: '/api/Department/GetDepartmentUser',
    method: 'post',
    data
  })
}


export function GetDepartmentChoseUser(data) {
  return request({
    url: '/api/Department/GetDepartmentChoseUser',
    method: 'post',
    data
  })
}


export function SaveDepartmentUser(data) {
  return request({
    url: '/api/Department/SaveDepartmentUser',
    method: 'post',
    data
  })
}


export function RemoveDepartmentUser(data) {
  return request({
    url: '/api/Department/RemoveDepartmentUser',
    method: 'post',
    data
  })
}
