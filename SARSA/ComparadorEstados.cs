using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SARSA
{
    public class ComparadorEstados : IEqualityComparer<double[]>
    {
        public bool Equals(double[]? x, double[]? y)
        {
            if (x == null || y == null) return x == y;
            return x.SequenceEqual(y); //compara todos da lista 1 a 1
        }

        public int GetHashCode([DisallowNull] double[] obj)
        {
            var hash = new HashCode();
            foreach (double d in obj) hash.Add(d);
            return hash.ToHashCode();
        }
    }
}
