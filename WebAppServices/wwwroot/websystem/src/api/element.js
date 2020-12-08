import request from '@/utils/request' 
export function getHeader(data) {
  return request({
    url: '/api/Element/GetHeader',
    method: 'post',
    data
  }) 
}

export function GetResult(data) {
  return request({
    url: '/api/Element/GetResult',
    method: 'post',
    data
  })
} 

export function Save(data) {
  return request({
    url: '/api/Element/Save',
    method: 'post',
    data
  })
} 

export function Remove(data) {
  return request({
    url: '/api/Element/Remove',
    method: 'post',
    data
  })
}
 
