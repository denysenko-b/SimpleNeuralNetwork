using static System.MathF;

namespace NeuralNetwork.Core.Activation.Functions
{
    public class TanH : IActivation
    {
        public float Function(float[] inputs, int index)
        {
            float e2 = Exp(2 * inputs[index]);
            return (e2 - 1) / (e2 + 1);
        }

        public float Derivative(float[] inputs, int index)
        {
            float e2 = Exp(2 * inputs[index]);
            float t = (e2 - 1) / (e2 + 1);
            return 1 - t * t;
        }

        public ActivationType Type => ActivationType.TanH;
    }
}
