using System;

namespace crmweb.Common.Extensions
{
    public static class ArrayExtensions
    {
        public static void ForEach(this Array AArray, Action<Array, int[]> AAction)
        {
            if (AArray.LongLength == 0)
                return;
            ArrayTraverse vWalker = new ArrayTraverse(AArray);

            do
                AAction(AArray, vWalker.Position);
            while (vWalker.Step());
        }
    }

    internal class ArrayTraverse
    {
        public readonly int[] Position;
        private readonly int[] MaxLengths;

        public ArrayTraverse(Array AArray)
        {
            MaxLengths = new int[AArray.Rank];
            for (int i = 0; i < AArray.Rank; ++i)
            {
                MaxLengths[i] = AArray.GetLength(i) - 1;
            }
            Position = new int[AArray.Rank];
        }

        public bool Step()
        {
            for (int i = 0; i < Position.Length; ++i)
            {
                if (Position[i] < MaxLengths[i])
                {
                    Position[i]++;
                    for (int j = 0; j < i; j++)
                    {
                        Position[j] = 0;
                    }
                    return true;
                }
            }
            return false;
        }
    }
}
