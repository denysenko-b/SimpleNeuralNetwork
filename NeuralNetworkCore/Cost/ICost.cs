namespace NeuralNetwork.Core.Cost
{
    public interface ICost
    {
        CostType Type { get; }

        float Function(float[] predictedOutputs, float[] expectedOutputs);
        float Derivative(float predictedOutput, float expectedOutput);
    }
}
