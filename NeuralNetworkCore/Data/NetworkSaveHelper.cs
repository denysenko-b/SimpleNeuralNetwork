using System.Text.Json;

namespace NeuralNetwork.Core.Data
{
    public static class NetworkSaveHelper
    {
        private static readonly JsonSerializerOptions DefaultOptions = new JsonSerializerOptions()
        {
            IncludeFields = true,
            WriteIndented = true
        };

        public static void Save(Network network, string path)
        {
            NetworkSaveData saveData = new NetworkSaveData(network);
            string json = JsonSerializer.Serialize(saveData, DefaultOptions);
            File.WriteAllText(path, json);
        }

        public static Network Load(string path)
        {
            var json = File.ReadAllText(path);

            var networkSaveData = JsonSerializer.Deserialize<NetworkSaveData>(json, DefaultOptions);

            if (networkSaveData == null)
            {
                throw new Exception("Network is null");
            }

            return networkSaveData.BuildNetwork();
        }
    }
}
