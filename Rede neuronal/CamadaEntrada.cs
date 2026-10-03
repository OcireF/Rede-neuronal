using Rede_neuronal.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rede_neuronal
{
    //Camada de entrada
    public class CamadaEntrada : ICamada
    {
        private double[] saidas;

        public void SetPesosAndPendores(double[][] pesos, double[] pendore) { }
        public void AdaptarCamada(double[] errosSaida, double[] entradas, double taxaAprendizagem) { }
        public double[] CalcularSaidas(double[] entradas) 
        {
            saidas = entradas;
            return saidas;
        }
        public double[] GetSaidas()
        {
            return saidas;
        }
        public Neuronio[] GetNeuronios() { return null; }
        public double[][] GetPesos() { return null; }
        public double[] GetPendores() { return null; }
    }
}
