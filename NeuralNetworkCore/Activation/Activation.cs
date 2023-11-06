using NeuralNetwork.Core.Activation.Functions;

namespace NeuralNetwork.Core.Activation
{
    public static class ActivationFabric
    {
        public static IActivation GetActivationFromType(ActivationType type)
            => type switch
            {
                ActivationType.TanH => new TanH(),
                ActivationType.ReLU => new ReLU(),
                ActivationType.SiLU => new SiLU(),
                ActivationType.Softmax => new Softmax(),
                ActivationType.Sigmoid or _ => new Sigmoid()
            };
    }
}
