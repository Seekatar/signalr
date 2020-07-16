import { HubConnectionBuilder, LogLevel } from "@aspnet/signalr"
import axios from "axios"

// CORS issue w/o https
const url = "https://localhost:5001/messagehub";

export default {
  userId: "wilma",
  install(Vue: any) {

    const connection = new  HubConnectionBuilder()
      .withUrl(url, {
        accessTokenFactory: async () => {
          // called before each SignalR request so can renew if needed here
          // see https://docs.microsoft.com/en-us/aspnet/core/signalr/authn-and-authz?view=aspnetcore-3.1
          console.log(`message-hub.ts connecting with userid of ${this.userId}`);
          const response = await axios.get(`https://localhost:5001/message/jwt?userId=${this.userId}`);
          return response.data;
        }
      })
      //?? .withAutomaticReconnect()
      .configureLogging(LogLevel.Debug)
      .build();

    // register $messageHub on all Vue components
    const messageHub = new Vue();
    Vue.prototype.$messageHub = messageHub;

    connection.on('ReceiveMessage', (message: any) => {
      messageHub.$emit('new-message', { ...message })
    });

    // start with reconnect logic
    let startedPromise
    function start() {
      startedPromise = connection.start().catch(err => {
        console.error('Failed to connect with hub', err)
        return new Promise((resolve, reject) =>
          setTimeout(() => start().then(resolve).catch(reject), 5000))
      })
      return startedPromise
    }
    connection.onclose(() => start())

    start()
  }
}
