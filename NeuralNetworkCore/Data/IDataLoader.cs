using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork.Core.Data
{

    public interface IDataLoader
    {
        DataPoint[] AllData { get; }

        void LoadData();
        void UpdateData();
    }
}
