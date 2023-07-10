namespace CsharpTBCAcademy.Assignment16;

public class Contact
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int Age { get; set; }
}

public delegate bool ContactFilter(Contact contact, string filterValue);

class Program
{
    static void Main(string[] args)
    {
        List<Contact> contacts = CreateContactList();

        Console.WriteLine("Welcome to the Contacts Search!");

        while (true)
        {
            Console.WriteLine("\nSelect a filter option:");
            Console.WriteLine("1. Search by name (not exact match)");
            Console.WriteLine("2. Search by last name (not exact match)");
            Console.WriteLine("3. Search by first and last name (inexact matching)");
            Console.WriteLine("4. Search by age (age range)");
            Console.WriteLine("5. Exit");

            Console.Write("Enter your choice (1-5): ");
            string choice = Console.ReadLine();

            if (choice == "5")
                break;

            ContactFilter filter = GetContactFilter(choice);

            if (filter != null)
            {
                try
                {
                    List<Contact> filteredContacts = SearchContacts(contacts, filter);
                    PrintContacts(filteredContacts);
                }
                catch (CustomException ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("Invalid choice! Please try again.");
            }
        }
    }

    static List<Contact> CreateContactList()
    {
        // Test list of contacts
        List<Contact> contacts = new List<Contact>
        {
            new Contact { FirstName = "John", LastName = "Doe", Age = 30 },
            new Contact { FirstName = "Jane", LastName = "Smith", Age = 25 },
            new Contact { FirstName = "Michael", LastName = "Johnson", Age = 35 },
            new Contact { FirstName = "Emily", LastName = "Davis", Age = 28 },
            new Contact { FirstName = "David", LastName = "Brown", Age = 40 }
        };

        return contacts;
    }

    static ContactFilter GetContactFilter(string choice)
    {
        switch (choice)
        {
            case "1":
                return FilterByName;
            case "2":
                return FilterByLastName;
            case "3":
                return FilterByFirstAndLastName;
            case "4":
                return FilterByAge;
            default:
                return null;
        }
    }

    static bool FilterByName(Contact contact, string filterValue)
    {
        return contact.FirstName.IndexOf(filterValue, StringComparison.OrdinalIgnoreCase) >= 0;
    }

    static bool FilterByLastName(Contact contact, string filterValue)
    {
        return contact.LastName.IndexOf(filterValue, StringComparison.OrdinalIgnoreCase) >= 0;
    }

    static bool FilterByFirstAndLastName(Contact contact, string filterValue)
    {
        string[] nameParts = filterValue.Split(' ');

        if (nameParts.Length != 2)
            throw new CustomException("Invalid filter value. Enter both first and last name separated by a space.");

        string firstName = nameParts[0];
        string lastName = nameParts[1];

        return contact.FirstName.IndexOf(firstName, StringComparison.OrdinalIgnoreCase) >= 0 &&
            contact.LastName.IndexOf(lastName, StringComparison.OrdinalIgnoreCase) >= 0;
    }

    static bool FilterByAge(Contact contact, string filterValue)
    {
        string[] range = filterValue.Split('-');

        if (range.Length != 2)
            throw new CustomException("Invalid age range. Enter the minimum and maximum age separated by a dash (-).");

        int minAge = int.Parse(range[0]);
        int maxAge = int.Parse(range[1]);

        return contact.Age >= minAge && contact.Age <= maxAge;
    }

    static List<Contact> SearchContacts(List<Contact> contacts, ContactFilter filter)
    {
        Console.Write("Enter the filter value: ");
        string filterValue = Console.ReadLine();

        List<Contact> filteredContacts = new List<Contact>();

        foreach (Contact contact in contacts)
        {
            if (filter(contact, filterValue))
                filteredContacts.Add(contact);
        }

        return filteredContacts;
    }

    static void PrintContacts(List<Contact> contacts)
    {
        if (contacts.Count == 0)
        {
            Console.WriteLine("No contacts found.");
            return;
        }

        Console.WriteLine("\nFiltered Contacts:");

        foreach (Contact contact in contacts)
        {
            Console.WriteLine($"Name: {contact.FirstName} {contact.LastName}, Age: {contact.Age}");
        }
    }
}

public class CustomException : Exception
{
    public CustomException(string message) : base(message)
    {
    }
}
