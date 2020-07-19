<template>
  <v-app>
    <h1>This is a page with SignalR</h1>
    <div>
      <v-form>
        <v-container>
          <v-row>
            <v-col cols="6">
              <v-text-field v-model="userId" label="UserId"></v-text-field>
            </v-col>
            <v-col cols="6">
              <v-btn v-on:click="setUser()">Set User</v-btn>
            </v-col>
          </v-row>
        </v-container>
      </v-form>
    </div>
    <div>Jwt is {{ jwt }}</div>
    <v-snackbar v-model="snackbar" :timeout="timeout" :color="color">
      {{ text }}
      <template v-slot:action="{ attrs }">
        <v-btn dark text v-bind="attrs" @click="snackbar = false"
          >Close</v-btn
        >
      </template>
    </v-snackbar>
  </v-app>
</template>

<script>
import axios from "axios";
import messageHub from "../message-hub";

// matches C#
const messageTypes = {
  info: "info",
  success: "success",
  warning: "warning",
  error: "error"
};

export default {
  data: () => ({
    snackbar: false,
    text: "",
    timeout: -1,
    color: "success",
    jwt: "",
    userId: messageHub.userId,
    message: ""
  }),
  props: {
    question: {
      type: Object,
      required: false
    }
  },
  created() {
    if (this.$route.query.userId.length > 0) {
      this.userId = this.$route.query.userId
      messageHub.userId = this.userId
    }
    axios
      .get(`https://localhost:5001/message/jwt?userId=${this.userId}`)
      .then(response => {
        this.jwt = response.data;
      });

    // Listen to score changes coming from SignalR events
    this.$messageHub.$on(messageHub.newMessageName, this.onNewMessage);
    console.log("started listening...");
  },
  methods: {
    setUser() {
      console.log(`User set to ${this.userId}`);
      messageHub.userId = this.userId;
    },
    // This is called from the server through SignalR
    onNewMessage({ timestamp, senderUsername, text, title, type }) {
      console.log(timestamp);
      this.text = `${type}: ${text}`;
      this.snackbar = true;
      this.color = type; // should be Vuetify success/fail color
      if (type === messageTypes.info || type === messageTypes.success) {
        this.timeout = 3000;
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
