namespace Däckarn
{
    public class Car
    {
        public string Make { get; set; }
        public string Model { get; set; }
        public string RegistrationNumber { get; set; }

        public override string ToString()
        {
            return $"{Make} {Model} ({RegistrationNumber})";
        }
    }

    public class CarOwner
    {
        public int ID { get; private set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public List<Car> Cars { get; set; } = new List<Car>();

        private static int nextCustomer = 1;

        public CarOwner(string firstName, string lastName, string email, string phoneNumber)
        {
            ID = nextCustomer++;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            PhoneNumber = phoneNumber;
        }

        public void AddCar(Car car)
        {
            Cars.Add(car);
        }

        public override string ToString()
        {
            string carsInfo = Cars.Count > 0
                ? string.Join(", ", Cars)
                : "No cars found";

            return $"ID: {ID}, Name: {FirstName} {LastName}, Email: {Email}, Phone: +46{PhoneNumber}, Cars: {carsInfo}";
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Dictionary<string, List<string>> weeklySchedule = new Dictionary<string, List<string>>
            {
                { "Måndag", new List<string> { "08:00", "09:00", "10:00", "11:00", "13:00", "14:00", "15:00" } },
                { "Tisdag", new List<string> { "08:00", "09:00", "10:00", "11:00", "13:00", "14:00", "15:00" } },
                { "Onsdag", new List<string> { "08:00", "09:00", "10:00", "11:00", "13:00", "14:00", "15:00" } },
                { "Torsdag", new List<string> { "08:00", "09:00", "10:00", "11:00", "13:00", "14:00", "15:00" } },
                { "Fredag", new List<string> { "08:00", "09:00", "10:00", "11:00", "13:00", "14:00", "15:00" } }
            };

            Dictionary<string, (CarOwner Owner, string Service, string Notes)> bookedAppointments = new Dictionary<string, (CarOwner Owner, string Service, string Notes)>();
            List<CarOwner> carOwners = new List<CarOwner>();

            while (true)
            {
                Console.WriteLine("\n*** Däckbyte Bokningssystem ***");
                Console.WriteLine("1. Visa tillgängliga tider");
                Console.WriteLine("2. Boka en tid");
                Console.WriteLine("3. Visa bokade tider");
                Console.WriteLine("4. Lägg till en ny person");
                Console.WriteLine("5. Redigera en bokning");
                Console.WriteLine("6. Avsluta");
                Console.Write("Välj ett alternativ: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ShowAvailableTimes(weeklySchedule);
                        break;

                    case "2":
                        BookTime(weeklySchedule, bookedAppointments, carOwners);
                        break;

                    case "3":
                        ShowBookedAppointments(bookedAppointments);
                        break;

                    case "4":
                        AddNewCarOwner(carOwners);
                        break;

                    case "5":
                        EditBooking(bookedAppointments);
                        break;

                    case "6":
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

        static void BookTime(Dictionary<string, List<string>> weeklySchedule, Dictionary<string, (CarOwner Owner, string Service, string Notes)> bookedAppointments, List<CarOwner> carOwners)
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
                if (carOwners.Count == 0)
                {
                    Console.WriteLine("Inga registrerade personer finns. Lägg till en ny person först.");
                    return;
                }

                Console.WriteLine("Välj en person genom att ange deras ID:");
                foreach (var owner in carOwners)
                {
                    Console.WriteLine(owner);
                }

                if (!int.TryParse(Console.ReadLine(), out int ownerId) || carOwners.Find(o => o.ID == ownerId) == null)
                {
                    Console.WriteLine("Ogiltigt ID. Försök igen.");
                    return;
                }

                CarOwner selectedOwner = carOwners.Find(o => o.ID == ownerId);

                Console.WriteLine("Ange vilken tjänst som ska utföras (t.ex. Däckbyte, Service, Reparation):");
                string service = Console.ReadLine();

                Console.WriteLine("Ange eventuella anteckningar för bokningen (valfritt):");
                string notes = Console.ReadLine();

                string appointmentKey = $"{day} {timeToBook}";

                if (bookedAppointments.ContainsKey(appointmentKey))
                {
                    Console.WriteLine("Tiden är redan bokad. Försök igen.");
                    return;
                }

                weeklySchedule[day].Remove(timeToBook);
                bookedAppointments[appointmentKey] = (selectedOwner, service, notes);
                Console.WriteLine($"Tiden {timeToBook} på {day} har bokats för {selectedOwner.FirstName} {selectedOwner.LastName} för tjänsten {service}!");
            }
            else
            {
                Console.WriteLine("Tiden är inte tillgänglig eller ogiltig. Försök igen.");
            }
        }

        static void ShowBookedAppointments(Dictionary<string, (CarOwner Owner, string Service, string Notes)> bookedAppointments)
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
                    var owner = appointment.Value.Owner;
                    string notes = string.IsNullOrEmpty(appointment.Value.Notes) ? "Inga anteckningar" : appointment.Value.Notes;
                    Console.WriteLine($"{appointment.Key}: {owner.FirstName} {owner.LastName} - {appointment.Value.Service} (Anteckningar: {notes})");
                }
            }
        }

