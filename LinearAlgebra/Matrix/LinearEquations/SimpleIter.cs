using System;
using LinearAlgebra.Vector;

namespace LinearAlgebra.Matrix.LinearEquations
{
    public class SimpleIter
    {
        public VectorFunction VectorFunction { get; }
        public Vector.Vector Guess { get; set; }

        public SimpleIter(VectorFunction vectorFunction, Vector.Vector guess) {
            VectorFunction = vectorFunction;
            Guess = guess;
        }
        
        public Vector.Vector Solve() {
            var xk = Guess;
            var xkp = new Vector.Vector(xk.Count);
            var countIteration = 0;
            do {
                xkp.From(xk);
                xk = VectorFunction.InvokeWithArgs(xk);
                countIteration++;
            } while (Norma.TwoVectorNorm(xk - xkp) > 1e-3m);

            Console.WriteLine(
                $"Номер итерации {countIteration}");
            return xk;
        }
        
        
    }
}