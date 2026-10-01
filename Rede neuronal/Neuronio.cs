using Rede_neuronal.Interfaces;

namespace Rede_neuronal
{
    public class Neuronio
    {
        private IFuncoaAtivacao ativador;
        public double[] pesos; //pesos do neuronio (w)
        public double pendore; //pendore do neuronio (b)
        public double saida = 0; //saida do neuronio (y)
        public double derivada = 0; //derivada do neuronio (y)


        //Constructor para criar um neuronio com pesos e pendores randomicos
        public Neuronio(int numeroEntradas, IFuncoaAtivacao a)
        {
            InitRandomPesosAndPendores(numeroEntradas); //inicializa pesos e pendores randomicos
            ativador = a;
        }

        //Constructor para criar um neuronio com pesos e pendores especificos
        public Neuronio(double[] pesos, double pendore, IFuncoaAtivacao a)
        {
            this.pesos = pesos;
            this.pendore = pendore;
            ativador = a;
        }

        //inicializa pesos e pendores randomicos
        private void InitRandomPesosAndPendores(int numeroEntradas)
        {
            pesos = new double[numeroEntradas]; //inializa a variavel [numeroEntradas]
            var rand = new Random();
            pendore = rand.NextDouble() * 2 - 1; //random pendore (-1 a 1) nota: o -1 é para acertar o (o a 2)

            for (int entrada = 0; entrada < numeroEntradas; entrada++) //inicializar todas as entradas
            {
                pesos[entrada] = rand.NextDouble() * 2 - 1; //random peso (-1 a 1) nota: o -1 é para acertar o (o a 2)
            }
        }

        //adaptar os pesos e pendores com base no erro, entradas e taxa de aprendizagem
        public void Adaptar(double erro, double[] entradas, double taxaAprendizagem)
        {
            for(int i = 0; i < entradas.Length; i++)
            {
                var variacaoPeso = -taxaAprendizagem * derivada * erro * entradas[i]; //calcula a variacao do peso (delta w)
                pesos[i] += variacaoPeso; //atualiza os peso (w = delta w + w)
            }
            var variacaoPendor = -taxaAprendizagem * derivada * erro; //calcula a variacao do pendore (delta b)
            pendore += variacaoPendor;  //atualiza o pendore (b + delta b)
        }

        //correr o neuronio para calcular a saida (y)
        public double RunNeuronio(double[] entradas)
        {
            double produtoEscalar = ProdutoEscalar(entradas); // (x * w)
            double h = produtoEscalar + pendore;            // h = (x * w) + b
            saida = ativador.FucaoAtivar(h);                //corre a funcao ativadora para ter o "y"
            derivada = ativador.Derivada(h);                //corre a funcao derivada para ter o (y')
            return saida;
        }

        //calcula o produto escalar entre as entradas e os pesos
        public double ProdutoEscalar(double[] entradas)
        {
            double soma = 0;
            for (int i = 0; i < entradas.Length; i++) // soma de todos os produtos (x * w)
            {
                soma += entradas[i] * pesos[i];
            }
            return soma;
        }


    }
}
