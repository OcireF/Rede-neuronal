using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rede_neuronal
{
    public class RedeNeuronal
    {
        public CamadaDensa[] camadaCalculos; //array de camadas de calculo (camadas densas)
        public int[] camada1; //camada de entrada (input layer)
        //construtor para criar a rede neuronal com pesos e pendores aleatorios
        public RedeNeuronal(int[] forma,FuncoaAtivacao f) {
            camadaCalculos = new CamadaDensa[forma.Length-1];   // O "-1" é porque a primeira camada são as entradas e não é de calculo
            camada1 = new int[forma[0]];                        // inicia a camada de entrada
            for(int i = 0; i < forma.Length-1; i++)             //cria todas as camadas -1 a "a camada de entrada"
            {
                camadaCalculos[i] = new CamadaDensa(forma[i], forma[i + 1], f); //inicia a camada (tamanhoEntrada,TamanhoSaida, funcaoAtivacao)
            }
        }
        //contrutor para ter pesos e pendores já pre defenidos
        public RedeNeuronal(int[] forma, FuncoaAtivacao f, double[][][] pesos, double[][] pendores)
        {
            camadaCalculos = new CamadaDensa[forma.Length - 1]; // O "-1" é porque a primeira camada são as entradas e não é de calculo
            camada1 = new int[forma[0]];                        // inicia a camada de entrada
            for (int i = 0; i < forma.Length - 1; i++)          //cria todas as camadas -1 a "a camada de entrada"
            {
                camadaCalculos[i] = new CamadaDensa(forma[i], forma[i + 1], f, pesos[i], pendores[i]); //inicia a camada (tamanhoEntrada,TamanhoSaida, funcaoAtivacao, pesos, pendores) 
            }
        }

        //se for preciso trocar os pesos e pendores de uma camada on the fly
        public void setPesosAndPendores(double[][][] pesos, double[][] pendore)
        {
            if(pesos.Length != camadaCalculos.Length || pendore.Length != camadaCalculos.Length)
            {
                Console.WriteLine("numero errado de pesos ou pendores (rede)");
                return;
            }

            for(int i =0; i < camadaCalculos.Length ; i++) //mudifica os pesos e pendores de cada camada
            {
                camadaCalculos[i].SetPesosAndPendores(pesos[i], pendore[i]);
            }
        }

        //calcula a saida da rede neuronal para um dado input
        public int[] calcRedeNeuronal(int[] input)
        {
            var entrada = input;//guarda a entrada, para mais tarde ser usada na aprendizagem
            int[] saida = {0};  //inicializa a saida para o caso de não ter camadas de calculo (Não é suposto)
            for (int i = 0; i < camadaCalculos.Length; i++) //corre todas as camadas de calculo
            {
                saida = camadaCalculos[i].CalcularSaidas(entrada); //calcula a saida da camada atual
                entrada = saida; //propaga a saida da camada atual para a entrada da proxima camada
            }

            return saida;
        }
    }
}
