using Rede_neuronal;

class Program
{
    static void Main(string[] args)
    {
        //Projeto de rede neural para simular a porta lógica XOR
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
        RedeNeuronal rede = new RedeNeuronal(forma,new FucaoAtivadoraStep(),pesos, pendore);

        //testes para todas as combinações de entradas da porta lógica XOR
        //nota o rede.calcRedeNeuronal(input)[0] é para pegar o resultado da saída da rede neural, que é um array de tamanho 1
        int[] input = { 0, 0 };
        Console.WriteLine("XOR com valores predefinidos:");
        Console.WriteLine("Entrada x0=" + input[0] + " e x1=" + input[1] + " resultado=" + rede.calcRedeNeuronal(input)[0]);
        input[1] = 1;
        Console.WriteLine("Entrada x0=" + input[0] + " e x1=" + input[1] + " resultado=" + rede.calcRedeNeuronal(input)[0]);
        input[0] = 1;
        input[1] = 0;
        Console.WriteLine("Entrada x0=" + input[0] + " e x1=" + input[1] + " resultado=" + rede.calcRedeNeuronal(input)[0]);
        input[1] = 1;
        Console.WriteLine("Entrada x0=" + input[0] + " e x1=" + input[1] + " resultado=" + rede.calcRedeNeuronal(input)[0]);

        //teste para pesos e pendores aleatórios
        int[] forma2 = { 2, 5, 3 };
        RedeNeuronal rede2 = new RedeNeuronal(forma2,new FucaoAtivadoraStep());
        int[] input2 = { 0, 0 };
        Console.WriteLine("\nRede com pesos e pendores randomicos(rede[2, 5, 3]):");
        Console.WriteLine("Entrada x0=" + input2[0] + " e x1=" + input2[1] + " resultado=" + rede2.calcRedeNeuronal(input2)[0] + rede2.calcRedeNeuronal(input2)[1] + rede2.calcRedeNeuronal(input2)[2]);
        input2[1] = 1;
        Console.WriteLine("Entrada x0=" + input2[0] + " e x1=" + input2[1] + " resultado=" + rede2.calcRedeNeuronal(input2)[0] + rede2.calcRedeNeuronal(input2)[1] + rede2.calcRedeNeuronal(input2)[2]);
        input2[0] = 1;
        input2[1] = 0;
        Console.WriteLine("Entrada x0=" + input2[0] + " e x1=" + input2[1] + " resultado=" + rede2.calcRedeNeuronal(input2)[0] + rede2.calcRedeNeuronal(input2)[1] + rede2.calcRedeNeuronal(input2)[2]);
        input2[1] = 1;
        Console.WriteLine("Entrada x0=" + input2[0] + " e x1=" + input2[1] + " resultado=" + rede2.calcRedeNeuronal(input2)[0] + rede2.calcRedeNeuronal(input2)[1] + rede2.calcRedeNeuronal(input2)[2]);
    }
}
