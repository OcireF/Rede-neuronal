namespace Rede_neuronal
{
    public class Neuronio
    {
        private FuncoaAtivacao ativador;
        public double[] pesos; //pesos do neuronio (w)
        public double pendore; //pendore do neuronio (b)
        public int saida = 0; //saida do neuronio (y)


        //Constructor para criar um neuronio com pesos e pendores randomicos
        public Neuronio(int numeroEntradas, FuncoaAtivacao a)
        {
            InitRandomPesosAndPendores(numeroEntradas);
            ativador = a;
        }

        //Constructor para criar um neuronio com pesos e pendores especificos
        public Neuronio(double[] pesos, double pendore, FuncoaAtivacao a)
        {
            this.pesos = pesos;
            this.pendore = pendore;
            ativador = a;
        }

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


        //correr o neuronio para calcular a saida (y)
        public int RunNeuronio(int[] entradas)
        {
            double produtoEscalar = ProdutoEscalar(entradas); // (x * w)
            double h = produtoEscalar + pendore;            // h = (x * w) + b
            saida = ativador.FucaoAtivar(h);                //corre a funcao ativadora para ter o "y"
            return saida;                                   //corre a funcao ativadora
        }

        //calcula o produto escalar entre as entradas e os pesos
        public double ProdutoEscalar(int[] entradas)
        {
            double soma = 0;
            for (int i = 0; i < entradas.Length; i++)
            {
                soma += entradas[i] * pesos[i];
            }
            return soma;

        }


    }
}
