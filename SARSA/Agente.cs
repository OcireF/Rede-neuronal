using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SARSA
{
    public class Agente
    {
        private IAlgoritmo algoritmo;
        private IAmbiente ambiente;
        //dados 
        private double[] estado;                                //s
        private int acao;                                       //a
        /*private double recompensa;                              //r
        private double[] estadoLinha;                           //s'
        private int acaoLinha;                                  //a'*/
        //dados do loop
        private int episodio = 0;
        private int numEpisodios;
        private int passo = 0;
        private int numPassos;

        public Agente(IAlgoritmo algoritmo, IAmbiente ambiente, int numEpisodios, int numPassos)//sarsa
        {
            this.algoritmo = algoritmo;
            this.ambiente = ambiente;
            this.numEpisodios = numEpisodios;
            this.numPassos = numPassos;
            //estado inicial e acao escolhida
            estado = ambiente.GetInicialState();
            acao = algoritmo.seleccionar_acao(estado);
        }

        public double[] RunPasso()
        {
            if(episodio > numEpisodios) //ultimo ep acabou
            {
                return estado; //retorna o ultimo estado
            }
            if(passo > numPassos || ambiente.IsTerminal(estado)) //reiniciar para um novo episodio
            {
                estado = ambiente.GetInicialState();
                acao = algoritmo.seleccionar_acao(estado);

                episodio += 1; //proximo episodio
                passo = 0; //reseta os passos
                return estado;
            }
            (estado, acao) = algoritmo.FazerPasso(estado, acao);
            /*(recompensa, estadoLinha) = algoritmo.executar_acao(estado, acao); //Executar ação "a"
            acaoLinha = algoritmo.seleccionar_acao(estadoLinha);                      //seleccionar acao Linha (a') para um estado linha (s')
            recompensa = algoritmo.Aprender(estado, acao, recompensa, estadoLinha, acaoLinha, ambiente.IsTerminal(estadoLinha)); //if s' is terminal
            estado = estadoLinha;   //atualizar estado (s <- s')
            acao = acaoLinha;       //atualizar acao (a <- a')*/

            return estado;
        }
        public double[] GetCurretEstado()
        {
            return estado;
        }


    }
}
