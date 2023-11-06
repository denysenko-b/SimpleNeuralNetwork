using NeuralNetwork.Core.Activation;
using static System.MathF;

namespace NeuralNetwork.Core.Activation.Functions
{
    public class SiLU : IActivation
    {
        public ActivationType Type => ActivationType.SiLU;

        public float Derivative(float[] inputs, int index)
        {
            float sig = 1 / (1 + Exp(-inputs[index]));
            return inputs[index] * sig * (1 - sig) + sig;
        }

        public float Function(float[] inputs, int index)
        {
            return inputs[index] / (1 + Exp(-inputs[index]));
        }
    }
}
