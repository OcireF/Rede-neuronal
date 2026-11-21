using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaciocinioAutomatico
{
    public class HillClimbingEstocasticoUnicoSucessor : HillClimbing
    {
        private Random r = new Random();

        public HillClimbingEstocasticoUnicoSucessor(IProblema problema) : base(problema)
        {

        }

        //Override escolha aleatoria, escolhe o primeiro sucessor que tiver maior valor que o atual, aleatoriamente
        public override double[] EscolherSucessor(List<double[]> sucessores, Func<double[], double> avaliador, double valorEstadoAtual) // IComparer<double[]> comparador
        {
            if (sucessores.Count == 0) //lista vasia
            {
                return null;
            }


            int[] indexs = Enumerable.Range(0, sucessores.Count).ToArray(); //cria index para dar suffle
            r.Shuffle(indexs);

            double[] sucessorEscolhido = null;
            double valorDoEscolhido = valorEstadoAtual;

            foreach (int i in indexs)
            {
                if (avaliador.Invoke(sucessores[indexs[i]]) - valorDoEscolhido > 0) //se o valor for maior é o novo sucessor escolhido
                {
                    sucessorEscolhido = sucessores[indexs[i]];
                    return sucessorEscolhido;
                }
            }
            return sucessorEscolhido;
        }
    }
}
