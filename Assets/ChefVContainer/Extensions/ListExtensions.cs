using System.Collections.Generic;
using System.Linq;

namespace gs.chef.vcontainer.extensions
{
    public static class ListExtensions
    {
        public static void Shuffle<T>(this IList<T> list)
        {
            var n = list.Count;
            while (n > 1)
            {
                n--;
                var k = UnityEngine.Random.Range(0, n);
                (list[k], list[n]) = (list[n], list[k]);
                // T value = list[k];
                // list[k] = list[n];
                // list[n] = value;
            }
        }

        public static void Shuffle<T>(this IList<T> list, int seed)
        {
            var rng = new System.Random(seed);
            var n = list.Count;

            while (n > 1)
            {
                n--;
                var k = rng.Next(n + 1);
                (list[k], list[n]) = (list[n], list[k]);
                // T value = list[k];
                // list[k] = list[n];
                // list[n] = value;
            }
        }

        public static void ShuffleRestricted<T>(this IList<T> list, int restrictionIndex)
        {
            var firstPart = list.Take(restrictionIndex).ToList();
            var secondPart = list.Skip(restrictionIndex).ToList();

            secondPart.Shuffle();

            var tempList = firstPart.Concat(secondPart).ToList();

            for (var i = 0; i < tempList.Count; i++)
            {
                list[i] = tempList[i];
            }
        }

        public static void ShuffleRestricted<T>(this IList<T> list, int restrictionIndex, int seed)
        {
            var firstPart = list.Take(restrictionIndex).ToList();
            var secondPart = list.Skip(restrictionIndex).ToList();

            secondPart.Shuffle(seed);

            var tempList = firstPart.Concat(secondPart).ToList();

            for (var i = 0; i < tempList.Count; i++)
            {
                list[i] = tempList[i];
            }
        }

        public static T GetRandomItem<T>(this IList<T> list, int seed)
        {
            var rng = new System.Random(seed);
            int n = list.Count;

            return list[rng.Next(n)];
        }

        public static Dictionary<T, K> DictReverse<T, K>(this Dictionary<T, K> dict)
        {
            List<T> keys = new List<T>();
            List<K> values = new List<K>();

            foreach (var kvp in dict)
            {
                keys.Add(kvp.Key);
                values.Add(kvp.Value);
            }

            values.Reverse();


            Dictionary<T, K> tempDict = new Dictionary<T, K>();

            for (int i = 0; i < keys.Count; i++)
            {
                tempDict.Add(keys[i], values[i]);
            }

            return tempDict;
        }

        
    }
}