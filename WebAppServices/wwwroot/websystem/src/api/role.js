import request from '@/utils/request'

export function getRoutes() {
  return request({
    url: '/vue-element-admin/routes',
    method: 'get'
  })
}

export function getRoles() {
  return request({
    url: '/vue-element-admin/roles',
    method: 'get'
  })
}

export function addRole(data) {
  return request({
    url: '/vue-element-admin/role',
    method: 'post',
    data
  })
}

export function updateRole(id, data) {
  return request({
    url: `/vue-element-admin/role/${id}`,
    method: 'put',
    data
  })
}

export function deleteRole(id) {
  return request({
    url: `/vue-element-admin/role/${id}`,
    method: 'delete'
  })
}



export function getHeader(data) {
  return request({
    url: 'http://localhost:5000/api/Roles/GetHeader',
    method: 'post',
    data
  })

}

export function GetResult(data) {
  return request({
    url: 'http://localhost:5000/api/Roles/GetResult',
    method: 'post',
    data
  })
}


export function Save(data) {
  return request({
    url: 'http://localhost:5000/api/Roles/Save',
    method: 'post',
    data
  })
}


export function Remove(data) {
  return request({
    url: 'http://localhost:5000/api/Roles/Remove',
    method: 'post',
    data
  })
}
