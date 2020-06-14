import Vue from "vue";
import App from "./App.vue";
import router from "./router";
import "./message-hub";
import messageHub from "./message-hub";

Vue.config.productionTip = false;

new Vue({
  router,
  render: h => h(App)
}).$mount("#app");

Vue.use(messageHub);
