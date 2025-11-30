using System;

namespace Assets.Source.Generators
{
    // In-place shuffle (exists in .NET 10, but Unity targets .NET Standard 2.1)
    public static class RandomExtensions
    {
        public static void Shuffle<T>(this Random rng, T[] array)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));
            for (int i = array.Length - 1; i > 0; i--)
            {
                int j = rng.Next(i + 1);
                (array[i], array[j]) = (array[j], array[i]);
            }
        }
    }
}
