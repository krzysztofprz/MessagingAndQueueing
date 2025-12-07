using dotenv.net;

DotEnv.Load();
var variables = DotEnv.Read();

await EventGridMessaging.SendEventAsync(variables);
await EventGridMessaging.ReadEventFromStorageQueue(variables);