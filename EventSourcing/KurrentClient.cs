using Google.Protobuf;
using KurrentDB.Client;
using System.Text;
using System.Text.Json;

namespace EventSourcing
{
    public static class KurrentClient
    {
        const string streamName = "test-stream";
        const string connectionString = "kurrentdb://admin:changeit@localhost:2113?tls=false&tlsVerifyCert=false";

        static KurrentDBClient client = new KurrentDBClient(KurrentDBClientSettings.Create(connectionString));

        public static async Task CreateEvent(string eventBody)
        {
            var evt = new
            {
                EntityId = Guid.NewGuid().ToString("N"),
                ImportantData = eventBody
            };

            var eventData = new EventData(
                Uuid.NewUuid(),
                "TestEvent",
                JsonSerializer.SerializeToUtf8Bytes(evt));

            await client.AppendToStreamAsync(
                streamName,
                StreamState.Any,
                [eventData],
                cancellationToken: CancellationToken.None);
        }

        public static async Task ReadEvent()
        {
            var startPosition = client.ReadStreamAsync(
                Direction.Forwards,
                streamName,
                StreamPosition.Start,
                cancellationToken: CancellationToken.None);

            var endPostion = client.ReadStreamAsync(
                Direction.Backwards,
                streamName,
                StreamPosition.End,
                cancellationToken: CancellationToken.None);

            foreach (var e in await startPosition.ToListAsync())
            {
                Console.WriteLine(DecodeEvent(e));
            }

            foreach (var e in await endPostion.ToListAsync())
            {
                Console.WriteLine(DecodeEvent(e));
            }
        }

        private static string DecodeEvent(ResolvedEvent resolvedEvent)
        {
            return Encoding.UTF8.GetString(resolvedEvent.OriginalEvent.Data.ToArray());
        }
    }
}
