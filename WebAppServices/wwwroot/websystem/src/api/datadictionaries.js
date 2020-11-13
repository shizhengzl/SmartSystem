import request from '@/utils/request'

export function datadictionariesgetdatabase() {
  return request({
    url: 'http://localhost:5000/api/DataBaseConnection/GetDataBaseList', // 假地址 自行替换
    method: 'get'
  })
}

export function datadictionariesgettables(id) {
  return request({
    url: 'http://localhost:5000/api/DataBaseConnection/GetDataBaseTableList/' + id, // 假地址 自行替换
    method: 'get'
  })
}

export function datadictionariesgetcolumns(table) {
  return request({
    url: 'http://localhost:5000/api/DataBaseConnection/GetTableColumnList/' + table, // 假地址 自行替换
    method: 'get'
  })
}

export function datadictionariessetextendedproperty(table) {
  return request({
    url: 'http://localhost:5000/api/DataBaseConnection/SetExtendedproperty/' + table, // 假地址 自行替换
    method: 'get'
  })
}

export function parsersql(data) {
  return request({
    url: 'http://localhost:5000/api/Values/ParserSQL', // 假地址 自行替换
    method: 'post',

    data
  })
}

export function parsersqlformat(data) {
  return request({
    url: 'http://localhost:5000/api/Values/ParserSQLFormat', // 假地址 自行替换
    method: 'post',
    data
  })
}


export function settabledescription(table) {
  return request({
    url: 'http://localhost:5000/api/DataBaseConnection/SetTableExtendedproperty/' + table, // 假地址 自行替换
    method: 'get'
  })
}
