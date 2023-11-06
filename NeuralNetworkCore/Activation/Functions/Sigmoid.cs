using NeuralNetwork.Core.Activation;
using static System.MathF;

namespace NeuralNetwork.Core.Activation.Functions
{
    public class Sigmoid : IActivation
    {
        public ActivationType Type => ActivationType.Sigmoid;

        public float Derivative(float[] inputs, int index)
        {
            float a = Function(inputs, index);
            return a * (1 - a);
        }

        public float Function(float[] inputs, int index)
        {
            return 1f / (1 + Exp(-inputs[index]));
        }
    }
}
