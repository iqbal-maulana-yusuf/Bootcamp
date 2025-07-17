

using System.Security.Cryptography.X509Certificates;
using Advance.Delegate;
// --------------------Bentuk Umum Delegate--------------------------------


// Transformer t = TransformerHelper.Square;
// int result = t(3);
// Console.WriteLine(result);

// --------------------Bentuk Umum Delegate--------------------------------

// int[] values = { 1, 2, 3 };
// Transformer t = PluginMethod.Cube;
// PluginMethod.Transform(values, t);
// foreach (var value in values)
// {
//     Console.WriteLine(value);
// }


// --------------------Instance Method Target--------------------------------

// var test = new Test();
// Transformer t = test.Square;
// Console.WriteLine(t(9));


// --------------------Multicast Delegate--------------------------------

// MessageHandler handler = Message.ShowMessage;
// handler += Message.UppercaseMessage;
// handler("Hello World");

// --------------------Generic Delegate--------------------------------
// TransformerGeneric<int, int> t = Utils.Square;

// int[] values = { 2, 2, 2 };

// Utils.Transform<int>(values, t);

// foreach (var value in values)
// {
//     Console.WriteLine(value);
// }


// --------------------Func and Action Delegate--------------------------------

int[] values = { 1, 2, 3, 4 };
Func<int, int> t = FuncAction.Cube;
FuncAction.Transform<int>(values, t);
foreach (var value in values)
{
    Console.WriteLine(value);
}

