using static System.MathF;

namespace NeuralNetwork.Core.Cost.Functions
{
    public class CrossEntropy : ICost
    {
        public CostType Type => CostType.CrossEntropy;

        public float Derivative(float predictedOutput, float expectedOutput)
        {
            float x = predictedOutput;
            float y = expectedOutput;
            if (x == 0 || x == 1)
            {
                return 0;
            }
            return (-x + y) / (x * (x - 1));
        }

        public float Function(float[] predictedOutputs, float[] expectedOutputs)
        {
            // cost is sum (for all x,y pairs) of: 0.5 * (x-y)^2
            float cost = 0;
            for (int i = 0; i < predictedOutputs.Length; i++)
            {
                float x = predictedOutputs[i];
                float y = expectedOutputs[i];
                float v = y == 1 ? Log(x) : Log(1 - x);
                cost += float.IsNaN(v) ? 0 : v;
            }
            return cost;
        }
    }
}
