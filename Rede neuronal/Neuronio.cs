namespace Rede_neuronal
{
    public class Neuronio
    {
        private FuncoaAtivacao ativador;
        public Neuronio(FuncoaAtivacao a)
        {
            ativador = a;
        }
        //correr o neuronio
        public int RunNeuronio(int[] entradas, double[] pesos, double pendore)
        {
            double produtoEscalar = ProdutoEscalar(entradas,pesos); // (x * w)
            double h = produtoEscalar + pendore; // h = (x * w) + b
            return ativador.FucaoAtivar(h); //corre a funcao ativadora
        }

        public double ProdutoEscalar(int[] entradas, double[] pesos)
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
