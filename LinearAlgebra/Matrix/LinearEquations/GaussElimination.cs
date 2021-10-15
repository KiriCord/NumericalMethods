namespace LinearAlgebra.Matrix.LinearEquations
{
    public static class GaussElimination
    {
        public static Vector.Vector Solve(Matrix matrix, Vector.Vector vec)
        {
            Matrix _matrix = matrix.AddColumn(vec);
            Matrix forwaredMatrix = Forward(_matrix);
            Matrix backwaredMatrix = Backward(forwaredMatrix);
            _matrix = backwaredMatrix;

            Vector.Vector X = new Vector.Vector(_matrix.RowCount);
            for (int i = 0; i < X.Count; i++)
            {
                X[i] = _matrix[i, _matrix.RowCount];
            }

            return X;
        }
        
        public static Matrix Forward(Matrix matrixF)
        {
            Matrix copyMatrixF = new Matrix(matrixF);
            for (int k = 0; k < matrixF.RowCount; k++) 
            {
                for (int i = 0; i < matrixF.ColumnCount; i++) 
                    copyMatrixF[k, i] /= matrixF[k, k]; 
                for (int i = k + 1; i < matrixF.RowCount; i++) 
                {
                    decimal K = copyMatrixF[i, k] / copyMatrixF[k, k];
                    for (int j = 0; j < matrixF.ColumnCount; j++)
                    {
                        copyMatrixF[i, j] -= copyMatrixF[k, j] * K;
                    }
                }
            }
            return copyMatrixF;
        }
        
         public static Matrix Backward(Matrix matrixF)
        {
            Matrix copyMatrixF = new Matrix(matrixF);
            for (int k = matrixF.RowCount - 1; k > -1; k--)
            {
                for (int i = matrixF.ColumnCount - 1; i > -1; i--)
                    copyMatrixF[k, i] /= matrixF[k, k];
                for (int i = k - 1; i > -1; i--)
                {
                    decimal K = copyMatrixF[i, k] / copyMatrixF[k, k];
                    for (int j = matrixF.ColumnCount - 1; j > -1; j--)
                    {
                        copyMatrixF[i, j] -= copyMatrixF[k, j] * K;
                    }
                }
            }
            return copyMatrixF;
        }
         
         public static Matrix Check(Matrix matrix, Matrix invMatrix)
         {
             return new Matrix(matrix * invMatrix);
         }
    }
}