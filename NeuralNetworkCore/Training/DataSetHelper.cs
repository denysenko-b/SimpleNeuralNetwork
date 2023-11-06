using NeuralNetwork.Core.Data;
using NeuralNetwork.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork.Core.Training
{
    public static class DataSetHelper
    {
        private static readonly Random Random = new Random();

        // Split data into training and validation sets
        public static (DataPoint[] train, DataPoint[] validate) SplitData(DataPoint[] allData, float trainingSplit = 0.75f, bool shuffle = true)
        {
            if (shuffle)
            {
                Random.Shuffle(allData);
            }

            int trainCount = (int)(allData.Length * Math.Clamp(trainingSplit, 0, 1));
            int validationCount = allData.Length - trainCount;

            DataPoint[] trainData = new DataPoint[trainCount];
            DataPoint[] validationData = new DataPoint[validationCount];


            Array.Copy(allData, trainData, trainCount);
            Array.Copy(allData, trainCount, validationData, 0, validationCount);

            return (trainData, validationData);
        }

        public static Batch[] CreateMiniBatches(DataPoint[] allData, int size, bool shuffle = true)
        {
            if (shuffle)
            {
                Random.Shuffle(allData);
            }

            int numBatches = allData.Length / size;
            Batch[] batches = new Batch[numBatches];

            for (int i = 0; i < batches.Length; i++)
            {
                DataPoint[] batchData = new DataPoint[size];
                Array.Copy(allData, i * size, batchData, 0, size);
                batches[i] = new Batch(batchData);
            }

            return batches;
        }

        public static void ShuffleBatches(Batch[] batches)
        {
            Random.Shuffle(batches);
        }

    }

    public sealed class Batch
    {
        public readonly DataPoint[] Data;

        public Batch(DataPoint[] data)
        {
            Data = data;
        }
    }
}
