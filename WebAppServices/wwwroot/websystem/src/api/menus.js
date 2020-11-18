import request from '@/utils/request'
 


export function getHeader(data) {
  return request({
    url: 'http://localhost:5000/api/Menus/GetHeader',
    method: 'post',
    data
  })

}

export function GetResult(data) {
  return request({
    url: 'http://localhost:5000/api/Menus/GetResult',
    method: 'post',
    data
  })
}


export function Save(data) {
  return request({
    url: 'http://localhost:5000/api/Menus/Save',
    method: 'post',
    data
  })
}


export function Remove(data) {
  return request({
    url: 'http://localhost:5000/api/Menus/Remove',
    method: 'post',
    data
  })
}
