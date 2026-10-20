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
            mapa = new int[sizeX][];
            for (int i = 0; i < mapa.Length; i++)
            {
                mapa[i] = new int[sizeY];
            } //tudo a floor
            Random r = new Random();

            for (int x = 0; x < sizeX; x++)
            {
                for(int y = 0; y < sizeY; y++)
                {
                    mapa[x][y] = r.Next(-1,2); //basic map
                }
            }
            //init exit
            escada = (sizeX -1, sizeY - 1); //last square
        }

        public int getFloorData(int x , int y)
        {
            return mapa[x][y];
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

        public bool IsTerminal()
        {
            return (player.x == escada.x && player.y == escada.y);
        }
    }
}
