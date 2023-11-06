using NeuralNetwork.Core.Activation;
using NeuralNetwork.Core.Cost;
using System.Text.Json.Serialization;

namespace NeuralNetwork.Core.Data
{
    internal sealed class NetworkSaveData
    {
        public readonly float[][] Weights;
        public readonly float[][] Biases;

        public readonly int[] LayerSizes;
        public readonly int BatchSize;

        public readonly ActivationType HiddenLayerActivation;
        public readonly ActivationType OutputLayerActivation;

        public readonly CostType Cost;

        [JsonConstructor]
        public NetworkSaveData(float[][] weights,
            float[][] biases,
            int[] layerSizes,
            int batchSize,
            ActivationType hiddenLayerActivation,
            ActivationType outputLayerActivation, CostType cost)
        {
            Weights = weights;
            Biases = biases;
            LayerSizes = layerSizes;
            BatchSize = batchSize;
            HiddenLayerActivation = hiddenLayerActivation;
            OutputLayerActivation = outputLayerActivation;
            Cost = cost;
        }

        public NetworkSaveData(Network network)
        {
            int numLayers = network.Layers.Length;

            LayerSizes = network.LayerSizes;
            BatchSize = network.BatchLearnData.Length;

            Weights = new float[numLayers][];
            Biases = new float[numLayers][];

            HiddenLayerActivation = network.HiddenLayerActivation.Type;
            OutputLayerActivation = network.OutputLayerActivation.Type;
            Cost = network.Cost.Type;

            for (int layerIndex = 0; layerIndex < numLayers; layerIndex++)
            {
                Layer layer = network.Layers[layerIndex];

                Weights[layerIndex] = layer.Weights;
                Biases[layerIndex] = layer.Biases;
            }
        }

        public Network BuildNetwork()
        {
            IActivation hiddenActivation = ActivationFabric.GetActivationFromType(HiddenLayerActivation);
            IActivation outputActivation = ActivationFabric.GetActivationFromType(OutputLayerActivation);
            ICost cost = CostFabric.GetCostFromType(Cost);

            Network network = new Network(LayerSizes, BatchSize, hiddenActivation, outputActivation, cost);

            int numLayers = network.Layers.Length;

            for (int layerIndex = 0; layerIndex < numLayers; layerIndex++)
            {
                Layer layer = network.Layers[layerIndex];

                int numWeights = layer.Weights.Length;
                int numBiases = layer.Biases.Length;

                Array.Copy(Weights[layerIndex], layer.Weights, numWeights);
                Array.Copy(Biases[layerIndex], layer.Biases, numBiases);
            }

            return network;
        }
    }
}
