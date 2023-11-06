using NeuralNetwork.Core.Cost;

namespace NeuralNetwork.Core.Cost.Functions
{
    public class MeanSquaredError : ICost
    {
        public CostType Type => CostType.MeanSquaredError;

        public float Derivative(float predictedOutput, float expectedOutput)
        {
            return predictedOutput - expectedOutput;
        }

        public float Function(float[] predictedOutputs, float[] expectedOutputs)
        {
            float cost = 0;
            for (int i = 0; i < predictedOutputs.Length; i++)
            {
                float error = predictedOutputs[i] - expectedOutputs[i];
                cost += error * error;
            }
            return 0.5f * cost;
        }
    }
}
