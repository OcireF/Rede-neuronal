using Rede_neuronal;
using static System.Net.Mime.MediaTypeNames;

class Program
{
    static bool showPesosPendores = false;
    static bool showArt = true; // por default é true para mostrar os graficos
    static int decimalCases = 3; //isto é para a apresentação de percentagens na consola
    static string trainingErrorsFileName = "erro"; //ficheiro padrão
    //Variaveis dos treinos:
    static double taxaAprendizagem = 0.02; //velocidade do treino
    static double fatorMomento = 0.05; // fartor de movimento
    static int epocas = 1000; // Epocas para treinar
    static double erroMaximo = 0.05; //0,1%

    static Random r = new Random(); //gera um randomizer que podemos chamar para os testes

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
                "\n" +
                "Textes Parte 1.3\n" +
                "(5) - treinamento para o Xor [2,5,1]\n" +
                "(6) - 2 treinamentos com Rede Xor [2,5,1] mas uma com Beta e outra com beta=0\n" +
                "\n" +
                "Program\n" +
                "(7) - Completar a linha testes\n" +
                "(8) - jogar contra a AI (completadora de linhas)\n" +
                "(9) - treino o algoritmo minimax(bruteforce allgames)\n" +
                "(10) - Jogar contra a AI (treinada com o algoritmo minimax)\n" +
                "\n" +
                "(20) - Show Pesos e Pendores\n" +
                "(21) - Show Art Barras Verticais\n" +
                "(22) - Save Training Errors\n" +
                "(23) - Change Training Parameters\n" +
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
                    ShowParameters();
                    TesteReconhecimentoBarrasVerticais();
                    break;
                case 4:
                    ShowParameters();
                    BigTreinoRedeBarrasVerticais();
                    break;
                //Textes Parte 1.3
                case 5:
                    ShowParameters();
                    TreinoXorMaior();
                    break;
                case 6:
                    ShowParameters();
                    TreinoXorDuplaRede();
                    break;
                //testes para o jogo do galo
                case 7:
                    ShowParameters();
                    CompletarLinhas();
                    break;
                case 8:
                    JogoContraAI(TreinoCompletarLinhas()); //Jogar contra a AI(gera uma nova sempre que jogar denovo)
                    break;
                case 9:
                    testeMinimax();
                    break;
                case 10:
                    JogoContraAI(TreinoMiniMax());
                    break;
                //Extras
                case 20:
                    showPesosPendores = !showPesosPendores;
                    Console.WriteLine("showPesosPendores " + showPesosPendores);
                    break;
                case 21:
                    showArt = !showArt;
                    Console.WriteLine("showArt " + showArt);
                    break;
                case 22: // Save Training Errors
                    Console.Write("FileName:");
                    trainingErrorsFileName = Console.ReadLine();
                    break;
                case 23: // Change Training Parameters
                    Console.Write("Taxa Aprendizagem ("+ taxaAprendizagem + "):"); //change training values
                    taxaAprendizagem = Convert.ToDouble(Console.ReadLine());

                    Console.Write("Fator Momento (" + fatorMomento + "):");
                    fatorMomento = Convert.ToDouble(Console.ReadLine());

                    Console.Write("Epocas (" + epocas + "):");
                    epocas = Convert.ToInt32(Console.ReadLine());

                    Console.Write("Erro Maximo (" + erroMaximo + "):");
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
            var foiUmSuceso = rede.Treinar(entradasDeTreino, SaidasEsperada, epocas, erroMaximo, taxaAprendizagem, fatorMomento);
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

