namespace BigButton.Models
{
    public class ColorModel
    {
        public string Color { get; private set; }
        public string Down { get; private set; }
        public string Up { get; private set; }

        static readonly object Sync = new object();
        static bool _red;

        public static ColorModel Create()
        {
            bool red;
            lock (Sync)
            {
                red = _red;
                _red = !_red;
            }

            return new ColorModel
            {
                Color = red ? "Red" : "Blue",
                Down = red ? "/images/OrangeButton.png" : "/images/BlueLight.png",
                Up = red ? "/images/RedButton.png" : "/images/BlueButton.png"
            };
        }
    }
}