namespace Advance.OperatorOverloading
{
    public struct Complex
    {
        private double _real, _img;

        public Complex(double real, double img)
        {
            _real = real;
            _img = img;
        }

        public static Complex operator +(Complex a, Complex b)
        {
            return new Complex(a._real + b._real, a._img + b._img);
        }
    }

    public struct Temprature
    {
        private float _deegre;

        public Temprature(float deg)
        {
            _deegre = deg;
        }

        public static Temprature operator -(Temprature temp)
        {
            return new Temprature(-temp._deegre);
        }

        public static Temprature operator ++(Temprature temp)
        {
            return new Temprature(temp._deegre + 1);
        }


        public override string ToString()
        {
            return $"{_deegre}°C";
        }

    }
}