using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;

namespace SARSA
{
    public class SARSA_Algoritmo : IAlgoritmo
    {
        public Random r = new Random();
        public Dictionary<double[], Dictionary<int, double>> Politica = new Dictionary<double[], Dictionary<int, double>>(new ComparadorEstados()); //Q(s,a) = recompensa
        public IAmbiente ambiente; //Mundo onde o algoritmo vai passar
        public double gamma; //fator de desconto do futuro
        public double alfa; //taxa de aprendizagem
        public double efen; //taxa de exploracao
        public double valor_omissao; //valor_omissao das politicas (valor inicial para cada uma)

        public SARSA_Algoritmo(IAmbiente ambiente, double gamma, double alfa, double efen, double valor_omissao = 0.0, string fileToLoadPoliticas = null)
        {
            this.ambiente = ambiente;
            this.gamma = gamma;
            this.alfa = alfa;
            this.efen = efen;
            this.valor_omissao = valor_omissao;

            if (fileToLoadPoliticas != null)
            {
                LoadPolitica(fileToLoadPoliticas);
            }
        }

        //Algoritmo de propagacao das recompensas
        public double Aprender(double[] s, int a, double r, double[] sLine, int aLine, bool isFinal) //s, a, r, s’, a’, final
        {
            double j = 0;
            if (isFinal)
            {
                j = r; // J = r
            }
            else
            {
                j = r + (gamma * Politica[sLine][ aLine]); // J = r + γQ(s’, a’)
            }
            Politica[s][a] += alfa * (j - Politica[s][a]); // Q(s, a) = Q(s, a) + alfa(J – Q(s, a))
            return j;
        }
        
        //Chamar esta função para adicioar sempre que tiver novos estados para adicionar na Politica
        public void CreatePolitica(double[] estado)
        {

            if (ambiente.IsTerminal(estado)) // não adiciona nenhuma polita (esta no estado terminal)
            {
                return;
            }
            if (Politica.ContainsKey(estado)) // o estado já existe na politica
            {
                return;
            }

            Politica.Add(estado, new Dictionary<int, double>());
            //cria as acoes do estado
            var acoes = ambiente.AcoesPossiveis(estado);
            foreach(int a in acoes)
            {
                Politica[estado].Add( a, valor_omissao);
            }
        }

        //Chamar esta função para load das politicas
        public void LoadPolitica(string fileToLoadPoliticas)
        {
            string caminhoPolitica = Path.Combine(
                AppContext.BaseDirectory,
                    fileToLoadPoliticas
                );
            var pt = new CultureInfo("pt-PT"); //para usar a Cultura Pt
            double[] estadoAtual = {}; //não é suposto adicinar uma politica para "[]"

            foreach (string linha in File.ReadLines(caminhoPolitica))
            {
                string l = linha.Trim(); //remove separadores
                if (l.Length == 0) continue; //saltar linhas sem conteudo

                if (l.StartsWith("s:")) //Um novo estado a adicionar
                {
                    //s:[1,3]->
                    string dados = l.Substring(3, l.IndexOf(']') -3); //remove os "s:" e os "[]"
                    estadoAtual = dados
                        .Split(";")                             //divide os dados do estado
                        .Select(x => double.Parse(x.Trim(), pt)) //tranforma em doubles
                        .ToArray();

                    Politica.Add(estadoAtual, new Dictionary<int, double>()); //adiciona o estado na politica
                }
                else if (l.StartsWith("a:")) //uma nova acao a adicionar
                {
                    //a:0;r:76,83431692147609;
                    string[] partes = l.Split(';', StringSplitOptions.RemoveEmptyEntries); // o "StringSplitOptions.RemoveEmptyEntries" é para remover a string vasia que o "split" vai fazer no final
                    int acao = int.Parse(partes[0].Substring(2));                   // remove "a:"
                    double valor = double.Parse(partes[1].Substring(2), pt);        // remove "r:"
                    Politica[estadoAtual].Add(acao, valor);                         //adiciona a acao na politica
                }
            }
        }

