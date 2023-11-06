using static System.MathF;

namespace NeuralNetwork.Core.Activation.Functions
{
    public class ReLU : IActivation
    {
        public ActivationType Type => ActivationType.ReLU;

        public float Derivative(float[] inputs, int index)
        {
            return inputs[index] > 0 ? 1 : 0;
        }

        public float Function(float[] inputs, int index)
        {
            return Max(0, inputs[index]);
        }
    }
}
