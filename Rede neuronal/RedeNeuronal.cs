using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rede_neuronal
{
    public class RedeNeuronal
    {
        public CamadaDensa[] camadaCalculos;
        //public int[][] transito; // entradas e saidas.
        public RedeNeuronal(int[] forma,FuncoaAtivacao f) {
            camadaCalculos = new CamadaDensa[forma.Length-1];// O "-1" é porque a primeira camada não é de calculo
            //transito = new int[forma.Length][]; //entradas e saidas
            //transito[0] = new int[forma[0]]; // tamanho da Camada de entrada.
            for(int i = 0; i < forma.Length-1; i++) //cria todas as camadas -1 a "a camada de entrada"
            {
                //transito[i+1] = new int[forma[i+1]]; //criar as saidas desta camada.
                camadaCalculos[i] = new CamadaDensa(forma[i], forma[i + 1], f); //inicia a camada 
            }
        }
        public RedeNeuronal(int[] forma, FuncoaAtivacao f, double[][][] pesos, double[][] pendores)
        {
            camadaCalculos = new CamadaDensa[forma.Length - 1];// O "-1" é porque a primeira camada não é de calculo
            //transito = new int[forma.Length][]; //entradas e saidas
            //transito[0] = new int[forma[0]]; // tamanho da Camada de entrada.
            for (int i = 0; i < forma.Length - 1; i++) //cria todas as camadas -1 a "a camada de entrada"
            {
                //transito[i + 1] = new int[forma[i + 1]]; //criar as saidas desta camada.
                camadaCalculos[i] = new CamadaDensa(forma[i], forma[i + 1], f, pesos[i], pendores[i]); //inicia a camada 
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

            for(int i =0; i < camadaCalculos.Length ; i++)
            {
                camadaCalculos[i].SetPesosAndPendores(pesos[i], pendore[i]);
            }
        }

        public int[] calcRedeNeuronal(int[] input)
        {
            var entrada = input;
            int[] saida = {0}; //inicializa a saida para o caso de não ter camadas de calculo (Não é suposto)
            for (int i = 0; i < camadaCalculos.Length; i++)
            {
                saida = camadaCalculos[i].CalcularSaidas(entrada);
                entrada = saida;
            }

            return saida;
        }
    }
}
