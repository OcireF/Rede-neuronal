using RaciocinioAutomatico;
using Raylib_cs;
using System.Numerics;
using System.Runtime.ConstrainedExecution;
class Program
{
    static float[] timer = { 0.001f, 0.05f, 0.1f, 0.5f }; //timer da velociade de atualização do player
    static int timerIndex = 1; //default speed "0.05f"
    static float acumulador = 0; //acumulador para contar o tempo passado
    static int frameSkiper = 1;

    //window stuff
    static int blockSize = 16;
    static int zoom = 1;
    //texturas
    static Texture2D tileset;// = Raylib.LoadTexture("Assets/Floor_TileSet.png");
    static Texture2D npcs;// = Raylib.LoadTexture("Assets/NPC.png");


    static void Main(string[] args)
    {
        int test = 0;
        //while (test != 99)
        {
            test = 0;

            Console.WriteLine(
                "Para os montes:\n" +
                "Vermelho = hillClimbler normal\n" +
                "Azul = hillClimbler estocastico\n" +
                "Roxo = hillClimbler estocastico com um unico sucessor\n" +
                "Verde = hillClimbler com reinício aleatório\n" +
                "Star = Simulated Annealing\n" +

                "\nTextes Parte 3.1\n" +
                "(1) - Big hill  ( 25x25 ) hillClimblers + Simulated Annealing\n" +
                "(2) - Maximos Locais ( 50x50 ) hillClimblers + Simulated Annealing\n" +
                "(3) - Damas ( 8x8 ) hillClimblerNormal\n" +

                "\nTextes Parte 3.2\n" +
                "(4) - zonas Planas (150x50) hillClimblers + Simulated Annealing\n" +

                "\nBotões:\n" +
                "N - recomeçar os algoritmos\n"
                );
            Console.Write("Choice: ");
            test = Convert.ToInt32(Console.ReadLine());
            SetupWindow();
            switch (test)
            {
                case 1:
                    smallHill((25, 25));
                    break;
                case 2:
                    maximosLocais((50, 50));
                    break;
                case 3:
                    DamasConflitos((8,8));
                    break;
                case 4:
                    zonasPlanas((100, 50));
                    break;

            }
        }
    }

    static void SetupWindow()
    {
        Raylib.SetConfigFlags(ConfigFlags.HiddenWindow); //inddenWindow, enquanto faz setup do zoom
        Raylib.InitWindow(800, 600, "App"); // tamanho temporario
        Raylib.SetTargetFPS(60);

        //init tileset
        tileset = Raylib.LoadTexture(AppContext.BaseDirectory + "Assets/Floor_TileSet.png");
        npcs = Raylib.LoadTexture(AppContext.BaseDirectory + "Assets/NPC.png");
        Raylib.SetTextureFilter(tileset, TextureFilter.Point); // para a pixel art nao ficar borrada
    }
    static void ResizeWindow((int x, int y) size)
    {
        //set Max Zoom
        int monitor = Raylib.GetCurrentMonitor();
        int maxW = (int)(Raylib.GetMonitorWidth(monitor) * 0.9); //margem para as bordas laterais
        int maxH = (int)(Raylib.GetMonitorHeight(monitor) * 0.85); //margem para o titulo e borda 

        zoom = Math.Max(1, Math.Min( //captura o zoom maximo
            maxW / (size.x * blockSize), //max zoom vertical
            maxH / (size.y * blockSize))); //max zoom horizontal

        //resize para o maior zoom
        int monitorW = size.x * blockSize * zoom;
        int monitorH = size.y * blockSize * zoom;
        Raylib.SetWindowSize(monitorW, monitorH);

        //centrar a janela do labirinto
        Raylib.SetWindowPosition(
            (Raylib.GetMonitorWidth(monitor) - monitorW) / 2,
            (Raylib.GetMonitorHeight(monitor) - monitorH) / 2);

        Raylib.ClearWindowState(ConfigFlags.HiddenWindow);
    }

    static void smallHill((int x , int y) size)
    {
        Montes monte = new Montes(size.x, size.y);
        
        List<double[]> objs = new List<double[]>(); 
        objs.Add(new double[] { size.x/2, size.y/2}); //midle

        //setup montes usando frente de onda
        FrenteDeOnda frenteDeOnda = new FrenteDeOnda(255, 0.9);
        monte.FrenteOndaSetup(frenteDeOnda, objs);

        /*HillClimbing hillClimmer = new HillClimbing(monte);
        var algoritmos = new HillClimbing[1] { hillClimmer };*/
        IAlgoritmoHill[] algoritmos = GetHillClibers(monte,255);

        DrawHills(size, monte, algoritmos);
    }

