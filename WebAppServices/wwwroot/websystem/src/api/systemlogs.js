import request from '@/utils/request'
import { debounce } from '@/utils';


export function getHeader(data) {
  return request({
    url: '/api/SystemLogs/GetHeader',
    method: 'post',
    data
  })

}

  export function GetResult(data) {
    return request({
      url: '/api/SystemLogs/GetResult',
      method: 'post',
      data
    }) 
  }


export function Save(data) {
  return request({
    url: '/api/SystemLogs/Save',
    method: 'post',
    data
  })
}


export function Remove(data) {
  return request({
    url: '/api/SystemLogs/Remove',
    method: 'post',
    data
  })
}
