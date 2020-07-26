<template>
<v-app>
  <div class="home">
    <MessageSender msg="Welcome to Your Vue.js App" />
  </div>
  </v-app>
</template>

<script>
// @ is an alias to /src
import MessageSender from "@/components/MessageSender.vue";

export default {
  name: "Home",
  components: {
    MessageSender
  }
};
</script>
