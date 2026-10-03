using Rede_neuronal.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rede_neuronal
{
    public class RedeNeuronal
    {
        public ICamada[] camadas; //array de camadas de calculo (input layer e camadas densas
        public string traningErrorsFileName; //file onde guarda os valores dos erros
        public bool writeErrors = false; //bool para saber se vamos escrever os erros medios e pendores num .csv
        private bool firstLine = true; //bool para saber se é a primeira linha da escrita

        //Construtor para guardar os updates do erros, num ficheiro especificado
        public RedeNeuronal(int[] forma, IFuncoaAtivacao f, string traningErrorsFileName) : this(forma, f)
        {
            if (traningErrorsFileName != null)
            {
                writeErrors = true;
                this.traningErrorsFileName = traningErrorsFileName;

                //apaga o ficheio e o recria vasio
                string caminho = Path.Combine(
                    AppContext.BaseDirectory,
                    traningErrorsFileName + ".csv"
                );
                File.WriteAllText(caminho, "");
            }
        }

        //construtor para criar a rede neuronal com pesos e pendores aleatorios
        public RedeNeuronal(int[] forma,IFuncoaAtivacao f) {
            camadas = new ICamada[forma.Length];   // inicia o Array de todas as camadas
            camadas[0] = new CamadaEntrada();      // inicia a camada de entrada
            for (int i = 1; i < forma.Length; i++)             //cria todas as camadas Densas
            {
                camadas[i] = new CamadaDensa(forma[i-1], forma[i], f); //inicia a camada (tamanhoEntrada,TamanhoSaida, funcaoAtivacao)
            }
        }
        //contrutor para ter pesos e pendores já pre defenidos
        public RedeNeuronal(int[] forma, IFuncoaAtivacao f, double[][][] pesos, double[][] pendores)
        {
            camadas = new ICamada[forma.Length]; // inicia o Array de todas as camadas
            camadas[0] = new CamadaEntrada();                        // inicia a camada de entrada
            for (int i = 1; i < forma.Length; i++)          //cria todas as camadas Densas
            {
                camadas[i] = new CamadaDensa(forma[i-1], forma[i], f, pesos[i-1], pendores[i-1]); //inicia a camada (tamanhoEntrada,TamanhoSaida, funcaoAtivacao, pesos, pendores) 
            }
        }

        //se for preciso trocar os pesos e pendores de uma camada on the fly
        public void setPesosAndPendores(double[][][] pesos, double[][] pendore)
        {
            if(pesos.Length != camadas.Length || pendore.Length != camadas.Length)
            {
                Console.WriteLine("numero errado de pesos ou pendores (rede)");
                return;
            }

            for(int i =0; i < camadas.Length ; i++) //mudifica os pesos e pendores de cada camada
            {
                camadas[i].SetPesosAndPendores(pesos[i], pendore[i]);
            }
        }

        //Calculo do array de erros da saida
        public double[] delta_saida(double[] saida, double[] saidaEsperada)
        {
            double[] erros = new double[saida.Length];
            for (int i = 0; i < erros.Length; i++)
            {
                erros[i] = saida[i] - saidaEsperada[i]; //calcula o erro da camada de saida
            }
            return erros;
        }
        
        //Algoritmo de RetroPropagação
        public void RetroPropagar(double[] erro, double taxaAprendizagem)
        {
            var erroSaida = erro; //guarda o erro da saida
            for (int Cam = camadas.Length - 1; Cam >= 1; Cam--) //retroPropagar em todas as camadas menos a ultima (camada 1)
            {
                var saidaAnterior = camadas[Cam - 1].GetSaidas();   //pega a saida da camada anterior "y^(n-1)"
                var dimensaoCamadaAnterior = saidaAnterior.Length;  //dimensao da camada anterior "d^(n-1)"

                var dimensaoCamadaAtual = camadas[Cam].GetSaidas().Length; //dimensao da camada atual (d^n)
                var neuroniosCamadaAtual = camadas[Cam].GetNeuronios();    //pega os neuronios da camada atual (neuro^n)

                double[] erroSaidaAnterior = new double[dimensaoCamadaAnterior]; //inicializa o array de erros^(n - 1)

                for(int i = 0; i < dimensaoCamadaAnterior; i++) //e = i dos slides
                {
                    erroSaidaAnterior[i] = 0; //start value at 0
                    for(int neuro = 0; neuro < dimensaoCamadaAtual; neuro++) // neuro = j dos slides
                    {
                        erroSaidaAnterior[i] += neuroniosCamadaAtual[neuro].pesos[i] * erroSaida[neuro] * neuroniosCamadaAtual[neuro].derivada;
                    }
                }

                camadas[Cam].AdaptarCamada(erroSaida, saidaAnterior, taxaAprendizagem); //adptar os pesos e pendores da camada

                erroSaida = erroSaidaAnterior; // no proximo loop o erro da saida Anterior, é o novo erro de saida
            }
        }

        public double Adaptar(double[] entradaTreino , double[] saidaEsperada , double taxaAprendizagem)
        {
            var saidaDaRede = calcRedeNeuronal(entradaTreino); //cacular a saida da rede
            var variacaoErroSaida = delta_saida(saidaDaRede, saidaEsperada); //calcular o erro de saida da rede
            RetroPropagar(variacaoErroSaida, taxaAprendizagem); //RetroPropagar o erro para recalcular os pesos e pendores da rede toda

            int dimensaoVetorPerda = variacaoErroSaida.Length; // Dimensão do vector de perda (K)
            double erro = 0;
            for (int k=0; k < dimensaoVetorPerda; k++) //Soma primeiro antes de fazer 1/K
            {
                erro += (variacaoErroSaida[k] * variacaoErroSaida[k]); //soma do quadrado de todos os erros de saida
            }

            var erroMedio = erro / dimensaoVetorPerda; // erroMedio = erro/K

            //Codigo de escrever num ficheiro csv o grafico de erro medio, pesos e pendores
            if (writeErrors)
            {
                string caminho = Path.Combine(AppContext.BaseDirectory, traningErrorsFileName + ".csv");
                double[][][] pesos = GetPesos();
                double[][] pendores = GetPendores();

                if (firstLine) //se for a primeira linha escrevemos as labels
                {
                    string labels = "ErroMedio;";
                    for (int i = 0; i < pesos.Length; i++)
                        for (int j = 0; j < pesos[i].Length; j++)
                            for (int k = 0; k < pesos[i][j].Length; k++)
                                labels += "peso["+i+"]["+j+"]["+k+"];";
                    for (int i = 0; i < pendores.Length; i++)
                        for (int j = 0; j < pendores[i].Length; j++)
                            labels += "pendor["+i+"]["+j+"];";
                    firstLine = false;

                    try
                    {
                        File.AppendAllText(caminho, labels + Environment.NewLine);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Erro ao escrever: " + ex.Message);
                    }
                }
                //escrita dos dados no .csv
                string linha = erroMedio.ToString(System.Globalization.CultureInfo.CurrentUICulture) + ";"; // erro;
                for (int i = 0; i < pesos.Length; i++)
                    for (int j = 0; j < pesos[i].Length; j++)
                        for (int k = 0; k < pesos[i][j].Length; k++)
                            linha += pesos[i][j][k].ToString(CultureInfo.CurrentUICulture) + ";";
                for (int i = 0; i < pendores.Length; i++)
                    for (int j = 0; j < pendores[i].Length; j++)
                        linha += pendores[i][j].ToString(CultureInfo.CurrentUICulture) + ";";

                try
                {
                    File.AppendAllText(caminho, linha + Environment.NewLine);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Erro ao escrever: " + ex.Message);
                }
            }

            return erroMedio;
        }

        //treino da rede
        public bool Treinar(double[][] entradas, double[][] saidasEsperadas, int numeroEpocas, double erroMaximo, double taxaAprendizagem)
        {
            for(int e = 0; e < numeroEpocas; e++) // fazer numeroEpocas treinos
            {
                double erro = 0; //erro começa a 0 até provas do contrario
                
                for(int i = 0; i < entradas.Length; i++) // loop até ao final das entradas (nota as saidasEsperadas devem ter o mesmo tamanho)
                {
                    var erroTreino = Adaptar(entradas[i], saidasEsperadas[i], taxaAprendizagem); //ada+tar a rede e capturar o erro do treino
                    erro = Math.Max(erro, erroTreino); //guarda o erro maior
                    
                    if (erro <= erroMaximo) //Caso o erro da epoca for menor que o erroMaximo retorna True, o treino foi um sucesso :)
                    {
                        return true;
                    }
                }
            }
            return false; //Caso acabar todas a epocas e o erro for maior que o "erroMximo" returnamos o treino como um não sucesso :(
        }


        //calcula a saida da rede neuronal para um dado input
        public double[] calcRedeNeuronal(double[] input)
        {
            var entrada = input;//guarda a entrada, para mais tarde ser usada na aprendizagem
            double[] saida = {0};  //inicializa a saida para o caso de não ter camadas de calculo (Não é suposto)
            for (int i = 0; i < camadas.Length; i++) //corre todas as camadas de calculo
            {
                saida = camadas[i].CalcularSaidas(entrada); //calcula a saida da camada atual
                entrada = saida; //propaga a saida da camada atual para a entrada da proxima camada
            }

            return saida;
        }

        //Codigo para correr a rede multiplas veses e calcular todas as suas saidas
        public double[][] Prever(double[][] input)
        {
            var saidas = new double[input.Length][];
            for (int i = 0; i < input.Length; i++) //corre todos os inputs e guarda todos os outputs
            {
                saidas[i] = calcRedeNeuronal(input[i]);
            }
            return saidas;
        }

        //Funções para capturar os Pesos e Pendores da rede Atual
        public Double[][][] GetPesos()
        {
            var pesos = new double[camadas.Length - 1][][];
            for (int i = 0; i + 1 < camadas.Length ; i++) //+ 1 porque a camada 1 não tem pesos 
                pesos[i] = camadas[i+1].GetPesos();
            return pesos;
        }
        public Double[][] GetPendores()
        {
            var pendores = new double[camadas.Length - 1][];
            for (int i = 0; i + 1 < camadas.Length; i++) //+ 1 porque a camada 1 não tem pesos 
                pendores[i] = camadas[i + 1].GetPendores();
            return pendores;
        }

    }
}
