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
            start = (2, 2);
            SquareRoad(size);
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
            return new List<int> { 0, 1, 2 , 3, 4}; // 0 esquerda, 1 frente, 2 direita, 3 acelerar, 4 desacelerar
        }

        public double[] ExecutarAcao(double[] estado, int a)
        {
            (int x, int y) posicao = ((int)estado[0], (int)estado[1]);
            int direcao = (int)estado[2];
            int aceleracao = (int)estado[3];

            (int x, int y) NovaPosicao = posicao;
            switch (direcao)
            {
                case 0: //esquerda
                    NovaPosicao.x -= aceleracao;
                    break;
                case 1: //cima
                    NovaPosicao.y -= aceleracao;
                    break;
                case 2: //direita
                    NovaPosicao.x += aceleracao;
                    break;
                case 3: //baixo
                    NovaPosicao.y += aceleracao;
                    break;
            }
            NovaPosicao = CheckObstaculos(posicao,NovaPosicao);

            //faz a mudança de direção
            if (a < 3) //direita, frente, esquerda
            {
                direcao = (direcao + a) % 4;
            }

            return new double[] { NovaPosicao.x, NovaPosicao.y, direcao };

        }
        public (int x, int y) CheckObstaculos((int x, int y) posicao, (int x, int y) novaPosicao)
        {
            int moveX = novaPosicao.x - posicao.x;
            int dx = moveX != 0 ? (moveX > 0 ? 1 : -1) : 0; //direcao do movimento X
            int moveY = novaPosicao.y - posicao.y;
            int dy = moveY != 0 ? (moveY > 0 ? 1 : -1) : 0; //direcao do movimento Y

            for (int x = posicao.x; x != novaPosicao.x; x = x + dx)
            {
                for (int y = posicao.y; y != novaPosicao.y; y = y + dy)
                {
                    if (mapa[x][y] == 2)
                    {
                        return (x,y); //colidiu com parede, retorna a posiçao da batida
                    }
                }
            }
            return novaPosicao; //não colidiu, retorna a nova posição
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
            return new double[] { start.x, start.y , 3, 0}; //position x, position y, direcao, velocidade
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
