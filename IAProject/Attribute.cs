using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAProject
{
    class Attribute
    {
        public string Name { get; set; }
        public dynamic[] Values {get; set; } // Since the value can be int, double or string

        // Constructor
        public Attribute(string name, object[,] values)
        {
            Name = name;
            Values = SetValues(values);
        }

        private dynamic[] SetValues(object[,] obj)
        {
            Values = new dynamic[300];

            int count = 0;
            foreach (var i in obj)
            {
                Values[count] = i;
                count++;
            }

            return Values;
        }

        public int MinInt()
        {
            return Convert.ToInt32(Values.Min());
        }

        public int MaxInt()
        {
            return Convert.ToInt32(Values.Max());
        }

        public int MedianInt()
        {
            return Convert.ToInt32(Median());
        }

        public double Median()
        {
            double[] tempList = new double[300];

            int count = 0;
            foreach (var i in Values)
            {
                tempList[count] = i;
                count++;
            }

            List<double> orderedList = tempList
                .OrderBy(numbers => numbers)
                .ToList();

            int listSize = orderedList.Count;
            double result;

            if (listSize % 2 == 0) // even
            {
                int midIndex = listSize / 2;
                result = ((orderedList.ElementAt(midIndex - 1) +
                           orderedList.ElementAt(midIndex)) / 2);
            }
            else // odd
            {
                double element = (double)listSize / 2;
                element = Math.Round(element, MidpointRounding.AwayFromZero);

                result = orderedList.ElementAt((int)(element - 1));
            }

            return result;
        }

        public static T Parse<T>(double[] value)
        {
            return (T)TypeDescriptor.GetConverter(typeof(T)).ConvertFrom(value);
        }
    }
}
