using System.Text;
using Azure.Messaging.EventGrid;
using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;

public static class EventGridMessaging
{
    public async static Task SendEventAsync(IDictionary<string, string> variables)
    {
        var topicEndpoint = variables["TOPIC_ENDPOINT"];
        var topicKey = variables["TOPIC_ACCESS_KEY"];

        if (string.IsNullOrEmpty(topicEndpoint) || string.IsNullOrEmpty(topicKey))
        {
            Console.WriteLine("Please set TOPIC_ENDPOINT and TOPIC_ACCESS_KEY in your .env file.");
            return;
        }

        EventGridPublisherClient client = new EventGridPublisherClient
            (new Uri(topicEndpoint),
            new Azure.AzureKeyCredential(topicKey));

        var eventGridEvent = new EventGridEvent(
            subject: "ExampleSubject",
            eventType: "ExampleEventType",
            dataVersion: "1.0",
            data: new { Message = "Hello, Event Grid!" }
        );

        await client.SendEventAsync(eventGridEvent);
        Console.WriteLine("Event sent successfully.");
    }

    public async static Task ReadEventFromStorageQueue(IDictionary<string, string> variables)
    {
        QueueClient client = new QueueClient(
            new Uri(variables["STORAGE_QUEUE_ENDPOINT"]),
            new Azure.Storage.StorageSharedKeyCredential(variables["STORAGE_ACCOUNT_NAME"], variables["STORAGE_ACCOUNT_KEY"]));
        
        QueueMessage[] messages = await client.ReceiveMessagesAsync();

        foreach (var message in messages)
        {
            var base64 = message.MessageText;

            var json = Encoding.UTF8.GetString(Convert.FromBase64String(base64));

            Console.WriteLine($"Decoded event: {json}");

            await client.DeleteMessageAsync(message.MessageId, message.PopReceipt);
        }
    }
}