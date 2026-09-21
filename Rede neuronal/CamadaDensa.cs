using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rede_neuronal
{
    public class CamadaDensa
    {

        private int[] saidas;
        private Neuronio[] neuronios;
        public CamadaDensa(int dimensaoEntrada, int dimensaoSaida, FuncoaAtivacao ativador) {
            saidas = new int[dimensaoSaida]; //inicializa a variavel que guarda todas as saidas da camada
            neuronios = new Neuronio[dimensaoSaida]; // inicia a varia que guarda todos os neuronios
            for (int neuro = 0; neuro < dimensaoSaida; neuro++)
            {
                neuronios[neuro] = new Neuronio(dimensaoEntrada, ativador);
            }
        }

        //criar a camada com pesos e pendores Pre-definidos
        public CamadaDensa(int dimensaoEntrada, int dimensaoSaida, FuncoaAtivacao ativador, double[][] pesos, double[] pendores)
        {
            saidas = new int[dimensaoSaida]; //inicializa a variavel que guarda todas as saidas da camada
            neuronios = new Neuronio[dimensaoSaida]; // inicia a varia que guarda todos os neuronios
            for (int neuro = 0; neuro < dimensaoSaida; neuro++)
            {
                neuronios[neuro] = new Neuronio(pesos[neuro], pendores[neuro], ativador);
            }
        }

        //se for preciso trocar os pesos e pendores de uma camada on the fly
        public void SetPesosAndPendores(double[][] pesos, double[] pendore)
        {
            for (int neuro = 0; neuro < neuronios.Length; neuro++)
            {
                neuronios[neuro].pesos = pesos[neuro];
                neuronios[neuro].pendore = pendore[neuro];
            }
        }

        //calcula as saidas da camada
        public int[] CalcularSaidas(int[] entradas)
        {
            for(int neuro = 0; neuro < neuronios.Length; neuro++) // corre todos os neuronios da camada
            {
                saidas[neuro] = neuronios[neuro].RunNeuronio(entradas); // escreve cada resutado numa saida
            }
            return saidas;
        }
    }
}
