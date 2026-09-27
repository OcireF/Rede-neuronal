using Rede_neuronal.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rede_neuronal
{
    public class RedeNeuronal
    {
        public ICamada[] camadas; //array de camadas de calculo (camadas densas)
        //public int[] camada1; //camada de entrada (input layer)
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

        public double[] delta_saida(double[] saida, double[] saidaEsperada)
        {
            double[] erros = new double[saida.Length];
            for (int i = 0; i < erros.Length; i++)
            {
                erros[i] = saidaEsperada[i] - saida[i]; //calcula o erro da camada de saida
            }
            return erros;
        }
        
        public void RetroPropagar(double[] erro, double taxaAprendizagem)
        {
            var erroSaida = erro; //guarda o erro da camada atual
            for (int Cam = camadas.Length - 1; Cam >= 1; Cam--) //retroPropagar em todas as camadas menos a ultima (camada 1)
            {
                var saidaAnterior = camadas[Cam - 1].GetSaidas();   //pega a saida da camada anterior "y^(n-1)"
                var dimensaoCamadaAnterior = saidaAnterior.Length;  //dimensao da camada anterior "d^(n-1)"

                var dimensaoCamadaAtual = camadas[Cam].GetSaidas().Length; //dimensao da camada atual (d^n)
                var neuroniosCamadaAtual = camadas[Cam].GetNeuronios();    //pega os neuronios da camada atual (neuro^n)

                var erroSaidaAnterior = new double[dimensaoCamadaAnterior]; //inicializa o array de erros (n - 1)

                /*for (int neuro = 0; neuro < dimensaoCamadaAtual; neuro++)
                {
                    for(int w = 0; w < dimensaoCamadaAnterior; w++)
                    {
                        for (int e = 0; e < erroSaida.Length; e++)
                        {
                            erroSaidaAnterior[neuro] += neuroniosCamadaAtual[neuro].pesos[w] * erroSaida[e] * neuroniosCamadaAtual[neuro].dirivada;
                        }
                    }
                }*/

                for(int e = 0; e < dimensaoCamadaAnterior; e++) //calcular todos os erros do array
                {
                    erroSaidaAnterior[e] = 0;
                    for(int neuro = 0; neuro < dimensaoCamadaAtual; neuro++) // neuro = j dos slides
                    {
                        for (int w = 0; w < dimensaoCamadaAnterior; w++) // w = i dos slides
                        {
                            erroSaidaAnterior[e] += neuroniosCamadaAtual[neuro].pesos[w] * erroSaida[neuro] * neuroniosCamadaAtual[neuro].dirivada;
                        }
                    }
                }

                camadas[Cam].AdaptarCamada(erroSaida, saidaAnterior, taxaAprendizagem);
                erroSaida = erroSaidaAnterior;
            }
        }

        public double Adaptar(double[] entradaTreino , double[] saidaEsperada , double taxaAprendizagem)
        {
            var saidaDaRede = calcRedeNeuronal(entradaTreino);
            var variacaoErroSaida = delta_saida(saidaDaRede, saidaEsperada);
            RetroPropagar(variacaoErroSaida, taxaAprendizagem);

            int dimensaoVetorPerda = variacaoErroSaida.Length; // Dimensão do vector de perda (K)
            double erroMedio = 0;
            for (int k=0; k < dimensaoVetorPerda; k++) //Soma primeiro antes de fazer 1/K
            {
                erroMedio += Math.Pow(variacaoErroSaida[k], 2) / dimensaoVetorPerda;
            }

            return erroMedio;
        }

        public bool Treinar(double[][] entradas, double[][] saidasEsperadas, int numeroEpocas, double erroMaximo, double taxaAprendizagem)
        {
            for(int e = 0; e < numeroEpocas; e++)
            {
                double erro = 0;
                
                for(int i = 0; i < entradas.Length; i++) // loop até ao final das entradas (nota as saidasEsperadas devem ter o mesmo tamanho)
                {
                    var erroTreino = Adaptar(entradas[i], saidasEsperadas[i], taxaAprendizagem);
                    erro = Math.Max(erro, erroTreino);
                    if (erro <= erroMaximo)
                    {
                        return true;
                    }
                }
            }
            return false;
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
    }
}
