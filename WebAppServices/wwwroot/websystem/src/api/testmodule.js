import request from '@/utils/request'
 



export function getHeader(data) {
  return request({
    url: '/api/TestModule/GetHeader',
    method: 'post',
    data
  })

}

export function GetResult(data) {
  return request({
    url: '/api/TestModule/GetResult',
    method: 'post',
    data
  })
}


export function Save(data) {
  return request({
    url: '/api/TestModule/Save',
    method: 'post',
    data
  })
}


export function Remove(data) {
  return request({
    url: '/api/TestModule/Remove',
    method: 'post',
    data
  })
}
