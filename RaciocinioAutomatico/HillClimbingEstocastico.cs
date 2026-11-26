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

        //Override escolha aleatoria, escolhe aleatoriamente entre os sucessores que tiverem maior valor que o atual
        public override double[] EscolherSucessor(List<double[]> sucessores, Func<double[], double> avaliador, double valorEstadoAtual) // IComparer<double[]> comparador
        {
            if (sucessores.Count == 0) //lista vasia
            {
                return null;
            }

            List<double[]> sucessoresEscolhidos = new List<double[]>();
            double valorDoEscolhido = valorEstadoAtual;

            foreach (double[] sucessor in sucessores) //verifica todos os sucessores
            {
                if (avaliador.Invoke(sucessor) - valorDoEscolhido > 0) //se o valor for maior é um sucessor escolhido
                {
                    sucessoresEscolhidos.Add(sucessor);
                }
            }

            if (sucessoresEscolhidos.Count > 0) //se existirem sucessores escolhidos, escolhe aleatoriamente entre eles
            {
                return sucessoresEscolhidos[r.Next(0, sucessoresEscolhidos.Count)]; //sucessor aleatorio
            }
            else
            {
                return null;
            }
        }

    }
}
