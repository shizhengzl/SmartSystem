import request from '@/utils/request' 


export function getHeader(data) {
  return request({
    url: '/api/RequestResponseLog/GetHeader',
    method: 'post',
    data
  })

}

export function GetResult(data) {
  return request({
    url: '/api/RequestResponseLog/GetResult',
    method: 'post',
    data
  })
}


export function Save(data) {
  return request({
    url: '/api/RequestResponseLog/Save',
    method: 'post',
    data
  })
}


export function Remove(data) {
  return request({
    url: '/api/RequestResponseLog/Remove',
    method: 'post',
    data
  })
}
