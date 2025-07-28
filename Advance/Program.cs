
using System.Numerics;
using System.Threading.Tasks.Dataflow;
using Advance.DelegateImplementation;
using Advance.EnumeratorAndIterator;
using Advance.Event;
using Advance.OperatorOverloading;

// ---------------------------DELEGATE----------------------------

// ---------------------------Generic Delegate--------------------
// int x = 10;
// GenericDelegate.DoubleNumber(x,MethodPointer.Doubled);

// var data = new Dictionary<int, string>
// {
//     {1,"iqbal"},
//     {2,"maulana"},
//     {3,"yusuf"}
// };

// GenericDelegate.Antrian(data, MethodPointer.AntrianPointer);



// ---------------------------EVENT----------------------------

// var seminar = new Seminar();
// var peserta1 = new Peserta("iqbal");
// var peserta2 = new Peserta("maulana");

// seminar.onSeminarMulai += peserta1.Respons;
// seminar.onSeminarMulai += peserta2.Respons;

// seminar.Mulai();

// ---------------------------Event Declaration-----------------

// var changePrice = new ChangePrice();
// var listener1 = new Listener();

// changePrice.priceChange += listener1.HandlePriceChange;
// changePrice.Price = 50;
// changePrice.UpdatePrice(100);

// ---------------------------Standard Event Pattern-----------------

// var stock = new Stock("BBCA");
// var listener = new Listener1();
// stock.PriceChange += listener.stock_PriceChanged;

// stock.Price = 10;  // No alert (karena belum ada old price)
// stock.Price = 11;  // Akan muncul alert (kenaikan dari 10 ke 11 = 10%)
// stock.Price = 11.5m;  // Tidak ada alert (kurang dari 10% dari 11)


// var notify = new Restaurant("Selamat anda mendapat promo 50%");
// var customer = new Listener2("Iqbal");

// notify.Notify += customer.Customer;

// notify.Send();

// ---------------------------Relaying Event-----------------
// var motor = new Motor();
// var dashboard = new Dashboard(motor);
// var aplikasi = new AplikasiPemantau();
// dashboard.IndikatorMesinBerputar += aplikasi.HandlerMesinBerputar;
// motor.NyalakanMesin();


// ---------------------------Memory Optimzed-----------------

// var myButton = new TombolUIOptimized();
// var listener = new UIListiner();
// myButton.AddingNewEventHandler("OnClick", (Delegate)listener.MyClickHandler);
// myButton.OnCLick();

// ---------------------------Explicit Interface Implementation of Event-----------------
// var foo = new Foo();
// var fooListener = new FooListener();
// IFoo fooInstance = foo;
// fooInstance.Ev += fooListener.DoSomething;
// foo.EventCaller();


// ---------------------------ENUMERATOR AND ITERATOR-----------------

// ---------------------------Foreach Manual--------------------------

// var foraeachManual = new ForeachManual();
// foraeachManual.ShowBuah();

// ---------------------------Foreach Manual Dictionary--------------------------


// var foraeachManualDict = new ForeachManualDict();
// foraeachManualDict.ShowBuah();

// ---------------------------Producing Sequences with yield--------------------------

// var sequence = new ProduceSequence();
// sequence.x = 10;
// sequence.Iterate();

// ---------------------------Composing Sequence---------------------------
// var composingSequence = new ComposingSequence();
// composingSequence.x = 5;
// var doubleNumber = composingSequence.DoubelNumber(composingSequence.GetNumber());
// composingSequence.Iterate(doubleNumber);



// int? umur = null;
// Console.WriteLine(umur.HasValue);
// Console.WriteLine(umur.GetValueOrDefault(100));



// ---------------------------OPERATOR OVERLOADING----------------


// ---------------------------unary operation----------------
// var temp = new Temprature(20);

// Console.WriteLine(++temp);


// ---------------------------binary operation----------------
// var a = new Complex(1, 2);
// var b = new Complex(2, 2);
// var c = a + b;
// Console.WriteLine(c);

// int x = int.MaxValue;
// int y = checked(x + 1);        // Tidak error, hasilnya overflow (wrap around jadi negatif) // Akan melempar OverflowException
// Console.WriteLine(y);

// ---------------------------implicit operation----------------
// Meter m = 5.0;
// double d = m;
// Console.WriteLine(d.GetType());

// Fahrenheit f = new Fahrenheit(98.6);
// Celsius c = (Celsius)f;
// Console.WriteLine(c);
// ---------------------------Boolean operation----------------

// var a = SqlBoolean.True;

// if (a)
//     Console.WriteLine("True");
// else if (!a)
//     Console.WriteLine("False");
// else
//     Console.WriteLine("Null");  // Ini tampil

// ---------------------------NULLABLE----------------------------

// implicit casting
// int intA = 5;
// int? nullableA = a;

// ---------------------------explicit casting----------------------------

// int? nullableX = 5;
// int intX = (int)nullableX;


// ---------------------------Boxing----------------------------

// int? x = null;
// object o = x;
// Console.WriteLine(o == null);

// ---------------------------Unboxing----------------------------
// object o = "string";
// int? x = o as int?;
// Console.WriteLine(x);


// ---------------------------Unboxing----------------------------
// int a = 10;
// int? b = null;
// Console.WriteLine(a + b);

// bool a = false;
// bool b = null;
// Console.WriteLine(a && b);








int? a = 5;
int? b = null;
int? c = a + b;
Console.WriteLine(c);