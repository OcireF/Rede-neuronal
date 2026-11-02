using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SARSA
{
    public class PistaCorrida : IAmbiente
    {
        public int[][] mapa;
        private (int x, int y) start; //start defaut
        //0 grass
        //1 pista
        //2 parede

        public PistaCorrida((int x,int y) size) 
        {
            InitFloor(size.x, size.y);
            SquareRoad(size);
            start = (2,2);
        }

        public void InitFloor(int sizeX, int sizeY) //init all at 0
        {
            mapa = new int[sizeX][];
            for (int i = 0; i < mapa.Length; i++)
            {
                mapa[i] = new int[sizeY];
            }
        }

        public void SquareRoad((int x, int y) size)
        {
            for (int x = 0; x < size.x; x++) //paredes horizontais
            {
                mapa[x][0] = 2;            // parede de cima
                mapa[x][size.y - 1] = 2;   // parede de baixo
            }

            for (int y = 0; y < size.y; y++) //parede verticais
            {
                mapa[0][y] = 2;            // parede da esquerda
                mapa[size.x - 1][y] = 2;   // parede da direita
            }

            //pista
            for (int x = 2; x < size.x - 2; x++) // horizontais
            {
                mapa[x][2] = 1;
                mapa[x][3] = 1;
                mapa[x][size.y - 1 - 2] = 1;
                mapa[x][size.y - 1 - 3] = 1;
            }

            for (int y = 2; y < size.y - 2; y++) //parede verticais
            {
                mapa[2][y] = 1;            // parede da esquerda
                mapa[3][y] = 1;            // parede da esquerda
                mapa[size.x - 1 - 2][y] = 1;   // parede da direita
                mapa[size.x - 1 - 3][y] = 1;   // parede da direita
            }

            //midle blocks
            (int x, int y) midle = (size.x/2, size.y/2);

            for (int x = midle.x - 2; x <= midle.x + 2; x++) 
            {
                mapa[x][midle.y + 2] = 2;
                mapa[x][midle.y - 2] = 2;
            }

            for (int y = midle.y - 2; y <= midle.y + 2; y++) 
            {
                mapa[midle.x + 2][y] = 2;  
                mapa[midle.x - 2][y] = 2;
            }

        }

        public static IAmbiente LoadAmbienteFromFile(string fileName)
        {
            throw new NotImplementedException();
        }

        public List<int> AcoesPossiveis(double[] estado)
        {
            throw new NotImplementedException();
        }

        public double[] ExecutarAcao(double[] estado, int a)
        {
            throw new NotImplementedException();
        }

        public (int x, int y) GetSize()
        {
            return (mapa.Length, mapa[0].Length);
        }
        public int getFloorData(int x, int y)
        {
            return mapa[x][y];
        }

        public double[] GetInicialState()
        {
            return new double[] { start.x, start.y };
        }

        public double GetRecompensa(double[] estado)
        {
            throw new NotImplementedException();
        }

        public bool IsTerminal(double[] estado)
        {
            throw new NotImplementedException();
        }

        public void SaveAmbienteInStream(StreamWriter fileStream)
        {
            throw new NotImplementedException();
        }
    }
}
