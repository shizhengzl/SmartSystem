import { asyncRoutes, constantRoutes } from '@/router'
import router from '@/router'
import Layout from '@/layout'
export function resolveRoutes(routes) {
  return routes.map(route => {
    if (!route.component || route.component === 'Layout') {
      route.redirec = 'noRedirect'
      route.component = Layout
    } else {
      delete route.children
      delete route.redirect
      delete route.id
      delete route.alwaysShow
      // 前面+''解决编译警告：Critical dependency: the request of a dependency is an expression
      // route.component = () => import(`${route.component}`)
      const path = route.component

      route.component = (resolve) => require([`@/views${path}.vue`], resolve)
    }
    //
    if (route.children && route.children.length > 0) {
      route.children = resolveRoutes(route.children)
    }
    return route
  })
}
/**
 * Use meta.role to determine if the current user has permission
 * @param roles
 * @param route
 */
function hasPermission(roles, route) {
  if (route.meta && route.meta.roles) {
    return roles.some(role => route.meta.roles.includes(role))
  } else {
    return true
  }
}

/**
 * Filter asynchronous routing tables by recursion
 * @param routes asyncRoutes
 * @param roles
 */
export function filterAsyncRoutes(routes, roles) {
  const res = []

  routes.forEach(route => {
    const tmp = { ...route }
    if (hasPermission(roles, tmp)) {
      if (tmp.children) {
        tmp.children = filterAsyncRoutes(tmp.children, roles)
      }
      res.push(tmp)
    }
  })

  return res
}

const state = {
  routes: [],
  addRoutes: []
}

const mutations = {
  SET_ROUTES: (state, routes) => {
    state.addRoutes = routes
    state.routes = constantRoutes.concat(routes)
  }
}

const actions = {
  generateRoutes({ commit }, roles) {
    return new Promise(resolve => {
      // let accessedRoutes
      // accessedRoutes = asyncRoutes || []

      // if (roles.includes('admin')) {
      //  accessedRoutes = asyncRoutes || []
      // } else {
      //  accessedRoutes = filterAsyncRoutes(asyncRoutes, roles)
      // }

      let accessedRoutes = roles.router

      accessedRoutes = resolveRoutes(accessedRoutes)

      // accessedRoutes.push({ path: '*', redirect: '/404', hidden: true })

      // router.addRoutes(accessedRoutes)
      commit('SET_ROUTES', accessedRoutes)
      resolve(accessedRoutes)
    })
  }
}

export default {
  namespaced: true,
  state,
  mutations,
  actions
}
