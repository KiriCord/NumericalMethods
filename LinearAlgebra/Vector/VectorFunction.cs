using System;
using System.Linq;

namespace LinearAlgebra.Vector
{
    public class VectorFunction
    {
        private Func<Vector, decimal>[] _functions;
        private int _size;

        public VectorFunction(Func<Vector, decimal>[] functions) {
            _functions = functions;
            _size = functions.Length;
        }

        public decimal InvokeWithArgs(Vector xs, int number) =>
            !xs.IsVector || number < 0 || number > _size - 1
                ? throw new ArgumentException()
                : _functions[number](xs);

        public Vector InvokeWithArgs(Vector xs) => 
            new Vector(_size).From(_functions.Select(item => item(xs)));
    }
}