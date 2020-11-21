import request from '@/utils/request' 


export function getHeader(data) {
  return request({
    url: '/api/TableArea/GetHeader',
    method: 'post',
    data
  })

}

export function GetResult(data) {
  return request({
    url: '/api/TableArea/GetResult',
    method: 'post',
    data
  })
}


export function Save(data) {
  return request({
    url: '/api/TableArea/Save',
    method: 'post',
    data
  })
}


export function Remove(data) {
  return request({
    url: '/api/TableArea/Remove',
    method: 'post',
    data
  })
}
