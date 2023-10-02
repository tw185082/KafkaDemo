using Common;
using Confluent.Kafka;
using Confluent.Kafka.Admin;
using Newtonsoft.Json;

Console.WriteLine("*******************************");
Console.WriteLine("****       PRODUCER     *******");
Console.WriteLine("*******************************");


var config = new ProducerConfig
{
    BootstrapServers = KafkaConstants.KafkaServer,
    Acks = Acks.Leader,  // Only wait Acks from leader
};

var producer = new ProducerBuilder<Null, string>(config).Build();

try
{
    try
    {
        // Create Topic - If topic exists, it throws exception which we ignore
        await CreateTopic(KafkaConstants.WeatherTopicName);
    }
    catch (Exception e)
    {
        Console.WriteLine(e.Message);
    }

    Console.WriteLine("Enter weather info:");
    Console.WriteLine("Example: NY,76,Cloudy");
    string? message;
    while ((message = Console.ReadLine()) != null)
    {
        var winfo = message.Split(",");
        if (winfo.Length == 3)
        {
            var weatherData = new Weather
            {
                State = winfo[0], 
                Temperature = int.Parse(winfo[1]), 
                Description = winfo[2]
            };

            var response = await producer.ProduceAsync(
                KafkaConstants.WeatherTopicName,
                new Message<Null, string> { Value = JsonConvert.SerializeObject(weatherData) }
                );
        }
        else
        {
            Console.WriteLine("[Invalid input]");
        }
    }
}
catch (Exception e)
{
    Console.WriteLine(e.Message);
}


// Partition and replication factors
async Task CreateTopic(string topic)
{
    using var adminClient = new AdminClientBuilder(config).Build();
    var topicSpec = new TopicSpecification
    {
        Name = topic,
        NumPartitions = 3,
        ReplicationFactor = 1,  // needs to match number of brokers (Kafka node)
        Configs = new Dictionary<string, string>
        {
            {"retention.ms", "300"}, // seconds
            {"compression.type", "gzip"}
        }
    };

    try
    {
        await adminClient.CreateTopicsAsync(new[] { topicSpec });
        Console.WriteLine("Kafka topic created");
    }
    catch (Exception e)
    {
        Console.WriteLine(e);
        throw;
    }
}