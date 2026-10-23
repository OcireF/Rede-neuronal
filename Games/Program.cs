using Raylib_cs;
using SARSA;
using System.Numerics;
using System.Reflection.Metadata.Ecma335;
class Program
{
    static void Main(string[] args)
    {
        LabirintoBase();
    }

    static (int x, int y) size = (15, 15); // samll(5,5)(8) meadle(15,15)(4) big (50,50)(1)
    static int zoom = 4;
    static int blockSize = 16;

    static int numTreinos = 5000;
    static int numPassos = size.x * size.y; //correr o labirinto inteiro
    static double gamma = 0.99; //gama alto para ver a escada no final
    static double alfa = 0.3;
    static double efen = 0.3; //faser decaimento
    static double valor_omissao = 0.0;

    static void LabirintoBase()
    {
        var labirinto = new Labirinto(size.x, size.y);
        var sarsa = new SARSA_Algoritmo(labirinto, gamma, alfa, efen, valor_omissao);
        //temp
        var estados = sarsa.treino(numTreinos, numPassos);



        int playerFrame = 0;      //frame do treino
        float timer = 0.05f; //timer da velociade de atualização do player
        float acumulador = 0; //acumulador para contar o tempo passado

        Raylib.InitWindow(size.x * blockSize * zoom, size.y * blockSize * zoom, "Labirinto");
        Raylib.SetTargetFPS(60);

        //init tileset
        Texture2D tileset = Raylib.LoadTexture("Assets/Floor_TileSet.png");
        Texture2D npcs = Raylib.LoadTexture("Assets/NPC.png");
        Texture2D arrows = Raylib.LoadTexture("Assets/Arrows.png");
        Raylib.SetTextureFilter(tileset, TextureFilter.Point); // para a pixel art nao ficar borrada

        while (!Raylib.WindowShouldClose())
        {
            acumulador += Raylib.GetFrameTime();

            if (acumulador >= timer)
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

            //draw Escada
            var escada = labirinto.getEscadaPosition();
            Rectangle escadaSprite = new Rectangle(1 * blockSize, 0, blockSize, blockSize);
            Rectangle escadaDestino = new Rectangle(escada.x * blockSize * zoom, escada.y * blockSize * zoom, blockSize * zoom, blockSize * zoom);
            Raylib.DrawTexturePro(npcs, escadaSprite, escadaDestino, Vector2.Zero, 0f, Color.White);

            //draw Arrows


            var politica = sarsa.Politica;
            foreach (double[] position in politica.Keys)
            {
                int acao = 0;
                double maxValue = int.MinValue; //minValue para sempre selecionar uma acao
                foreach (int a in politica[position].Keys)
                {
                    if (politica[position][a] > maxValue)
                    {
                        acao = a; //max argument
                        maxValue = politica[position][a];
                    }
                }
                //draw a
                Rectangle arrowSprite = new Rectangle(acao * blockSize, 0, blockSize, blockSize);
                Rectangle arrowDestino = new Rectangle((int)position[0] * blockSize * zoom, (int)position[1] * blockSize * zoom, blockSize * zoom, blockSize * zoom);
                Raylib.DrawTexturePro(arrows, arrowSprite, arrowDestino, Vector2.Zero, 0f, Color.White);
            }

            //draw Player (sempre em cima)
            var player = estados[playerFrame];
            Rectangle playerSprite = new Rectangle(0, 0, blockSize, blockSize);
            Rectangle playerDestino = new Rectangle((int)player[0] * blockSize * zoom, (int)player[1] * blockSize * zoom, blockSize * zoom, blockSize * zoom);
            Raylib.DrawTexturePro(npcs, playerSprite, playerDestino, Vector2.Zero, 0f, Color.White);

            Raylib.EndDrawing();

            if (Raylib.IsKeyPressed(KeyboardKey.N)) //Novo labirinto
            {
                labirinto = new Labirinto(size.x, size.y);
                sarsa = new SARSA_Algoritmo(labirinto, gamma, alfa, efen, valor_omissao);
                estados = sarsa.treino(numTreinos, numPassos);
                playerFrame = 0;
            }
            if (Raylib.IsKeyPressed(KeyboardKey.P)) //Aproveitar o treino
            {
                sarsa.efen = 0.0; //para não explorar mais
                estados.Clear();
                estados = sarsa.treino(10, numPassos);
                playerFrame = 0;
            }
        }
    }

}