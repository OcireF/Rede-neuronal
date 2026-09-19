using Rede_neuronal;

class Program
{
    static void Main(string[] args)
    {
        int[] forma = { 2, 2, 1 };
        double[][][] pesos = {
                new double[][]
                {
                    new double[] { 1, -1 },
                    new double[] { -1, 1 }
                },
                new double[][]
                {
                    new double[] { 1, 1 }
                }
            };
        double[][] pendore =
        {
            new double[]
            {
               -0.5, -0.5
            },
            new double[]
            {
                -0.5
            }

        };
        RedeNeuronal rede = new RedeNeuronal(forma,new FuncoaAtivacao());
        rede.setPesosAndPendores(pesos, pendore);

        int[] input = { 0, 0 };
        Console.WriteLine("Entrada x0=0 e x1=0 =" + rede.calcRedeNeuronal(input)[0] );
        input[1] = 1;
        Console.WriteLine("Entrada x0=0 e x1=1 =" + rede.calcRedeNeuronal(input)[0]);
        input[0] = 1;
        input[1] = 0;
        Console.WriteLine("Entrada x0=1 e x1=0 =" + rede.calcRedeNeuronal(input)[0]);
        input[1] = 1;
        Console.WriteLine("Entrada x0=1 e x1=1 =" + rede.calcRedeNeuronal(input)[0]);
        while (true) ;
    }
}
