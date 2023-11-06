using NeuralNetwork.Core.Cost.Functions;

namespace NeuralNetwork.Core.Cost
{
    public class CostFabric
    {
        public static ICost GetCostFromType(CostType type)
            => type switch
            {
                CostType.CrossEntropy => new CrossEntropy(),
                CostType.BinaryCrossEntropy => new BinaryCrossEntropy(),
                CostType.MeanSquaredError or _ => new MeanSquaredError(),
            };
    }
}