    public static void TreinoXorDuplaRede()
    {
        int[] forma = { 2, 5, 1 };
        var fucaoTanh = new FucaoAtivadoraTanh();
        double[][] entradasDeTreino = { new double[] { 0, 0 }, new double[] { 0, 1 }, new double[] { 1, 0 }, new double[] { 1, 1 } };
        double[][] SaidasEsperada = { new double[] { 0 }, new double[] { 1 }, new double[] { 1 }, new double[] { 0 } };

        RedeNeuronal redeBeta = new RedeNeuronal(forma, fucaoTanh, trainingErrorsFileName + "Beta"); //cria uma rede neuronal para gerar os pesos e pendores aleatorios
        RedeNeuronal redeNoBeta = new RedeNeuronal(forma, fucaoTanh, trainingErrorsFileName + "noBeta", redeBeta.GetPesos(),redeBeta.GetPendores()); //duplicada da rede com os mesmos pesos e pendores

        if (showPesosPendores)
        {
            Console.WriteLine("Pesos Iniciais Beta:");
            ShowPesosPendores(redeBeta.GetPesos(), redeBeta.GetPendores());
            Console.WriteLine("Pesos Iniciais Beta = 0:");
            ShowPesosPendores(redeNoBeta.GetPesos(), redeNoBeta.GetPendores());
        }

        var foiUmSuceso = false;
        while (!foiUmSuceso) //treina até for um sucesso
        {
            Console.WriteLine("Treino do Big Xor com beta");
            foiUmSuceso = redeBeta.Treinar(entradasDeTreino, SaidasEsperada, epocas, erroMaximo, taxaAprendizagem, fatorMomento);
            Console.WriteLine("O treino foi um Suceso? " + (foiUmSuceso ? "sim" : "não"));
        }

        foiUmSuceso = false;
        while (!foiUmSuceso) //treina até for um sucesso
        {
            Console.WriteLine("Treino do Big Xor com beta = 0");
            foiUmSuceso = redeNoBeta.Treinar(entradasDeTreino, SaidasEsperada, epocas, erroMaximo, taxaAprendizagem, 0);
            Console.WriteLine("O treino foi um Suceso? " + (foiUmSuceso ? "sim" : "não"));
        }

        for (int t = 0; t < entradasDeTreino.Length; t++)
        {
            ShowQuadriculas(entradasDeTreino[t], 2);
            double[] resultBeta = redeBeta.calcRedeNeuronal(entradasDeTreino[t]);
            double[] resultNoBeta = redeNoBeta.calcRedeNeuronal(entradasDeTreino[t]);

            Console.WriteLine("Beta Result=" + ShowPercentagens(resultBeta[0]));
            Console.WriteLine("NoBeta Result=" + ShowPercentagens(resultNoBeta[0]));
        }

    }

