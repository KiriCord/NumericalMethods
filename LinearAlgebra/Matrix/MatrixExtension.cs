using System;
using System.Collections.Generic;
using System.Linq;

namespace LinearAlgebra.Matrix
{
    public static class MatrixExtension
    {
        public static Matrix ToMatrix(this Vector.Vector[] vectors)
        {
            var firstVectorN = vectors[0].Count;
            if (vectors.Skip(1).Any(vector => vector.Count != firstVectorN))
            {
                throw new Exception("БЛИН");
            }

            var result = new Matrix(firstVectorN, vectors.Length);

            for (int i = 0; i < vectors.Length; i++)
            {
                for (int j = 0; j < firstVectorN; j++)
                {
                    result[j, i] = vectors[i][j];
                }
            }

            return result;
        }

        public static Matrix ToTriangle(this Matrix matrix)
        {
            Matrix result = new Matrix(matrix);
            for (int i = 0; i < result.RowCount - 1; i++)
            {
                for (int j = i + 1; j < result.ColumnCount; j++)
                {
                    decimal koef = result[j, i] / result[i, i];
                    for (int k = i; k < result.RowCount; k++)
                        result[j, k] -= result[i, k] * koef;
                }
            }

            return result;
        }
    }
}