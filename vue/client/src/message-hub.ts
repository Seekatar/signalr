import { HubConnectionBuilder, LogLevel } from "@aspnet/signalr"

const url = "https://localhost:5001/messagehub";

export default {
  install(Vue: any) {
    const connection = new HubConnectionBuilder()
      .withUrl(url)
      .configureLogging(LogLevel.Debug)
      .build();
    
    const messageHub = new Vue();
    Vue.prototype.$messageHub = messageHub;

    connection.on('ReceiveMessage', (message: any ) => {
      messageHub.$emit('new-message', {...message})
    });

    connection.start();
}
}
