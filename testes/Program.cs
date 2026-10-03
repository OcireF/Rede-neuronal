using Rede_neuronal;
using System.ComponentModel;
using System.Diagnostics;

class Program
{
    static bool showPesosPendores = false;
    static bool showArt = false;
    static int decimalCases = 3; //isto é para a apresentação de percentagens na consola
    static string trainingErrorsFileName = null;
    //Variaveis dos treinos:
    static double taxaAprendizagem = 0.02; //velocidade do treino
    static int epocas = 1000; // Epocas para treinar
    static double erroMaximo = 0.001; //0,1%

    static void Main(string[] args)
    {

        int test = 0;
        while(test != 99)
        {
            test = 0;
            Console.WriteLine(
                "\nTextes Parte 1.1\n" +
                "(1) - Rede Neuronal Xor (preValues)\n" +
                "(2) - Rede Neuronal Random Pesos e Pendores\n" +
                "\n" +
                "Textes Parte 1.2\n" +
                "(3) - Reconhecimento de Barras Verticais\n" +
                "(4) - BigInput(treino) Reconhecimento de Barras Verticais\n" +
                "(5) - treinamento para o Xor [2,5,1]\n" +
                "\n" +
                "(10) - Show Pesos e Pendores\n" +
                "(11) - Show Art Barras Verticais\n" +
                "(12) - Save Training Errors\n" +
                "(13) - Change Training Parameters\n" +
                "(99) - Exit\n"
                );
            Console.Write("Choice: ");
            test = Convert.ToInt32(Console.ReadLine());
            switch(test) 
                {
                //Textes Parte 1.1
                case 1:
                    TesteRedeNeuronalXor();
                    break;
                case 2:
                    TesteRedeNeuronalRandomValues();
                    break;
                //Textes Parte 1.2
                case 3:
                    TesteReconhecimentoBarrasVerticais();
                    break;
                case 4:
                    BigTreinoRedeBarrasVerticais();
                    break;
                case 5:
                    TreinoXorMaior();
                    break;
                //Extras
                case 10:
                    showPesosPendores = !showPesosPendores;
                    Console.Write("showPesosPendores " + showPesosPendores);
                    break;
                case 11:
                    showArt = !showArt;
                    Console.Write("showArt " + showArt);
                    break;
                case 12: // Save Training Errors
                    Console.Write("FileName:");
                    trainingErrorsFileName = Console.ReadLine();
                    break;
                case 13: // Change Training Parameters
                    Console.Write("Taxa Aprendizagem ("+ taxaAprendizagem + "):"); //change training values
                    taxaAprendizagem = Convert.ToDouble(Console.ReadLine());

                    Console.Write("Epocas (" + epocas + "):");
                    epocas = Convert.ToInt32(Console.ReadLine());

                    Console.Write("erroMaximo (" + erroMaximo + "):");
                    erroMaximo = Convert.ToDouble(Console.ReadLine());
                    break;
                    //Exit
            }
        } 
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
        /*double taxaAprendizagem = 0.02;
        int epocas = 1000;
        double erroMaximo = 0.05;//0.001;*/
        var fucaoTanh = new FucaoAtivadoraTanh();

        double[][] entradasTreino = {
                new double[]{0, 0, 0, 0, 0, 0, 0, 0, 0},
                new double[]{1, 0, 0, 1, 0, 0, 1, 0, 0},
                new double[]{0, 1, 0, 0, 1, 0, 0, 1, 0},
                new double[]{0, 0, 1, 0, 0, 1, 0, 0, 1},
                new double[]{1, 1, 1, 0, 0, 0, 0, 0, 0},
                new double[]{0, 0, 0, 1, 1, 1, 0, 0, 0},
                new double[]{0, 0, 0, 0, 0, 0, 1, 1, 1}
        };
        double[][] saidasTreino =
        {
            new double[]{-1},
            new double[]{1},
            new double[]{1},
            new double[]{1},
            new double[]{-1},
            new double[]{-1},
            new double[]{-1}
        };

        while (true) {
            Console.WriteLine("Treino da rede Barras Verticais");
            RedeNeuronal rede = new RedeNeuronal(forma, fucaoTanh, trainingErrorsFileName); //só escreve no ficheiro se o filename for diferente de null
            if (showPesosPendores)
            {
                Console.WriteLine("Pesos Iniciais:");
                ShowPesosPendores(rede.GetPesos(), rede.GetPendores());
            }

            var foiUmSuceso = rede.Treinar(entradasTreino, saidasTreino, epocas, erroMaximo, taxaAprendizagem);
            Console.WriteLine("O treino foi um Suceso? " + (foiUmSuceso ? "sim" : "não"));
            if (foiUmSuceso) // se não foi um sucesso não vale a pena testar o treino
            {
                if (showPesosPendores)
                {
                    Console.WriteLine("Pesos finais:");
                    ShowPesosPendores(rede.GetPesos(), rede.GetPendores());
                }
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
                    0, 0, 0},
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
                    1, 0, 0,
                    1, 0, 0 },
                new double[]{
                    0, 1, 0,
                    0, 1, 0,
                    0, 1, 0 },
                new double[]{
                    0, 0, 1,
                    0, 0, 1,
                    0, 0, 1 }
        };
        string[] saidasEsperadas = { "66%", "66%", "66%", "-50%", "-50%", "-50", "100%", "100%" , "100%"};

