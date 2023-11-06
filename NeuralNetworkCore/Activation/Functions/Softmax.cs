using static System.MathF;

namespace NeuralNetwork.Core.Activation.Functions
{
    public class Softmax : IActivation
    {
        public ActivationType Type => ActivationType.Softmax;

        public float Function(float[] inputs, int index)
        {
            float expSum = 0;
            for (int i = 0; i < inputs.Length; i++)
            {
                expSum += Exp(inputs[i]);
            }

            float res = Exp(inputs[index]) / expSum;

            return res;
        }

        public float Derivative(float[] inputs, int index)
        {
            float expSum = 0;
            for (int i = 0; i < inputs.Length; i++)
            {
                expSum += Exp(inputs[i]);
            }

            float ex = Exp(inputs[index]);

            return (ex * expSum - ex * ex) / (expSum * expSum);
        }
    }
}
