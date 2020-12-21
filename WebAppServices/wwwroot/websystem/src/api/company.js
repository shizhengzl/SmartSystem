import request from '@/utils/request'
import { debounce } from '@/utils';


export function getHeader(data) {
  return request({
    url: '/api/Company/GetHeader',
    method: 'post',
    data
  })

}

  export function GetResult(data) {
    return request({
      url: '/api/Company/GetResult',
      method: 'post',
      data
    }) 
  }


export function Save(data) {
  return request({
    url: '/api/Company/Save',
    method: 'post',
    data
  })
}


export function Remove(data) {
  return request({
    url: '/api/Company/Remove',
    method: 'post',
    data
  })
}


