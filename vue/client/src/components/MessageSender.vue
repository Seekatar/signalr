<template>
  <div class="hello">
    <v-container>
      <v-row>
        <v-col cols="8">
          <v-text-field v-model="userId" label="Send to userId, @INS1 for group, empty for all"></v-text-field>
        </v-col>
      </v-row>
      <v-row>
        <v-col cols="4">
          <v-text-field v-model="message" label="Message"></v-text-field>
        </v-col>
        <v-col cols="4">
          <v-combobox
            v-model="msgType"
            :items="msgTypes"
            label="Select a message type"
          >
          </v-combobox>
        </v-col>
      </v-row>
      <v-row>
        <v-col cols="8">
          <v-btn v-on:click="sendMessage" :disabled='message.length == 0'>Send Message</v-btn>
        </v-col>
      </v-row>
    </v-container>
  </div>
</template>

<script lang="ts">
import { Component, Prop, Vue } from "vue-property-decorator";
import axios from "axios";

@Component
export default class MessageSender extends Vue {
  @Prop() private msg!: string;
  message = "";
  userId = "";

  // this just "happen" to match up with
  // Vuetify colors for snackbar
  msgTypes = ["info", "success", "warning", "error"];
  msgType = "info";

  sendMessage() {
    console.log(`Sending message ${this.message} to ${this.userId} of type ${this.msgType}`);
    if (this.userId.length <= 0) {
      axios.get(`https://localhost:5001/message?msg=${this.message}&type=${this.msgType}`);
    } else {
      axios.get(
        `https://localhost:5001/message/send-to?msg=${this.message}&userId=${this.userId}&type=${this.msgType}`
      );
    }
  }
}
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>
h3 {
  margin: 40px 0 0;
}
ul {
  list-style-type: none;
  padding: 0;
}
li {
  display: inline-block;
  margin: 0 10px;
}
a {
  color: #42b983;
}
</style>
