using System;

namespace LinearAlgebra.Matrix.LinearEquations
{
    public class SquareRoot
    {
        public Matrix Coefficient { get; set; }
        public Vector.Vector Answers { get; set; }
        
        public SquareRoot(Matrix coeff, Vector.Vector answers)
        {
            Coefficient = coeff;
            Answers = answers;
        }
        

        public Vector.Vector Solve()
        {
            var l = FindLDecomposition();
            var result = GaussElimination.Forward(Matrix.Concat(l, Answers));
            return GaussElimination.Backward(Matrix.Concat(l.Transponse, result.GetLastColumn())).GetLastColumn();
        }
        
        private Matrix FindLDecomposition() {
            var n = Coefficient.RowCount;
            var l = new Matrix(n, n);
            for (var i = 0; i < n; i++) {
                decimal sumLs = 0;
                for (var k = 0; k < i; k++)
                {
                    sumLs += l[i, k] * l[i, k];
                }

                l[i, i] = (decimal)Math.Sqrt((double)(Coefficient[i, i] - sumLs));

                for (var j = i; j < n; j++) {
                    decimal suml = 0;
                    for (int k = 0; k < i; k++)
                    {
                        suml += l[i, k] * l[j, k];
                    }
                    
                    l[j, i] = (Coefficient[j, i] - suml) / l[i, i];
                }
            }

            return l;
        }
    }
}