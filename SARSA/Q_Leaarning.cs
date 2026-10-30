using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SARSA
{
    public class Q_Learning : SARSA_Algoritmo
    {

        public Q_Learning(IAmbiente ambiente, double gamma, double alfa, double efen, double valor_omissao = 0.0, string fileToLoadPoliticas = null) : base(ambiente,gamma,alfa,efen,valor_omissao,fileToLoadPoliticas)
        {

        }

        //public double Aprender(double[] s, int a, double r, double[] sLine, int aLine, bool isFinal)

        public double Aprender(double[] s, int a, double r, double[] sLine, bool isFinal)
        {
            var aline = Acao_sofrega(sLine);
            return Aprender(s, a, r, sLine, aline, isFinal);
        }

        //overwrite Treino para dar no modo Q-learning
        /*public List<double[]> treino(int numEpisodios, int numPassos, bool justLastEP = false)
        {
            List<double[]> estadosTreino = new List<double[]>();
            //CreatePolitica(ambiente.GetInicialState()); //Primeira politica
            //Variables
            double[] estado;                                //s
            int acao;                                       //a
            double recompensa;                              //r
            double[] estadoLinha;                           //s'
            int acaoLinha;                                  //a'
            for (int ep = 0; ep < numEpisodios; ep++) //loop todos os Episodios
            {
                if (justLastEP) //caso queremos apenas salvar o ultimo ep para mostrar o treinamento
                {
                    estadosTreino.Clear(); //limpa o Treino para guardar apenas o ultimo episodio
                }
                estado = ambiente.GetInicialState();        //init s
                acao = seleccionar_acao(estado);            //seleccionar acao (a)
                for (int p = 0; p < numPassos; p++)          //passos
                {
                    estadosTreino.Add(estado); //adicona o estado na lista para mais tarde vermos o treino @TEMP
                    (recompensa, estadoLinha) = executar_acao(estado, acao);//Executar ação "a"
                    //CreatePolitica(estadoLinha);                                    //cria novas politicas para o novo estado caso nao existirem
                    acaoLinha = seleccionar_acao(estadoLinha);                      //seleccionar acao Linha (a') para um estado linha (s')
                    recompensa = Aprender(estado, acao, recompensa, estadoLinha, ambiente.IsTerminal(estadoLinha)); //no Q-lerning o s' é calculado o maxArg e mão o escolhido com efen
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
        }*/

        public (double[] estado, int acao) FazerPasso(double[] estado, int acao)
        {
            (double recompensa, double[] estadoLinha) = executar_acao(estado, acao);//Executar ação "a"
            int acaoLinha = seleccionar_acao(estadoLinha);                      //seleccionar acao Linha (a') para um estado linha (s')
            recompensa = Aprender(estado, acao, recompensa, estadoLinha, ambiente.IsTerminal(estadoLinha)); //no Q-lerning o s' é calculado o maxArg e mão o escolhido com efen

            return (estadoLinha, acaoLinha);
        }

    }
}
