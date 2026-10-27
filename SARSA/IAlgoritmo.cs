using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SARSA
{
    internal interface IAlgoritmo
    {
        public List<double[]> treino(int numEpisodios, int numPassos, bool justLastEP = false);
    }
}
