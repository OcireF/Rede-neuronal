using Raylib_cs;
using SARSA;
using System.Numerics;
class Program
{
    //static (int x, int y) size = (50, 50); // samll(5,5)(8) meadle(15,15)(4) big (50,50)(1)
    //static int numTreinos = 5000;
    //static int numPassos = size.x * size.y; //correr o labirinto inteiro

    static double gamma = 0.99; //gama alto para ver a escada no final
    static double alfa = 0.3;
    static double efen = 0.1; // 10% de explorar
    static double valor_omissao = 0.0;
    static bool showArrows = false; //mostrar as setas da politica
    static float[] timer = { 0.001f, 0.05f, 0.1f, 0.5f }; //timer da velociade de atualização do player
    static int timerIndex = 1; //default speed "0.05f"
    static float acumulador = 0; //acumulador para contar o tempo passado

    //window stuff
    static int blockSize = 16;
    static int zoom = 1;
    //texturas
    static Texture2D tileset;// = Raylib.LoadTexture("Assets/Floor_TileSet.png");
    static Texture2D npcs;// = Raylib.LoadTexture("Assets/NPC.png");
    static Texture2D arrows; //= Raylib.LoadTexture("Assets/Arrows.png");

    static void Main(string[] args)
    {
        int test = 0;
        //while (test != 99)
        {
            test = 0;

            Console.WriteLine(
                "Textes Parte 2.1/2.2\n" +
                "(1) - Random small  ( 5x5 ) Labirinto (Sarsa)\n" +
                "(2) - Random Medium (15x15) Labirinto (Sarsa)\n" +
                "(3) - Random Big    (50x50) Labirinto (Sarsa)\n" +
                "Textes Parte 2.2\n" +
                "(4) - Load mapa e politicas guardadas e faz um novo treino (Sarsa e Q-Learning)\n" +
                "(5) - Random Medium (15x15) Labirinto (Q-Learning)\n" +
                "(6) - Random Medium (10x10) Labirinto Sarsa (vermelho) vs Q-Learning (Azul)\n" +
                "Programa 2.3\n" +
                "(7) - Pista de corrida\n" +
                "\n Notes:\n" +
                "Algoritmo Primario - algorimo do primeiro agente" +
                "\nBotões:\n" +
                "N - Novo labirinto Usando Agentes (Sarsa)\n" +
                "P - Modifica o efen para 0 (apenas para o Algorimo Primario)\n" +
                "A - Apresenta as Setas do algorimo (apenas para o Algorimo Primario)\n" +
                "S - Salva o labirinto e a politica atual (apenas para o Algorimo Primario)\n"
                );
            Console.Write("Choice: ");
            test = Convert.ToInt32(Console.ReadLine());
            SetupWindow();
            switch (test)
            {
                case 1:
                    LabirintoAgentesSarsa((5, 5), 300);
                    break;
                case 2:
                    LabirintoAgentesSarsa((15, 15), 5000);
                    break;
                case 3:
                    LabirintoAgentesSarsa((50, 50), 5000);
                    break;
                case 4:
                    LabirintoAgentesLoadMapa("Labirinto"); //não define o size porque vai ser lido do ficheiro
                    break;
                case 5:
                    LabirintoAgentesQ_learning((15, 15), 5000); //não define o size porque vai ser lido do ficheiro
                    break;
                case 6:
                    LabirintoSarsaVsQ_lerning((10, 10), 500);
                    break;
                case 7:
                    PistaCorrida((15, 15));
                    break;
            }
        }
    }

    static void LabirintoAgentesSarsa((int x, int y) size, int numTreinos, int numAgentes = 0)
    {
        if (numAgentes == 0) {
            Console.Write("Numero de agentes: ");
            numAgentes = Convert.ToInt32(Console.ReadLine());
        }

        Labirinto labirinto = new Labirinto(size.x, size.y);
        SARSA_Algoritmo sarsa = new SARSA_Algoritmo(labirinto, gamma, alfa, efen, valor_omissao);
        
        int numPassos = size.x * size.y; //set o numero de passos para o treino

        Agente[] agentes = new Agente[numAgentes]; //setup agentes
        for (int i = 0; i < agentes.Length; i++) 
        {
            agentes[i] = sarsa.CreateAgente(numTreinos, numPassos);
        }
        ResizeWindow(size);
        LabirintoBase(sarsa, labirinto, agentes, 4);
    }

