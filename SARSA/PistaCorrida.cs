using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SARSA
{
    public class PistaCorrida : IAmbiente
    {
        public class SessaoAgente //metadados do agente
        {
            public int CurrentCheckPoint = 0; //this is the ones in metadata, o estado tem um diferente, a diferença indentifica a travessia para as reconpensas
            public int Voltas = 0;
            //public bool PassedCheckpoint = false;
            //public int Voltas = 0;
        }


        private int[][] mapa;
        private (int x, int y) start; //start defaut
        //private (int x, int y) end; //start defaut
        private int minSpeed = -1;
        private int maxSpeed = 3;
        //0 grass
        //1 pista
        //2 parede

        private List<SessaoAgente> sessoes = new List<SessaoAgente>();

        private Dictionary<int, Rectangle> checkPoints = new Dictionary<int, Rectangle>();

        public PistaCorrida((int x,int y) size)//min size (15x15)
        {
            InitFloor(size.x, size.y);
            start = (size.x - 4, size.y/2);
            //end = (2, size.y / 2);
            SquareRoad(size);
        }

        public PistaCorrida(int[][] mapa, (int x, int y) start)//min size (15x15)
        {
            this.mapa = mapa;
            this.start = start;
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
            (int x, int y) midle = (size.x / 2, size.y / 2);

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
            checkPoints.Add(0, new Rectangle(size.x / 2, size.y / 2, 1, size.y / 2));//meio de baixo
            checkPoints.Add(1, new Rectangle(0, size.y / 2, size.x / 2, 1));//meio esquerda
            checkPoints.Add(2, new Rectangle(size.x / 2, 0, 1, size.y / 2));//meio cima
            checkPoints.Add(3, new Rectangle(size.x / 2, size.y / 2, size.x / 2, 1));//meio da direita (meta)

            //meta 
            // (size.x - 4, size.y/2)
            mapa[size.x - 4][size.y / 2] = 3;
            mapa[size.x - 3][size.y / 2] = 3;
            //mapa[2][size.y / 2] = 3;
        }



        public List<int> AcoesPossiveis(double[] estado)
        {
            return new List<int> { 0, 1, 2 , 3, 4}; // 0 esquerda, 1 frente, 2 direita, 3 desacelerar, 4 acelerar
        }

        public double[] ExecutarAcao(double[] estado, int a, int id = 0)
        {
            (int x, int y) posicao = ((int)estado[0], (int)estado[1]);
            int direcao = (int)estado[2];
            int aceleracao = (int)estado[3];
            int checkPoint = (int)estado[4];

            //troca de mudanças
            if (a == 3)
            {
                aceleracao = ChangeSpeed(aceleracao, -1); 
            }
            else if(a == 4)
            {
                aceleracao = ChangeSpeed(aceleracao, 1);
            }

            //movimento
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

            //fazer checks
            (NovaPosicao, checkPoint) = Checks(posicao,NovaPosicao, checkPoint, id);

            //faz a mudança de direção
            if (a < 3) //direita, frente, esquerda
            {
                direcao = ((direcao + (a - 1)) % 4 + 4) % 4;
            }

            return new double[] { NovaPosicao.x, NovaPosicao.y, direcao, aceleracao, checkPoint};

        }
        public ((int x, int y), int checkPoint) Checks((int x, int y) posicao, (int x, int y) novaPosicao,int currentCheckPoint , int agenteID)
        {
            int moveX = novaPosicao.x - posicao.x;
            int moveY = novaPosicao.y - posicao.y;
            int dx = moveX != 0 ? (moveX > 0 ? 1 : -1) : 0; //direcao do movimento X
            int dy = moveY != 0 ? (moveY > 0 ? 1 : -1) : 0; //direcao do movimento Y
            (int x, int y) passo = (dx, dy);

            for ((int x, int y) p = posicao; 
                p.x != novaPosicao.x || p.y != novaPosicao.y;
                p.x += passo.x, p.y += passo.y)
            {
                if (mapa[p.x][p.y] == 2)
                {
                    return (p, currentCheckPoint); //colidiu com parede, retorna a posiçao da batida
                }
                else if (checkPoints[currentCheckPoint].Contains(new Point(p.x, p.y))) //caso passou um checkpoint atualisa o checkPoint
                {
                    currentCheckPoint = (currentCheckPoint + 1) % checkPoints.Count;
                    if(currentCheckPoint == 0)//volta completa
                    {
                        sessoes[agenteID].Voltas++;
                    }
                }
            }
            return (novaPosicao,currentCheckPoint); //não colidiu, retorna a nova posição
        }

        public int ChangeSpeed(int old,int update)
        {
            //return 1;
            var newSpeed = old + update;
            if (newSpeed < minSpeed)
            {
                return minSpeed;
            }
            else if (newSpeed > maxSpeed)
            {
                return maxSpeed;
            }

            return newSpeed;
        }

        public (int x, int y) GetSize()
        {
            return (mapa.Length, mapa[0].Length);
        }
        public int getFloorData(int x, int y)
        {
            return mapa[x][y];
        }

        public double[] GetInicialState(int id = 0)
        {
            //reset metadata
            sessoes[id].CurrentCheckPoint = 0;
            sessoes[id].Voltas = 0;

            return new double[] { start.x, start.y , 3, 0, 0}; //position x, position y, direcao, velocidade, checkPoint
        }

        public double GetRecompensa(double[] estado, int id)
        {
            (int x, int y) posicao = ((int)estado[0], (int)estado[1]);
            var info = mapa[posicao.x][posicao.y];
            var checkpointState = estado[4];

            if (info == 2) //Parede
            {
                return -100;
            }
            else if (sessoes[id].CurrentCheckPoint != checkpointState)
            {
                sessoes[id].CurrentCheckPoint = (sessoes[id].CurrentCheckPoint + 1) % checkPoints.Count; //isto é feito para casso o estado passe multiplos checkpoins num estado
                return 1 * (sessoes[id].CurrentCheckPoint + 1); // o +1 é caso o checkpoint atual for index 0
            }
            else if (info == 0) //relva
            {
                return -1;
            }

            return 0;
        }

        public bool IsTerminal(double[] estado, int id)
        {
            (int x, int y) posicao = ((int)estado[0], (int)estado[1]);
            var info = mapa[posicao.x][posicao.y];

            if (info == 2) //bateu na parede
            {
                return true;
            }
            else if (sessoes[id].Voltas >= 3)
            {
                return true;
            }
            return false;
        }

        public void SaveAmbienteInStream(StreamWriter fileStream)
        {
            fileStream.Write("tamanho:(" + mapa.Length + "," + mapa[0].Length + ")\n"); //tamanho
            fileStream.Write("start:(" + start.x + "," + start.y + ")\n"); //start

            fileStream.Write("labirinto:\n");
            for (int y = 0; y < mapa[0].Length; y++)
            {
                var linha = Enumerable.Range(0, mapa.Length).Select(x => mapa[x][y]); //os dados na classe sao colunas, mas no ficheiro dá mais geito guardar por linhas, por isto esta tranformacao
                fileStream.WriteLine("[" + string.Join(",", linha) + "]");
            }
        }

        public static IAmbiente LoadAmbienteFromFile(string fileName)
        {
            string caminho = Path.Combine(
                AppContext.BaseDirectory,
                    fileName
                );

            int[][] mapa = { };
            (int x, int y) start = (0, 0);
            int yAtual = 0;

            foreach (string linha in File.ReadLines(caminho))
            {
                string l = linha.Trim();
                if (l.Length == 0 || l.StartsWith("labirinto:")) continue; //salta linhas vasias e "labirinto:"

                if (l.StartsWith("tamanho:")) //load mapa
                {
                    var (largura, altura) = ParseCoordenada(l); //lé a largura e altura
                    mapa = new int[largura][];                  //init mapa
                    for (int x = 0; x < largura; x++)
                        mapa[x] = new int[altura];
                }
                else if (l.StartsWith("[")) //linha a adicionar no mapa 
                {
                    int[] valores = l.Trim('[', ']') //remove os "[]"
                        .Split(',')
                        .Select(v => int.Parse(v.Trim())) //tranforma a string em inteiros
                        .ToArray();

                    for (int x = 0; x < valores.Length; x++) //escreve a linha
                        mapa[x][yAtual] = valores[x];

                    yAtual++;
                }
                else if (l.StartsWith("start:")) start = ParseCoordenada(l); //lé o start
            }

            return new PistaCorrida(mapa, start);
        }

        //função apenas para ler cordenadas em "LoadAmbienteFromFile" com ajuda de AI
        private static (int x, int y) ParseCoordenada(string l)
        {
            // "start:(1,2)" -> (1,2)
            int ini = l.IndexOf('(') + 1;
            int fim = l.IndexOf(')');
            int[] p = l.Substring(ini, fim - ini)   //remove tudo antes e depois de "(" ")"
                       .Split(',')                  //divide os 2 numeros
                       .Select(v => int.Parse(v.Trim()))
                       .ToArray();                  //tranforma num array
            return (p[0], p[1]);
        }

        public Dictionary<int, Rectangle> GetCheckPoints()
        {
            return checkPoints;
        }

        public int RegistarAgente()
        {
            sessoes.Add(new SessaoAgente());
            return sessoes.Count - 1;
        }
    }
}
