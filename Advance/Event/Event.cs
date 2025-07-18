namespace Advance.Event
{
    public delegate void Notifikasi();

    public class Seminar
    {
        public Notifikasi? onSeminarMulai;
        public void Mulai()
        {
            Console.WriteLine("Seminar dimulai");
            onSeminarMulai?.Invoke();
        }
    }

    public class Peserta
    {
        private string _nama;

        public Peserta(string nama)
        {
            _nama = nama;
        }

        public void Respons()
        {
            Console.WriteLine($"Peserta {_nama} mendengar pengumuman");
        }
    }

    // Declaring event on delegate

    public delegate void PriceChangeHandler(decimal oldPrice, decimal newPrice);

    public class ChangePrice
    {
        public event PriceChangeHandler? priceChange;

        public decimal Price;

        public void UpdatePrice(decimal newPrice)
        {
            if (Price != newPrice)
            {
                decimal oldPrice = Price;
                Price = newPrice;
                priceChange?.Invoke(oldPrice, newPrice);
            }
        }

    }

    public class Listener
    {
        public void HandlePriceChange(decimal oldPrice, decimal newPrice)
        {
            Console.WriteLine($"harga berubah dari {oldPrice}$ ke {newPrice}$");
        }
    }

    // Standard Event Pattern
    public class PriceChangeEventArgs : EventArgs
    {
        public readonly decimal LastPrice;
        public readonly decimal NewPrice;

        public PriceChangeEventArgs(decimal lastPrice, decimal newPrice)
        {
            LastPrice = lastPrice;
            NewPrice = newPrice;
        }
    }

    public class Stock
    {
        public string? symbol;
        public decimal price;

        public Stock(string symbol) => this.symbol = symbol;
        public event EventHandler<PriceChangeEventArgs>? PriceChange;

        protected virtual void OnPriceChange(PriceChangeEventArgs e)
        {
            PriceChange?.Invoke(this, e);
        }

        public decimal Price
        {
            get => price;
            set
            {
                if (price == value) return;
                decimal oldPrice = price;
                price = value;
                OnPriceChange(new PriceChangeEventArgs(oldPrice, price));
            }

        }

    }

    public class Listener1
    {
        public void stock_PriceChanged(object? sender, PriceChangeEventArgs e)
        {
            if (e.NewPrice >= e.LastPrice * 1.1m) // 10% atau lebih
            {
                Console.WriteLine("Alert, 10% stock price increase!");
            }
        }
    }

    public class NotifyEventArgs : EventArgs
    {
        public readonly string Message;

        public NotifyEventArgs(string message)
        {
            Message = message;
        }

    }

    public class Restaurant
    {
        string message;

        public Restaurant(string msg)
        {
            message = msg;
        }

        public event EventHandler<NotifyEventArgs>? Notify;
        protected virtual void onNotify(NotifyEventArgs e)
        {
            Notify?.Invoke(this, e);
        }

        public void Send()
        {
            onNotify(new NotifyEventArgs(message));
        }

    }

    public class Listener2
    {
        public string? Nama;

        public Listener2(string nama)
        {
            Nama = nama;
        }
        public void Customer(object? sender, NotifyEventArgs e)
        {
            Console.WriteLine($"Hallo {Nama} {e.Message} ");
        }
    }

    // Relaying Event
    public class Motor
    {
        public event EventHandler? MesinBeruptar;

        public void NyalakanMesin()
        {
            Console.WriteLine("Motor: Mesin dinyalakan!");
            MesinBeruptar?.Invoke(this, EventArgs.Empty);
        }

        public void MatikanMesin()
        {
            Console.WriteLine("Motor: Mesin dimatikan!");
        }

    }

    public class Dashboard
    {
        private readonly Motor _motor;

        public Dashboard(Motor motor)
        {
            _motor = motor;
        }

        public event EventHandler? IndikatorMesinBerputar
        {
            add
            {
                Console.WriteLine("Dashboard: Menambahkan subscriber ke event MesinBerputar Motor.");
                _motor.MesinBeruptar += value;

            }
            remove
            {
                Console.WriteLine("Dashboard: Menghapus subscriber dari event MesinBerputar Motor.");
                _motor.MesinBeruptar -= value;


            }

        }

    }

    public class AplikasiPemantau
    {
        public void HandlerMesinBerputar(object? sender, EventArgs e)
        {
            Console.WriteLine("Aplikasi Pemantau: Menerima notifikasi dari Dashboard bahwa mesin berputar!");
        }
    }
}