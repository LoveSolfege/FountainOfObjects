namespace HiddenFountain.Extensions {
    internal static class ListExtensions {
        private static readonly Random _random = new();

        //randomly shuffle any list
        public static void Shuffle<T>(this List<T> list) {

            if (list == null || list.Count <= 1) return;

            int n = list.Count;
            for (int i = n - 1; i > 0; i--) {
                int j = _random.Next(i + 1);
                (list[j], list[i]) = (list[i], list[j]);
            }
        }
    }
}