        for(int t = 0; t < entradasTeste.Length; t++)
        {
            ShowQuadriculas(entradasTeste[t], 3);
            double[] result = rede.calcRedeNeuronal(entradasTeste[t]);
            Console.WriteLine("Esperado= " + saidasEsperadas[t] + "\nResult= " + ShowPercentagens(result[0]));
        }
    }

    public static void BigTreinoRedeBarrasVerticais()
    {
        //variaveis
        int[] forma = { 9, 3, 1 };
        /*double taxaAprendizagem = 0.2;
        int epocas = 1000;
        double erroMaximo = 0.05;*/
        var fucaoTanh = new FucaoAtivadoraTanh();

        //pega imagens aleatorias reais
        Random r = new Random();
        Console.Write("Numero De Imagens de Treino(1-511): ");
        var numeroDeImagens = Convert.ToInt32(Console.ReadLine());       //numero de imagens a testar
        if(numeroDeImagens == null || numeroDeImagens <= 0)
        {
            Console.WriteLine("impossible Value");
            return;
        }
        else if(numeroDeImagens > 511)
        {
            Console.WriteLine("Default to max (511)");
            numeroDeImagens = 511;
        }

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
            ShowQuadriculas(imagem,3); //mostra as imagens geradas
            Console.WriteLine("Imagem:" + imagensTreino[i]); // e o seu numero

            entradasDeTreino[i] = imagem;
            //if(imagensTreino[i] == 73 || imagensTreino[i] == 146 || imagensTreino[i] == 292) //nota isto é apenas porque temos um numero limitado de imagens que são linhas verticais
            if ((imagensTreino[i] & 73) == 73 || (imagensTreino[i] & 146) == 146 || (imagensTreino[i] & 292) == 292) //checa todas as linhas verticais mesmo as com ruido 
            {
                //Console.WriteLine("Tem Linha Verical");
                SaidasEsperada[i] = new double[]{ 1 } ;
            }
            else //saida não é uma linha vertical
            {
                SaidasEsperada[i] = new double[] { -1 };
            }

        }

        RedeNeuronal rede = new RedeNeuronal(forma, fucaoTanh,trainingErrorsFileName); //cria uma rede neuronal
        while (true)
        {
            Console.WriteLine("Treino da rede Barras Verticais (Big Treino)");
            var foiUmSuceso = rede.Treinar(entradasDeTreino, SaidasEsperada, epocas, erroMaximo, taxaAprendizagem);
            Console.WriteLine("O treino foi um Suceso? " + (foiUmSuceso ? "sim" : "não"));
            if (foiUmSuceso) // se não foi um sucesso não vale a pena testar o treino
            {
                break;
            }
            else //caso o treino for um insucesso criamos uma nova rede e tentamos denovo
            {
                rede = new RedeNeuronal(forma, fucaoTanh, trainingErrorsFileName);
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
                    0, 0, 0},
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
                    1, 0, 0,
                    1, 0, 0 },
                new double[]{
                    0, 1, 0,
                    0, 1, 0,
                    0, 1, 0 },
                new double[]{
                    0, 0, 1,
                    0, 0, 1,
                    0, 0, 1 }
        };

        for (int t = 0; t < entradasTeste.Length; t++)
        {
            ShowQuadriculas(entradasTeste[t], 3);
            double[] result = rede.calcRedeNeuronal(entradasTeste[t]);
            Console.WriteLine("Result= " + ShowPercentagens(result[0]));
        }

    }

    public static void TreinoXorMaior()
    {
        int[] forma = { 2, 5, 1 };
        /*double taxaAprendizagem = 0.01;
        int epocas = 10000;
        double erroMaximo = 0.005;*/
        var fucaoTanh = new FucaoAtivadoraTanh();
        double[][] entradasDeTreino = { new double[]{ 0, 0}, new double[] { 0, 1}, new double[] { 1, 0}, new double[] { 1, 1}};
        double[][] SaidasEsperada = { new double[] { 0 }, new double[] { 1 }, new double[] { 1 }, new double[] { 0 } };

        RedeNeuronal rede = new RedeNeuronal(forma, fucaoTanh,trainingErrorsFileName); //cria uma rede neuronal
        while (true)
        {
            Console.WriteLine("Treino do Big Xor");
            var foiUmSuceso = rede.Treinar(entradasDeTreino, SaidasEsperada, epocas, erroMaximo, taxaAprendizagem);
            Console.WriteLine("O treino foi um Suceso? " + (foiUmSuceso ? "sim" : "não"));
            if (foiUmSuceso) // se não foi um sucesso não vale a pena testar o treino
            {
                break;
            }
            else //caso o treino for um insucesso criamos uma nova rede e tentamos denovo
            {
                rede = new RedeNeuronal(forma, fucaoTanh, trainingErrorsFileName);
                Console.WriteLine("RIP: descartar Rede e criar outra\n");
            }
        }


        for (int t = 0; t < entradasDeTreino.Length; t++)
        {
            ShowQuadriculas(entradasDeTreino[t], 2);
            double[] result = rede.calcRedeNeuronal(entradasDeTreino[t]);
            Console.WriteLine("Result=" + ShowPercentagens(result[0]));
        }

    }

    //codigo apenas para visualizar as imagens que estamos a colocar na rede neuronal
    public static void ShowQuadriculas(double[] image, int largura)
    {
        if (!showArt)
            return;

        int l = 0;

        for (int i = 0; i < image.Length; i++)
        {
            Console.BackgroundColor = ConsoleColor.White; //reset quando troca de linha
            Console.ForegroundColor = ConsoleColor.Blue;
            if (image[i] == 1) //"█"
                Console.Write("██");
            else
                Console.Write("  ");
            //Console.Write(image[i]);
            l++;
            if (l == largura)
            {
                Console.ResetColor(); // isto é feito aqui para que não exista 1 linha embaixo que tem a cor errada
                Console.WriteLine("");
                l = 0;
            }
        }
        Console.ResetColor(); //reseta as cores
    }

    //apresenta na consola os pesos e pendores, (não é a forma mais limpa, mas ajuda a visualizar)
    //mais tarde tambem podera os guardar num ficheiro
    public static void ShowPesosPendores(double[][][] pesos, double[][] pendores)
    {
        Console.WriteLine("Pesos:\n[ ");
        for (int i = 0; i < pesos.Length; i++)
        {
            Console.WriteLine("\t[ ");
            for (int j = 0; j < pesos[i].Length; j++)
            {
                Console.Write("\t\t[ ");
                for (int k = 0; k < pesos[i][j].Length; k++)
                {
                    Console.Write(pesos[i][j][k] + "; ");
                }
                Console.Write("];\n");
            }
            Console.WriteLine("\t];");
        }
        Console.WriteLine("]");

        Console.WriteLine("Pendores:\n[ ");
        for (int i = 0; i < pesos.Length; i++)
        {
            Console.Write("\t[ ");
            for (int j = 0; j < pesos[i].Length; j++)
            {
                Console.Write(pendores[i][j] + "; ");
            }
            Console.WriteLine("];");
        }
        Console.WriteLine("]");
    }

    //transforma apenas numeros(double) em percentagens com as decimalCases predefenidas no codigo
    public static string ShowPercentagens(double d)
    {
        double n = d * 100;
        return n.ToString($"F{decimalCases}") + "%";
    }
}