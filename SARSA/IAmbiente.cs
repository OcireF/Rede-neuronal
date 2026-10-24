using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SARSA
{
    public interface IAmbiente
    {
        public double[] GetInicialState(); //retorna o estado inicial
        public double[] ExecutarAcao(double[] estado, int a); //executa a acao e retorna o estado resultante
        public List<int> AcoesPossiveis(double[] estado); //retorna todas as acoes para um dado estado
        public double GetRecompensa(double[] estado); //receber a recompensa do estado
        public bool IsTerminal(double[] estado); //verifica se o estado é inicial
    }
}
