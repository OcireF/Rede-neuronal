using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rede_neuronal.Interfaces
{
    //interface das Camadas de rede neuronal
    public interface ICamada
    {
        public void SetPesosAndPendores(double[][] pesos, double[] pendore);
        public void AdaptarCamada(double[] errosSaida, double[] entradas, double taxaAprendizagem, double fatorMomento);
        public double[] CalcularSaidas(double[] entradas);
        public double[] GetSaidas();
        public Neuronio[] GetNeuronios();
        public double[][] GetPesos();
        public double[] GetPendores();
    }
}
