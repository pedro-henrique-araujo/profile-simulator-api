namespace ProfileSimulator.Api
{
    public static class ExtensionMethods
    {
        private static Random _random;

        static ExtensionMethods()
        {
            _random = new Random();
        }

        public static T PickARandomItem<T>(this T[] array)
        {
            var length = array.Length;
            var randomKey = _random.Next(length);
            var item = array[randomKey];
            return item;
        }
    }
}
