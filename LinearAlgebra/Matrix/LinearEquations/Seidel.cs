using System;
using LinearAlgebra.Vector;

namespace LinearAlgebra.Matrix.LinearEquations
{
    public class Seidel
    {
        public Seidel(VectorFunction reducedVectorFunction, Vector.Vector guess) {
                VectorFunction = reducedVectorFunction;
                Guess = guess;
            }

            public VectorFunction VectorFunction { get; }

            public Vector.Vector Guess { get; set; }
            

            public Vector.Vector Solve() {
                var xk = Guess;
                var xkp = new Vector.Vector(Guess.Count);
                var countIteration = 0;
                do {
                    xkp.From(xk);
                    for (var i = 0; i < xk.Count; i++) {
                        xk[i] = VectorFunction.InvokeWithArgs(xk, i);
                    }

                    countIteration++;
                } while (Norma.TwoVectorNorm(xk - xkp) > 1e-3m);

                Console.WriteLine(
                    $"Номер итерации {countIteration}");
                return xk;
            }
        }
    }
