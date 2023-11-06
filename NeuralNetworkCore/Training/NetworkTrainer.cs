using NeuralNetwork.Core.Activation;
using NeuralNetwork.Core.Cost;
using NeuralNetwork.Core.Data;
using NeuralNetwork.Extensions;

namespace NeuralNetwork.Core.Training
{
    public sealed class NetworkTrainer
    {
        #region Events, Delegates

        public event Action<int>? OnEpochEnd;
        public event Action? OnTrainingStarted;

        #endregion

        #region Fields, Properties

        public readonly NetworkParameters Parameters;
        public readonly Network Network;

        private readonly IDataLoader DataLoader;

        private bool TrainingActive;
        private float CurrentLearningRate;
        private int EpochCount;

        public DataPoint[] TrainingData;
        public DataPoint[] ValidationData;
        private Batch[] TrainingBatches;

        #endregion

        public NetworkTrainer(NetworkParameters parameters, IDataLoader dataLoader, int epochCount, float trainingSplit = 0.8f)
        {
            Parameters = parameters;
            DataLoader = dataLoader;
            EpochCount = epochCount;

            (TrainingData, ValidationData, TrainingBatches) = LoadData(trainingSplit);


            var activation = ActivationFabric.GetActivationFromType(Parameters.HiddenLayerActivation);
            var outputLayerActivation = ActivationFabric.GetActivationFromType(Parameters.OutputLayerActivation);
            var cost = CostFabric.GetCostFromType(Parameters.Cost);
            Network = new Network(Parameters.LayerSizes, Parameters.MiniBatchSize, activation, outputLayerActivation, cost);
        }

        public void StartTrainingSession()
        {
            CurrentLearningRate = Parameters.LearningRate;
            TrainingActive = true;
            OnTrainingStarted?.Invoke();
            Train();
        }

        private void Train()
        {
            for (int epoch = 0; epoch < EpochCount; epoch++)
            {
                foreach (var batch in TrainingBatches)
                {
                    Network.Train(batch.Data, CurrentLearningRate, Parameters.Regularization, Parameters.Momentum);
                }

                EpochCompleted(epoch);
            }
        }

        private void EpochCompleted(int epoch)
        {
            OnEpochEnd?.Invoke(epoch);
            DataLoader.UpdateData();
            DataSetHelper.ShuffleBatches(TrainingBatches);
            //DropConnections();

            CurrentLearningRate = (1.0f / (1.0f + epoch * Parameters.LearnRateDecay)) * Parameters.LearningRate;
        }

        private void DropConnections()
        {
            float dropOutRate = 0.5f;

            var layer = Network.Layers[^1];
            //foreach (var layer in Network.Layers)
            //{
                float[] weights = layer.Weights;

                for (int i = 0; i < weights.Length; i++)
                {
                    if (Random.Shared.NextSingle() < dropOutRate)
                    {
                        //weights[i] = Random.Shared.SampleGaussian();
                        weights[i] = 0;
                    }
                }
            //}
        }

        private (DataPoint[] trainingData, DataPoint[] validationData, Batch[] trainingBatches) LoadData(float trainingSplit)
        {
            DataLoader.LoadData();
            var (trainingData, validationData) = DataSetHelper.SplitData(DataLoader.AllData, trainingSplit);
            var trainingBatches = DataSetHelper.CreateMiniBatches(trainingData, Parameters.MiniBatchSize);

            return (trainingData, validationData, trainingBatches);
        }
    }
}
