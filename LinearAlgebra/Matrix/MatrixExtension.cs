using System;
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
    }
}