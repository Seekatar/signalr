import { HubConnectionBuilder, LogLevel } from "@aspnet/signalr"

// CORS issue w/o https
const url = "https://localhost:5001/messagehub";

export default {
  install(Vue: any) {
    const connection = new HubConnectionBuilder()
      .withUrl(url)
      .configureLogging(LogLevel.Debug)
      .build();

    // register $messageHub on all Vue components
    const messageHub = new Vue();
    Vue.prototype.$messageHub = messageHub;

    connection.on('ReceiveMessage', (message: any ) => {
      messageHub.$emit('new-message', {...message})
    });

    // start with reconnect logic
    let startedPromise
    function start () {
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
