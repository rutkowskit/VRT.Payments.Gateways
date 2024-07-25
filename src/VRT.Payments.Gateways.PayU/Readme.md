# VRT.Payments.Gateways.PayU

Basic PayU payment gateway integration for .net 8+ applications.

## Registering in DI container

To register PayU integration in your DI container, use the following `IServiceCollection` extension method:

`services.AddPayUPaymentService()`

## Configuration

The library uses  `options pattern`.
Options are read from `IConfiguration` by default.

### Configuring using appsettings.json

The sample configuration of the `PayU` gateway is provided below.
It is configured to default, public `PayU` Sandobx.

```json
{  
 "PaymentGateways": {
    "PayU": {
      "_comment": "Public Sandbox. If you have external endpoint for your notifications,set NotificationEndpoint value",
      "BaseApiUrl": "https://secure.snd.payu.com",
      "ClientId": "300746",
      "ClientSecret": "2ee86a66e5d97e3fadc400c9f19b065d",
      "SecondKey": "b6ca15b0d1020e8094d9b5f8d163db54",
      "PointOfSellId": "300746",
      "NotificationEndpointUrl": ""
    }
  }
}
```

> :bulb: `NotificationEndpointUrl` should be absolute url to your notification handling endpoint

If you do not want to use appsettings configuration, then you can also provide `IOptions<PayUOptions>` yourself during DI configuration.
