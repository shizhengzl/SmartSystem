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
