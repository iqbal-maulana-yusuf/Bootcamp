namespace CreatedType.Inheritance
{
    public class Animal
    {
        public void Speak()
        {
            Console.WriteLine("Animal sound");
        }

    }

    public class Dog : Animal
    {
        public void Bark()
        {
            Console.WriteLine("Woof!");
        }
    }

    public class VirtualMethod
    {
        public string? Name;
        public virtual decimal Liability => 0;
    }

    public class Stock : VirtualMethod
    {
        public long ShareOwned;
    }

    public class House : VirtualMethod
    {
        public decimal Mortgage;
        public override decimal Liability => Mortgage;
    }

    //Covariant Return Type

    public class Asset
    {
        public string? Name;

        public virtual Asset Clone() => new Asset { Name = this.Name };

    }

    public class Retail : Asset
    {
        public decimal Mortgage;

        public override Retail Clone() => new Retail { Name = this.Name, Mortgage = this.Mortgage };
    }

    // Abstract Class
    public abstract class Car
    {
        public void Start()
        {
            Console.WriteLine("car is starting");
        }

        public abstract void Engine();

    }

    public class ElectricalEngine : Car
    {
        public override void Engine()
        {
            Console.WriteLine("Electric Engine");
        }
    }

    // Hiding Inherite Member

    public class Hewan
    {
        public void Makan()
        {
            Console.WriteLine("Hewan Makan");
        }

    }

    public class Kucing : Hewan
    {
        public new void Makan()
        {
            Console.WriteLine("Kucing Makan");
        }
    }

    // Sealing Function And Class

    public class Motor
    {
        public virtual int Kecepatan => 0;
    }

    public class Honda : Motor
    {
        public sealed override int Kecepatan => 100;
    }

    // public class Yamaha : Honda
    // {
    //     public override int Kecepatan => 150; akan error
    // }

    // Base Keyword

    public class Asset1
    {
        public virtual int Liability => 0;
    }

    public class Restaurant : Asset1
    {
        public int Mortgage = 1000;

        public override int Liability => base.Liability + Mortgage;
    }

    //Calling Base Constructor

    public class Person
    {
        public Person(string name)
        {
            Console.WriteLine("ini person bernama " + name);
        }
    }

    public class Student : Person
    {
        public Student(string firstname) : base(firstname)
        {
            Console.WriteLine("Ini student bernama " + firstname);
        }
    }



}