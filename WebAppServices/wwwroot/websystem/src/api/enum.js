import request from '@/utils/request'
 
export function GetDataType(data) {
  return request({
    url: '/api/Enum/GetDataType',
    method: 'post',
    data
  }) 
}
