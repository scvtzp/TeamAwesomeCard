using System;
using System.Collections.Generic;
using System.Linq;

namespace DefaultNamespace
{
    public static class GameUtil
    {
        private static readonly Random Random = new Random();

        public static TValue GetRandomValue<TKey, TValue>(this Dictionary<TKey, TValue> dictionary)
        {
            if (dictionary == null || dictionary.Count == 0)
                throw new InvalidOperationException("딕셔너리가 비어 있습니다.");

            int index = Random.Next(dictionary.Count);
            return dictionary.ElementAt(index).Value;
        }
    }
}