using System.Reflection.Metadata.Ecma335;
using CreatedType.Classes;

// -------------------------ReadOnly-------------------------

// var ReadOnly = new ClassReadOnly("iqbal");
// ReadOnly.Show();

// ----------------------Overloading Constructur-------------

// var overLoadingConstructur1 = new OverLoadingConstructur("iqbal");
// var overLoadingConstructur2 = new OverLoadingConstructur("iqbal", 25);

// overLoadingConstructur1.Show();
// overLoadingConstructur2.Show();

// ----------------------Non Public Constructur-------------

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

int count = 123;
string name = nameof(count);
Console.WriteLine(name);