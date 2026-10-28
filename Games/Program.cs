using Raylib_cs;
using SARSA;
using System.Numerics;
using System.Reflection.Metadata.Ecma335;
class Program
{
    static void Main(string[] args)
    {
        int test = 0;
        //while (test != 99)
        {
            test = 0;

            Console.WriteLine(
                "Textes Parte 2.1/2.2\n" +
                "(1) - Random small  ( 5x5 ) Labirinto\n" +
                "(2) - Random Medion (15x15) Labirinto\n" +
                "(3) - Random Big    (50x50) Labirinto\n" +
                "(4) - Load mapa e politicas guardadas e faz um novo treino\n" +
                "Textes Parte 2.2\n" +
                "(5) - Criar graficos de treino Sarsa vs Q-Learning\n" +
                "\nNotes:\n" +
                "O calculo do algorimo é feito antes do visual, o visual é apenas para visualização\n" +
                "\nBotões:\n" +
                "N - Novo labirinto\n" +
                "P - Faz um novo treino com efen=0 e mostra a sua ultima volta no labirinto\n" +
                "S - Salva o labirinto e a politica atual\n"
                );
            Console.Write("Choice: ");
            test = Convert.ToInt32(Console.ReadLine());
            switch (test)
            {
                case 1:
                    LabirintoBase((5, 5), 300);
                    break;
                case 2:
                    LabirintoBase((15, 15), 5000);
                    break;
                case 3:
                    LabirintoBase((50, 50), 5000);
                    break;
                case 4:
                    LabirintoBase((0,0), 100, "Labirinto"); //não defene o size porque vai ser lido do ficheiro
                    break;
                case 5:

                    break;
            }
        }
    }



    //static (int x, int y) size = (50, 50); // samll(5,5)(8) meadle(15,15)(4) big (50,50)(1)
    //static int zoom = 1;
    //static int numTreinos = 5000;
    //static int numPassos = size.x * size.y; //correr o labirinto inteiro
    static int blockSize = 16;
    static double gamma = 0.99; //gama alto para ver a escada no final
    static double alfa = 0.3;
    static double efen = 0.3; // 30% de explorar
    static double valor_omissao = 0.0;
    static bool showArrows = false; //mostrar as setas da politica

    static void LabirintoBase((int x, int y) size, int numTreinos, string file = null) //, SARSA_Algoritmo algoritmo
    {
        Labirinto labirinto;
        SARSA_Algoritmo sarsa;
        Q_Learning q_Learning;

        if (file == null)
        {
            labirinto = new Labirinto(size.x, size.y);
            sarsa = new SARSA_Algoritmo(labirinto, gamma, alfa, efen, valor_omissao);
            q_Learning = new Q_Learning(labirinto, gamma, alfa, efen, valor_omissao);
        }
        else //ler labirinto e politicas de ficheiros
        {
            labirinto = Labirinto.LoadAmbienteFromFile(file + ".ambiente");
            size = labirinto.GetSize(); //set size do labirinto
            sarsa = new SARSA_Algoritmo(labirinto, gamma, alfa, efen, valor_omissao, file + ".sarsa");
            q_Learning = new Q_Learning(labirinto, gamma, alfa, efen, valor_omissao);
        }

        int numPassos = size.x * size.y; //set o numero de passos para o treino
        var estados = sarsa.treino(numTreinos, numPassos); //pré calcula o algortimo e retorna todos os estados no treino
        var estadosQlerning = q_Learning.treino(numTreinos, numPassos);

        int playerFrame = 0;  //frame do treino
        float timer = 0.05f; //timer da velociade de atualização do player
        float acumulador = 0; //acumulador para contar o tempo passado

        Raylib.SetConfigFlags(ConfigFlags.HiddenWindow); //inddenWindow, enquanto faz setup do zoom
        Raylib.InitWindow(800, 600, "Labirinto"); // tamanho temporario
        //Raylib.InitWindow(size.x * blockSize * zoom, size.y * blockSize * zoom, "Labirinto");
        Raylib.SetTargetFPS(60);

        //set Max Zoom
        int monitor = Raylib.GetCurrentMonitor();
        int maxW = (int)(Raylib.GetMonitorWidth(monitor) * 0.9); //margem para as bordas laterais
        int maxH = (int)(Raylib.GetMonitorHeight(monitor) * 0.85); //margem para o titulo e borda 

        int zoom = Math.Max(1, Math.Min( //captura o zoom maximo
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
        Console.WriteLine("w"+ monitorW + "h" + monitorH);

        Raylib.ClearWindowState(ConfigFlags.HiddenWindow);

        //init tileset
        Texture2D tileset = Raylib.LoadTexture("Assets/Floor_TileSet.png");
        Texture2D npcs = Raylib.LoadTexture("Assets/NPC.png");
        Texture2D arrows = Raylib.LoadTexture("Assets/Arrows.png");
        Raylib.SetTextureFilter(tileset, TextureFilter.Point); // para a pixel art nao ficar borrada

        while (!Raylib.WindowShouldClose())
        {
            acumulador += Raylib.GetFrameTime();

            if (acumulador >= timer) //para controlar a velocidade de update do player nos estados
            {
                acumulador -= timer;
                if(playerFrame < estados.Count()-1)
                {
                    playerFrame++;
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
                    Rectangle tile = new Rectangle(tileIndex * blockSize, tileIndex * blockSize, blockSize, blockSize);
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
                var politica = sarsa.Politica;
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

            //draw Player (sempre em cima de tudo o resto) SARSA
            var player = estados[playerFrame];
            Rectangle playerSprite = new Rectangle(0, 0, blockSize, blockSize);
            Rectangle playerDestino = new Rectangle((int)player[0] * blockSize * zoom, (int)player[1] * blockSize * zoom, blockSize * zoom, blockSize * zoom);
            Raylib.DrawTexturePro(npcs, playerSprite, playerDestino, Vector2.Zero, 0f, Color.White);

            //draw Player (sempre em cima de tudo o resto) Q-lerning
            var player2 = estadosQlerning[playerFrame];
            Rectangle playerSprite2 = new Rectangle(0, 16, blockSize, blockSize);
            Rectangle playerDestino2 = new Rectangle((int)player2[0] * blockSize * zoom, (int)player2[1] * blockSize * zoom, blockSize * zoom, blockSize * zoom);
            Raylib.DrawTexturePro(npcs, playerSprite2, playerDestino2, Vector2.Zero, 0f, Color.White);


            Raylib.EndDrawing();

            if (Raylib.IsKeyPressed(KeyboardKey.N)) //Novo labirinto
            {
                labirinto = new Labirinto(size.x, size.y);
                sarsa = new SARSA_Algoritmo(labirinto, gamma, alfa, efen, valor_omissao);
                estados.Clear();
                estados = sarsa.treino(numTreinos, numPassos);
                showArrows = false;
                playerFrame = 0;
            }
            if (Raylib.IsKeyPressed(KeyboardKey.P)) //Aproveitar o treino
            {
                sarsa.efen = 0.0; //para não explorar mais
                estados.Clear();
                estados = sarsa.treino(numTreinos, numPassos, true);
                showArrows = true;
                playerFrame = 0;
            }
            if (Raylib.IsKeyPressed(KeyboardKey.S)) //Aproveitar o treino
            {
                sarsa.SavePoliticaOnFile("Labirinto");
                Console.WriteLine("Save Done do Labirinto e Politica");
            }
        }
    }

}