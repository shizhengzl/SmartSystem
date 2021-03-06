import Cookies from 'js-cookie'
const TokenKey = 'token' 
const UserInfo = 'userinfo' 
export function getToken() {
  return Cookies.get(TokenKey)
}

export function setToken(token) {
  return Cookies.set(TokenKey, token)
}

export function removeToken() {
  return Cookies.remove(TokenKey)
}



export function setUserinfo(userinfo) {
  return Cookies.set(UserInfo,userinfo)
}

export function getUserinfo() {
  return Cookies.get(UserInfo)
}


