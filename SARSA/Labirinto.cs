using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SARSA
{
    public class Labirinto : IAmbiente
    {
        //-1 Hole
        //0 is floor
        //1 Wall
        //2 midlle point (para ajudar o treino)
        //3 ice (deslisar até bater numa parede ou acabar o gelo)
        //-2 water (pode usar um barco)
        private int[][] mapa;
        private (int x, int y) start; //start defaut
        private (int x, int y) escada;
        private Random r = new Random();
        private List<(int x, int y)> pointshouses = new List<(int x, int y)>();
        private Dictionary<(int x, int y), bool> pointshousesUsed = new Dictionary<(int x, int y), bool>(); //to make the pointshouses dynamic, only give points the first time
        //private List<(int x, int y)> housesPassed = new List<(int x, int y)>();

        private enum Diretions
        {
            left = 0,
            up = 1,
            right = 2,
            down = 3
        }

        public Labirinto(int sizeX, int sizeY)
        {
            //init floor
            mapa = new int[sizeX][];
            for (int i = 0; i < mapa.Length; i++)
            {
                mapa[i] = new int[sizeY];
            }
            Random r = new Random();
            for (int x = 0; x < sizeX; x++)
            {
                for(int y = 0; y < sizeY; y++)
                {
                    mapa[x][y] = r.Next(-1,2); //basic map
                }
            }
            //init player e a escada
            start = (0, 0);
            escada = (sizeX -1, sizeY - 1); //last square

            //faz com que a entrada e saida serem "floor"
            mapa[0][0] = 0; //player position to be floor
            mapa[sizeX - 1][sizeY - 1] = 0; //exit to be floor

            //cria um caminho possivel
            DoubleLineClearPath();
        }

        //função para limpar um caminho possivel,
        //Usando a base que para chegar no final no menor caminho possivel é
        //(escada.x - player.x) passos horizontais e (escada.y - player.y) passos Verticais
        public void DoubleLineClearPath()
        {                                        //ex: player(0,0) escada(9,9) / player(5,3) escada(7,2)
            var passosH = (escada.x - start.x); // 9 / 2 passos
            var passosV = (escada.y - start.y); // 9 / -1
            var passosToPointsHouse = (passosH + passosV) / 2;
            var passosToNextPointsHouse = passosToPointsHouse;
            var position = start;

            var mode = r.NextDouble(); //apenas para deixar este caminho menos diagonal se fizer 50/50 apenas
            double chanceH;
            double chanceV;
            if(mode > 0.66) // 70% Horizontal 30% Vertical
            {
                chanceH = 0.7;
                chanceV = 0.3;
            } 
            else if (mode > 0.33) // 30% Horizontal 70% Vertical
            {
                chanceH = 0.3;
                chanceV = 0.7;
            }
            else // 50/50 %
            {
                chanceH = 0.5;
                chanceV = 0.5;
            }

            //Criar caminho
            while( passosH != 0 || passosV != 0)
            {
                var chance = r.NextDouble();
                if (chance < chanceH) //mover horizontal
                {
                    var passo = (passosH > 0) ? -1 : 1; //caso o numero for positivo "-1" caso positivo "+1"
                    position.x -= passo;  //mover
                    passosH += passo;
                    if (passosH == 0) //acabou passos horizontais
                    {
                        chanceH = 0; //sem passos horizontais
                        chanceV = 1; //agora apenas verticais
                    }
                }
                else //mover Vertical
                {
                    var passo = (passosV > 0) ? -1 : 1; //caso o numero for positivo "-1" caso positivo "+1"
                    position.y -= passo;
                    passosV += passo;
                    if (passosV == 0) //acabou passos horizontais
                    {
                        chanceH = 1; //agora apenas Horizontais
                        chanceV = 0; //sem passs Verticais
                    }
                }
                passosToNextPointsHouse--;
                if (passosToNextPointsHouse == 0)
                {
                    mapa[position.x][position.y] = 2; //midle point
                    pointshouses.Add(position); //give 5 points for this position
                    pointshousesUsed[position] = false; //mark as not used
                    passosToNextPointsHouse = passosToPointsHouse;
                }
                else
                {
                    mapa[position.x][position.y] = 0; //clear floor
                }

            }
        }

        //faz o labirinto possivel
        /*public void Flood()
        {
            List<(int x, int y)> visitado = new List<(int x, int y)>();
            FloodRecurcivo((0, 0), visitado);
        }

        public void FloodRecurcivo((int x, int y) position, List<(int x, int y)> visitado)
        {
            visitado.Add(position);
            var acoes = AcoesPossiveis();
            foreach(int a in acoes)
            {
                (int x, int y) newFloor = GetNextFloor(position, a); //get next position
                if (!visitado.Contains(newFloor))//) && mapa[newFloor.x][newFloor.y] == 0) //apenas investiga caminhos possiveis
                {
                    FloodRecurcivo(newFloor, visitado);
                }
            }
        }*/

        public int getFloorData(int x , int y)
        {
            return mapa[x][y];
        }



        public (int x, int y) getEscadaPosition()
        {
            return escada;
        }

        //movendo na direção escolhida qual é a nova posição
        public (int x, int y) GetNextFloor((int x, int y) position,int dir)
        {
            //housesPassed.Add(position); //passed floor
            if (dir == 0) //left
            {
                if (position.x - 1 > 0) 
                {
                    position.x -= 1;
                }
            }
            else if (dir == 1) //up
            {
                if (position.y - 1 > 0) 
                {
                    position.y -= 1;
                }
            }
            else if (dir == 2) //right
            {
                if (position.x + 1 < mapa.Length)
                {
                    position.x += 1;
                }
            }
            else if (dir == 3 ) //down
            {
                if (position.y + 1 < mapa[0].Length)
                {
                    position.y += 1;
                }
            }
            return position;
        }

        public double[] GetInicialState()
        {
            //reset pointshousesUsed
            foreach (var key in pointshousesUsed.Keys.ToList())
            {
                pointshousesUsed[key] = false;
            }
            //housesPassed.Clear();
            return new double[] { start.x, start.y };
        }

        public double[] ExecutarAcao(double[] estado, int a)
        {
            int x = (int)estado[0];
            int y = (int)estado[1];
            var newPosition = GetNextFloor((x,y),a);
            return new double[] { newPosition.x, newPosition.y };
        }

        public List<int> AcoesPossiveis(double[] estado)
        {
            int x = (int)estado[0];
            int y = (int)estado[1];
            List<int> acoesPossiveis = new List<int>();
            if (x - 1 >= 0) //left
            {
                if (mapa[x - 1][y] != 1)
                {
                    acoesPossiveis.Add((int)Diretions.left);
                }
            }
            if (y - 1 >= 0) //up
            {
                if(mapa[x][y - 1] != 1)
                {
                    acoesPossiveis.Add((int)Diretions.up);
                }
            }
            if (x + 1 < mapa.Length) //right
            {
                if (mapa[x + 1][y] != 1)
                {
                    acoesPossiveis.Add((int)Diretions.right);
                }
            }
            if (y + 1 < mapa[x].Length) //down
            {
                if (mapa[x][y + 1] != 1)
                {
                    acoesPossiveis.Add((int)Diretions.down);
                }
            }
            return acoesPossiveis;
        }

        public double GetRecompensa(double[] estado)
        {
            int x = (int)estado[0];
            int y = (int)estado[1];

            if (x == escada.x && y == escada.y) //escada
            {
                return 100;
            }
            if (mapa[x][y] == -1) //hole
            {
                return -100;
            }
            if (pointshouses.Contains((x, y)) && !pointshousesUsed[(x, y)]) //middle point houses
            {
                pointshousesUsed[(x, y)] = true; //mark as used
                return 50;
            }

            /*if (housesPassed.Contains(((int)estado[0], (int)estado[1])))
            {
                return -5; //punir passar por casas repetidas
            }*/

            return -1;
        }

        public bool IsTerminal(double[] estado) //estado [x , y]
        {
            int x = (int) estado[0];
            int y = (int) estado[1];
            return (x == escada.x && y == escada.y) || mapa[x][y] == -1; //chegou no final ou caiu num buraco
        }
    }
}
