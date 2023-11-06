namespace NeuralNetwork.Core.Activation
{
    public interface IActivation
    {
        ActivationType Type { get; }

        float Function(float[] inputs, int index);
        float Derivative(float[] inputs, int index);
    }
}
