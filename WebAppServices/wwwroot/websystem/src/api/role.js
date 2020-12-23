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
    url: '/api/Roles/GetHeader',
    method: 'post',
    data
  })

}

export function GetResult(data) {
  return request({
    url: '/api/Roles/GetResult',
    method: 'post',
    data
  })
}


export function Save(data) {
  return request({
    url: '/api/Roles/Save',
    method: 'post',
    data
  })
}


export function Remove(data) {
  return request({
    url: '/api/Roles/Remove',
    method: 'post',
    data
  })
}



export function GetRoleUser(data) {
  return request({
    url: '/api/Roles/GetRoleUser',
    method: 'post',
    data
  })
}

export function GetRoleChoseUser(data) {
  return request({
    url: '/api/Roles/GetRoleChoseUser',
    method: 'post',
    data
  })
}
export function SaveRoleUser(data) {
  return request({
    url: '/api/Roles/SaveRoleUser',
    method: 'post',
    data
  })
}

export function RemoveRoleUser(data) {
  return request({
    url: '/api/Roles/RemoveRoleUser',
    method: 'post',
    data
  })
}

 
export function SaveGrant(data) {
  return request({
    url: '/api/Roles/SaveGrant',
    method: 'post',
    data
  })
}

export function GetRoleMenus(data) {
  return request({
    url: '/api/Roles/GetRoleMenus',
    method: 'post',
    data
  })
}
