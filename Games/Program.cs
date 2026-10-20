using Raylib_cs;
using SARSA;
class Program
{
    static void Main(string[] args)
    {
        LabirintoBase();
    }

    static (int x, int y) size = (25, 25);
    static int zoom = 1;
    static int blockSize = 16;
    static void LabirintoBase()
    {
        var labirinto = new Labirinto(size.x, size.y);

        Raylib.InitWindow(size.x * blockSize * zoom, size.y * blockSize * zoom, "Labirinto");
        Raylib.SetTargetFPS(30);

        while (!Raylib.WindowShouldClose())
        {
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.Black);

            for (int x = 0; x < size.x; x++)
            {
                for (int y = 0; y < size.y; y++)
                {
                    //draw tileset
                    if (labirinto.getFloorData(x, y) == 1)
                        Raylib.DrawRectangle(x * blockSize * zoom, y * blockSize * zoom, blockSize * zoom, blockSize * zoom, Color.Gray);
                    else if (labirinto.getFloorData(x, y) == 0)
                        Raylib.DrawRectangle(x * blockSize * zoom, y * blockSize * zoom, blockSize * zoom, blockSize * zoom, Color.White);
                    else if (labirinto.getFloorData(x, y) == -1)
                        Raylib.DrawRectangle(x * blockSize * zoom, y * blockSize * zoom, blockSize * zoom, blockSize * zoom, Color.Black);
                }
            }

            //Raylib.DrawCircle(px * tam + tam / 2, py * tam + tam / 2, tam / 3, Color.Yellow);
            Raylib.EndDrawing();
        }
    }
}