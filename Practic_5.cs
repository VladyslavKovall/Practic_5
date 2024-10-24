using System;
using System.Collections.Generic;

class Passenger
{
    public int Number { get; private set; }

    public Passenger(int number)
    {
        Number = number;
    }
}

class Driver
{
    public string Name { get; private set; }

    public Driver(string name)
    {
        Name = name;
    }

    public void OnRouteFull(object sender, EventArgs e)
    {
        Console.WriteLine($"{Name}: Еду по маршруту.");
    }

    public void OnRouteEmpty(object sender, EventArgs e)
    {
        Console.WriteLine($"{Name}: Еду в парк.");
    }
}

class Route
{
    public int Seats { get; private set; }
    private List<Passenger> passengers;
    public bool HasSeats => passengers.Count < Seats;
    public bool HasPassengers => passengers.Count > 0;

    public event EventHandler RouteFull;
    public event EventHandler RouteEmpty;

    public Route(int seats)
    {
        Seats = seats;
        passengers = new List<Passenger>();
    }

    public void AddPassenger(Passenger passenger)
    {
        if (HasSeats)
        {
            passengers.Add(passenger);
            Console.WriteLine($"Пассажир {passenger.Number} сел в маршрутку.");
            if (!HasSeats)
            {
                OnRouteFull();
            }
        }
        else
        {
            Console.WriteLine("Нет свободных мест.");
        }
    }

    public void RemovePassenger()
    {
        if (HasPassengers)
        {
            var passenger = passengers[passengers.Count - 1];
            passengers.RemoveAt(passengers.Count - 1);
            Console.WriteLine($"Пассажир {passenger.Number} вышел из маршрутки.");
            if (passengers.Count == 0)
            {
                OnRouteEmpty();
            }
        }
        else
        {
            Console.WriteLine("Маршрутка пустая.");
        }
    }

    protected virtual void OnRouteFull()
    {
        Console.WriteLine("Все места заняты.");
        RouteFull?.Invoke(this, EventArgs.Empty);
    }

    protected virtual void OnRouteEmpty()
    {
        Console.WriteLine("Все места свободны.");
        RouteEmpty?.Invoke(this, EventArgs.Empty);
    }
}

class Program
{
    static void Main(string[] args)
    {
        Console.Write("Введите количество мест в маршрутке: ");
        int seats = int.Parse(Console.ReadLine());

        Route route = new Route(seats);
        Driver driver = new Driver("Иван");

        route.RouteFull += driver.OnRouteFull;
        route.RouteEmpty += driver.OnRouteEmpty;

        int passengerNumber = 1;
        while (route.HasSeats)
        {
            Passenger passenger = new Passenger(passengerNumber++);
            route.AddPassenger(passenger);
        }

        while (route.HasPassengers)
        {
            route.RemovePassenger();
        }
    }
}
