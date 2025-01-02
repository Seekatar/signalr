<template>
  <div class="message-form">
    <h2>Send a Message</h2>
    <form @submit.prevent="sendMessage">
      <div>
        <label for="message">Message:</label>
        <textarea id="message" v-model="message" placeholder="Enter your message" required></textarea>
      </div>

      <div>
        <label for="type">Message Type:</label>
        <select id="type" v-model="messageType" required>
          <option value="info">Info</option>
          <option value="success">Success</option>
          <option value="warning">Warning</option>
          <option value="error">Error</option>
        </select>
      </div>

      <div>
        <label for="userId">User ID:</label>
        <input type="text" id="userId" v-model="userId" placeholder="Enter user Id or @group" />
      </div>

      <button type="submit">Send Message</button>
    </form>

    <p v-if="responseMessage">{{ responseMessage }}</p>

    <Toast v-if="toastMessage" :message="toastMessage" :timestamp="toastTimestamp" :type="toastType" />
  </div>
</template>

<script lang="ts">
import axios from "axios";
import { HubConnectionBuilder } from '@microsoft/signalr';
import Toast from './Toast.vue';

export default {
  name: 'MessageSender',
  components: {
    Toast,
  },
  data() {
    return {
      message: '',
      messageType: 'info',
      userId: '',
      responseMessage: '',
      toastMessage: '',
      toastTimestamp: '',
      toastType: 'info',
      connection: null,
    };
  },
  async mounted() {
    console.log('MessageSender mounted');
    // Set up SignalR connection
    this.connection = new HubConnectionBuilder()
      .withUrl('https://localhost:5000/messagehub')
      .withAutomaticReconnect()
      .build();

    try {
      await this.connection.start();
      console.log('SignalR connected.');

      // Listen for ReceiveMessage events
      this.connection.on('ReceiveMessage', (msg) => {
        this.showToast(msg);
      });
    } catch (error) {
      console.error('Error connecting to SignalR:', error);
    }
  },
  methods: {
    async sendMessage() {
      try {
        let response = null;
        if (this.userId) {
          response = await axios.get(`https://localhost:5000/message/send-to?msg=${this.message}&type=${this.messageType}&userId=${this.userId}`);
        } else {
          response = await axios.get(`https://localhost:5000/message?msg=${this.message}&type=${this.messageType}`);
        }
        this.responseMessage = 'Message sent successfully: ' + response.data;
      } catch (error) {
        console.error('Error sending message:', error);
        this.responseMessage = 'Failed to send the message. Please try again.';
      }
      // Clear the responseMessage after 5 seconds
      setTimeout(() => {
        this.responseMessage = '';
      }, 5000);
    },
    showToast(message) {
      this.toastMessage = message.text;
      this.toastType = message.type;
      this.toastTimestamp = message.timestamp;

      setTimeout(() => {
        this.toastMessage = '';
      }, 5000); // Clear toast after 5 seconds
    },
  },
};

</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>
.message-form {
  width: 400px;
  margin: 0 auto;
}

label {
  display: block;
  margin: 0.5rem 0 0.2rem;
}

textarea,
input,
select {
  width: 100%;
  padding: 0.5rem;
  margin-bottom: 1rem;
  border: 1px solid #ccc;
  border-radius: 4px;
}

button {
  padding: 0.5rem 1rem;
  background-color: #007bff;
  color: white;
  border: none;
  border-radius: 4px;
  cursor: pointer;
}

button:hover {
  background-color: #0056b3;
}

p {
  margin-top: 1rem;
  max-width: 400px;
  color: green;
}
/* .response-message {
  margin-top: 1rem;
  padding: 0.5rem;
  background-color: #f8f9fa;
  border: 1px solid #ccc;
  border-radius: 4px;
  color: #333;
  word-wrap: break-word;
  word-break: break-word;
} */
</style>
