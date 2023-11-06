using NeuralNetwork.Core.Activation;
using NeuralNetwork.Core.Cost;

namespace NeuralNetwork.Core
{
    public sealed class NetworkParameters
    {
        public readonly int[] LayerSizes;

        public readonly ActivationType HiddenLayerActivation;
        public readonly ActivationType OutputLayerActivation;

        public readonly CostType Cost;

        public readonly int MiniBatchSize;

        /// <summary>
        /// Initial learning rate
        /// </summary>
        public readonly float LearningRate;

        public readonly float Momentum;

        public readonly float LearnRateDecay;

        public readonly float Regularization;


        public NetworkParameters(
            int[] layerSizes,
            int miniBatchSize = 32,
            float leariningRate = 0.05f,
            float momentum = 0.9f,
            float learnRateDecay = 0.075f,
            float regularization = 0.1f,
            ActivationType hiddenLayerActivation = ActivationType.ReLU,
            ActivationType outputLayerActivation = ActivationType.Sigmoid,
            CostType cost = CostType.MeanSquaredError)
        {
            LearningRate = leariningRate;
            Momentum = momentum;
            LearnRateDecay = learnRateDecay;
            Regularization = regularization;

            LayerSizes = layerSizes;
            MiniBatchSize = miniBatchSize;

            HiddenLayerActivation = hiddenLayerActivation;
            OutputLayerActivation = outputLayerActivation;

            Cost = cost;
        }
    }
}
