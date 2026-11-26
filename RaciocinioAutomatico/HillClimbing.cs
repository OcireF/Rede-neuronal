namespace RaciocinioAutomatico
{
    public class HillClimbing : IAlgoritmoHill
    {
        protected double[] estado;
        protected double valorEstado;
        protected bool completed = false;
        protected IProblema problema;

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

        public virtual void FaserPasso()
        {
            if (completed) //hillClibing completo
            {
                return;
            }

            List<double[]> sucessores = problema.sucessores(estado);    //vé tudo a volta
            double[] novoEstado = EscolherSucessor(sucessores, problema.avaliar(),valorEstado);  //vé o maior valor e escolhe o estado seguinte
            double valorNovoEstado = problema.avaliar(novoEstado); //valor deste novo estado

            if (novoEstado == null || valorNovoEstado <= valorEstado) //região plana/maximo local ou não existem mais estados
            {
                completed = true;
                return;
            }
            else //uptade o novo estado e valor
            {
                estado = novoEstado;
                valorEstado = valorNovoEstado;
                return;
            }
        }

        //Max Fuction, retorna o maior valor dos Sucessores
        public virtual double[] EscolherSucessor(List<double[]> sucessores, Func<double[], double> avaliador, double valorEstadoAtual) // IComparer<double[]> comparador
        {
            if(sucessores.Count == 0) //lista vasia
            {
                return null;
            }

            double[] sucessorEscolhido = null;
            double valorDoEscolhido = valorEstadoAtual;

            foreach (double[] sucessor in sucessores)
            {
                double valorSucessor = avaliador.Invoke(sucessor);
                if (valorSucessor - valorDoEscolhido > 0) //se o valor for maior é o novo sucessor escolhido
                {
                    sucessorEscolhido = sucessor;
                    valorDoEscolhido = valorSucessor;
                }
            }
            return sucessorEscolhido;
        }

        //renicia o hillClimbing, voltando ao estado inicial e valor minimo
        public void Restart()
        {
            estado = problema.estado_inicial(); // estado inicial
            valorEstado = double.MinValue; // - infinito
            completed = false;
        }

        //estado atual do hillClimbing
        public double[] GetEstado()
        {
            return estado;
        }

        public bool IsCompleted()
        {
            return completed;
        }


    }
}
