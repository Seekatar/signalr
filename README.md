# SignalR Sample ASP.NET Core 9.0 to Vue

> ⚠ Note the Vue code is stale and hasn't been updated. Using my [SignalRLogger](https://github.com/MrSeekatar/SignalRLogger) as a client works with the current server code.

![.NET Core](https://github.com/Seekatar/signalr/workflows/.NET%20Core/badge.svg)
![Vue CI](https://github.com/Seekatar/signalr/workflows/Vue%20CI/badge.svg)

Sample project that uses a .NET Core ASP.NET project for a server that uses SignalR to send messages to a VueJs client.

Started with older sample code from [here](https://www.dotnetcurry.com/aspnet-core/1480/aspnet-core-vuejs-signalr-app). Updated in December 2024 to .NET 9.

## Testing the Server

The [server/server.http](server/server.http) has calls to the server to test the SignalR hub.

- /message/jwt - gets a JWT for the SignalR client to use
- /message - sends a message to all SignalR clients
- /message/sent-to - sends a message to one SignalR client, or if `@` is prefixed, it will send to clients in that group

## GitHub Actions

This uses GitHub Actions to build the apps

### Links

- [Configuring a Workflow](https://docs.github.com/en/actions/configuring-and-managing-workflows/configuring-a-workflow)
- [Variables](https://docs.github.com/en/actions/configuring-and-managing-workflows/using-variables-and-secrets-in-a-workflow)
- [Syntax Reference](https://docs.github.com/en/actions/reference/workflow-syntax-for-github-actions)
- [Build Machines](https://docs.github.com/en/actions/reference/virtual-environments-for-github-hosted-runners)
