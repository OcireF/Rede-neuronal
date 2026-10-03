using Rede_neuronal.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rede_neuronal
{
    public class CamadaDensa : ICamada
    {

        private double[] saidas;
        private Neuronio[] neuronios;

        //criar a camada com pesos e pendores aleatorios
        public CamadaDensa(int dimensaoEntrada, int dimensaoSaida, IFuncoaAtivacao ativador) {
            saidas = new double[dimensaoSaida]; //inicializa a variavel que guarda todas as saidas da camada
            neuronios = new Neuronio[dimensaoSaida]; // inicia a varia que guarda todos os neuronios
            for (int neuro = 0; neuro < dimensaoSaida; neuro++)
            {
                neuronios[neuro] = new Neuronio(dimensaoEntrada, ativador);
            }
        }

        //criar a camada com pesos e pendores Pre-definidos
        public CamadaDensa(int dimensaoEntrada, int dimensaoSaida, IFuncoaAtivacao ativador, double[][] pesos, double[] pendores)
        {
            saidas = new double[dimensaoSaida]; //inicializa a variavel que guarda todas as saidas da camada
            neuronios = new Neuronio[dimensaoSaida]; // inicia a varia que guarda todos os neuronios
            for (int neuro = 0; neuro < dimensaoSaida; neuro++)
            {
                neuronios[neuro] = new Neuronio(pesos[neuro], pendores[neuro], ativador);
            }
        }

        //se for preciso trocar os pesos e pendores de uma camada on the fly temos esta função
        public void SetPesosAndPendores(double[][] pesos, double[] pendore)
        {
            for (int neuro = 0; neuro < neuronios.Length; neuro++)
            {
                neuronios[neuro].pesos = pesos[neuro];
                neuronios[neuro].pendore = pendore[neuro];
            }
        }

        //adaptar os pesos e pendores com base nos erros da camada e nas entradas da camada anterior
        public void AdaptarCamada(double[] errosSaida, double[] entradas, double taxaAprendizagem)
        {
            for (int neuro = 0; neuro < neuronios.Length; neuro++)
            {
                neuronios[neuro].Adaptar(errosSaida[neuro], entradas, taxaAprendizagem);
            }
        }

        //calcula as saidas da camada
        public double[] CalcularSaidas(double[] entradas)
        {
            for(int neuro = 0; neuro < neuronios.Length; neuro++) // corre todos os neuronios da camada
            {
                saidas[neuro] = neuronios[neuro].RunNeuronio(entradas); // escreve cada resutado numa saida
            }
            return saidas;
        }

        //Funções para capturar dados da rede
        public double[] GetSaidas()
        {
            return saidas;
        }
        public Neuronio[] GetNeuronios()
        {
            return neuronios;
        }
        public double[][] GetPesos()
        {
            var pesos = new double[neuronios.Length][];
            for (int neuro = 0; neuro < neuronios.Length; neuro++)
                pesos[neuro] = neuronios[neuro].pesos;
            return pesos;
        }

        public double[] GetPendores()
        {
            var pendores = new double[neuronios.Length];
            for (int neuro = 0; neuro < neuronios.Length; neuro++)
                pendores[neuro] = neuronios[neuro].pendore;
            return pendores;
        }
    }
}
