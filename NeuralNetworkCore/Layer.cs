using NeuralNetwork.Core.Activation;
using NeuralNetwork.Core.Cost;
using NeuralNetwork.Extensions;

namespace NeuralNetwork.Core
{

    public sealed class Layer
    {
        #region Fields, Properties

        public readonly float[] Weights;
        public readonly float[] Biases;
        public readonly IActivation Activation;

        public readonly int InputNeurons;
        public readonly int OutputNeurons;

        private readonly float[] costGradientW;
        private readonly float[] costGradientB;

        private readonly float[] weightVelocities;
        private readonly float[] bisesVelocities;

        private readonly static Random random;

        #endregion

        #region  Constructors

        static Layer()
        {
            random = new Random();
        }

        public Layer(int inputNeurons, int outputNeurons, IActivation? activation = null)
        {
            InputNeurons = inputNeurons;
            OutputNeurons = outputNeurons;

            Weights = new float[inputNeurons * outputNeurons];
            Biases = new float[outputNeurons];

            costGradientW = new float[Weights.Length];
            costGradientB = new float[Biases.Length];

            weightVelocities = new float[Weights.Length];
            bisesVelocities = new float[Biases.Length];


            Activation = activation ?? ActivationFabric.GetActivationFromType(ActivationType.Sigmoid);

            InitWeights();
        }

        #endregion

        #region Public Methods

        public float[] CalculateOutputs(float[] inputs)
        {
            float[] weightedInputs = new float[OutputNeurons];

            for (int outputIndex = 0; outputIndex < OutputNeurons; outputIndex++)
            {
                float sum = Biases[outputIndex];

                for (int inputIndex = 0; inputIndex < InputNeurons; inputIndex++)
                {
                    sum += inputs[inputIndex] * Weights[GetWeightIndex(inputIndex, outputIndex)];
                }

                weightedInputs[outputIndex] = sum;
            }

            float[] outputs = new float[OutputNeurons];

            for (int i = 0; i < OutputNeurons; i++)
            {
                outputs[i] = Activation.Function(weightedInputs, i);
            }

            return outputs;
        }

        #endregion

        #region Hidden Methods

        internal float[] CalculateOutputs(float[] inputs, LayerLearnData learnData)
        {
            learnData.Inputs = inputs;

            for (int outputIndex = 0; outputIndex < OutputNeurons; outputIndex++)
            {
                float sum = Biases[outputIndex];

                for (int inputIndex = 0; inputIndex < InputNeurons; inputIndex++)
                {
                    sum += inputs[inputIndex] * Weights[GetWeightIndex(inputIndex, outputIndex)];
                }

                learnData.WeightedInputs[outputIndex] = sum;
            }

            for (int i = 0; i < OutputNeurons; i++)
            {
                learnData.Activations[i] = Activation.Function(learnData.WeightedInputs, i);
            }

            return learnData.Activations;
        }

        internal void CalculateOutputLayerNeuronData(LayerLearnData learnData, float[] expectedOutputs, ICost cost)
        {
            for (int i = 0; i < learnData.NeuronData.Length; i++)
            {
                float costDerivative = cost.Derivative(learnData.Activations[i], expectedOutputs[i]);
                float activationDerivative = Activation.Derivative(learnData.WeightedInputs, i);

                learnData.NeuronData[i] = costDerivative * activationDerivative;
            }
        }

        internal void CalculateHiddenLayerNeuronData(LayerLearnData learnData, Layer prevLayer, float[] prevNeuronData)
        {
            for (int neuronIndex = 0; neuronIndex < OutputNeurons; neuronIndex++)
            {
                float neuronData = 0;

                for (int prevNeuronIndex = 0; prevNeuronIndex < prevNeuronData.Length; prevNeuronIndex++)
                {
                    float weightInputDerivative = prevLayer.GetWeight(neuronIndex, prevNeuronIndex);
                    neuronData += weightInputDerivative * prevNeuronData[prevNeuronIndex];
                }

                neuronData *= Activation.Derivative(learnData.WeightedInputs, neuronIndex);
                learnData.NeuronData[neuronIndex] = neuronData;
            }
        }

        internal void UpdateGradients(LayerLearnData layerLearnData)
        {
            lock (costGradientW)
            {
                for (int outIndex = 0; outIndex < OutputNeurons; outIndex++)
                {
                    float neuronData = layerLearnData.NeuronData[outIndex];

                    for (int inIndex = 0; inIndex < InputNeurons; inIndex++)
                    {
                        float derivative = neuronData * layerLearnData.Inputs[inIndex];

                        costGradientW[GetWeightIndex(inIndex, outIndex)] += derivative;
                    }
                }
            }

            lock (costGradientB)
            {
                for (int outIndex = 0; outIndex < OutputNeurons; outIndex++)
                {
                    float derivative = layerLearnData.NeuronData[outIndex];

                    costGradientB[outIndex] += derivative;
                }
            }
        }

        internal void ApplyGradients(float learningRate, float regularization, float momentum)
        {
            float weightDecay = (1 - regularization * learningRate);

            for (int i = 0; i < Weights.Length; i++)
            {
                float weight = Weights[i];
                float velocity = weightVelocities[i] * momentum - costGradientW[i] * learningRate;
                weightVelocities[i] = velocity;

                Weights[i] = weight * weightDecay + velocity;
                costGradientW[i] = 0;
            }

            for (int i = 0; i < Biases.Length; i++)
            {
                float bias = Biases[i];
                float velocity = bisesVelocities[i] * momentum - costGradientB[i] * learningRate;
                bisesVelocities[i] = velocity;

                Biases[i] = bias + velocity;
                costGradientB[i] = 0;
            }
        }

        private float GetWeight(int inputNeuronIndex, int outputNeuronIndex)
        {
            int index = outputNeuronIndex * InputNeurons + inputNeuronIndex;
            return Weights[index];
        }

        private int GetWeightIndex(int inputNeuronIndex, int outputNeuronIndex)
        {
            return outputNeuronIndex * InputNeurons + inputNeuronIndex;
        }

        private void InitWeights()
        {
            for (int i = 0; i < Weights.Length; i++)
            {
                Weights[i] = random.SampleGaussian() / MathF.Sqrt(InputNeurons);
            }
        }

        #endregion
    }
}
