using Microsoft.Extensions.Configuration;
using NeuralNetwork.Core;
using NeuralNetwork.Core.Activation;
using NeuralNetwork.Core.Cost;
using NeuralNetwork.Core.Data;
using NeuralNetwork.Core.Data.Image;
using NeuralNetwork.Core.Training;
using System.Diagnostics;
using System.Text.Json;

namespace TestTrain
{
    internal static partial class Program
    {


        private static void Train(string dataPath, string saveToPath)
        {
            int width = 28;
            int height = 28;

            ImageLoader dataLoader = new ImageLoader(dataPath, width, height);

            CostType costType = CostType.CrossEntropy;

            NetworkParameters networkParameters = new NetworkParameters(new[] { width * height, 64, 32, 10 },
                miniBatchSize: 32,
                leariningRate: 0.05f,
                hiddenLayerActivation: ActivationType.SiLU,
                outputLayerActivation: ActivationType.Softmax,
                cost: costType
            );

            int epoches = 4;

            NetworkTrainer networkTrainer = new NetworkTrainer(networkParameters, dataLoader, epoches, trainingSplit: 0.8f);

            Network network = networkTrainer.Network;

            networkTrainer.OnTrainingStarted += () =>
            {
                Debug.WriteLine("Training start");
            };

            ICost cost = CostFabric.GetCostFromType(costType);

            var options = new JsonSerializerOptions { IncludeFields = true };

            networkTrainer.OnEpochEnd += (epoch) =>
            {
                int validationCount = networkTrainer.ValidationData.Length;

                int success = 0;
                long totalLoss = 0;

                Parallel.For(0, validationCount, i =>
                {
                    var data = networkTrainer.ValidationData[i];
                    var (predictedClass, predictedOutputs) = network.Predict(data.Inputs);

                    if (data.Label == predictedClass)
                    {
                        Interlocked.Increment(ref success);
                    }
                    long loss = (long)(cost.Function(predictedOutputs, data.ExpectedOutputs) * 1000);
                    Interlocked.Add(ref totalLoss, loss);
                });

                Console.WriteLine($"Epoch {epoch} finished, " +
                    $"validation_accuracy: {(float)success / validationCount}, " +
                    $"loss: {(float)(totalLoss) / (1000 * validationCount)}, ");
            };

            networkTrainer.StartTrainingSession();

            Console.WriteLine("=> Test predicitons");

            for (int i = 0; i < 10; i++)
            {
                var (predictedClass, _) = network.Predict(dataLoader.AllData[i].Inputs);
                Console.WriteLine(predictedClass + " = " + dataLoader.AllData[i].Label);
            }

            NetworkSaveHelper.Save(network, saveToPath);
        }


        static void Main()
        {
            IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true)
                .Build();

            string path = config["path"]!;

            Console.WriteLine($"Path: {path}");

            Train(path, "network.json");
        }
    }
}