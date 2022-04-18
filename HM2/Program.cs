using HM2.GameSolve.Structures;
using System;

namespace HM2
{
    class Program
    {
        static void Main(string[] args)
        {
            Vector vect = new Vector();
            vect.VelosityVectNow = new VelosityVect { Angular = 30, Velosity = 100 };

            vect.VelosityVectModifer = new VelosityVect { Angular = 0, Velosity = 200 };

            vect.ModifVelosityVect();
        }
    }
}
