using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaciocinioAutomatico
{
    public class HillClimbingEstocastico : HillClimbing
    {
        private Random r = new Random();

        public HillClimbingEstocastico(IProblema problema) : base(problema)
        {

        }

        //Override escolha aleatoria, escolhe o primeiro sucessor que tiver maior valor que o atual, aleatoriamente
        public override double[] EscolherSucessor(List<double[]> sucessores, Func<double[], double> avaliador, double valorEstadoAtual) // IComparer<double[]> comparador
        {
            if (sucessores.Count == 0) //lista vasia
            {
                return null;
            }

            List<double[]> sucessoresEscolhidos = new List<double[]>();
            double valorDoEscolhido = valorEstadoAtual;

            foreach (double[] sucessor in sucessores)
            {
                if (avaliador.Invoke(sucessor) - valorDoEscolhido > 0) //se o valor for maior é o novo sucessor escolhido
                {
                    sucessoresEscolhidos.Add(sucessor);
                }
            }

            if (sucessoresEscolhidos.Count > 0)
            {
                int[] indexs = Enumerable.Range(0, sucessoresEscolhidos.Count).ToArray(); //cria index para dar suffle
                r.Shuffle(indexs);

                return sucessoresEscolhidos[indexs[0]]; //sucessor aleatorio
            }
            else
            {
                return null;
            }
        }

    }
}
