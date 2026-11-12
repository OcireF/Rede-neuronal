using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SARSA
{
    public interface IAmbiente
    {
        public double[] GetInicialState(int agenteid = 0); //retorna o estado inicial
        public int RegistarAgente(); //para o Ambiente poder ter metadados por agente não guardados no seu estado.
        public double[] ExecutarAcao(double[] estado, int a, int agenteid); //executa a acao e retorna o estado resultante
        public List<int> AcoesPossiveis(double[] estado); //retorna todas as acoes para um dado estado
        public double GetRecompensa(double[] estado, int agenteid); //receber a recompensa do estado
        public bool IsTerminal(double[] estado, int agenteid); //verifica se o estado é inicial
        public void SaveAmbienteInStream(StreamWriter fileStream);
        public static abstract IAmbiente LoadAmbienteFromFile(string fileName);
    }
}
