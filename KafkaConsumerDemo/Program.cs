using Common;
using Confluent.Kafka;
using Newtonsoft.Json;

Console.WriteLine("*******************************");
Console.WriteLine("****       CONSUMER     *******");
Console.WriteLine("*******************************");

var consumerConfig = new ConsumerConfig
{
    GroupId = KafkaConstants.ConsumerGroupId,
    BootstrapServers = KafkaConstants.KafkaServer,
    AutoOffsetReset = AutoOffsetReset.Earliest
};

var consumer = new ConsumerBuilder<Null, string>(consumerConfig).Build();
consumer.Subscribe(KafkaConstants.WeatherTopicName);

var cancellationToken = new CancellationTokenSource();

try
{
    while (true)
    {
        var response = consumer.Consume(cancellationToken.Token);
        if (response.Message != null)
        {
            var weather = JsonConvert.DeserializeObject<Weather>(response.Message.Value);
            if (weather != null)
            {
                Console.WriteLine($"State: {weather.State}, Temperature: {weather.Temperature} ({weather.Description})");
            }
        }
    }
}
catch (Exception e)
{
    Console.WriteLine(e.Message);
}