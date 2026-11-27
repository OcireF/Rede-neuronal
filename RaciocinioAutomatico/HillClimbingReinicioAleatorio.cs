using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace RaciocinioAutomatico
{
    public class HillClimbingReinicioAleatorio : IAlgoritmoHill
    {
        private IProblema problema;
        private int max_reinicios;
        private int loop = 0;
        private HillClimbing hillClimber;
        private bool foundBest = false;
        private double[] melhorEstado = null;
        private double valor_melhor_estado = double.MinValue;
        private double valorAlvo;
        public int passos = 0;

        public HillClimbingReinicioAleatorio(double valorAlvo, int max_reinicios, HillClimbing hillClimber, IProblema problema)
        {
            this.hillClimber = hillClimber;     //algorimo a usar para chegar nos objs
            this.valorAlvo = valorAlvo;         //valor alvo que estamos a tentar chegar
            this.max_reinicios = max_reinicios; //maximo de reinicios para chegar no obj
            this.problema = problema;           //ambiente do problema
            this.hillClimber.SuprimirPrints();
        }

        public void FaserPasso()
        {
            passos++;
            if (foundBest || loop >= max_reinicios)
            {
                return;
            }

            if (!hillClimber.IsCompleted()) //se o hillClimber ainda não finalisou ficamos no seu loop
            {
                hillClimber.FaserPasso();               //fazer um passo no algoritmo
                return;
            }
            else //hillClimber acabou agora verificação dos resultados
            {
                double[] estadoHillClimber = hillClimber.GetEstado();
                double valorEstadoNovo = problema.avaliar(estadoHillClimber);

                if (valorEstadoNovo >= valorAlvo) //ESTADO COM VALOR ALVO ENCONTRADO
                {
                    foundBest = true;                                   //melhor encontrado
                    melhorEstado = (double[])estadoHillClimber.Clone(); //clone o melhor estado
                    valor_melhor_estado = valorEstadoNovo;              //valor do melhor estado
                    PrintResults();
                    return;//break;
                }
                else //não encontramos o objetivo neste loop
                {
                    if(valorEstadoNovo > valor_melhor_estado) // o valor do estado é maior do que o maior encontrado até agora
                    {
                        melhorEstado = (double[]) estadoHillClimber.Clone(); //novo melhor estado até agora
                        valor_melhor_estado = valorEstadoNovo;               //valor do melhor estado até agora
                    }
                }

                //reset do hillClimber
                hillClimber.Restart();  //renicia o hillCliber
                loop++;                 //loop++

                if(loop >= max_reinicios) //não vai ter mais loops por isso dar print dos resultados
                {
                    PrintResults();
                }
            }
        }

        public double[] GetMelhorEstado()
        {
            return melhorEstado;
        }

        public double[] GetEstado()
        {
            if(loop >= max_reinicios) //caso finalizou retorna o melhor estado encontrado
            { 
                return melhorEstado;
            }
            else //se não retorna o estado atual do hillClimber
            {
                return hillClimber.GetEstado();
            }
        }

        public void Restart()
        {
            foundBest = false;
            loop = 0;
            melhorEstado = null;
            valor_melhor_estado = double.MinValue;
            hillClimber.Restart();
            passos = 0;
        }

        public void PrintResults()
        {
            Console.WriteLine(
                "\nAlgoritmo : Reinicio Aleatorio" +
                "\n Passos feitos = " + passos +
                "\n Reset feitos = " + loop +
                "\n Valor do melhor Estado = " + valor_melhor_estado +
                "\n Melhor Estado = [" + string.Join(", ", melhorEstado) + "]"
                );
        }
    }
}
