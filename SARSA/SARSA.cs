using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;

namespace SARSA
{
    public class SARSA
    {
        public Dictionary<(int, int), double> Politica = new Dictionary<(int estado, int acao), double>(); //Q(s,a)
        public List<int> terminalStates = new List<int>();
        public double y;
        public double alfa;
        public Random r = new Random();
        public double efen = 0.5;

        SARSA()
        {

        }

        public double Aprender(int s, int a, double r, int sLine, int aLine, bool isFinal) //s, a, r, s’, a’, final
        {
            double j = 0;
            if (isFinal)
            {
                j = r; // J = r
            }
            else
            {
                j = r + (y * Politica[(sLine, aLine)]); // J = r + γQ(s’, a’)
                Politica[(s, a)] += alfa * (j - Politica[(s, a)]); // Q(s, a) = Q(s, a) + alfa(J – Q(s, a))
            }
            return j;
        }

        public void InicializarPoliticas(int estados, int acoes, List<int> terminalStates)
        {
            this.terminalStates = terminalStates; //terminal states
            for (int s = 0; s < estados; s++)
            {
                for (int a = 0; a < acoes; a++)
                {
                    Politica[(s, a)] = 0;
                }
            }
        }

        public void treino(int estadoInicial,int numEstados, int numAcoes, int numEpisodios, int numPassos)
        {
            var estado = estadoInicial;
            double recompensa;
            var estadoLinha = 0;
            var acaoLinha = 0;
            InicializarPoliticas(numEstados, numAcoes, new List<int>());
            for(int ep = 0; ep < numEpisodios; ep++)
            {
                var acao = seleccionar_acao(estado);
                for(int p = 0; p < numPassos; p++)
                {
                    (recompensa, estadoLinha) = executar_acao(estado, acao);//Executar ação "a"
                    acaoLinha = seleccionar_acao(estadoLinha);
                    recompensa = Aprender(estado, acao, recompensa, estadoLinha, acaoLinha,false); //is final should be differnt
                    estado = estadoLinha;
                    acao = acaoLinha;
                }
            }
        }

        public int seleccionar_acao(int estado)
        {
            int a = 0;
            if ( r.Next(0,1) > efen)
            {
                a = Acao_sofrega(estado);
            }
            else
            {
                a = Acao_exploracao(estado);
            }
            return a;
        }
        public int Acao_sofrega(int estado)
        {
            //suffle acoes
            return 0; //maxarg
        }

        public int Acao_exploracao(int estado)
        {
            return 0; //random
        }

        public (int r, int sLinha) executar_acao(int s, int a)
        {
            return (0, 0);
        }
    }
}
