
# Passport API Sample App

## Getting Started
This project provides a simple demonstration of how to use some of the features of the IdRamp [Passport API](https://passport-api.idramp.com/) to support SSI (Self-Sovereign Identity) cloud-based scenarios based on the [Hyperledger Aries](https://github.com/hyperledger/aries-rfcs) protocols and [Hyperledger Indy](https://github.com/hyperledger/indy-sdk).

The Passport API is described by an OpenAPI 3 specification available at the documentation site [here](https://passport-api.idramp.com/swagger/index.html).

Features demonstrated by this project include the following:
* creating a connection via a QR code that can be scanned by an edge agent
* issuing a credential over an existing connection
* sending a proof request over an existing connection, receiving the proof and viewing attribute values
* sending a [Basic Message](https://github.com/hyperledger/aries-rfcs/tree/master/features/0095-basic-message) over a connection
* creating a connectionless proof request displayed as a QR code, receiving the proof and viewing attribute values
* creating a connectionless credential that is issued when scanning a QR code

The code exercising these features also highlights some of the options the API provides to adjust the behavior of these various pieces of functionality.

To test, you will need a wallet app to act as the edge agent--try out IdRamp's [Passport](https://idramp.com/passport-identity-wallet/) identity wallet!

### Get an API key
To start, you'll need an API key for the Passport API. This key provides access to your cloud wallet, and is required to get started playing with the API. Please visit idramp.com for more information!

### Environment Setup
When you have a cloud wallet created, and you have received an API key, this demo expects to find that in your ASP.net Core configuration as a value with key `passportApiBearerToken`. It can be set in various places, e.g. environment variables, but one easy way to get started with local testing is to add it to your `appsettings.<environment>.json` file or to use Visual Studio's user secrets tool:

```json
{
  "passportApiBearerToken": "<your wallet's secret token>"
}
```

You may need to select the `AspNetDemo` launch profile, and feel free to customize `applicationUrl` in `launchSettings.json` as you see fit to test locally and bind to the correct host/port.

At this point, hitting 'F5' to run in Visual Studio will launch your browser pointed at the `applicationUrl` that is set, and you are ready to go!

## Project Structure
The project is organized with Extensions, Models, Pages and Services making up the core of the code one would change to try out different features. Pages are written using ASP.net Razor Pages, and the project uses Bootstrap for simple visual styling.

Static assets are in `wwwroot`, and the file relevant to the application is `embed.js` which performs front-end waiting during certain actions so as to show results or redirect when a connection is completed or a proof is returned in response to a proof request. There are also some QR code generation and embedding helper functions.

Key external dependencies are `Newtonsoft.Json` and `NSwag.ApiDescription.Client`, the latter of which is used to generate an OpenAPI client. Via the project's "Connected Services," a `swagger.json` file is downloaded and stored in the `/OpenAPIs` directory.

The `/ids` directory that is created on demand is used as a simple file store for some IDs, such as credential definition and proof IDs. This way these can be looked up quickly without needing to recreate them on each app execution, which can involve writing to a ledger and can take a noticeable amount of time.

## Advanced Settings

By default, the sample app points to the production [Passport API](https://passport-api.idramp.com/), but if you need to point to another version of the API for whatever reason, this can also be configured in your environment by way of a value for the `apiEndpoint` key. For example, in `appsettings.<environment>.json`:

```json
{
  "apiEndpoint": "<URL to API>",
...
}
```

## Troubleshooting

If you are seeing issues, here are a few things to try, depending on the error:

* Refresh the OpenAPI specification. Go to the `Connected Services` UI in VS to refresh the OpenAPI spec, which will download a new `swagger.json` from the location specified by the `<SourceUri>` element in the `.csproj`, and then build the solution--it will generate new client code. Alternatively, you can edit or manually download this file from the documentation site.
* If you have been trying out this sample app with various wallets, make sure you are using the correct `passportApiBearerToken`.
* If you have been using this sample app on multiple ledgers (networks), try clearing files out of the `/ids` directory, as you may be trying to use a cached ID that is no longer valid on your current ledger.
* If all else fails, feel free to create an issue in this Github project, or visit idramp.com.