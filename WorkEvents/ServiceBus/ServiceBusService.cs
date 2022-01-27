using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;

namespace WorkEvents.ServiceBus
{

    public class ServiceBusService
    {
        // connection string to your Service Bus namespace
        static string connectionString = "<<service-bus-connection>>";

        // name of your Service Bus queue
        static string topicName = "techtalk";

        // the client that owns the connection and can be used to create senders and receivers
        static ServiceBusClient client;

        // the sender used to publish messages to the queue
        static ServiceBusSender sender;

        public async static Task SendMessage(string message)
        {
            // The Service Bus client types are safe to cache and use as a singleton for the lifetime
            // of the application, which is best practice when messages are being published or read
            // regularly.
            //
            // Create the clients that we'll use for sending and processing messages.
            client = new ServiceBusClient(connectionString);
            sender = client.CreateSender(topicName);

            // create a batch 
            using ServiceBusMessageBatch messageBatch = await sender.CreateMessageBatchAsync();

            if (!messageBatch.TryAddMessage(new ServiceBusMessage(message)))
            {
                // if it is too large for the batch
                throw new Exception($"The message {message} is too large to fit in the batch.");
            }

            try
            {
                // Use the producer client to send the batch of messages to the Service Bus queue
                await sender.SendMessagesAsync(messageBatch);
                Console.WriteLine($"Mensagem publicada na fila...");
            }
            finally
            {
                // Calling DisposeAsync on client types is required to ensure that network
                // resources and other unmanaged objects are properly cleaned up.
                await sender.DisposeAsync();
                await client.DisposeAsync();
            }

        }
    }
}