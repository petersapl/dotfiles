using System.IO;
using YamlDotNet.Serialization;
using YamlDotNet.Core;
using System;
namespace DotfilesWrapper
{
    static class Serial
    {
        public static Optional<T> Deserialize<T>(string file)
        {
            using (StringReader reader = new StringReader(File.ReadAllText(file)))
            {
                Deserializer deserializer = new Deserializer();

                try
                {
                    return Optional.Of(deserializer.Deserialize<T>(reader));
                }
                catch (YamlException ex)
                {
                    Console.WriteLine(ex.Message);
                    return Optional.Empty<T>();
                }
            }
        }
    }
}
