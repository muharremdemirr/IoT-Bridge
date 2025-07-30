using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MQTTnet;
using System.Text;
using System.Text.Json;


namespace MqttSubscriber_WorkerService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private IMqttClient _client;
        private HttpClient _httpClient;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
            _httpClient = new HttpClient();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var factory = new MqttClientFactory();
            _client = factory.CreateMqttClient();

            var options = new MqttClientOptionsBuilder()
                .WithTcpServer("b29eee83a1e8495ab8973cb91025e251.s1.eu.hivemq.cloud", 8883)
                .WithCredentials("deneme", "Deneme55")
                .WithTlsOptions(o => o.UseTls())
                .WithClientId("_sub" + Guid.NewGuid().ToString("N"))
                .Build();

            _client.ApplicationMessageReceivedAsync += async e =>
            {
                string deviceId = "deviceId";
                string topic = e.ApplicationMessage.Topic;
                string payload = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
                DateTime timestamp = DateTime.UtcNow;

                _logger.LogInformation($"{timestamp} Mesaj: {topic} -> {payload}");

                var json = JsonSerializer.Serialize(new
                {
                    deviceId,
                    topic,
                    payload,
                    timestamp
                });

                try
                {
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = await _httpClient.PostAsync("https://localhost:7057/api/sensordata", content);
                    _logger.LogInformation($"POST durumu: {response.StatusCode}");
                }
                catch (Exception httpEx)
                {
                    _logger.LogError($"HTTP POST hatasý: {httpEx.Message}");
                }
            };

            try
            {
                await _client.ConnectAsync(options, stoppingToken);
                _logger.LogInformation("MQTT baðlantýsý baþarýlý.");

                await _client.SubscribeAsync(
                    new MqttTopicFilterBuilder()
                        .WithTopic("deneme55")
                        .WithAtMostOnceQoS()
                        .Build(),
                    stoppingToken
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "MQTT baðlantýsý baþarýsýz.");
            }

            // Worker sonsuz çalýþsýn
            await Task.Delay(Timeout.Infinite, stoppingToken);
        }
    }

}