    static void maximosLocais((int x, int y) size)
    {
        Montes monte = new Montes(size.x, size.y);

        //Maximo global
        {
            List<double[]> objs = new List<double[]>();
            objs.Add(new double[] { size.x / 4, size.y / 4 }); //esquerda topo

            //setup montes usando frente de onda
            FrenteDeOnda frenteDeOnda = new FrenteDeOnda(255, 0.9); //Maximo global
            monte.FrenteOndaSetup(frenteDeOnda, objs);
        }
        //Maximos Locais
        {
            List<double[]> objs = new List<double[]>();
            objs.Add(new double[] { size.x * 3 / 4, size.y     / 4 }); //direita topo
            objs.Add(new double[] { size.x     / 4, size.y * 3 / 4 }); //esquerda baixo
            objs.Add(new double[] { size.x * 3 / 4, size.y * 3 / 4 }); //direita baixo

            FrenteDeOnda frenteDeOnda = new FrenteDeOnda(150, 0.9); //init maximos locais
            monte.FrenteOndaSetup(frenteDeOnda, objs);
        }

        /*HillClimbing hillClimmer = new HillClimbing(monte);
        var algoritmos = new HillClimbing[1] { hillClimmer };*/
        IAlgoritmoHill[] algoritmos = GetHillClibers(monte,255);

        DrawHills(size, monte, algoritmos); //desenhar a simulação
    }

    static void zonasPlanas((int x, int y) size)
    {
        Montes monte = new Montes(size.x, size.y);


        { //maximo global
            List<double[]> objs = new List<double[]>();
            objs.Add(new double[] { size.x * 3 / 4, size.y * 3 / 4 }); //esquerda topo

            //setup montes usando frente de onda
            FrenteDeOnda frenteDeOnda = new FrenteDeOnda(255, 0.8, 10); //Maximo global
            monte.FrenteOndaSetup(frenteDeOnda, objs);
        }

        { //maximo local 1
            List<double[]> objs = new List<double[]>();
            objs.Add(new double[] { size.x / 2, size.y / 4 });

            //setup montes usando frente de onda
            FrenteDeOnda frenteDeOnda = new FrenteDeOnda(120, 0.8, 10); //Maximo local
            monte.FrenteOndaSetup(frenteDeOnda, objs);
        }

        { //maximo local 2
            List<double[]> objs = new List<double[]>();
            objs.Add(new double[] { size.x / 4, size.y * 3 / 4 });

            //setup montes usando frente de onda
            FrenteDeOnda frenteDeOnda = new FrenteDeOnda(80, 0.8, 10); //Maximo local
            monte.FrenteOndaSetup(frenteDeOnda, objs);
        }

        timerIndex = 0;     //max speed
        frameSkiper = 5;    //skip 5 frames para acelarar o visual

        IAlgoritmoHill[] algoritmos = GetHillClibers(monte, 255);

        DrawHills(size, monte, algoritmos); //desenhar a simulação
    }

    static void DamasConflitos((int x, int y) size)
    {
        Damas damas = new Damas(size.x,size.y);
        int objetivo = 0; //numero de conflitos que queremos atingir
        int numeroReinicios = 30; //numero de reinicios aleatorios que queremos fazer
        IAlgoritmoHill hillclimbmer = new HillClimbingReinicioAleatorio(objetivo, numeroReinicios, new HillClimbing(damas), damas);

        DrawDamas(size, damas, hillclimbmer); //desenhar a simulação
    }

    static IAlgoritmoHill[] GetHillClibers(IProblema problema, double valorAlvo)
    {
        HillClimbing hillClimmer1 = new HillClimbing(problema);
        HillClimbingEstocastico hillClimmer2 = new HillClimbingEstocastico(problema);
        HillClimbingEstocasticoUnicoSucessor hillClimmer3 = new HillClimbingEstocasticoUnicoSucessor(problema);
        HillClimbingReinicioAleatorio hillClimmer4 = new HillClimbingReinicioAleatorio(valorAlvo, 20, new HillClimbing(problema), problema);
        SimulatedAnnealing annealing = new SimulatedAnnealing(problema, valorAlvo, 100, (t) => t * 0.999);
        var hillClimmers = new IAlgoritmoHill[] { hillClimmer1, hillClimmer2, hillClimmer3 , hillClimmer4 , annealing };

        return hillClimmers;
    }

