# Passport API Sample App

## Getting Started
This project provides a simple demonstration of how to use some of the features of the IdRamp [Passport API](https://passport-api.idramp.com/) to support SSI (Self-Sovereign Identity) cloud-based scenarios based on the [Aries](https://github.com/hyperledger/aries-rfcs) protocols and [Hyperledger Indy](https://github.com/hyperledger/indy-sdk). This API is described by an OpenAPI 3 specification available at the documentation site [here](https://passport-api.idramp.com/swagger/index.html).

It demonstrates creating a connection via a QR code that can be scanned by an edge agent as well as performing actions such as issuing a credential, proofing and sending a [Basic Message](https://github.com/hyperledger/aries-rfcs/tree/master/features/0095-basic-message) over that connection. It also shows how one could present connectionless proofs and connectionless credentials via a QR code.

To test, you will need a wallet app to act as the edge agent--try out IdRamp's [Passport](https://idramp.com/passport-identity-wallet/) identity wallet!

### Get an API key
To start, you'll need the API key that represents your wallet in order to get started playing with the API. Please visit idramp.com for more information!

### Environment Setup
When you have a cloud wallet created, and you have received an API key, this demo expects to find that in your ASP.net Core configuration as a value with key `passportApiBearerToken`. It can be set in various places, e.g. environment variables, but one easy way to get started with local testing is to add it to your `appsettings.<environment>.json` file or to use Visual Studio's user secrets tool:

```xml
{
  "passportApiBearerToken": "<your_wallet's_secret_token>"
}
```

By default, the sample app points to the production [Passport API](https://passport-api.idramp.com/), but if you need to point to another version of the API for whatever reason, this can also be configured in your environment by way of a value for the `apiEndpoint` key. For example, in `appsettings.<environment>.json`:

```xml
{
  "apiEndpoint": "<URL_to_API>",
...
}
```

You'll likely want to select the `AspNetDemo` launch profile, and feel free to customize `applicationUrl` in `launchSettings.json` as you see fit to test locally and bind to the correct host/port.

At this point, hitting 'F5' to run in Visual Studio will launch your browser pointed at the `applicationUrl` that is set, and you are ready to go!

## Project Structure
The project is organized in a fairly straightforward manner, with Extensions, Models, Pages and Services making up the core of the code one would change to try out different features. Pages are written using ASP.net Razor Pages, and the project uses Bootstrap for simple visual styling.

Static assets are in `wwwroot`, and the most interesting part there is the `embed.js` file which does some front-end waiting during certain actions, so as to show results or redirect when a connection is completed, or a proof is returned in response to a proof request. There also are some QR code generation and embedding helper functions.

Key external dependencies are `Newtonsoft.Json` and `NSwag.ApiDescription.Client`, the latter of which is used to generate an OpenAPI client. Via the project's "Connected Services," a `swagger.json` file is downloaded and stored in the `/OpenAPIs` directory.

The `/ids` directory that is created on demand is used as a simple file store for some IDs, such as credential definition and proof IDs. This way these can be looked up quickly without needing to recreate them on each app execution, which can involve writing to a ledger and can take a noticeable amount of time.

## Troubleshooting

If you are seeing issues, here are a few things to try, depending on the error:

* Refresh the OpenAPI specification. Go to the `Connected Services` UI in VS to refresh the OpenAPI spec, which will download a new `swagger.json`, and then build the solution--it will generate new client code. Alternatively, you can edit or manually download this file from the documentation site, but the easiest path is to let VS do it.
*  If you have been trying out this sample app with various wallets, make sure you are using the correct `passportApiBearerToken`.
* If you have been using this sample app on multiple ledgers (networks), try clearing files out of the `/ids` directory, as you may be trying to use a cached ID that is no longer valid on your current ledger.
* If all else fails, feel free to create an issue in this Github project, or visit idramp.com.