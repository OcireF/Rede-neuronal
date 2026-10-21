using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SARSA
{
    public interface IAmbiente
    {
        public double[] GetInicialState();
        public double[] ExecutarAcao(double[] estado, int a);
        public List<int> AcoesPossiveis(double[] estado);
        public double GetRecompensa(double[] estado);
        public bool IsTerminal(double[] estado);
    }
}