    static void DrawHills((int x, int y) size, Montes monte, IAlgoritmoHill[] hillClimmers)
    {
        ResizeWindow(size);
        while (!Raylib.WindowShouldClose())
        {
            acumulador += Raylib.GetFrameTime();

            if (acumulador >= timer[timerIndex]) //para controlar a velocidade de update do player nos estados
            {
                acumulador -= timer[timerIndex];
                for (int i = 0; i < frameSkiper; i++)
                {
                    foreach (IAlgoritmoHill hc in hillClimmers)
                    {
                        hc.FaserPasso();
                    }
                }
            }


            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.Black);

            for (int x = 0; x < size.x; x++) //desenha mapa
            {
                for (int y = 0; y < size.y; y++)
                {
                    //draw tile
                    double tileValue = monte.GetData(x, y);
                    Rectangle tile = new Rectangle(0, 0, blockSize, blockSize);
                    Rectangle destino = new Rectangle(x * blockSize * zoom, y * blockSize * zoom, blockSize * zoom, blockSize * zoom);

                    //Color cor = Color.Lerp(Color.Red, Color.Green,(float) tileValue / 255);

                    float t = MathF.Pow((float)(tileValue / 255.0), 0.5f);
                    Color cor = Gradiente(t);

                    Raylib.DrawTexturePro(tileset, tile, destino, Vector2.Zero, 0f, cor);
                }
            }

            //draw Player (sempre em cima de tudo o resto)
            for (int i = 0; i < hillClimmers.Length; i++) //draw hillClimbers
            {
                var player = hillClimmers[i].GetEstado();
                Rectangle playerSprite = new Rectangle(0, ((i % hillClimmers.Length)) * 16, blockSize , blockSize);
                Rectangle playerDestino = new Rectangle((int)player[0] * blockSize * zoom, (int)player[1] * blockSize * zoom, blockSize * zoom, blockSize * zoom);
                Raylib.DrawTexturePro(npcs, playerSprite, playerDestino, Vector2.Zero, 0f, Color.White);
            }


            Raylib.EndDrawing();

            //keys
            if (Raylib.IsKeyPressed(KeyboardKey.N)) //reniciar os hillClibers
            {
                Console.WriteLine("\nreset Algoritmos");
                foreach (IAlgoritmoHill hc in hillClimmers)
                {
                    hc.Restart();
                }
            }
            else if (Raylib.IsKeyPressed(KeyboardKey.Up))
            {
                updateSpeed(1);
            }
            else if (Raylib.IsKeyPressed(KeyboardKey.Down))
            {
                updateSpeed(-1);
            }
        }
    }

    static void DrawDamas((int x, int y) size, Damas damas, IAlgoritmoHill hillClimmer)
    {
        ResizeWindow(size);
        while (!Raylib.WindowShouldClose())
        {
            acumulador += Raylib.GetFrameTime();

            if (acumulador >= timer[timerIndex]) //para controlar a velocidade de update do player nos estados
            {
                acumulador -= timer[timerIndex];
                hillClimmer.FaserPasso();
            }


            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.Black);

            for (int x = 0; x < size.x; x++) //desenha mapa
            {
                for (int y = 0; y < size.y; y++)
                {
                    //draw tile
                    Rectangle tile = new Rectangle(0, 0, blockSize, blockSize);
                    Rectangle destino = new Rectangle(x * blockSize * zoom, y * blockSize * zoom, blockSize * zoom, blockSize * zoom);

                    Color cor = ((x + y) % 2) == 0 ? Color.DarkGray : Color.LightGray;

                    Raylib.DrawTexturePro(tileset, tile, destino, Vector2.Zero, 0f, cor);
                }
            }

            //draw Damas 
            
            double[] player = hillClimmer.GetEstado();
            for (int i = 0; i < player.Length; i++) {
                Rectangle playerSprite = new Rectangle(16, 0, blockSize, blockSize);
                Rectangle playerDestino = new Rectangle(i * blockSize * zoom, (int)player[i] * blockSize * zoom, blockSize * zoom, blockSize * zoom);
                Raylib.DrawTexturePro(npcs, playerSprite, playerDestino, Vector2.Zero, 0f, Color.White);
            }


            Raylib.EndDrawing();

            //keys
            if (Raylib.IsKeyPressed(KeyboardKey.N)) //reniciar os hillClibers
            {
                Console.WriteLine("\nReset");
                hillClimmer.Restart();
            }
            else if(Raylib.IsKeyPressed(KeyboardKey.Up))
            {
                updateSpeed(1);
            }
            else if(Raylib.IsKeyPressed(KeyboardKey.Down))
            {
                updateSpeed(-1);
            }
        }
    }

    //função para ajudar a criar o gradiente de verde->amarelo->vermelho, feita com ajuda da AI
    static Color Gradiente(float t)
    {
        t = Math.Clamp(t, 0f, 1f);
        if (t < 0.5f)
            return Color.Lerp(Color.Red, Color.Yellow, t * 2f);       // 0 → 0.5: vermelho → amarelo
        else
            return Color.Lerp(Color.Yellow, Color.Green, (t - 0.5f) * 2f); // 0.5 → 1: amarelo → verde
    }

    static void updateSpeed(int update)
    {
        if (update >= 1) //aumentar velocidade
        {
            if (timerIndex == 0) //velociade maxima
            {
                frameSkiper++; //frame Skip
                return;
            }
            else
            {
                acumulador = 0;//reset acumulador
                timerIndex += -1; //aumenta
            }
        }
        else //diminuir
        {
            if(frameSkiper > 1) //resetFrameSkiper
            {
                frameSkiper = 1;
            }

            if (timerIndex == timer.Length - 1) //velociade minima
            {
                return;
            }
            else
            {
                acumulador = 0; //reset acumulador
                timerIndex += 1; //diminui
            }
        }
    }
}