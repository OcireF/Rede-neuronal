using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;

namespace SARSA
{
    public class SARSA_Algoritmo
    {
        public Random r = new Random();
        public Dictionary<double[], Dictionary<int, double>> Politica = new Dictionary<double[], Dictionary<int, double>>(new ComparadorEstados()); //Q(s,a) = recompensa
        private IAmbiente ambiente;
        public double gamma; //y
        public double alfa;
        public double efen;
        public double valor_omissao;

        public SARSA_Algoritmo(IAmbiente ambiente, double gamma, double alfa, double efen, double volor_omissao = 0.0)
        {
            this.ambiente = ambiente;
            this.gamma = gamma;
            this.alfa = alfa;
            this.efen = efen;
        }

        public double Aprender(double[] s, int a, double r, double[] sLine, int aLine, bool isFinal) //s, a, r, s’, a’, final
        {
            double j = 0;
            if (isFinal)
            {
                j = r; // J = r
            }
            else
            {
                j = r + (gamma * Politica[sLine][ aLine]); // J = r + γQ(s’, a’)
                Politica[s][a] += alfa * (j - Politica[s][a]); // Q(s, a) = Q(s, a) + alfa(J – Q(s, a))
            }
            return j;
        }
        
        //Chamar esta função para adicioar sempre que tiver novos estados para adicionar na Politica
        public void CreatePolitica(double[] estado)
        {

            if (ambiente.IsTerminal(estado)) // não adiciona nenhuma polita (esta no estado terminal)
            {
                return;
            }
            if (Politica.ContainsKey(estado)) // o estado já existe na politica
            {
                return;
            }

            Politica.Add(estado, new Dictionary<int, double>());
            //cria as acoes do estado
            var acoes = ambiente.AcoesPossiveis(estado);
            foreach(int a in acoes)
            {
                Politica[estado].Add( a, valor_omissao);
            }
        }

        public List<double[]> treino(int numEpisodios, int numPassos)
        {
            List<double[]> estadosTreino = new List<double[]>();
            CreatePolitica(ambiente.GetInicialState()); //Primeira politica
            //Variables
            double[] estado;                                //s
            int acao;                                       //a
            double recompensa;                              //r
            double[] estadoLinha;                           //s'
            int acaoLinha;                                  //a'
            for (int ep = 0; ep < numEpisodios; ep++)
            {
                estado = ambiente.GetInicialState();        //init s
                acao = seleccionar_acao(estado);            //seleccionar acao (a)
                for(int p = 0; p < numPassos; p++)          //passos
                {
                    estadosTreino.Add(estado); //adicona o estado na lista para mais tarde vermos o treino @TEMP
                    (recompensa, estadoLinha) = executar_acao(estado, acao);//Executar ação "a"
                    CreatePolitica(estadoLinha);                                    //cria novas politicas para o novo estado caso nao existirem
                    acaoLinha = seleccionar_acao(estadoLinha);                      //seleccionar acao Linha (a') para um estado linha (s')
                    recompensa = Aprender(estado, acao, recompensa, estadoLinha, acaoLinha,ambiente.IsTerminal(estadoLinha)); //if s' is terminal
                    estado = estadoLinha;   //atualizar estado (s <- s')
                    acao = acaoLinha;       //atualizar acao (a <- a')

                    if (ambiente.IsTerminal(estado))
                    {  //acaba os passos quando estado (s) for terminal
                        estadosTreino.Add(estado); //adiciona o estado final
                        break;
                    }
                }
            }
            return estadosTreino;
        }

        public int seleccionar_acao(double[] estado)
        {
            int acao = 0;
            if ( r.NextDouble() > efen)
            {
                acao = Acao_sofrega(estado); //Aproveitar conhecimento
            }
            else
            {
                acao = Acao_exploracao(estado); //Explorar
            }
            return acao;
        }
        public int Acao_sofrega(double[] estado)
        {
            if (!Politica.ContainsKey(estado)) //terminal não existe mais acoes
            {
                return 0;
            }

            //suffle acoes
            int[] acoes = Politica[estado].Keys.ToArray();
            r.Shuffle(acoes);

            int acao = 0;
            double maxValue = int.MinValue; //minValue para sempre selecionar uma acao

            foreach(int a in acoes)
            {
                if (Politica[estado][a] > maxValue)
                {
                    acao = a; //max argument
                    maxValue = Politica[estado][a];
                }
            }
            return acao; //maxArg
        }

        public int Acao_exploracao(double[] estado)
        {
            if (!Politica.ContainsKey(estado)) //terminal não existe mais acoes
            {
                return 0; 
            }

            int[] acoes = Politica[estado].Keys.ToArray();
            r.Shuffle(acoes); //suffle para ter uma acao aleatoria
            return acoes[0]; //random
        }

        public (double recompensa, double[] estadoLinha) executar_acao(double[] estado, int acao)
        {
            var estadoLinha = ambiente.ExecutarAcao(estado, acao); // ver o novo estado
            var recompensa = ambiente.GetRecompensa(estadoLinha); // recompensa do novo estado
            return (recompensa, estadoLinha);
        }
    }
}
