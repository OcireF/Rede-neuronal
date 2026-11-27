using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaciocinioAutomatico
{
    public class RotaCidades : IProblema
    {
        private (int x, int y) size;
        private Dictionary<int, (int x, int y) > cidades = new Dictionary<int, (int x, int y)>();
        private int numCidades;
        private Random r = new Random();
        public RotaCidades((int x, int y) size, int numCitys) 
        { 
            this.size = size;
            numCidades = numCitys;
            for (int i = 0; i < numCitys ;i++) //init cidades
            {
                cidades.Add(i, (r.Next(0, size.x), r.Next(0, size.y)));
            }
        }

        public Func<double[], double> avaliar()
        {
            return (double[] estado) => avaliar(estado);
        }

        public double avaliar(double[] estado)
        {
            if(estado == null)
            {
                return double.MinValue;
            }
            double distancia = 0.0;
            for (int i = 0; i < estado.Length - 2; i++) //o -1 é para parar 1 cidade antes.
            {
                distancia += DistanciaEntreCidades((int) estado[i], (int) estado[i + 1]);
            }
            distancia += DistanciaEntreCidades((int) estado[estado.Length - 1], (int)estado[0]); //distancia entre a ultima e a primeira

            return -distancia;//hill clibber sobe por isso fazemos com valores negativos internamente
        }

        public double DistanciaEntreCidades(int cidade1, int cidade2)
        {
            var c1 = cidades[cidade1];
            var c2 = cidades[cidade2];
            double dx = c1.x - c2.x;
            double dy = c1.y - c2.y;
            return Math.Sqrt(dx * dx + dy * dy);
        }

        public double[] estado_inicial()//ordem inicalaleatoria de cidades
        {
            double[] estadoInicial = Enumerable.Range(0, cidades.Count).Select(x => (double)x).ToArray();
            r.Shuffle(estadoInicial);
            return estadoInicial;
        }

        public void NewCitys()
        {
            cidades.Clear();
            for (int i = 0; i < numCidades; i++) //novas cidades
            {
                cidades.Add(i, (r.Next(0, size.x), r.Next(0, size.y)));
            }
        }

        public List<double[]> sucessores(double[] estado)
        {
            List<double[]> sucessores = new List<double[]>();
            for(int i = 0; i < estado.Length - 1; i++)
            {
                var novoEstado = (double[])estado.Clone();
                //swap cidades
                double temp = novoEstado[i];
                novoEstado[i] = novoEstado[i + 1];
                novoEstado[i + 1] = temp;
                sucessores.Add(novoEstado);
            }

            var ultimaTroca = (double[])estado.Clone();
            //swap ultima cidade com a primeira
            double temp2 = ultimaTroca[estado.Length - 1];
            ultimaTroca[estado.Length - 1] = ultimaTroca[0];
            ultimaTroca[0] = temp2;

            sucessores.Add(ultimaTroca);

            return sucessores;
        }

        public Dictionary<int, (int x, int y)> getCidades()
        {
            return cidades;
        }
    }
}
