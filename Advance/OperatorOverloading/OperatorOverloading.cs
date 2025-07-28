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

    // Implicit overloading

    public class Meter
    {
        public double Value;
        public Meter(double value)
        {
            Value = value;
        }

        public static implicit operator double(Meter m)
        {
            return m.Value;
        }

        public static implicit operator Meter(double d)
        {
            return new Meter(d);
        }
    }

    // Eksplisit Overloading
    public class Fahrenheit
    {
        public double Degrees;
        public Fahrenheit(double deg) => Degrees = deg;

        public static explicit operator Celsius(Fahrenheit f)
            => new Celsius((f.Degrees - 32) * 5 / 9);
        public override string ToString()
        {
            return $"{Degrees} °C";
        }
    }


    public class Celsius
    {
        public double Degrees;
        public Celsius(double deg) => Degrees = deg;

        public static explicit operator Fahrenheit(Celsius c)
            => new Fahrenheit((c.Degrees * 9 / 5) + 32);

        public override string ToString()
        {
            return $"{Degrees} °C";
        }
    }

    // Boolean Overloading

    public struct SqlBoolean
    {
        private byte m_value;

        // Representasi nilai boolean 3-keadaan
        public static readonly SqlBoolean Null = new SqlBoolean(0);
        public static readonly SqlBoolean False = new SqlBoolean(1);
        public static readonly SqlBoolean True = new SqlBoolean(2);

        private SqlBoolean(byte value) { m_value = value; }

        // Konversi ke 'if (obj)' akan gunakan ini
        public static bool operator true(SqlBoolean x)
            => x.m_value == True.m_value;

        // Konversi ke 'if (!obj)' akan gunakan ini
        public static bool operator false(SqlBoolean x)
            => x.m_value == False.m_value;

        // Operator NOT (!)
        public static SqlBoolean operator !(SqlBoolean x)
        {
            if (x.m_value == Null.m_value) return Null;
            if (x.m_value == False.m_value) return True;
            return False;
        }
    }


}