    static void LabirintoAgentesLoadMapa(string file) //cria o mapa a partir do ficheiro salvado anteriormente
    {
        if (!File.Exists(AppContext.BaseDirectory + file + ".ambiente"))
        {
            Console.WriteLine("Labirinto não encontrado");
            return;
        }

        Agente[] agentes = new Agente[2]; //Sarsa

        Labirinto labirinto = Labirinto.LoadAmbienteFromFile(file + ".ambiente");
        (int x, int y) size = labirinto.GetSize(); //set size do labirinto
        SARSA_Algoritmo sarsa = new SARSA_Algoritmo(labirinto, gamma, alfa, efen, valor_omissao, file + ".sarsa");
        Q_Learning q_Learning = new Q_Learning(labirinto, gamma, alfa, efen, valor_omissao, file + ".sarsa");

        int numPassos = size.x * size.y; //set o numero de passos para o treino

        agentes[0] = sarsa.CreateAgente(100, numPassos);         //Sarsa
        agentes[1] = q_Learning.CreateAgente(100, numPassos);    //Q_Learning

        ResizeWindow(size);
        LabirintoBase(sarsa, labirinto, agentes, 4);
    }

    static void LabirintoAgentesQ_learning((int x, int y) size, int numTreinos, int numAgentes = 0)
    {
        if (numAgentes == 0)
        {
            Console.Write("Numero de agentes: ");
            numAgentes = Convert.ToInt32(Console.ReadLine());
        }

        Agente[] agentes = new Agente[numAgentes]; // Q_Learning agentes

        Labirinto labirinto = new Labirinto(size.x, size.y);
        Q_Learning q_Learning = new Q_Learning(labirinto, gamma, alfa, efen, valor_omissao);

        int numPassos = size.x * size.y; //set o numero de passos para o treino

        for (int i = 0; i < agentes.Length; i++)
        {
            agentes[i] = q_Learning.CreateAgente(numTreinos, numPassos);
        }

        ResizeWindow(size);
        LabirintoBase(q_Learning, labirinto, agentes, 2);
    }

    static void LabirintoSarsaVsQ_lerning((int x, int y) size, int numTreinos)
    {
        Agente[] agentes = new Agente[2]; //Sarsa vs Q_Learning

        Labirinto labirinto = new Labirinto(size.x, size.y);
        SARSA_Algoritmo sarsa = new SARSA_Algoritmo(labirinto, gamma, alfa, efen, valor_omissao);
        Q_Learning q_Learning = new Q_Learning(labirinto, gamma, alfa, efen, valor_omissao);

        int numPassos = size.x * size.y; //set o numero de passos para o treino

        agentes[0] = sarsa.CreateAgente(numTreinos, numPassos);         //Sarsa
        agentes[1] = q_Learning.CreateAgente(numTreinos, numPassos);    //Q_Learning

        ResizeWindow(size);
        LabirintoBase(sarsa, labirinto, agentes, 2);
    }

