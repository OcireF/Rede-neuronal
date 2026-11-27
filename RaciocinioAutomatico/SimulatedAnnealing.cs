using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaciocinioAutomatico
{
    public class SimulatedAnnealing : IAlgoritmoHill
    {
        private double[] estadoAtual;
        private double valorEstadoAtual;
        private bool completed = false;
        private IProblema problema;
        private double valorAlvo;
        private double temperaturaInicial;
        private double temperaturaAtual;
        private Func<double, double> Arrefecedor;
        private Random r = new Random();
        public int passos = 0;

        public SimulatedAnnealing(IProblema problema, double valorAlvo, double temperaturaInicial, Func<double, double> Arrefecedor) 
        {
            this.problema = problema;
            this.valorAlvo = valorAlvo;
            this.temperaturaInicial = temperaturaInicial;
            this.Arrefecedor = Arrefecedor;

            estadoAtual = problema.estado_inicial();        // estado inicial
            valorEstadoAtual = problema.avaliar(estadoAtual); // valor do estado
            temperaturaAtual = temperaturaInicial;          //temperatura inicial
        }

        public void FaserPasso()
        {
            passos++;
            if (completed) //o algoritmo ja completou
            {
                return;
            }

            temperaturaAtual = Arrefecedor.Invoke(temperaturaAtual); //arrefe a temperatura atual

            if(temperaturaAtual <= 1) //congelou
            {
                completed = true;
                PrintResults();
                return;
            }

            List<double[]> sucessores = problema.sucessores(estadoAtual); //sucessores = problema.sucessores(estado)
            double[] sucessor = RandomChoice(sucessores);                 //sucessor escolhido randomicamente
            double valorSucessor = problema.avaliar(sucessor);            //valor do sucessor

            if (valorSucessor >= valorAlvo) //encontrou o valorAlvo
            {
                estadoAtual = sucessor;
                valorEstadoAtual = valorSucessor;
                completed = true;
                PrintResults();
                return;
            }

            double varicaoEnergia = valorSucessor - valorEstadoAtual;

            //varicao positiva ou quanto maior a temperatura mais provavel de escolher a escolha aleatoria
            if (varicaoEnergia > 0 || r.NextDouble() < Math.Exp(varicaoEnergia / temperaturaAtual)) 
            {   //update o estado atual
                estadoAtual = sucessor;
                valorEstadoAtual = valorSucessor;
            }
        }

        public double[] RandomChoice(List<double[]> sucessores)
        {
            return sucessores[r.Next(0, sucessores.Count)];
        }

        public double[] GetEstado()
        {
            return estadoAtual;
        }

        public void Restart()
        {
            temperaturaAtual = temperaturaInicial;
            completed = false;
            estadoAtual = problema.estado_inicial();     // estado inicial
            valorEstadoAtual = problema.avaliar(estadoAtual); // valor do estado
            passos = 0;
        }

        public void PrintResults()
        {
            Console.WriteLine(
                "\nAlgoritmo : Simulated Annealing" + 
                "\n Passos feitos = " + passos + 
                "\n Temperatura = " + temperaturaAtual +
                "\n Valor do estado = " + valorEstadoAtual+
                "\n Estado = [" + string.Join(", ", estadoAtual) + "]");
        }
    }
}
