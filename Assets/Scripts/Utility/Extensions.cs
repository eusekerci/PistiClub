using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PistiClub
{
    public static class Extensions
    {
        private static System.Random rng = new System.Random();

        //Fisher-Yates Shuffle
        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}