    public static RedeNeuronal TreinoCompletarLinhas()
    {
        //Forma 9 entradas (9 casas do jogo do galo), 24 combinações de jogadas vencedoras, 9 saídas (uma para cada casa do jogo do galo)
        int[] forma = { 9, 24, 9 };
        var fucaoTanh = new FucaoAtivadoraTanh();
        RedeNeuronal rede = new RedeNeuronal(forma, fucaoTanh, trainingErrorsFileName); //cria uma rede neuronal

        string[][] boards = {
            // Horizontal, linha de cima (casas 0,1,2)
            new string[]{ " ","X","X",   " "," "," ",   " "," "," " }, // -> 0
            new string[]{ "X"," ","X",   " "," "," ",   " "," "," " }, // -> 1
            new string[]{ "X","X"," ",   " "," "," ",   " "," "," " }, // -> 2
            // Horizontal, linha do meio (casas 3,4,5)
            new string[]{ " "," "," ",   " ","X","X",   " "," "," " }, // -> 3
            new string[]{ " "," "," ",   "X"," ","X",   " "," "," " }, // -> 4
            new string[]{ " "," "," ",   "X","X"," ",   " "," "," " }, // -> 5
            // Horizontal, linha de baixo (casas 6,7,8)
            new string[]{ " "," "," ",   " "," "," ",   " ","X","X" }, // -> 6
            new string[]{ " "," "," ",   " "," "," ",   "X"," ","X" }, // -> 7
            new string[]{ " "," "," ",   " "," "," ",   "X","X"," " }, // -> 8

            // Vertical, coluna da esquerda (casas 0,3,6)
            new string[]{ " "," "," ",   "X"," "," ",   "X"," "," " }, // -> 0
            new string[]{ "X"," "," ",   " "," "," ",   "X"," "," " }, // -> 3
            new string[]{ "X"," "," ",   "X"," "," ",   " "," "," " }, // -> 6
            // Vertical, coluna do meio (casas 1,4,7)
            new string[]{ " "," "," ",   " ","X"," ",   " ","X"," " }, // -> 1
            new string[]{ " ","X"," ",   " "," "," ",   " ","X"," " }, // -> 4
            new string[]{ " ","X"," ",   " ","X"," ",   " "," "," " }, // -> 7
            // Vertical, coluna da direita (casas 2,5,8)
            new string[]{ " "," "," ",   " "," ","X",   " "," ","X" }, // -> 2
            new string[]{ " "," ","X",   " "," "," ",   " "," ","X" }, // -> 5
            new string[]{ " "," ","X",   " "," ","X",   " "," "," " }, // -> 8

            // Diagonal \ (casas 0,4,8)
            new string[]{ " "," "," ",   " ","X"," ",   " "," ","X" }, // -> 0
            new string[]{ "X"," "," ",   " "," "," ",   " "," ","X" }, // -> 4
            new string[]{ "X"," "," ",   " ","X"," ",   " "," "," " }, // -> 8
            // Diagonal / (casas 2,4,6)
            new string[]{ " "," "," ",   " ","X"," ",   "X"," "," " }, // -> 2
            new string[]{ " "," ","X",   " "," "," ",   "X"," "," " }, // -> 4
            new string[]{ " "," ","X",   " ","X"," ",   " "," "," " }, // -> 6
        };
        string[][] saidasEsperadas = {
            // Horizontal, linha de cima
            new string[]{ "X"," "," ",   " "," "," ",   " "," "," " }, // 0
            new string[]{ " ","X"," ",   " "," "," ",   " "," "," " }, // 1
            new string[]{ " "," ","X",   " "," "," ",   " "," "," " }, // 2
            // Horizontal, linha do meio
            new string[]{ " "," "," ",   "X"," "," ",   " "," "," " }, // 3
            new string[]{ " "," "," ",   " ","X"," ",   " "," "," " }, // 4
            new string[]{ " "," "," ",   " "," ","X",   " "," "," " }, // 5
            // Horizontal, linha de baixo
            new string[]{ " "," "," ",   " "," "," ",   "X"," "," " }, // 6
            new string[]{ " "," "," ",   " "," "," ",   " ","X"," " }, // 7
            new string[]{ " "," "," ",   " "," "," ",   " "," ","X" }, // 8
            // Vertical, coluna da esquerda
            new string[]{ "X"," "," ",   " "," "," ",   " "," "," " }, // 0
            new string[]{ " "," "," ",   "X"," "," ",   " "," "," " }, // 3
            new string[]{ " "," "," ",   " "," "," ",   "X"," "," " }, // 6
            // Vertical, coluna do meio
            new string[]{ " ","X"," ",   " "," "," ",   " "," "," " }, // 1
            new string[]{ " "," "," ",   " ","X"," ",   " "," "," " }, // 4
            new string[]{ " "," "," ",   " "," "," ",   " ","X"," " }, // 7
            // Vertical, coluna da direita
            new string[]{ " "," ","X",   " "," "," ",   " "," "," " }, // 2
            new string[]{ " "," "," ",   " "," ","X",   " "," "," " }, // 5
            new string[]{ " "," "," ",   " "," "," ",   " "," ","X" }, // 8
            // Diagonal \
            new string[]{ "X"," "," ",   " "," "," ",   " "," "," " }, // 0
            new string[]{ " "," "," ",   " ","X"," ",   " "," "," " }, // 4
            new string[]{ " "," "," ",   " "," "," ",   " "," ","X" }, // 8
            // Diagonal /
            new string[]{ " "," ","X",   " "," "," ",   " "," "," " }, // 2
            new string[]{ " "," "," ",   " ","X"," ",   " "," "," " }, // 4
            new string[]{ " "," "," ",   " "," "," ",   "X"," "," " }, // 6
        };

        double[][] trainingBoards = tranlateBoard(boards); //traduz os simbolos para numeros que a rede pode usar
        double[][] trainingSaidasEsperadas = tranlateBoard(saidasEsperadas); //traduz os simbolos para numeros que a rede pode usar

        //Treino
        var foiUmSuceso = false;
        while (true) //treina até for um sucesso (simples loop)
        {
            Console.WriteLine("Treino Linhas Vencedoras");
            foiUmSuceso = rede.Treinar(trainingBoards, trainingSaidasEsperadas, epocas, erroMaximo, taxaAprendizagem, fatorMomento);
            Console.WriteLine("O treino foi um Suceso? " + (foiUmSuceso ? "sim" : "não"));

            if (foiUmSuceso)
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

    public static RedeNeuronal TreinoMiniMax()
    {
        int[] forma = { 9, 64, 32, 16, 9 };
        var fucaoTanh = new FucaoAtivadoraTanh();
        RedeNeuronal rede = new RedeNeuronal(forma, fucaoTanh, trainingErrorsFileName,null,null,new FucaoAtivadoraLiniar()); //cria uma rede neuronal

        double[] board = new double[9];
        var dicionario = new Dictionary<string, (double[] x, double[] y)>(); //gera para todos os boards(x) as melhores escolhas (y) 
        GerarJogoDoGalo(board, 1, dicionario);

        (double[] x, double[] y)[] trainingdata = dicionario.Values.ToArray();
        r.Shuffle(trainingdata); //suffle the training data
        Console.Write("Numero de tabuleiros para treinar (Max 4520)=");
        var boardNumber = Convert.ToInt32(Console.ReadLine());

        double[][] trainingBoards = trainingdata.Select(v => v.x).Take(boardNumber).ToArray();
        double[][] trainingSaidasEsperadas = trainingdata.Select(v => v.y).Take(boardNumber).ToArray();

        //Treino
        var foiUmSuceso = false;
        while (true) //treina até for um sucesso (simples loop)
        {
            Console.WriteLine("Treino AI minimax");
            foiUmSuceso = rede.Treinar(trainingBoards, trainingSaidasEsperadas, epocas, 0.3, 0.01, 0);
            Console.WriteLine("O treino foi um Suceso? " + (foiUmSuceso ? "sim" : "não"));

            if (foiUmSuceso)
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
                return rede; //temp para dar trobleShot
            }
        }

        return rede;
    }

    //Obj: implementar o jogo do galo com a rede neuronal
    //AI é o "X", o jogador é o "O"
    //Objetivo inicial é uma rede que saiba a jogada vencedora
    public static void CompletarLinhas()
    {
        RedeNeuronal rede = TreinoCompletarLinhas(); //Pre Treina a rede para os testes

        //boards de testes usados originalmente usados no teino
        string[][] boards = {
            // Horizontal, linha de cima (casas 1,2,3)
            new string[]{ " ","X","X",   " "," "," ",   " "," "," " }, // -> 1
            new string[]{ "X"," ","X",   " "," "," ",   " "," "," " }, // -> 2
            new string[]{ "X","X"," ",   " "," "," ",   " "," "," " }, // -> 3
            // Horizontal, linha do meio (casas 4,5,6)
            new string[]{ " "," "," ",   " ","X","X",   " "," "," " }, // -> 4
            new string[]{ " "," "," ",   "X"," ","X",   " "," "," " }, // -> 5
            new string[]{ " "," "," ",   "X","X"," ",   " "," "," " }, // -> 6
            // Horizontal, linha de baixo (casas 7,8,9)
            new string[]{ " "," "," ",   " "," "," ",   " ","X","X" }, // -> 7
            new string[]{ " "," "," ",   " "," "," ",   "X"," ","X" }, // -> 8
            new string[]{ " "," "," ",   " "," "," ",   "X","X"," " }, // -> 9

            // Vertical, coluna da esquerda (casas 1,4,7)
            new string[]{ " "," "," ",   "X"," "," ",   "X"," "," " }, // -> 1
            new string[]{ "X"," "," ",   " "," "," ",   "X"," "," " }, // -> 4
            new string[]{ "X"," "," ",   "X"," "," ",   " "," "," " }, // -> 7
            // Vertical, coluna do meio (casas 2,5,8)
            new string[]{ " "," "," ",   " ","X"," ",   " ","X"," " }, // -> 2
            new string[]{ " ","X"," ",   " "," "," ",   " ","X"," " }, // -> 5
            new string[]{ " ","X"," ",   " ","X"," ",   " "," "," " }, // -> 8
            // Vertical, coluna da direita (casas 3,6,9)
            new string[]{ " "," "," ",   " "," ","X",   " "," ","X" }, // -> 3
            new string[]{ " "," ","X",   " "," "," ",   " "," ","X" }, // -> 6
            new string[]{ " "," ","X",   " "," ","X",   " "," "," " }, // -> 9

            // Diagonal \ (casas 1,5,9)
            new string[]{ " "," "," ",   " ","X"," ",   " "," ","X" }, // -> 1
            new string[]{ "X"," "," ",   " "," "," ",   " "," ","X" }, // -> 5
            new string[]{ "X"," "," ",   " ","X"," ",   " "," "," " }, // -> 9
            // Diagonal / (casas 3,5,7)
            new string[]{ " "," "," ",   " ","X"," ",   "X"," "," " }, // -> 3
            new string[]{ " "," ","X",   " "," "," ",   "X"," "," " }, // -> 5
            new string[]{ " "," ","X",   " ","X"," ",   " "," "," " }, // -> 7
        };
        string[][] saidasEsperadas = {
            // Horizontal, linha de cima
            new string[]{ "X"," "," ",   " "," "," ",   " "," "," " }, // 1
            new string[]{ " ","X"," ",   " "," "," ",   " "," "," " }, // 2
            new string[]{ " "," ","X",   " "," "," ",   " "," "," " }, // 3
            // Horizontal, linha do meio
            new string[]{ " "," "," ",   "X"," "," ",   " "," "," " }, // 4
            new string[]{ " "," "," ",   " ","X"," ",   " "," "," " }, // 5
            new string[]{ " "," "," ",   " "," ","X",   " "," "," " }, // 6
            // Horizontal, linha de baixo
            new string[]{ " "," "," ",   " "," "," ",   "X"," "," " }, // 7
            new string[]{ " "," "," ",   " "," "," ",   " ","X"," " }, // 8
            new string[]{ " "," "," ",   " "," "," ",   " "," ","X" }, // 9
            // Vertical, coluna da esquerda
            new string[]{ "X"," "," ",   " "," "," ",   " "," "," " }, // 1
            new string[]{ " "," "," ",   "X"," "," ",   " "," "," " }, // 4
            new string[]{ " "," "," ",   " "," "," ",   "X"," "," " }, // 7
            // Vertical, coluna do meio
            new string[]{ " ","X"," ",   " "," "," ",   " "," "," " }, // 2
            new string[]{ " "," "," ",   " ","X"," ",   " "," "," " }, // 5
            new string[]{ " "," "," ",   " "," "," ",   " ","X"," " }, // 8
            // Vertical, coluna da direita
            new string[]{ " "," ","X",   " "," "," ",   " "," "," " }, // 3
            new string[]{ " "," "," ",   " "," ","X",   " "," "," " }, // 6
            new string[]{ " "," "," ",   " "," "," ",   " "," ","X" }, // 9
            // Diagonal \
            new string[]{ "X"," "," ",   " "," "," ",   " "," "," " }, // 1
            new string[]{ " "," "," ",   " ","X"," ",   " "," "," " }, // 5
            new string[]{ " "," "," ",   " "," "," ",   " "," ","X" }, // 9
            // Diagonal /
            new string[]{ " "," ","X",   " "," "," ",   " "," "," " }, // 3
            new string[]{ " "," "," ",   " ","X"," ",   " "," "," " }, // 5
            new string[]{ " "," "," ",   " "," "," ",   "X"," "," " }, // 7
        };

        double[][] trainingBoards = tranlateBoard(boards); //traduz os simbolos para numeros que a rede pode usar
        double[][] trainingSaidasEsperadas = tranlateBoard(saidasEsperadas); //traduz os simbolos para numeros que a rede pode usar

        //testes de completar as linhas
        for (int t = 0; t < trainingBoards.Length; t += r.Next(1,5)) //randomicamente salta alguns testos
        {
            //calcular a saida
            double[] escolhaAI = rede.calcRedeNeuronal(trainingBoards[t]);

            /*double[] newBoard = result.Select(r => r == result.Max() ? 2 : 0) //flaten the exits to 0 or 2 (Ai move) apenas o resultado maior da AI
                .Zip(trainingBoards[t], (r, b) => r + b).ToArray(); //sum the 2 boards*/

            double[] newBoard = PlayBoard(trainingBoards[t], escolhaAI);

            Console.WriteLine("Tabuleiro Resultado:");
            ShowQuadriculas(newBoard, 3);

            Console.WriteLine("---------------");
        }

        //random ponto no centro, ver o que faz com isto
        Console.WriteLine("Teste de ponto no centro do tabuleiro:"); // apenas para testar o que a AI tenta fazer quando não tem linha para completar
        double[] boardCentro = new double[9];
        boardCentro[4] = 1;
        double[] resultCentro = rede.calcRedeNeuronal(boardCentro);
        double[] newBoardCentro = PlayBoard(boardCentro, resultCentro);
        ShowQuadriculas(newBoardCentro, 3);
        Console.WriteLine("---------------");

        //Testes de multiplas linhas
        Console.WriteLine("Teste de Cruz no centro (+)");
        double[] boardCrux = new double[9];
        boardCrux[1] = 1; boardCrux[3] = 1; boardCrux[5] = 1; boardCrux[7] = 1; // +
        double[] resultCrux = rede.calcRedeNeuronal(boardCrux);
        double[] newBoardCrux = PlayBoard(boardCrux, resultCrux);
        ShowQuadriculas(newBoardCrux, 3);
        Console.WriteLine("---------------");

        Console.WriteLine("Teste de Cruz no centro (x)");
        double[] boardCrux2 = new double[9];
        boardCrux2[0] = 1; boardCrux2[2] = 1; boardCrux2[6] = 1; boardCrux2[8] = 1; // x
        double[] resultCrux2 = rede.calcRedeNeuronal(boardCrux2);
        double[] newBoardCrux2 = PlayBoard(boardCrux2, resultCrux2);
        ShowQuadriculas(newBoardCrux2, 3);
        Console.WriteLine("---------------");

        //teste para ver a AI a completar um tabuleiro
        Console.WriteLine("Teste try play until full");
        double[] zeroBoard = new double[9];
        for(int i = 0; i<zeroBoard.Length; i++)
        {
            var AIplay = rede.calcRedeNeuronal(zeroBoard);
            zeroBoard = PlayBoard(zeroBoard, AIplay);
            Console.WriteLine("Play = " + i);
            ShowQuadriculas(zeroBoard, 3);
            Console.WriteLine("---------------");
        }
    }

    public static void testeMinimax()
    {
        var rede = TreinoMiniMax();

        double[] board = new double[9];
        var dicionario = new Dictionary<string, (double[] x, double[] y)>(); //gera para todos os boards(x) as melhores escolhas (y) 
        GerarJogoDoGalo(board, 1, dicionario);
        (double[] x, double[] y)[] boardData = dicionario.Values.ToArray();

        r.Shuffle(boardData); //suffle the training data
        Console.Write("Numero de tabuleiros para testar (Max 4520)=");
        var boardNumber = Convert.ToInt32(Console.ReadLine());

        double[][] boardsTestes = boardData.Select(v => v.x).Take(boardNumber).ToArray();
        double[][] pontosTestes = boardData.Select(v => v.y).Take(boardNumber).ToArray();

        Console.WriteLine("Teste try play until full");
        double soma = 0;
        for (int i = 0; i < boardsTestes.Length; i++)
        {
            Console.WriteLine("Original board");
            ShowQuadriculas(boardsTestes[i], 3);
            var AIplay = rede.calcRedeNeuronal(boardsTestes[i]);
            var boardResult = PlayBoard(boardsTestes[i], AIplay);
            var points = pontosTestes[i][Array.IndexOf(boardResult, boardResult.Max())]; //Captura os pontos da maior escolha da AI
            Console.WriteLine("Pontos:" + points);
            ShowQuadriculas(boardResult, 3);
            Console.WriteLine("---------------");

            soma += points;
        }
        double mediaPoints = soma / boardsTestes.Length;
        Console.WriteLine("Media de pontos:" + mediaPoints);


    }

    public static void JogoContraAI(RedeNeuronal rede)
    {
        var player = -1;
        var ai = 1;
        Console.WriteLine("Player buttons:\n" +
            "1 2 3\n" +
            "4 5 6\n" +
            "7 8 9");
        //var rede = TreinoCompletarLinhas();
        double[] board = new double[9];

        ShowQuadriculas(board, 3); //mostra a partida base

        for (int jogadas = 9; jogadas > 0; jogadas--) {
            Console.Write("Play = ");
            var jogada = Convert.ToInt32(Console.ReadLine());
            if (jogada < 1 || jogada > 9)
            {
                Console.WriteLine("Jogada Invalida");
                continue;
            }
            var indexJogada = jogada - 1;

            if (board[indexJogada] == 0)
            {
                board[indexJogada] = -2; //nova jogada do Jogador
            }
            else
            {
                Console.WriteLine("Jogada Invalida");
                continue;
            }

            ShowQuadriculas(board, 3); //mostra a partida
            if(CheckVictory(board, player))
            {
                Console.WriteLine("Player Wins");
                return;
            }

            Console.WriteLine("Ai play = ");
            //jogada da AI
            var AIplay = rede.calcRedeNeuronal(board.Select(b => (b >= 1 ? 1.0 : (b <= -1 ? -1.0 : 0.0))).ToArray());
            board = PlayBoard(board,AIplay);

            ShowQuadriculas(board, 3);
            if (CheckVictory(board, ai))
            {
                Console.WriteLine("AI Wins");
                return;
            }
        }
        Console.WriteLine("Draw");
        return;
    }

    //Recebe o board atual, e as escolhas da AI de onde jogar, e tenta fazer esta jogada
    public static double[] PlayBoard(double[] board, double[] AIplay)
    {
        var indexZero = Array.IndexOf(board, 0); //verifica se tem algum lugar que é possivel de escrita
        if (indexZero == -1) //caso não exista 
            return board;

        double[] flattenBoard = board.Select(b => (b >= 1 ? 1.0 : (b <= -1 ? -1.0 : 0.0))).ToArray(); //flatten the old values to (-1 ou 0 ou 1)

        while (true) {
            var index = Array.IndexOf(AIplay, AIplay.Max()); //captura a jogada da AI (aka a que ela deu maior valor)
            if(flattenBoard[index] == 0)
            {
                flattenBoard[index] = 2; //2 ultima jogada da Ai
                break; //sai do loop
            }
            else
            {
                AIplay[index] = int.MinValue; //ilimina essa jogada não possivel da AI e tenta ver qual a proxima melhor
            }
        }
        return flattenBoard;
    }
    public static bool CheckVictory(double[] board, double jogador)
    {
        double[] flattenBoard = board.Select(b => (b >= 1 ? 1.0 : (b <= -1 ? -1.0 : 0.0))).ToArray(); //flatten the old values to (-1 ou 0 ou 1)

        //check horizontal
        for(int i = 0; i < flattenBoard.Length; i += 3)
        {
            if (flattenBoard[i] == jogador && flattenBoard[i + 1] == jogador && flattenBoard[i + 2] == jogador)
            {
                return true;
            }
        }

        //check Vertical
        for (int i = 0; i < 3; i++)
        {
            if (flattenBoard[i] == jogador && flattenBoard[i + 3] == jogador && flattenBoard[i + 6] == jogador)
            {
                return true;
            }
        }

        //check diagonais
        if (flattenBoard[0] == jogador && flattenBoard[4] == jogador && flattenBoard[8] == jogador)
        {
            return true;
        }
        if (flattenBoard[2] == jogador && flattenBoard[4] == jogador && flattenBoard[6] == jogador)
        {
            return true;
        }

        return false;
    }

    // Algoritmo Minimax para dar brute force da melhor jogada possivel
    // Valor da posição para quem joga (player): +1 ganha, 0 empata, -1 perde
    static int Minimax(double[] board, int player)
    {
        if (CheckVictory(board, -player)) return -1;              // o adversário acabou de ganhar "-1"
        if (!board.Contains(0)) return 0;                         // se não existir um 0 é um empate "0"
        int melhor = -2;                                          // valor inicial da jogada "-2" para ser pior que todas as jogadas possiveis (-1,0,1)
        for (int i = 0; i < 9; i++)                               // basicamente vamos exprimentar todas as jogads possiveis
        {
            if (board[i] != 0) continue;                            // caso esta casa não estiver vasia ignora
            board[i] = player;                                      // Exprimenta jogar aqui
            melhor = Math.Max(melhor, -Minimax(board, -player));    // Agora calcula se esta jogada faz o adversario perder "-1" ou empatar "0", 
            board[i] = 0;
        }
        return melhor;
    }

    //gerador de jogos do Galo
    static void GerarJogoDoGalo(double[] board, int player, Dictionary<string, (double[] x, double[] y)> dados)
    {
        if (CheckVictory(board, player) || CheckVictory(board, -player) || !board.Contains(0)) return; //caso alguem tenha ganho return

        var x = board.Select(c => (c * player)).ToArray();   // ponto de vista de quem joga (player "-1" flipa o jogo)(player 1 deixa o jogo igual)
        string chave = string.Join(",", x);                  // gerar a chave do jogo atual Exemplo: "X,0,0, ,X,0, , , " para ivitar repetições

        if (!dados.ContainsKey(chave)) //se o Dicionario ja tiver a chave não existe necessidade de calcular os dados
        {
            var pontos = new int[9]; //Inicializa um array de pontos a [0,0,0 ,0,0,0 ,0,0,0]
            int max = -2;           //max ser -2 para mais tarde ser substituido por uma pontuação real
            for (int i = 0; i < 9; i++) //verifica todas as jogads possiveis do board atual
            {
                if (board[i] != 0) { pontos[i] = -3; continue; } //caso a casa já estiver ocupada a pontuação é -3 para essa casa
                board[i] = player;                               //tenta jogar nesta casa
                pontos[i] = -Minimax(board, -player);            //calcula o contrario do minimax do adversario no board atual (se ele pontuação negativa sgnifica que nesta casa podemos ganhar)
                board[i] = 0;                                    //reseta a casa para usar no proximo loop
                max = Math.Max(max, pontos[i]);                  //guardados o valor maximo
            }
            var y = pontos.Select(v => v == max ? 0.8 : -0.8).ToArray(); //normaliza o array pontos exemplo "[ -3, -3, -3, -3, -3, -3,  -1,   0,  +1 ]" -> "[ -1, -1, -1, -1, -1, -1,  -1,  -1,  +1 ]"
            dados[chave] = (x, y); //adiciona no dicionario o par board atual(x), e melhor jogada(y)
        }

        //aqui vamos fazer recursividade,
        //tenta jogar numa posição,
        //e verifica as proximas jogadas no ramo (GerarJogoDoGalo),
        //depois renicia a posição da jogada para poder verificar outro ramo no proximo loop
        for (int i = 0; i < 9; i++)
        {
            if (board[i] != 0) continue;
            board[i] = player;
            GerarJogoDoGalo(board, -player, dados);
            board[i] = 0;
        }
    }

    //função para traduzir board strings para numeros que a rede neuronal pode ler
    //"O" = "-1" Nada = "0" "X" = "1"
    public static double[][] tranlateBoard(String[][] board)
    {
        double[][] translate = new double[board.Length][];
        for (int boa = 0; boa < board.Length; boa++) {
            translate[boa] = new double[board[boa].Length]; //inicialize the board
            for (int i = 0; i < board[boa].Length; i++)
            {
                if (board[boa][i] == "O")
                {
                    translate[boa][i] = -1;
                }
                else if (board[boa][i] == " ")
                {
                    translate[boa][i] = 0;
                }
                else if (board[boa][i] == "X")
                {
                    translate[boa][i] = 1;
                }
            }
        }
        return translate;
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

            switch (image[i])
            {
                case -1:
                    Console.ForegroundColor = ConsoleColor.DarkBlue; //old value AI
                    break;
                case -2:
                    Console.ForegroundColor = ConsoleColor.Blue; //new value AI
                    break;
                case 1:
                    Console.ForegroundColor = ConsoleColor.DarkRed; //old value player
                    break;
                case 2:
                    Console.ForegroundColor = ConsoleColor.Red; //new value player
                    break;
            }

            if (image[i] >= 1 || image[i] <= -1) //"█"
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
    //codigo para dar print dos parametros de treino atuais
    public static void ShowParameters()
    {
        Console.WriteLine(
            "Taxa Aprendizagem (" + taxaAprendizagem + ")\n" +
            "Fator Momento (" + fatorMomento + ")\n" +
            "Epocas (" + epocas + ")\n" +
            "erroMaximo (" + erroMaximo + ")");
    }

}