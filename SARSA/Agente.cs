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
        /*private double recompensa;                            //r
        private double[] estadoLinha;                           //s'
        private int acaoLinha;                                  //a'*/
        //dados do loop
        private int episodio = 0;
        private int numEpisodios;
        private int passo = 0;
        private int numPassos;
        private int AgenteID = 0;

        public Agente(IAlgoritmo algoritmo, IAmbiente ambiente, int numEpisodios, int numPassos)
        {
            this.algoritmo = algoritmo;
            this.ambiente = ambiente;
            this.numEpisodios = numEpisodios;
            this.numPassos = numPassos;
            //estado inicial e acao escolhida
            AgenteID = ambiente.RegistarAgente();
            estado = ambiente.GetInicialState(AgenteID);
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
                estado = ambiente.GetInicialState(AgenteID); //estado inicial
                acao = algoritmo.seleccionar_acao(estado);

                episodio += 1; //proximo episodio
                passo = 0; //reseta os passos
                return estado; //estado incial
            }
            (estado, acao) = algoritmo.FazerPasso(estado, acao, AgenteID); //passo feito de acordo com o algorimo, assim este agente pode funcionar para varios tipos

            return estado; //o novo estado depois do passo
        }
        public double[] GetCurretEstado()
        {
            return estado;
        }


    }
}
