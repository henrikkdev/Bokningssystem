using System;
using System.Collections.Generic;

namespace bokningssystem
{
    internal class Program
    {
        static List<string> bookedSlots = new List<string>();

        static void Main(string[] args)
        {
            bool exit = false;

            while (!exit)
            {
                Console.Clear();
                Console.WriteLine("Welcome to the Booking System");
                Console.WriteLine("1. Book Tire Change");
                Console.WriteLine("2. Book Tire Setting");
                Console.WriteLine("3. View Booked Times");
                Console.WriteLine("4. Exit");
                Console.Write("Please select an option: ");

                switch (Console.ReadLine())
                {
                    case "1":
                        BookService("Tire Change");
                        break;
                    case "2":
                        BookService("Tire Setting");
                        break;
                    case "3":
                        ViewBookedTimes();
                        break;
                    case "4":
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Invalid option, please try again.");
                        break;
                }
            }
        }

        static void BookService(string serviceType)
        {
            Console.Clear();
            Console.WriteLine($"Booking {serviceType}");
            List<string> availableSlots = GetAvailableTimeSlots();
            for (int i = 0; i < availableSlots.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {availableSlots[i]}");
            }
            Console.Write("Please select a time slot: ");
            if (int.TryParse(Console.ReadLine(), out int slotIndex) && slotIndex > 0 && slotIndex <= availableSlots.Count)
            {
                string selectedSlot = availableSlots[slotIndex - 1];
                if (!bookedSlots.Contains(selectedSlot))
                {
                    bookedSlots.Add(selectedSlot);
                    Console.WriteLine($"{serviceType} booked successfully at {selectedSlot}!");
                }
                else
                {
                    Console.WriteLine("This time slot is already booked. Please select another slot.");
                }
            }
            else
            {
                Console.WriteLine("Invalid selection, returning to main menu.");
            }
            Console.WriteLine("Press any key to return to the main menu.");
            Console.ReadKey();
        }

        static void ViewBookedTimes()
        {
            Console.Clear();
            Console.WriteLine("Booked Times:");
            if (bookedSlots.Count == 0)
            {
                Console.WriteLine("No times are booked yet.");
            }
            else
            {
                foreach (var slot in bookedSlots)
                {
                    Console.WriteLine(slot);
                }
            }
            Console.WriteLine("Press any key to return to the main menu.");
            Console.ReadKey();
        }

        static List<string> GetAvailableTimeSlots()
        {
            List<string> timeSlots = new List<string>();
            DateTime startTime = DateTime.Today.AddHours(8); // 08:00 AM
            DateTime endTime = DateTime.Today.AddHours(17); // 05:00 PM

            while (startTime < endTime)
            {
                if (startTime.Hour != 11) // Skip 11:00 to 12:00
                {
                    timeSlots.Add(startTime.ToString("HH:mm"));
                }
                startTime = startTime.AddMinutes(30);
            }

            return timeSlots;
        }
    }
}
