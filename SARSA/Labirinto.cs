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
        //3 ice (deslisar até bater numa parede ou acabar o gelo)   @NOT EMPLEMENTED
        //-2 water (pode usar um barco)                             @NOT EMPLEMENTED
        private int[][] mapa;          //dados do labirinto
        private (int x, int y) start; //start defaut
        private (int x, int y) escada; //final
        private Random r = new Random();
        //private List<string> pointshousesUsed = new List<string>(); //to make the pointshouses dynamic, only give points the first time para não viciar em voltas no ponto
        //private List<(int x, int y)> housesPassed = new List<(int x, int y)>(); //para tirar pontos de passar em casas repetidas

        private enum Diretions
        {
            left = 0,
            up = 1,
            right = 2,
            down = 3
        }

        public Labirinto(int sizeX, int sizeY)
        {
            //gerar labirinto
            RandomLabirinto(sizeX, sizeY);
            //init player start e a escada
            start = (0, 0);
            escada = (sizeX -1, sizeY - 1); //last square

            //faz com que a entrada e saida serem "floor"
            mapa[0][0] = 0; //player position to be floor
            mapa[sizeX - 1][sizeY - 1] = 0; //exit to be floor

            //cria um caminho possivel
            DoubleLineClearPath();
        }

        //lambirinto pré Feito
        public Labirinto(int[][] mapa, (int x, int y) start, (int x, int y) escada)
        {
            this.mapa = mapa;
            this.start = start;
            this.escada = escada;
        }

        //Init a variavel mapa
        public void InitFloor(int sizeX, int sizeY)
        {
            mapa = new int[sizeX][];
            for (int i = 0; i < mapa.Length; i++)
            {
                mapa[i] = new int[sizeY];
            }
        }

        public void RandomLabirinto(int sizeX, int sizeY)
        {
            InitFloor(sizeX, sizeY);
            Random r = new Random();
            for (int x = 0; x < sizeX; x++) //gera um labirinto com obstaculos random
            {
                for (int y = 0; y < sizeY; y++)
                {
                    mapa[x][y] = r.Next(-1, 2); //basic map (-1 to 1)
                }
            }
        }

        //função para limpar um caminho possivel,
        //Usando a base que para chegar no final no menor caminho possivel é
        //(escada.x - player.x) passos horizontais e (escada.y - player.y) passos Verticais
        public void DoubleLineClearPath()
        {                                        //ex: player(0,0) escada(9,9) / player(5,3) escada(7,2)
            var passosH = (escada.x - start.x); // 9 / 2 passos
            var passosV = (escada.y - start.y); // 9 / -1
            //var passosToPointsHouse = (passosH + passosV) / 2;
            //var passosToNextPointsHouse = passosToPointsHouse;
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
                mapa[position.x][position.y] = 0; //clear floor

                /*passosToNextPointsHouse--;
                if (passosToNextPointsHouse == 0)
                {
                    //mapa[position.x][position.y] = 2; //midle point
                    //pointshouses.Add(position); //give 5 points for this position
                    passosToNextPointsHouse = passosToPointsHouse;
                }*/
            }
        }
        
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
        public (int x, int y) GetSize()
        {
            return (mapa.Length, mapa[0].Length);
        }

        //reseta o ambiente e retorna o estado inicial
        public double[] GetInicialState()
        {
            //reset pointshousesUsed
            /*foreach (var key in pointshousesUsed.Keys.ToList())
            {
                pointshousesUsed[key] = false;
            }*/
            //pointshousesUsed.Clear();
            //housesPassed.Clear();
            return new double[] { start.x, start.y };
        }

        //executa a acao e retorna o estado resultante
        public double[] ExecutarAcao(double[] estado, int a)
        {
            int x = (int)estado[0];
            int y = (int)estado[1];
            var newPosition = GetNextFloor((x,y),a);
            return new double[] { newPosition.x, newPosition.y };
        }
        //retorna todas as acoes possiveis para um dado estado
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
        //retorna a recompensa do estado
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
            /*if (mapa[x][y] == 2 && !pointshousesUsed.Contains("(" + x + "," + y + ")")) //middle point houses
            {
                pointshousesUsed.Add("(" + x + "," + y + ")"); //mark as used
                return 10;
            }*/

            /*if (housesPassed.Contains(((int)estado[0], (int)estado[1])))
            {
                return -5; //punir passar por casas repetidas
            }*/

            return -1; // punir andar muito
        }


        //Verifica se o estado é terminal
        public bool IsTerminal(double[] estado) //estado [x , y]
        {
            int x = (int) estado[0];
            int y = (int) estado[1];
            return (x == escada.x && y == escada.y) || mapa[x][y] == -1; //chegou no final ou caiu num buraco
        }

        //guarda dados do lambirinto num ficheiro
        public void SaveAmbienteInStream(StreamWriter fileStream)
        {
            fileStream.Write("tamanho:(" + mapa.Length + "," + mapa[0].Length + ")\n"); //tamanho
            fileStream.Write("start:(" + start.x + "," + start.y + ")\n"); //start
            fileStream.Write("escada:(" + escada.x + "," + escada.y + ")\n"); //end

            fileStream.Write("labirinto:\n");
            for(int y = 0; y < mapa[0].Length; y++)
            {
                var linha = Enumerable.Range(0, mapa.Length).Select(x => mapa[x][y]); //os dados na classe sao colunas, mas no ficheiro dá mais geito guardar por linhas, por isto esta tranformacao
                fileStream.WriteLine("[" + string.Join(",", linha) + "]");
            }

        }

        //Ler o Ambiente guardado num file
        public static Labirinto LoadAmbienteFromFile(string fileName)
        {
            string caminho = Path.Combine(
                AppContext.BaseDirectory,
                    fileName
                );

            int[][] mapa = {};
            (int x, int y) start = (0, 0), escada = (0, 0);
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
                else if (l.StartsWith("escada:")) escada = ParseCoordenada(l); //lé a escada
            }

            return new Labirinto(mapa,start,escada);
        }

        //função apenas para ler cordenadas em "LoadAmbienteFromFile"
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

        //se quizer apenas pegar pelo ambiente
        static IAmbiente IAmbiente.LoadAmbienteFromFile(string fileName)
        {
            return LoadAmbienteFromFile(fileName);
        }
    }
}
