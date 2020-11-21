<template>
  <!-- @mousedown.stop  阻止冒泡事件-->
  <!-- @contextmenu.prevent 阻止右键的默认事件 -->
  <div id="dropMenu"
       :style="style"
       style="display: block;"
       v-show="show"
       @mousedown.stop
       @contextmenu.prevent>
    <slot></slot>
  </div>
</template>
<script>
  export default {
    name: "menuContext",
    data() {
      return {
        triggerShowFn: () => {
        },
        triggerHideFn: () => {
        },
        x: null,
        y: null,
        style: {},
        binded: false
      }
    },
    props: {
      target: null,
      show: Boolean
    },
    mounted() {
      this.bindEvents()
    },
    watch: {
      show(show) {
        if (show) {
          this.bindHideEvents()
        } else {
          this.unbindHideEvents()
        }
      },
      target(target) {
        this.bindEvents()
      }
    },
    methods: {
      // 初始化事件
      bindEvents() {
        this.$nextTick(() => {
          var that = this
          if (!this.target || this.binded) return
          this.triggerShowFn = this.contextMenuHandler.bind(this)
          this.target.addEventListener('contextmenu', this.triggerShowFn)
          //this.binded = true
        })
      },
      // 取消绑定事件
      unbindEvents() {
        if (!this.target) return
        this.target.removeEventListener('contextmenu', this.triggerShowFn)
      },
      // 绑定隐藏菜单事件
      bindHideEvents() {
        this.triggerHideFn = this.clickDocumentHandler.bind(this)
        document.addEventListener('mousedown', this.triggerHideFn)
        document.addEventListener('mousewheel', this.triggerHideFn)
      },
      // 取消绑定隐藏菜单事件
      unbindHideEvents() {
        document.removeEventListener('mousedown', this.triggerHideFn)
        document.removeEventListener('mousewheel', this.triggerHideFn)
      },
      // 鼠标按压事件处理器
      clickDocumentHandler(e) {
        this.$emit('update:show', false) //隐藏
      },
      // 右键事件事件处理
      contextMenuHandler(e) {
        e.target.click()//这个是因为我需要获取tree节点的数据，所以我通过点击事件去获取数据
        this.x = e.clientX - 240
        this.y = e.clientY - 110
        this.layout()
        this.$emit('update:show', true)  //显示
        e.preventDefault()
        e.stopPropagation()

        this.$emit('targetElement', e.target) //我还要获取右键的DOM元素进行操作
      },
      // 布局
      layout() {
        this.style = {
          left: this.x + 'px',
          top: this.y + 'px'
        }
      }
    }
  }
</script>
<style lang="scss">
  #dropMenu {
    position: absolute;
    margin: 0;
    padding: 0;
    width: 80px;
    height: auto;
    border: 1px solid #ccc;
    border-radius: 4px;
    ul

  {
    list-style: none;
    margin: 0;
    padding: 0;
    li

  {
    width: 100%;
    text-align: center;
    height: 30px;
    line-height: 30px;
    background: #eee;
    margin-bottom: 1px;
    cursor: pointer;
  }

  }
  }
</style>
