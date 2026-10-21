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

    static (int x, int y) size = (10, 10);
    static int zoom = 4;
    static int blockSize = 16;
    static Random r = new Random();
    static void LabirintoBase()
    {
        var labirinto = new Labirinto(size.x, size.y);

        Raylib.InitWindow(size.x * blockSize * zoom, size.y * blockSize * zoom, "Labirinto");
        Raylib.SetTargetFPS(30);

        //init tileset
        Texture2D tileset = Raylib.LoadTexture("Assets/Floor_TileSet.png");
        Texture2D npcs = Raylib.LoadTexture("Assets/NPC.png");
        Raylib.SetTextureFilter(tileset, TextureFilter.Point); // para a pixel art nao ficar borrada

        while (!Raylib.WindowShouldClose())
        {
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.Black);

            for (int x = 0; x < size.x; x++)
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

            //draw Player (sempre em cima)
            var player = labirinto.getPlayerPosition();
            Rectangle playerSprite = new Rectangle(0, 0, blockSize, blockSize);
            Rectangle playerDestino = new Rectangle(player.x * blockSize * zoom, player.y * blockSize * zoom, blockSize * zoom, blockSize * zoom);
            Raylib.DrawTexturePro(npcs, playerSprite, playerDestino, Vector2.Zero, 0f, Color.White);

            //labirinto.MovePlayer(r.Next(0, 5));


            Raylib.EndDrawing();
        }
    }

}