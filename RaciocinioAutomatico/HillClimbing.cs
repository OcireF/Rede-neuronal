namespace RaciocinioAutomatico
{
    public class HillClimbing
    {
        public double[] NormalHillClimbing(IProblema problema)
        {
            double[] estado = problema.estado_inicial(); // estado inicial
            double valorEstado = double.MinValue; // - infinito

            while (true)
            {
                List<double[]> sucessores = problema.sucessores(estado);
                double[] novoEstado = Max(sucessores, problema.avaliar());
                double valorNovoEstado = problema.avaliar(novoEstado);

                if (novoEstado == null || valorNovoEstado <= valorEstado) //região plana/topo local ou não existem mais estados
                {
                    break;
                }
                else
                {
                    estado = novoEstado;
                    valorEstado = valorNovoEstado;
                }
            }
            return estado;
        }

        public double[] Max(List<double[]> sucessores, Func<double[], double> avaliador) // IComparer<double[]> comparador
        {
            if(sucessores.Count == 0) //lista vasia
            {
                return null;
            }

            double[] sucessorEscolhido = sucessores[0]; //defalt valio
            double valorDoEscolhido = avaliador.Invoke(sucessorEscolhido); //pegar o primeiro valor

            foreach (double[] sucessor in sucessores)
            {
                if( avaliador.Invoke(sucessor) - valorDoEscolhido > 0) //se o valor for maior é o novo sucessor escolhido
                {
                    sucessorEscolhido = sucessor;
                }
            }
            return sucessorEscolhido;
        }
    }
}
