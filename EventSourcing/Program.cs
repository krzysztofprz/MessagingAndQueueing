// See https://aka.ms/new-console-template for more information
using EventSourcing;

Console.WriteLine("Hello, World!");

await KurrentClient.CreateEvent("triggered 1 event");
await KurrentClient.CreateEvent("triggered 2 event");
await KurrentClient.CreateEvent("triggered 3 event");
await KurrentClient.CreateEvent("triggered 4 event");

await KurrentClient.ReadEvent();