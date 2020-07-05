<template>
  <div class="about">
    <h1>This is an aboot page</h1>
    <a href="https://localhost:5001/message?msg=From Vue" target="_blank">Localhost</a>
    <v-snackbar v-model="snackbar" :timeout="timeout" :color="color">
      {{ text }}

      <template v-slot:action="{ attrs }">
        <v-btn color="pink" text v-bind="attrs" @click="snackbar = false">
          Close
        </v-btn>
      </template>
    </v-snackbar>
  </div>
</template>

<script>
export default {
  data: () => ({
    snackbar: false,
    text: "",
    timeout: -1,
    color: "success"
  }),
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
      this.text = text;
      this.snackbar = true;
      this.color = type;
      if (type === "INFO") {
        this.timeout = 5000;
      } else {
        this.timeout = -1;
      }
      console.log(
        `>>>>Got a SignalR message! Type:${type} Title: ${title} Text:${text} Timestamp:${timestamp} User:${senderUsername}`
      );
    }
  }
};
</script>
