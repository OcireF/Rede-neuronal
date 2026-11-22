using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaciocinioAutomatico
{
    public class Damas : IProblema
    {
        //private double[][] tabuleiro;
        private (int x, int y) size;
        private List<(int x, int y)> damas = new List<(int x, int y)>();
        private Random r = new Random();


        public Damas(int x, int y)
        {
            size = (x, y);
            //initTabuleiro(x, y);
            for(int i = 0; i < x; i++) //1 rainha por coluna
            {
                damas.Add((i,r.Next(0,y)));
            }
        }

        /*public void initTabuleiro(int x, int y)
        {
            tabuleiro = new double[x][];
            for (int i = 0; i < x; i++)
            {
                tabuleiro[i] = new double[y];
            }
        }*/

        public Func<double[], double> avaliar()
        {
            return (double[] estado) => avaliar(estado);
        }

        public double avaliar(double[] estado)
        {
            int numAtaques = 0;
            for(int r=0; r < size.x; r++)
            {
                for(int i = 1; i + r < size.x; i++)//checa se esta a atacar as procimas rainhas, PS: isto é feito para não ter repetidas umas com as outras
                {
                    if (estado[r] == estado[r + i]) //conflito horizontal
                    {
                        numAtaques++;
                    }

                    if (estado[r] - i == estado[r + i]) //diagonal up
                    {
                        numAtaques++;
                    }

                    if (estado[r] + i == estado[r + i]) //diagonal down
                    {
                        numAtaques++;
                    }

                }
            }
            return -numAtaques; //valor negativo, a tentar ser 0
        }

        public double[] estado_inicial() //exemplo [4,6,3,2,1,4,0] para um tabuleiro de 7x7
        {
            double[] estadoInicial = new double[size.x]; // onde vão ser armasenados a linha de cada rainha
            for (int i = 0; i < size.x; i++) //1 rainha por coluna
            {
                estadoInicial[i] = r.Next(0, size.y);
            }
            return estadoInicial;
        }

        public List<double[]> sucessores(double[] estado)
        {
            List<double[]> sucessores = new List<double[]>();
            for (int i = 0; i < size.x; i++) //1 rainha por coluna
            {
                int rainhaLinha = (int) estado[i];
                /*if(rainhaLinha - 1 >= 0) //movimento up
                {
                    double[] sucessorUp = (double[]) estado.Clone();
                    sucessorUp[i] = rainhaLinha - 1;
                    sucessores.Add(sucessorUp);
                }
                if(rainhaLinha + 1 < size.y) //movimento down
                {
                    double[] sucessorDown = (double[])estado.Clone();
                    sucessorDown[i] = rainhaLinha + 1;
                    sucessores.Add(sucessorDown);
                }*/

                for (int l = 0; l < size.y;l++) //sucessores de movimento vertical completo de 1 rainha
                {
                    if(l == rainhaLinha) //posicao original
                    {
                        continue;
                    }

                    double[] sucessor = (double[])estado.Clone(); //clono o estado atual
                    sucessor[i] = l;            //atualiso o movimento possivel da rainha
                    sucessores.Add(sucessor);   //e marco como sucessor
                }

            }
            return sucessores;
        }
    }
}
