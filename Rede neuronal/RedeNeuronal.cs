using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rede_neuronal
{
    public class RedeNeuronal
    {
        public CamadaCalculo[] camadaCalculos;
        public int[][] transito; // entradas e saidas.
        //public int[][] saidas;
        public RedeNeuronal(int[] forma,FuncoaAtivacao f) {
            camadaCalculos = new CamadaCalculo[forma.Length-1];// O "-1" é porque a primeira camada não é de calculo
            transito = new int[forma.Length][]; //entradas e saidas
            transito[0] = new int[forma[0]]; // tamanho da Camada de entrada.
            for(int i = 0; i < forma.Length-1; i++) //cria todas as camadas -1 a "a camada de entrada"
            {
                transito[i+1] = new int[forma[i+1]]; //criar as saidas desta camada.
                camadaCalculos[i] = new CamadaCalculo(transito[i], forma[i + 1]); //inicia a camada 
            }
        }
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
            transito[0] = input;
            for (int i = 0; i < camadaCalculos.Length; i++)
            {
                transito[i+1] = camadaCalculos[i].CalcularSaidas(transito[i]);
            }

            return transito[transito.Length-1];
        }
    }
}
