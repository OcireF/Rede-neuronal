using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SARSA
{
    internal class Pista : IAmbiente
    {
        public int[][] mapa;

        public Pista((int x,int y) sise) 
        {
            InitFloor(sise.x, sise.y);
        }

        public void InitFloor(int sizeX, int sizeY)
        {
            mapa = new int[sizeX][];
            for (int i = 0; i < mapa.Length; i++)
            {
                mapa[i] = new int[sizeY];
            }
        }

        public static IAmbiente LoadAmbienteFromFile(string fileName)
        {
            throw new NotImplementedException();
        }

        public List<int> AcoesPossiveis(double[] estado)
        {
            throw new NotImplementedException();
        }

        public double[] ExecutarAcao(double[] estado, int a)
        {
            throw new NotImplementedException();
        }

        public double[] GetInicialState()
        {
            throw new NotImplementedException();
        }

        public double GetRecompensa(double[] estado)
        {
            throw new NotImplementedException();
        }

        public bool IsTerminal(double[] estado)
        {
            throw new NotImplementedException();
        }

        public void SaveAmbienteInStream(StreamWriter fileStream)
        {
            throw new NotImplementedException();
        }
    }
}
