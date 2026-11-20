namespace RaciocinioAutomatico
{
    public class HillClimbing
    {
        private double[] estado;
        private double valorEstado;
        private bool complete = false;
        private IProblema problema;

        public HillClimbing(IProblema problema)
        {
            estado = problema.estado_inicial(); // estado inicial
            valorEstado = double.MinValue; // - infinito
            this.problema = problema; //problema
        }

        //exemplo do codigo do prof
        /*public double[] NormalHillClimbing(IProblema problema)
        {
            estado = problema.estado_inicial(); // estado inicial
            valorEstado = double.MinValue; // - infinito

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
        }*/

        public void FaserPasso()
        {
            if (complete) //hillClibing completo
            {
                return;
            }

            List<double[]> sucessores = problema.sucessores(estado);    //vé tudo a volta
            double[] novoEstado = Max(sucessores, problema.avaliar());  //vé o maior valor e escolhe o estado seguinte
            double valorNovoEstado = problema.avaliar(novoEstado);      //pega o novoValor do estado escolhido

            if (novoEstado == null || valorNovoEstado <= valorEstado) //região plana/topo local ou não existem mais estados
            {
                complete = true;
                return;
            }
            else //uptade o novo estado e valor
            {
                estado = novoEstado;
                valorEstado = valorNovoEstado;
                return;
            }
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

        public void Restart()
        {
            estado = problema.estado_inicial(); // estado inicial
            valorEstado = double.MinValue; // - infinito
            complete = false;
        }

        public double[] GetEstado()
        {
            return estado;
        }

    }
}
