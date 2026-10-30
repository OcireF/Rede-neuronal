using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SARSA
{
    public interface IAlgoritmo
    {
        public List<double[]> treino(int numEpisodios, int numPassos, bool justLastEP = false);
        public double Aprender(double[] s, int a, double r, double[] sLine, int aLine, bool isFinal);
        public (double[] estado, int acao) FazerPasso(double[] estado, int acao);
        public Agente CreateAgente(int numEpisodios, int numPassos);
        public int seleccionar_acao(double[] estado);
        public (double recompensa, double[] estadoLinha) executar_acao(double[] estado, int acao);
        public void SavePoliticaOnFile(string filename);
    }
}
