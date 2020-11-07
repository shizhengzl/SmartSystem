import request from '@/utils/request'
import { debounce } from '@/utils';


export function getHeader(data) {
  return request({
    url: 'http://localhost:5000/api/Intellisence/GetHeader',
    method: 'post',
    data
  })

}

  export function GetResult(data) {
    return request({
      url: 'http://localhost:5000/api/Intellisence/GetResult',
      method: 'post',
      data
    }) 
  }
