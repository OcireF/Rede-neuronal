using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaciocinioAutomatico
{
    public class Montes : IProblema
    {
        private double[][] mapa;
        private Random r = new Random();

        public Montes(int x , int y) 
        {
            initMap(x, y);
        }

        public void initMap(int x, int y)
        {
            mapa = new double[x][];
            for (int i = 0; i < x; i++)
            {
                mapa[i] = new double[y];
            }
        }

        public Func<double[], double> avaliar()
        {
            return (double[] estado) => avaliar(estado);
        }

        public double avaliar(double[] estado)
        {
            return mapa[(int)estado[0]][(int)estado[1]];
        }

        public double[] estado_inicial()
        {
            return new double[] { r.Next(0, mapa.Length), r.Next(0, mapa[0].Length) };
        }

        public List<double[]> sucessores(double[] estado)
        {
            int x = (int)estado[0];
            int y = (int)estado[1];
            List<double[]> sucessores = new List<double[]>();
            if (x - 1 >= 0) //left
            {
                if (mapa[x - 1][y] != 1)
                {
                    sucessores.Add(new double[] { x - 1, y });
                }
            }
            if (y - 1 >= 0) //up
            {
                if (mapa[x][y - 1] != 1)
                {
                    sucessores.Add(new double[] { x , y - 1});
                }
            }
            if (x + 1 < mapa.Length) //right
            {
                if (mapa[x + 1][y] != 1)
                {
                    sucessores.Add(new double[] { x + 1, y });
                }
            }
            if (y + 1 < mapa[x].Length) //down
            {
                if (mapa[x][y + 1] != 1)
                {
                    sucessores.Add(new double[] { x , y + 1});
                }
            }
            return sucessores;
        }
    }
}
