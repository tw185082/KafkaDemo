using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public static class KafkaConstants
    {
        public static string KafkaServer => "localhost:9092";
        public static string WeatherTopicName => "TestTopic";
        public static string ConsumerGroupId => "WeatherTopic-ConsumerGroup";
    }
}
