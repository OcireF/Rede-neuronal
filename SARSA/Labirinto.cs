using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SARSA
{
    public class Labirinto : IEstado
    {
        //-1 Hole
        //0 is floor
        //1 Wall
        //2 ice (deslisar até bater numa parede ou acabar o gelo)
        //-2 water (pode usar um barco)
        private int[][] mapa;
        private (int x, int y) player = (0,0); //start defaut
        private (int x, int y) escada;

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
            //faz com que a entrada e saida serem "floor"
            mapa[0][0] = 0; //player position to be floor
            mapa[sizeX - 1][sizeY - 1] = 0; //exit to be floor

            //init exit
            escada = (sizeX -1, sizeY - 1); //last square
        }

        //faz o labirinto possivel
        public void Flood()
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
        }

        public int getFloorData(int x , int y)
        {
            return mapa[x][y];
        }

        public (int x, int y) getPlayerPosition()
        {
            return player;
        }

        public (int x, int y) getEscadaPosition()
        {
            return escada;
        }
        public void MovePlayerTo((int x, int y) position)
        {
            player = position;
        }

        public (int x, int y) GetNextFloor((int x, int y) position,int dir)
        {
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

        public List<int> AcoesPossiveis()
        {
            List<int> acoesPossiveis = new List<int>();
            if (player.x - 1 > 0) //left
            {
                if (mapa[player.x - 1][player.y] != 1)
                {
                    acoesPossiveis.Add((int)Diretions.left);
                }
            }
            if (player.y - 1 > 0) //up
            {
                if(mapa[player.x][player.y - 1] != 1)
                {
                    acoesPossiveis.Add((int)Diretions.up);
                }
            }
            if (player.x + 1 < mapa.Length) //right
            {
                if (mapa[player.x + 1][player.y] != 1)
                {
                    acoesPossiveis.Add((int)Diretions.right);
                }
            }
            if (player.y + 1 < mapa[player.x].Length) //down
            {
                if (mapa[player.x][player.y + 1] != 1)
                {
                    acoesPossiveis.Add((int)Diretions.down);
                }
            }
            return acoesPossiveis;
        }

        public bool IsTerminal((int x, int y) position)
        {
            return (position.x == escada.x && position.y == escada.y);
        }
    }
}
