using ArffTools;
using SINeuronLibrary;
using System.Collections.Generic;

namespace SINeuronWPFApp.Data
{
    public class WekaReader
    {
        public void OpenSet(string path, List<ValuePoint> trainingSet)
        {
            using (var reader = new ArffReader(path))
            {
                var header = reader.ReadHeader();
                object[] instance;
                while ((instance = reader.ReadInstance()) != null)
                {
                    trainingSet.Add(new ValuePoint
                    {
                        X = (double)instance[0],
                        Y = (double)instance[1],
                        Value = ((int)instance[2] == 1) ? 1 : -1
                    });
                }
            }
        }


        //public void OpenAppState(string path)
        //{
        //    using (var reader = new ArffReader(path))
        //    {
        //        var header = reader.ReadHeader();

        //        object[] instance;
        //        while ((instance = reader.ReadInstance()) != null)
        //        {
        //            trainingSet.Add(new ValuePoint
        //            {
        //                X = (double)instance[0],
        //                Y = (double)instance[1],
        //                Value = ((int)instance[2] == 1) ? 1 : -1
        //            });
        //        }
        //    }
        //}
    }
}