        static void AddNewCarOwner(List<CarOwner> carOwners)
        {
            Console.WriteLine("\nAnge förnamn:");
            string firstName = Console.ReadLine();
            Console.WriteLine("Ange efternamn:");
            string lastName = Console.ReadLine();

            Console.WriteLine("Ange e-postadress:");
            string email = Console.ReadLine();

            Console.WriteLine("Ange telefonnummer:");
            string phoneNumber = Console.ReadLine();

            CarOwner newOwner = new CarOwner(firstName, lastName, email, phoneNumber);

            while (true)
            {
                Console.WriteLine("Vill du lägga till en bil till denna person? (ja/nej):");
                string addCarChoice = Console.ReadLine()?.ToLower();

                if (addCarChoice == "ja")
                {
                    Console.WriteLine("Ange bilens märke:");
                    string make = Console.ReadLine();

                    Console.WriteLine("Ange bilens modell:");
                    string model = Console.ReadLine();

                    Console.WriteLine("Ange bilens registreringsnummer:");
                    string registrationNumber = Console.ReadLine();

                    Car car = new Car { Make = make, Model = model, RegistrationNumber = registrationNumber };
                    newOwner.AddCar(car);
                }
                else if (addCarChoice == "nej")
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Ogiltigt val. Försök igen.");
                }
            }

            carOwners.Add(newOwner);
            Console.WriteLine($"Ny person med namn {firstName} {lastName} har lagts till med ID {newOwner.ID}!");
        }

        static void EditBooking(Dictionary<string, (CarOwner Owner, string Service, string Notes)> bookedAppointments)
        {
            Console.WriteLine("\nAnge dag och tid för bokningen du vill redigera (t.ex. Måndag 10:00):");
            string appointmentKey = Console.ReadLine();

            if (!bookedAppointments.ContainsKey(appointmentKey))
            {
                Console.WriteLine("Ingen bokning hittades för den angivna tiden. Försök igen.");
                return;
            }

            var currentAppointment = bookedAppointments[appointmentKey];

            Console.WriteLine($"Nuvarande bokning: {currentAppointment.Owner.FirstName} {currentAppointment.Owner.LastName} - {currentAppointment.Service}");
            Console.WriteLine("Ange ny tjänst (lämna tomt för att behålla nuvarande):");
            string newService = Console.ReadLine();

            Console.WriteLine("Ange nya anteckningar (lämna tomt för att behålla nuvarande):");
            string newNotes = Console.ReadLine();

            bookedAppointments[appointmentKey] = (
                currentAppointment.Owner,
                string.IsNullOrEmpty(newService) ? currentAppointment.Service : newService,
                string.IsNullOrEmpty(newNotes) ? currentAppointment.Notes : newNotes
            );

            Console.WriteLine("Bokningen har uppdaterats!");
        }
    }
}