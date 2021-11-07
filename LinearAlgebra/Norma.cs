using System;
using System.Collections.Generic;
using System.Linq;

namespace LinearAlgebra
{
    public class Norma
    {
        public static decimal TwoVectorNorm(Vector.Vector vector)
        {
            return (decimal)Math.Sqrt((double)vector.Sum(item => item * item));
        }

        public static decimal InfMatrixNorm(Matrix.Matrix matrix)
        {
            IEnumerable<decimal> RowNorm(Matrix.Matrix rows) {
                for (var i = 0; i < rows.RowCount; i++)
                    yield return rows.GetRow(i).Sum(Math.Abs);
            }

            return RowNorm(matrix).Max();
        }
    }
}