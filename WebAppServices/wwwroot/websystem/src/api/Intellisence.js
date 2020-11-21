import request from '@/utils/request'
import { debounce } from '@/utils';


export function getHeader(data) {
  return request({
    url: '/api/Intellisence/GetHeader',
    method: 'post',
    data
  })

}

  export function GetResult(data) {
    return request({
      url: '/api/Intellisence/GetResult',
      method: 'post',
      data
    }) 
  }


export function Save(data) {
  return request({
    url: '/api/Intellisence/Save',
    method: 'post',
    data
  })
}


export function Remove(data) {
  return request({
    url: '/api/Intellisence/Remove',
    method: 'post',
    data
  })
}
