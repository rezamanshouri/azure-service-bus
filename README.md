# Azure Service Bus .NET Core Sample

This is a sample code for implementing a producer and a consumer for Azure Service Bus using _Azure.Messaging.ServiceBus_ package. This code is based on [Service Bus Documentation]. The solution includes two main projects:
- ServiceBusProducer: This is a ASP .NET web application with a few endpoints to insert various messages into the topic.
- ServiceBusConsumer: A long running background task to consume messages from a subscription

## Running Application
To Run the application, first fill out the value for fileds under _ServiceBus_ in `appsettings.json` file with values from your Azure resource.


[Service Bus Documentation]: <https://docs.microsoft.com/en-us/dotnet/api/overview/azure/messaging.servicebus-readme?view=azure-dotnet>
