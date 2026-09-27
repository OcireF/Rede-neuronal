using Rede_neuronal;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Textes Parte 1.1");
        TesteRedeNeuronalXor();
        Console.WriteLine();
        TesteRedeNeuronalRandomValues();

        Console.WriteLine("\nTextes Parte 1.2");
        TesteReconhecimentoBarrasVerticais();
        BigTreinoRedeBarrasVerticais();
    }

    public static void TesteRedeNeuronalXor()
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
        RedeNeuronal rede = new RedeNeuronal(forma, new FucaoAtivadoraStep(), pesos, pendore);

        //testes para todas as combinações de entradas da porta lógica XOR
        //nota o rede.calcRedeNeuronal(input)[0] é para pegar o resultado da saída da rede neural, que é um array de tamanho 1
        double[] input = { 0, 0 };
        Console.WriteLine("XOR com valores predefinidos:");
        Console.WriteLine("Entrada x0=" + input[0] + " e x1=" + input[1] + " resultado=" + rede.calcRedeNeuronal(input)[0]);
        input[1] = 1;
        Console.WriteLine("Entrada x0=" + input[0] + " e x1=" + input[1] + " resultado=" + rede.calcRedeNeuronal(input)[0]);
        input[0] = 1;
        input[1] = 0;
        Console.WriteLine("Entrada x0=" + input[0] + " e x1=" + input[1] + " resultado=" + rede.calcRedeNeuronal(input)[0]);
        input[1] = 1;
        Console.WriteLine("Entrada x0=" + input[0] + " e x1=" + input[1] + " resultado=" + rede.calcRedeNeuronal(input)[0]);
    }

    public static void TesteRedeNeuronalRandomValues()
    {
        //teste para pesos e pendores aleatórios
        int[] forma2 = { 2, 5, 3 };
        RedeNeuronal rede2 = new RedeNeuronal(forma2, new FucaoAtivadoraStep());
        double[] input2 = { 0, 0 };
        Console.WriteLine("Rede com pesos e pendores randomicos(rede[2, 5, 3]):");
        Console.WriteLine("Entrada x0=" + input2[0] + " e x1=" + input2[1] + " resultado=" + rede2.calcRedeNeuronal(input2)[0] + rede2.calcRedeNeuronal(input2)[1] + rede2.calcRedeNeuronal(input2)[2]);
        input2[1] = 1;
        Console.WriteLine("Entrada x0=" + input2[0] + " e x1=" + input2[1] + " resultado=" + rede2.calcRedeNeuronal(input2)[0] + rede2.calcRedeNeuronal(input2)[1] + rede2.calcRedeNeuronal(input2)[2]);
        input2[0] = 1;
        input2[1] = 0;
        Console.WriteLine("Entrada x0=" + input2[0] + " e x1=" + input2[1] + " resultado=" + rede2.calcRedeNeuronal(input2)[0] + rede2.calcRedeNeuronal(input2)[1] + rede2.calcRedeNeuronal(input2)[2]);
        input2[1] = 1;
        Console.WriteLine("Entrada x0=" + input2[0] + " e x1=" + input2[1] + " resultado=" + rede2.calcRedeNeuronal(input2)[0] + rede2.calcRedeNeuronal(input2)[1] + rede2.calcRedeNeuronal(input2)[2]);
    }

    //treinar até ter uma rede que complete o treino com sucesso
    public static RedeNeuronal TreinoRedeBarrasVerticais()
    {
        int[] forma = { 9, 3, 1 };
        double taxaAprendizagem = 0.2;
        int epocas = 1000;
        double erroMaximo = 0.05;
        var fucaoTanh = new FucaoAtivadoraTanh();

        double[][] entradasTreino = {
                new double[]{ 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                new double[]{1, 0, 0, 1, 0, 0, 1, 0, 0},
                new double[]{0, 1, 0, 0, 1, 0, 0, 1, 0},
                new double[]{0, 0, 1, 0, 0, 1, 0, 0, 1},
                new double[]{1, 1, 1, 0, 0, 0, 0, 0, 0},
                new double[]{0, 0, 0, 1, 1, 1, 0, 0, 0},
                new double[]{0, 0, 0, 0, 0, 0, 1, 1, 1}
        };
        double[][] saidasTreino =
        {
            new double[]{0},
            new double[]{1},
            new double[]{1},
            new double[]{1},
            new double[]{0},
            new double[]{0},
            new double[]{0},
        };

        while (true) {
            Console.WriteLine("Treino da rede Barras Verticais");
            RedeNeuronal rede = new RedeNeuronal(forma, fucaoTanh);
            var foiUmSuceso = rede.Treinar(entradasTreino, saidasTreino, epocas, erroMaximo, taxaAprendizagem);
            Console.WriteLine("O treino foi um Suceso? " + (foiUmSuceso ? "sim" : "não"));
            if (foiUmSuceso) // se não foi um sucesso não vale a pena testar o treino
            {
                return rede;
            }
            else
            {
                Console.WriteLine("RIP: descartar Rede\n");
            }
        }

    }

    public static void TesteReconhecimentoBarrasVerticais()
    {
        var rede = TreinoRedeBarrasVerticais();

        double[][] entradasTeste = {
                new double[]{ 
                    0, 0, 0, 
                    1, 0, 0, 
                    1, 0, 0 },
                new double[]{
                    0, 1, 0,
                    0, 1, 0,
                    0, 0, 0 },
                new double[]{
                    0, 0, 1,
                    0, 0, 0,
                    0, 0, 1},
                new double[]{
                    0, 1, 1,
                    0, 0, 0,
                    0, 0, 0 },
                new double[]{
                    0, 0, 0,
                    1, 0, 1,
                    0, 0, 0 },
                new double[]{
                    0, 0, 0,
                    0, 0, 0,
                    1, 1, 0 },
                new double[]{
                    1, 0, 0,
                    1, 0, 1,
                    1, 0, 0 },
                new double[]{
                    0, 1, 0,
                    0, 1, 0,
                    0, 1, 0 } 
        };

        for(int t = 0; t < entradasTeste.Length; t++)
        {
            ShowQuadriculas(entradasTeste[t], 3);
            double[] result = rede.calcRedeNeuronal(entradasTeste[t]);
            Console.WriteLine("Result=" + result[0]);
        }
    }

    public static void BigTreinoRedeBarrasVerticais()
    {
        //variaveis
        int[] forma = { 9, 3, 1 };
        double taxaAprendizagem = 0.2;
        int epocas = 10000;
        double erroMaximo = 0.05;
        var fucaoTanh = new FucaoAtivadoraTanh();

        //pega imagens aleatorias reais
        Random r = new Random();
        var numeroDeImagens = 332;                //numero de imagens a testar
        int[] allImages = Enumerable.Range(0, 511).ToArray();           //gera uma array de {0 a 511}
        r.Shuffle(allImages);                                           //mistura todas as imagens no array

        //A lista éra apenas para ser mais simples adicionar os valores que estamos a procura
        //nota tambem podia ter usado "HashSet" para não ter numeros duplicados, 
        List<int> ListimagesTreino = allImages.Take(numeroDeImagens).ToList(); //pega as imagens que vamos usar para o treino
        ListimagesTreino.AddRange(new int[] { 73, 146, 292 });              // garantir que tenha todas as entradas que deve returnar 1
        int[] imagensTreino = ListimagesTreino.ToArray();

        r.Shuffle(imagensTreino);  //shuggle again para misturar os valores lá dentro

        int treinoSize = imagensTreino.Length;


        double[][] entradasDeTreino = new double[treinoSize][];
        double[][] SaidasEsperada = new double[treinoSize][];

        for (int i = 0; i < treinoSize; i++)
        {
            double[] imagem = {
                    0, 0, 0,
                    0, 0, 0,
                    0, 0, 0 };

            for (int b=0; b < imagem.Length; b++) // 9 bits
            {
                imagem[b] = (imagensTreino[i] >> b) & 1; //Capturar o valor de "b" bit
            }

            entradasDeTreino[i] = imagem;

            if(imagensTreino[i] == 73 || imagensTreino[i] == 146 || imagensTreino[i] == 292) //nota isto é apenas porque temos um numero limitado de imagens que são linhas verticais
            {
                SaidasEsperada[i] = new double[]{ 1 } ;
            }
            else //saida não é uma linha vertical
            {
                SaidasEsperada[i] = new double[] { 0 };
            }

        }

        RedeNeuronal rede = new RedeNeuronal(forma, fucaoTanh); //cria uma rede neuronal
        while (true)
        {
            Console.WriteLine("Treino da rede Barras Verticais (Big Treino)");
            var foiUmSuceso = rede.Treinar(entradasDeTreino, SaidasEsperada, epocas, erroMaximo, taxaAprendizagem);
            Console.WriteLine("O treino foi um Suceso? " + (foiUmSuceso ? "sim" : "não"));
            if (foiUmSuceso) // se não foi um sucesso não vale a pena testar o treino
            {
                break;
            }
            else //caso o treino foir um insucesso criamos uma nova rede e tentamos denovo
            {
                rede = new RedeNeuronal(forma, fucaoTanh);
                Console.WriteLine("RIP: descartar Rede e criar outra\n");
            }
        }

        //Texte da rede
        Console.WriteLine("Teste Da Rede");
        double[][] entradasTeste = {
                new double[]{
                    0, 0, 0,
                    1, 0, 0,
                    1, 0, 0 },
                new double[]{
                    0, 1, 0,
                    0, 1, 0,
                    0, 0, 0 },
                new double[]{
                    0, 0, 1,
                    0, 0, 0,
                    0, 0, 1},
                new double[]{
                    0, 1, 1,
                    0, 0, 0,
                    0, 0, 0 },
                new double[]{
                    0, 0, 0,
                    1, 0, 1,
                    0, 0, 0 },
                new double[]{
                    0, 0, 0,
                    0, 0, 0,
                    1, 1, 0 },
                new double[]{
                    1, 0, 0,
                    1, 0, 1,
                    1, 0, 0 },
                new double[]{
                    0, 1, 0,
                    0, 1, 0,
                    0, 1, 0 }
        };

        for (int t = 0; t < entradasTeste.Length; t++)
        {
            ShowQuadriculas(entradasTeste[t], 3);
            double[] result = rede.calcRedeNeuronal(entradasTeste[t]);
            Console.WriteLine("Result=" + result[0]);
        }

    }



    //codigo apenas para viulizar as imagens que estamos a colocar na rede neuronal
    public static void ShowQuadriculas(double[]image,int largura)
    {
        int l = 0;

        for (int i = 0; i < image.Length; i++)
        {
            Console.Write(image[i]);
            l++;
            if (l == largura) 
            {
                Console.Write("\n");
                l = 0;
            }
        }
    }


}