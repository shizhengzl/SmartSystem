import request from '@/utils/request' 


export function getHeader(data) {
  return request({
    url: '/api/TableAreaData/GetHeader',
    method: 'post',
    data
  })

}

export function GetResult(data) {
  return request({
    url: '/api/TableAreaData/GetResult',
    method: 'post',
    data
  })
}


export function Save(data) {
  return request({
    url: '/api/TableAreaData/Save',
    method: 'post',
    data
  })
}


export function SaveList(data) {
  return request({
    url: '/api/TableAreaData/SaveList',
    method: 'post',
    data
  })
}


export function Remove(data) {
  return request({
    url: '/api/TableAreaData/Remove',
    method: 'post',
    data
  })
}
