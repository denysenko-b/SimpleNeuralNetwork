using static System.MathF;

namespace NeuralNetwork.Core.Cost.Functions
{
    public class BinaryCrossEntropy : ICost
    {
        public CostType Type => CostType.BinaryCrossEntropy;

        public float Derivative(float predictedOutput, float expectedOutput)
        {
            float x = predictedOutput;
            float y = expectedOutput;

            return -y/x + (1-y)/(1-x);
        }

        public float Function(float[] predictedOutputs, float[] expectedOutputs)
        {
            float cost = 0;

            for (int i = 0; i < predictedOutputs.Length; i++)
            {
                cost += expectedOutputs[i] * Log(predictedOutputs[i]) + (1 - expectedOutputs[i]) * Log(1 - predictedOutputs[i]);
            }

            return cost;
        }
    }
}