    static void PistaCorrida((int x, int y) size)
    {
        PistaCorrida pista = new PistaCorrida(size);

        DrawPista(null, pista);

    }
    static void DrawPista(IAlgoritmo algoritmo, PistaCorrida pista)
    {
        (int x, int y) size = pista.GetSize();
        ResizeWindow(size);

        while (!Raylib.WindowShouldClose())
        {
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.Black);

            for (int x = 0; x < size.x; x++) //desenha o labirinto
            {
                for (int y = 0; y < size.y; y++)
                {
                    //draw tile
                    var tileIndex = pista.getFloorData(x, y); //O indice começa no -1 (buraco,flor,wall)
                    Rectangle tile = new Rectangle(tileIndex * blockSize, 16, blockSize, blockSize);
                    Rectangle destino = new Rectangle(x * blockSize * zoom, y * blockSize * zoom, blockSize * zoom, blockSize * zoom);
                    Raylib.DrawTexturePro(tileset, tile, destino, Vector2.Zero, 0f, Color.White);
                }
            }
            Raylib.EndDrawing();
        }
    }

    static void SetupWindow()
    {
        Raylib.SetConfigFlags(ConfigFlags.HiddenWindow); //inddenWindow, enquanto faz setup do zoom
        Raylib.InitWindow(800, 600, "Labirinto"); // tamanho temporario
        //Raylib.InitWindow(size.x * blockSize * zoom, size.y * blockSize * zoom, "Labirinto");
        Raylib.SetTargetFPS(60);

        //init tileset
        tileset = Raylib.LoadTexture(AppContext.BaseDirectory + "Assets/Floor_TileSet.png");
        npcs = Raylib.LoadTexture(AppContext.BaseDirectory + "Assets/NPC.png");
        arrows = Raylib.LoadTexture(AppContext.BaseDirectory + "Assets/Arrows.png");
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

    static void LabirintoBase(IAlgoritmo algoritmo, Labirinto labirinto, Agente[] agentes, int numColors = 1) //colors para diferenciar quando usamos mais de um algoritmo
    {
        (int x, int y) size = labirinto.GetSize();

        while (!Raylib.WindowShouldClose())
        {
            acumulador += Raylib.GetFrameTime();

            if (acumulador >= timer[timerIndex]) //para controlar a velocidade de update do player nos estados
            {
                acumulador -= timer[timerIndex];
                foreach (Agente a in agentes)
                {
                    a.RunPasso();
                }
            }

            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.Black);

            for (int x = 0; x < size.x; x++) //desenha o labirinto
            {
                for (int y = 0; y < size.y; y++)
                {
                    //draw tile
                    var tileIndex = labirinto.getFloorData(x, y) + 1; //O indice começa no -1 (buraco,flor,wall)
                    Rectangle tile = new Rectangle(tileIndex * blockSize, 0, blockSize, blockSize);
                    Rectangle destino = new Rectangle(x * blockSize * zoom, y * blockSize * zoom, blockSize * zoom, blockSize * zoom);
                    Raylib.DrawTexturePro(tileset, tile, destino, Vector2.Zero, 0f, Color.White);
                }
            }

            //desenha a Escada
            var escada = labirinto.getEscadaPosition();
            Rectangle escadaSprite = new Rectangle(1 * blockSize, 0, blockSize, blockSize);
            Rectangle escadaDestino = new Rectangle(escada.x * blockSize * zoom, escada.y * blockSize * zoom, blockSize * zoom, blockSize * zoom);
            Raylib.DrawTexturePro(npcs, escadaSprite, escadaDestino, Vector2.Zero, 0f, Color.White);

            //desenha as setas
            if (showArrows)
            {
                var politica = algoritmo.GetPolitica();
                foreach (double[] position in politica.Keys)
                {
                    int acao = 5; //acao erro no visual
                    double maxValue = int.MinValue; //minValue para sempre selecionar uma acao
                    foreach (int a in politica[position].Keys)
                    {
                        if (politica[position][a] > maxValue)
                        {
                            acao = a; //max argument
                            maxValue = politica[position][a];
                        }
                    }
                    //desenha a seta
                    Rectangle arrowSprite = new Rectangle(acao * blockSize, 0, blockSize, blockSize);
                    Rectangle arrowDestino = new Rectangle((int)position[0] * blockSize * zoom, (int)position[1] * blockSize * zoom, blockSize * zoom, blockSize * zoom);
                    Raylib.DrawTexturePro(arrows, arrowSprite, arrowDestino, Vector2.Zero, 0f, Color.White);
                }
            }

            //draw Player (sempre em cima de tudo o resto)
            for(int i = 0; i < agentes.Length; i++) //draw agentes
            {
                //draw Player (sempre em cima de tudo o resto) Q-lerning
                var player2 = agentes[i].GetCurretEstado();
                Rectangle playerSprite2 = new Rectangle(0, ((i % numColors)) * 16, blockSize, blockSize);
                Rectangle playerDestino2 = new Rectangle((int)player2[0] * blockSize * zoom, (int)player2[1] * blockSize * zoom, blockSize * zoom, blockSize * zoom);
                Raylib.DrawTexturePro(npcs, playerSprite2, playerDestino2, Vector2.Zero, 0f, Color.White);
            }

            Raylib.EndDrawing();

            if (Raylib.IsKeyPressed(KeyboardKey.N)) //Novo labirinto
            {
                LabirintoAgentesSarsa(size, 5000, agentes.Length); //cria um novo labirinto com o mesmo mumero de agentes Sarsa
            }
            if (Raylib.IsKeyPressed(KeyboardKey.P)) //Aproveitar o treino
            {
                algoritmo.ChangeEfen(-algoritmo.GetEfen()); //defene o efen a 0
            }
            if (Raylib.IsKeyPressed(KeyboardKey.A)) //Aproveitar o treino
            {
                showArrows = !showArrows;
            }
            if (Raylib.IsKeyPressed(KeyboardKey.S)) //Aproveitar o treino
            {
                algoritmo.SavePoliticaOnFile("Labirinto");
                Console.WriteLine("Save Done do Labirinto e Politica");
            }
            if (Raylib.IsKeyPressed(KeyboardKey.Up))
            {
                updateSpeed(1);
            }
            if (Raylib.IsKeyPressed(KeyboardKey.Down))
            {
                updateSpeed(-1);
            }
        }

        static void updateSpeed(int update)
        {
            if(update >= 1) //aumentar velocidade
            {
                if(timerIndex == 0) //velociade maxima
                {
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
                if (timerIndex == timer.Length-1) //velociade minima
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

}