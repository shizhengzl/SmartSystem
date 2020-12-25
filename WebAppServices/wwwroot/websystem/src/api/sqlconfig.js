import request from '@/utils/request'
import { debounce } from '@/utils';


export function getHeader(data) {
  return request({
    url: '/api/SQLConfig/GetHeader',
    method: 'post',
    data
  })

}

  export function GetResult(data) {
    return request({
      url: '/api/SQLConfig/GetResult',
      method: 'post',
      data
    }) 
  }


export function Save(data) {
  return request({
    url: '/api/SQLConfig/Save',
    method: 'post',
    data
  })
}


export function Remove(data) {
  return request({
    url: '/api/SQLConfig/Remove',
    method: 'post',
    data
  })
}
