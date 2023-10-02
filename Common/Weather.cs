using ProtoBuf;

namespace Common
{
    [ProtoContract]
    public class Weather
    {
        [ProtoMember(1)]
        public string State { get; set; }

        [ProtoMember(2)]
        public int Temperature { get; set; }

        [ProtoMember(3)]        
        public string Description { get; set; }
    }

    public static class ProtoBufSerializer
    {
        public static byte[] Serialize(object obj)
        {
            using (var ms = new MemoryStream())
            {
                Serializer.Serialize(ms, obj);
                return ms.ToArray();
            }
        }

        public static object Deserialize<T>(byte[] bytes)
        {
            using (var ms = new MemoryStream())
            {
                return Serializer.Deserialize(typeof(T), ms);
            }
        }
    }
}