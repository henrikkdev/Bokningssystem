{
    using System;
    using System.Collections.Generic;

    class Program
{
    static void Main(string[] args)
    {
        // Lista med tillgängliga tider för en vecka (måndag till fredag)
        Dictionary<string, List<string>> weeklySchedule = new Dictionary<string, List<string>>
        {
            { "Måndag", new List<string> { "08:00", "09:00", "10:00", "11:00", "13:00", "14:00", "15:00" } },
            { "Tisdag", new List<string> { "08:00", "09:00", "10:00", "11:00", "13:00", "14:00", "15:00" } },
            { "Onsdag", new List<string> { "08:00", "09:00", "10:00", "11:00", "13:00", "14:00", "15:00" } },
            { "Torsdag", new List<string> { "08:00", "09:00", "10:00", "11:00", "13:00", "14:00", "15:00" } },
            { "Fredag", new List<string> { "08:00", "09:00", "10:00", "11:00", "13:00", "14:00", "15:00" } }
        };

        // Lista med bokade tider kopplade till personer och tjänster
        Dictionary<string, (string Name, string Service)> bookedAppointments = new Dictionary<string, (string Name, string Service)>();

        while (true)
        {
            Console.WriteLine("\n*** Däckbyte Bokningssystem ***");
            Console.WriteLine("1. Visa tillgängliga tider");
            Console.WriteLine("2. Boka en tid");
            Console.WriteLine("3. Visa bokade tider");
            Console.WriteLine("4. Avsluta");
            Console.Write("Välj ett alternativ: ");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    ShowAvailableTimes(weeklySchedule);
                    break;

                case "2":
                    BookTime(weeklySchedule, bookedAppointments);
                    break;

                case "3":
                    ShowBookedAppointments(bookedAppointments);
                    break;

                case "4":
                    Console.WriteLine("Avslutar programmet. Tack för att du använde bokningssystemet!");
                    return;

                default:
                    Console.WriteLine("Ogiltigt val. Försök igen.");
                    break;
            }
        }
    }

    static void ShowAvailableTimes(Dictionary<string, List<string>> weeklySchedule)
    {
        Console.WriteLine("\nTillgängliga tider för veckan:");

        foreach (var day in weeklySchedule)
        {
            Console.WriteLine($"{day.Key}:");
            if (day.Value.Count == 0)
            {
                Console.WriteLine("  Inga lediga tider.");
            }
            else
            {
                foreach (var time in day.Value)
                {
                    Console.WriteLine($"  {time}");
                }
            }
        }
    }

    static void BookTime(Dictionary<string, List<string>> weeklySchedule, Dictionary<string, (string Name, string Service)> bookedAppointments)
    {
        Console.WriteLine("\nAnge vilken dag du vill boka (Måndag, Tisdag, Onsdag, Torsdag, Fredag):");
        string day = Console.ReadLine();

        if (!weeklySchedule.ContainsKey(day))
        {
            Console.WriteLine("Ogiltig dag. Försök igen.");
            return;
        }

        Console.WriteLine("Ange tiden du vill boka (HH:MM):");
        string timeToBook = Console.ReadLine();

        if (weeklySchedule[day].Contains(timeToBook))
        {
            Console.WriteLine("Ange ditt namn:");
            string name = Console.ReadLine();

            Console.WriteLine("Ange vilken tjänst som ska utföras (t.ex. Däckbyte, Service, Reparation):");
            string service = Console.ReadLine();

            string appointmentKey = $"{day} {timeToBook}";

            if (bookedAppointments.ContainsKey(appointmentKey))
            {
                Console.WriteLine("Tiden är redan bokad. Försök igen.");
                return;
            }

            weeklySchedule[day].Remove(timeToBook);
            bookedAppointments[appointmentKey] = (name, service);
            Console.WriteLine($"Tiden {timeToBook} på {day} har bokats för {name} för tjänsten {service}!");
        }
        else
        {
            Console.WriteLine("Tiden är inte tillgänglig eller ogiltig. Försök igen.");
        }
    }

    static void ShowBookedAppointments(Dictionary<string, (string Name, string Service)> bookedAppointments)
    {
        Console.WriteLine("\nBokade tider:");

        if (bookedAppointments.Count == 0)
        {
            Console.WriteLine("Inga bokade tider finns.");
        }
        else
        {
            foreach (var appointment in bookedAppointments)
            {
                Console.WriteLine($"{appointment.Key}: {appointment.Value.Name} - {appointment.Value.Service}");
            }
        }
    }
}
}