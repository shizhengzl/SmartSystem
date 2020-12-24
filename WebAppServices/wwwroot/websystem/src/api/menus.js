import request from '@/utils/request'
 


export function getHeader(data) {
  return request({
    url: '/api/Menus/GetHeader',
    method: 'post',
    data
  })

}

export function GetResult(data) {
  return request({
    url: '/api/Menus/GetResult',
    method: 'post',
    data
  })
}


export function Save(data) {
  return request({
    url: '/api/Menus/Save',
    method: 'post',
    data
  })
}


export function Remove(data) {
  return request({
    url: '/api/Menus/Remove',
    method: 'post',
    data
  })
}



export function GetTree(data) {
  return request({
    url: '/api/Menus/GetTree',
    method: 'post',
    data
  })
}


export function GetCompanyTree(data) {
  return request({
    url: '/api/Menus/GetCompanyTree',
    method: 'post',
    data
  })
}

