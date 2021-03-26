import request from '@/utils/request' 


export function getHeader(data) {
  return request({
    url: '/api/Document/GetHeader',
    method: 'post',
    data
  })

}

export function GetResult(data) {
  return request({
    url: '/api/Document/GetResult',
    method: 'post',
    data
  })
}


export function Save(data) {
  return request({
    url: '/api/Document/Save',
    method: 'post',
    data
  })
}


export function Remove(data) {
  return request({
    url: '/api/Document/Remove',
    method: 'post',
    data
  })
}
