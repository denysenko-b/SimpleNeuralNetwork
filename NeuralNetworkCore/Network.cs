using NeuralNetwork.Core.Activation;
using NeuralNetwork.Core.Cost;
using NeuralNetwork.Core.Data;

namespace NeuralNetwork.Core
{
    /// <summary>
    /// Contains network data for learning allowing multithreading
    /// </summary>
    internal sealed class NetworkLearnData
    {
        public readonly LayerLearnData[] LayerData;

        public NetworkLearnData(Layer[] layers)
        {
            LayerData = new LayerLearnData[layers.Length];

            for (int i = 0; i < LayerData.Length; i++)
            {
                LayerData[i] = new LayerLearnData(layers[i]);
            }
        }
    }

    /// <summary>
    /// Contains layer data for learning allowing multithreading
    /// </summary>
    internal sealed class LayerLearnData
    {
        public float[] Inputs;

        public readonly float[] WeightedInputs;
        public readonly float[] Activations;
        public readonly float[] NeuronData;

        public LayerLearnData(Layer layer)
        {
            Inputs = Array.Empty<float>();
            WeightedInputs = new float[layer.OutputNeurons];
            Activations = new float[layer.OutputNeurons];
            NeuronData = new float[layer.OutputNeurons];
        }
    }

    public sealed class Network
    {
        #region Fields, Properties

        public readonly Layer[] Layers;
        public readonly int[] LayerSizes;

        public readonly IActivation HiddenLayerActivation;
        public readonly IActivation OutputLayerActivation;

        public readonly ICost Cost;

        internal readonly NetworkLearnData[] BatchLearnData;

        #endregion

        #region Constructors

        public Network(int[] layerSizes, int batchSize, IActivation hiddenLayerActivation, IActivation outputLayerActivation, ICost cost)
        {
            HiddenLayerActivation = hiddenLayerActivation;
            OutputLayerActivation = outputLayerActivation;
            Cost = cost;

            LayerSizes = layerSizes;
            Layers = new Layer[layerSizes.Length - 1];

            for (int i = 0; i < Layers.Length; i++)
            {
                Layers[i] = new Layer(layerSizes[i], layerSizes[i + 1]);
            }

            BatchLearnData = new NetworkLearnData[batchSize];

            for (int i = 0; i < BatchLearnData.Length; i++)
            {
                BatchLearnData[i] = new NetworkLearnData(Layers);
            }
        }

        #endregion

        #region Public Methods

        public (int predictedClass, float[] outputs) Predict(float[] inputs)
        {
            float[] outputs = CalculateOutputs(inputs);
            int predictedClass = MaxValueIndex(outputs);
            return (predictedClass, outputs);
        }

        public float[] CalculateOutputs(float[] inputs)
        {
            foreach (var layer in Layers)
            {
                inputs = layer.CalculateOutputs(inputs);
            }
            return inputs;
        }

        public void Train(DataPoint[] batch, float learningRate, float regularization, float momentum)
        {
            Parallel.For(0, batch.Length, new ParallelOptions() { MaxDegreeOfParallelism = Environment.ProcessorCount }, i =>
            {
                UpdateGradients(batch[i], BatchLearnData[i]);
            });

            //for (int i = 0; i < batch.Length; i++)
            //{
            //    UpdateGradients(batch[i], BatchLearnData[i]);
            //}

            foreach (var layer in Layers)
            {
                layer.ApplyGradients(learningRate / batch.Length, regularization, momentum);
            }
        }

        #endregion

        #region Private Methods

        private void UpdateGradients(DataPoint dataPoint, NetworkLearnData learnData)
        {
            float[] _inputs = dataPoint.Inputs;

            for (int i = 0; i < Layers.Length; i++)
            {
                _inputs = Layers[i].CalculateOutputs(_inputs, learnData.LayerData[i]);
            }

            int outputLayerIndex = Layers.Length - 1;
            Layer outputLayer = Layers[outputLayerIndex];
            LayerLearnData outputLayerData = learnData.LayerData[outputLayerIndex];

            outputLayer.CalculateOutputLayerNeuronData(outputLayerData, dataPoint.ExpectedOutputs, Cost);
            outputLayer.UpdateGradients(outputLayerData);

            for (int i = outputLayerIndex - 1; i >= 0; i--)
            {
                LayerLearnData layerLearnData = learnData.LayerData[i];
                Layer layer = Layers[i];

                layer.CalculateHiddenLayerNeuronData(layerLearnData, Layers[i + 1], learnData.LayerData[i + 1].NeuronData);
                layer.UpdateGradients(layerLearnData);
            }
        }

        private int MaxValueIndex(float[] array)
        {
            int maxIndex = 0;
            float maxValue = array[0];

            for (int i = 1; i < array.Length; i++)
            {
                if (array[i] > maxValue)
                {
                    maxIndex = i;
                    maxValue = array[i];
                }
            }

            return maxIndex;
        }

        #endregion
    }
}
