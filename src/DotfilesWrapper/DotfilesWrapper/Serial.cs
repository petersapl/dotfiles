using System.IO;
using YamlDotNet.Serialization;

namespace DotfilesWrapper
{
    static class Serial
    {
        public static Optional<T> Deserialize<T>(string file)
        {
            using (StringReader reader = new StringReader(File.ReadAllText(file)))
            {
                Deserializer deserializer = new Deserializer();
                return Optional.Of(deserializer.Deserialize<T>(reader));
            }
        }
    }
}
