using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rede_neuronal
{
    public class CamadaCalculo
    {
        //private int[] entradas;
        private int[] saidas;
        private double[][] pesos;
        private double[] pendore;
        private Neuronio[] neuronios;
        public CamadaCalculo(int[] x, int numeroNeuronios) {
            //entradas = x;
            saidas = new int[numeroNeuronios]; //o tamanho da saida é o mesmo do numero de neuronios
            InitNeuronios(numeroNeuronios); //inicializa os neuronios
            pendore = new double[numeroNeuronios]; //o numero de pendores é o mesmo de neuronios
            pesos = InitPesosAndPendores(x.Length, numeroNeuronios);//inicializa os pesos e pendores com valores random 
            neuronios = new Neuronio[numeroNeuronios]; // inicia a varia que guarda todos os neuronios
        }
        //inicia os Pesos e pendores com valores "random"
        private double[][] InitPesosAndPendores(int numeroEntradas, int numeroNeuronios)
        {
            var novosPesos = new double[numeroNeuronios][]; //inializa a variavel [numeroNeuronios][numeroEntradas]
            var rand = new Random();
            for(int neuro=0; neuro < numeroNeuronios; neuro++) //para todos os neuronios
            {
                novosPesos[neuro] = new double[numeroEntradas]; //criar o array de pesos de 1 neuronio
                pendore[neuro] = rand.NextDouble() * 2 - 1; //random pendore (-1 a 1) nota: o -1 é para acertar o (o a 2)
                for (int entrada=0; entrada < numeroEntradas; entrada++) //inicializar todas as entradas
                {
                    novosPesos[neuro][entrada] = rand.NextDouble()*2 - 1; //random peso (-1 a 1) nota: o -1 é para acertar o (o a 2)
                }
            }
            return novosPesos;
        }
        //inicia os neuronios com a funcao de ativação
        private void InitNeuronios( int numeroNeuronios) 
        { 
            for (int neuro=0;neuro < numeroNeuronios; neuro++)
            {
                neuronios[neuro] = new Neuronio( new FuncoaAtivacao());
            }
        }
        //troca os pesos e pendores por novos
        public void SetPesosAndPendores(double[][] pesos, double[] pendore)
        {
            this.pesos = pesos;
            this.pendore = pendore;
        }

        //calcula as saidas da camada
        public int[] CalcularSaidas(int[] entradas)
        {
            for(int neuro = 0; neuro < neuronios.Length; neuro++) // corre todos os neuronios da camada
            {
                saidas[neuro] = neuronios[neuro].RunNeuronio(entradas, pesos[neuro], pendore[neuro]); // escreve cada resutado numa saida
            }
            return saidas;
        }
    }
}