        //treino do algoritmo SARSA, retorna uma lista de todos os estados passados no treino ou apenas o ultimo Episodio se o "justLastEP" = true
        public List<double[]> treino(int numEpisodios, int numPassos,bool justLastEP = false)
        {
            List<double[]> estadosTreino = new List<double[]>();
            //CreatePolitica(ambiente.GetInicialState()); //Primeira politica
            //Variables
            double[] estado;                                //s
            int acao;                                       //a
            double recompensa;                              //r
            double[] estadoLinha;                           //s'
            int acaoLinha;                                  //a'
            for (int ep = 0; ep < numEpisodios; ep++) //loop todos os Episodios
            {
                if(justLastEP) //caso queremos apenas salvar o ultimo ep para mostrar o treinamento
                {
                    estadosTreino.Clear(); //limpa o Treino para guardar apenas o ultimo episodio
                } 
                estado = ambiente.GetInicialState();        //init s
                acao = seleccionar_acao(estado);            //seleccionar acao (a)
                for(int p = 0; p < numPassos; p++)          //passos
                {
                    estadosTreino.Add(estado); //adicona o estado na lista para mais tarde vermos o treino @TEMP
                    (recompensa, estadoLinha) = executar_acao(estado, acao);//Executar ação "a"
                    //CreatePolitica(estadoLinha);                                    //cria novas politicas para o novo estado caso nao existirem
                    acaoLinha = seleccionar_acao(estadoLinha);                      //seleccionar acao Linha (a') para um estado linha (s')
                    recompensa = Aprender(estado, acao, recompensa, estadoLinha, acaoLinha,ambiente.IsTerminal(estadoLinha)); //if s' is terminal
                    estado = estadoLinha;   //atualizar estado (s <- s')
                    acao = acaoLinha;       //atualizar acao (a <- a')

                    if (ambiente.IsTerminal(estado))
                    {  //acaba os passos quando estado (s) for terminal
                        estadosTreino.Add(estado); //adiciona o estado final
                        break;
                    }
                }
            }
            return estadosTreino;
        }

        public Agente CreateAgente(int numEpisodios, int numPassos)
        {
            return null;
        }

        //Selecionador de Acoes
        public int seleccionar_acao(double[] estado)
        {
            if (!Politica.ContainsKey(estado)) //caso a politica não tiver o estado cria uma nova
            {
                CreatePolitica(estado);
            }

            int acao;
            if ( r.NextDouble() > efen)
            {
                acao = Acao_sofrega(estado); //Aproveitar conhecimento
            }
            else
            {
                acao = Acao_exploracao(estado); //Explorar
            }
            return acao;
        }

        //aproveita o conhecimento da politica
        public int Acao_sofrega(double[] estado)
        {
            if (!Politica.ContainsKey(estado)) //isto para casos terminais não existem mais acoes
            {
                return -1;
            }

            //suffle acoes
            int[] acoes = Politica[estado].Keys.ToArray();
            r.Shuffle(acoes);

            int acao = -1; //começa a -1 para caso não existem acoes
            double maxValue = int.MinValue; //minValue para sempre selecionar uma acao

            foreach(int a in acoes)
            {
                if (Politica[estado][a] > maxValue)
                {
                    acao = a; //max argument
                    maxValue = Politica[estado][a];
                }
            }
            return acao; //maxArg
        }

        //exploracao de novos estados
        public int Acao_exploracao(double[] estado)
        {
            if (!Politica.ContainsKey(estado)) //isto para casos terminais não existem mais acoes
            {
                return -1; //não existem acoes
            }

            int[] acoes = Politica[estado].Keys.ToArray();
            r.Shuffle(acoes); //suffle para ter uma acao aleatoria
            return acoes != null ? acoes[0] : -1; //random or -1 action
        }

        //executar a acao selecionada e retornar a recompensa e o estado resultante
        public (double recompensa, double[] estadoLinha) executar_acao(double[] estado, int acao)
        {
            var estadoLinha = ambiente.ExecutarAcao(estado, acao); // ver o novo estado
            var recompensa = ambiente.GetRecompensa(estadoLinha); // recompensa do novo estado
            return (recompensa, estadoLinha);
        }

        //guarda a politica e o ambiente associado em 2 ficheiros com nome "filename" e extensoes .sarsa e .ambiente
        public void SavePoliticaOnFile(string filename) {
            string caminhoPolitica = Path.Combine(
                AppContext.BaseDirectory,
                    filename + ".sarsa"
                );
            File.WriteAllText(caminhoPolitica, ""); //delete old text
            var stream = File.AppendText(caminhoPolitica); //stream Onde vamos escrever

            var estados = Politica.Keys;
            foreach (double[] estado in estados)
            {
                stream.Write("s:[" + string.Join(";", estado) + "]\n"); // string join, adiciona todos os dados do estado separados por virgulas
                foreach (var acao in Politica[estado])
                {
                    stream.Write("a:" + acao.Key + ";r:" + acao.Value + ";\n"); //acao e reward associados
                }
                stream.Write("\n");
            }
            stream.Close();

            //salva o ambiente em outro ficheiro
            string caminhoAmbiente = Path.Combine(
                AppContext.BaseDirectory,
                    filename + ".ambiente"
                );
            File.WriteAllText(caminhoAmbiente, ""); //clear text first
            var streamAmbiente = File.AppendText(caminhoAmbiente); //stream Onde vamos escrever
            ambiente.SaveAmbienteInStream(streamAmbiente);
            streamAmbiente.Close();
        }
    }
}
