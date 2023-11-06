namespace NeuralNetwork.Core.Data
{

    public class DataPoint
    {
        public readonly float[] Inputs;
        public readonly float[] ExpectedOutputs;
        public readonly int Label;
        public readonly int NumLabels;

        protected DataPoint(float[] inputs, float[] expectedOutputs, int label, int numLabels)
        {
            Inputs = inputs;
            ExpectedOutputs = expectedOutputs;
            Label = label;
            NumLabels = numLabels;
        }

        public DataPoint(float[] inputs, int label, int numLabels)
        {
            Inputs = inputs;
            Label = label;
            NumLabels = numLabels;
            ExpectedOutputs = CreateOneHot(label, numLabels);
        }

        protected virtual float[] CreateOneHot(int index, int num)
        {
            float[] oneHot = new float[num];
            oneHot[index] = 1;
            return oneHot;
        }
    }

    public class BinaryDataPoint : DataPoint
    {
        public BinaryDataPoint(float[] inputs, int label) : base(inputs, new float[1] { label }, label, 2)
        {

        }
    }
}
