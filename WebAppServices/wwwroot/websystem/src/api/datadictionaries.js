import request from '@/utils/request'

export function getHeader(data) {
  return request({
    url: '/api/DataBaseConnection/GetHeader',
    method: 'post',
    data
  })
}

export function GetResult(data) {
  return request({
    url: '/api/DataBaseConnection/GetResult',
    method: 'post',
    data
  })
}

export function Save(data) {
  return request({
    url: '/api/DataBaseConnection/Save',
    method: 'post',
    data
  })
}

export function Remove(data) {
  return request({
    url: '/api/DataBaseConnection/Remove',
    method: 'post',
    data
  })
}

export function datadictionariesgetdatabase() {
  return request({
    url: '/api/DataBaseConnection/GetDataBaseList', // 假地址 自行替换
    method: 'get'
  })
}

export function datadictionariesgettables(id) {
  return request({
    url: '/api/DataBaseConnection/GetDataBaseTableList/' + id, // 假地址 自行替换
    method: 'get'
  })
}

export function datadictionariesgetcolumns(table) {
  return request({
    url: '/api/DataBaseConnection/GetTableColumnList/' + table, // 假地址 自行替换
    method: 'get'
  })
}

export function datadictionariessetextendedproperty(table) {
  return request({
    url: '/api/DataBaseConnection/SetExtendedproperty/' + table, // 假地址 自行替换
    method: 'get'
  })
}

export function parsersql(data) {
  return request({
    url: '/api/Values/ParserSQL', // 假地址 自行替换
    method: 'post',

    data
  })
}

export function parsersqlformat(data) {
  return request({
    url: '/api/Values/ParserSQLFormat', // 假地址 自行替换
    method: 'post',
    data
  })
}

export function settabledescription(table) {
  return request({
    url: '/api/DataBaseConnection/SetTableExtendedproperty/' + table, // 假地址 自行替换
    method: 'get'
  })
}
