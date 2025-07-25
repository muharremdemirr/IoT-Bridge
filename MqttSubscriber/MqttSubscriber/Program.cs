using MQTTnet;
using System;
using System.Text;
using System.Threading.Tasks;

var factory = new MQTTnet.MqttClientFactory();
var client = factory.CreateMqttClient();

var options = new MqttClientOptionsBuilder()
    .WithTcpServer("b29eee83a1e8495ab8973cb91025e251.s1.eu.hivemq.cloud", 8883)
    .WithCredentials("deneme", "Deneme55")
    .WithTlsOptions(o => o.UseTls())
    .WithClientId("_sub" + Guid.NewGuid().ToString("N"))
    .Build();


HttpClient httpClient = null;

try
{
    httpClient = new HttpClient();
}
catch (Exception e)
{
    Console.WriteLine($"HttpClient oluşturma hatası: {e}");
}

try
{
    client.ApplicationMessageReceivedAsync += async e =>
    {
        string deviceId = "deviceId";
        string topic = e.ApplicationMessage.Topic;
        string payload = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
        DateTime timestamp = DateTime.UtcNow;

        Console.WriteLine($"{timestamp} Mesaj: {topic} -> {payload}");

        if (httpClient != null)
        {
            try
            {
                var json = System.Text.Json.JsonSerializer.Serialize(new
                {
                    deviceId,
                    topic,
                    payload,
                    timestamp
                });

                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync("https://localhost:7057/api/sensordata", content);

                Console.WriteLine($"POST durumu: {response.StatusCode}");
            }
            catch (Exception httpEx)
            {
                Console.WriteLine($"HTTP POST hatası: {httpEx.Message}");
            }
        }

        await Task.CompletedTask;
    };
}
catch (Exception e)
{
    Console.WriteLine(e);
}
try
{
    await client.ConnectAsync(options);
    Console.WriteLine("Connected");
    await client.SubscribeAsync(
        new MqttTopicFilterBuilder()
        .WithTopic("deneme55")
        .WithAtMostOnceQoS()
        .Build()
        );
}
catch (Exception e)
{
    Console.WriteLine(e);
}
await Task.Delay(Timeout.Infinite);


