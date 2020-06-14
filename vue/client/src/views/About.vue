<template>
  <div class="about">
    <h1>This is an aboot page</h1>
    <a href="http://localhost:5000">Localhost</a>
  </div>
</template>

<script>
export default {
  props: {
    question: {
      type: Object,
      required: false
    }
  },
  created() {
    // Listen to score changes coming from SignalR events
    this.$messageHub.$on("new-message", this.onNewMessage);
    console.log("started listening...");
  },
  methods: {
    // This is called from the server through SignalR
    onNewMessage({ timestamp, senderUsername, text, title, type }) {
      console.log(timestamp);
      console.log(
        `>>>>Yay! Title: ${title} Text:${text} Timestamp:${timestamp} User:${senderUsername} Type:${type}`
      );
    }
  }
};
</script>
