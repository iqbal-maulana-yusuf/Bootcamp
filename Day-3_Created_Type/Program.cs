using System.Reflection.Metadata.Ecma335;
using CreatedType.Classes;
using CreatedType.Inheritance;
using CreatedType.Interface;
using CreatedType.ObjectType;
// using CreatedType.Struct;


// -------------------------ReadOnly-------------------------

// var ReadOnly = new ClassReadOnly("iqbal");
// ReadOnly.Show();

// ----------------------Overloading Constructur-------------

// var overLoadingConstructur1 = new OverLoadingConstructur("iqbal");
// var overLoadingConstructur2 = new OverLoadingConstructur("iqbal", 25);

// overLoadingConstructur1.Show();
// overLoadingConstructur2.Show();

// ----------------------Non Public Constructur-------------l

// var log = Logger.GetInstace();
// log.Log("Hallo");


// ----------------------Deconstructor----------------------

// var rect = new Rectangle(10, 5);
// (int h, int w) = rect;
// Console.WriteLine($"H: {h} \nW: {w}");


// ----------------------InitSetter----------------------

// var init = new InitSetter { Name = "iqbal" };
// Console.WriteLine(init.Name);

// ----------------------Indexer----------------------
// var idx = new IndexerClass();
// Console.WriteLine(idx[1]);



// ----------------------Primary Constructor----------------------
// var primaryContructor = new PrimaryConstructor("Bandung");
// primaryContructor.Print();


// ----------------------Static Constructor----------------------
// var server1 = new Server();
// server1.Run();
// var server2 = new Server();
// server2.Run();
// constructor hanya sekali muncul meskipun terjadi duakali instansiasi objek


// ----------------------Partial Class----------------------

// var person = new Person();
// person.FirstName = "iqbal";
// person.LastName = "maulana";
// person.sayHello();

// ---------------------Upcasting----------------------
// var dog = new Dog();
// Animal myAnimal = dog;
// myAnimal.Speak();

// ---------------------Downcasting----------------------
// Animal animal = new Dog();
// var dog = (Dog)animal;
// dog.Bark();


// ---------------------as Operator----------------------

// Animal animal = new Dog();
// Dog? dog = animal as Dog;
// dog?.Bark();



// ---------------------is Operator----------------------

// Animal animal = new Dog();
// if (animal is Dog)
// {
//     Dog? dog = animal as Dog;
//     dog?.Bark();
// }

// ---------------------is Operator + Pattern Matching----------------------
// Animal animal = new Dog();
// if (animal is Dog dog)
// {
//     dog.Bark();
// }


// ---------------------Virtual Method----------------------

// var mansion = new House { Name = "Iqbal", Mortgage = 2000 };
// VirtualMethod asset = mansion;
// Console.WriteLine(asset.Liability);


// ---------------------Virtual Method----------------------

// var engine = new ElectricalEngine();
// engine.Start();
// engine.Engine();


// ---------------------Hiding Inherite Member----------------------

// var kucing = new Kucing();
// kucing.Makan();
// Hewan hewan = kucing;
// hewan.Makan();


// ---------------------Sealing Function----------------------

// var honda = new Honda();
// Console.WriteLine(honda.Kecepatan);
// var yamaha = new Yamaha();
// Console.WriteLine(yamaha.Kecepatan);

// ---------------------Base----------------------
// var restaurant = new Restaurant();
// Console.WriteLine(restaurant.Liability);


// ---------------------OBJECT TYPE----------------------

// var stack = new Stack();
// stack.Push("sosis");
// string s = (string)stack.Pop();
// Console.WriteLine(s);


// var a = new PointStruct(1, 6);
// PointStruct b = a;
// b.A = 5;

// Console.WriteLine($"{a.A} {a.B}");
// Console.WriteLine($"{b.A} {b.B}");

// ---------------------Interface----------------------


// ---------------------Eksplisit Interface-------------

// var text = new TextBox();
// var dokumen = new Dokumen();

// IUndoable undo = dokumen;
// undo.Undo();


// ---------------------Static Interface-------------

Console.WriteLine(IStatic.Cek());


