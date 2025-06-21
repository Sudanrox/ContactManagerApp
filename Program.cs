using System;
using System.Collections.Generic;
using System.IO;

class Contact {
    public string Name { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public string Address { get; set; }

    public Contact(string name, string phone, string email, string address) {
        Name = name;
        Phone = phone;
        Email = email;
        Address = address;
    }

    public override string ToString() {
        return $"Name: {Name}, Phone: {Phone}, Email: {Email}, Address: {Address}";
    }
}

class Program {
    static List<Contact> contacts = new List<Contact>();
    static string dataFile = "contacts.txt";

    static void Main() {
        // Load existing contacts from file (if the file exists)
        if (File.Exists(dataFile)) {
            string[] lines = File.ReadAllLines(dataFile);
            foreach (string line in lines) {
                if (string.IsNullOrWhiteSpace(line)) continue;
                string[] parts = line.Split('|');
                if (parts.Length == 4) {
                    Contact c = new Contact(parts[0], parts[1], parts[2], parts[3]);
                    contacts.Add(c);
                }
            }
        }

        bool running = true;
        while (running) {
            Console.WriteLine("\nContact Manager Menu:");
            Console.WriteLine("1. Add New Contact");
            Console.WriteLine("2. View All Contacts");
            Console.WriteLine("3. Search Contacts by Name");
            Console.WriteLine("4. Edit a Contact");
            Console.WriteLine("5. Delete a Contact");
            Console.WriteLine("6. Exit");
            Console.Write("Enter your choice: ");
            string choice = Console.ReadLine();
            switch (choice) {
                case "1":
                    AddContact();
                    break;
                case "2":
                    ListContacts();
                    break;
                case "3":
                    SearchContacts();
                    break;
                case "4":
                    EditContact();
                    break;
                case "5":
                    DeleteContact();
                    break;
                case "6":
                    running = false;
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please enter a number 1-6.");
                    break;
            }
        }

        // Save contacts to file before exiting
        SaveContactsToFile();
        Console.WriteLine("Exiting program. Goodbye!");
    }

    static void AddContact() {
        Console.WriteLine("\n--- Add New Contact ---");
        Console.Write("Name: ");
        string name = Console.ReadLine();
        Console.Write("Phone: ");
        string phone = Console.ReadLine();
        Console.Write("Email: ");
        string email = Console.ReadLine();
        Console.Write("Address: ");
        string address = Console.ReadLine();
        Contact newContact = new Contact(name, phone, email, address);
        contacts.Add(newContact);
        Console.WriteLine("Contact added successfully.");
    }

    static void ListContacts() {
        Console.WriteLine("\n--- Contact List ---");
        if (contacts.Count == 0) {
            Console.WriteLine("No contacts available.");
        } else {
            for (int i = 0; i < contacts.Count; i++) {
                Console.WriteLine($"{i+1}. {contacts[i]}");
            }
        }
    }

    static void SearchContacts() {
        Console.WriteLine("\n--- Search Contacts ---");
        Console.Write("Enter name to search: ");
        string query = Console.ReadLine().ToLower();
        bool found = false;
        foreach (Contact c in contacts) {
            if (c.Name.ToLower().Contains(query)) {
                Console.WriteLine(c);
                found = true;
            }
        }
        if (!found) {
            Console.WriteLine("No contacts found matching that name.");
        }
    }

    static void EditContact() {
        Console.WriteLine("\n--- Edit Contact ---");
        Console.Write("Enter the name of the contact to edit: ");
        string nameToEdit = Console.ReadLine();
        // Find the contact by name (case-insensitive match)
        Contact contactToEdit = null;
        foreach (Contact c in contacts) {
            if (c.Name.Equals(nameToEdit, StringComparison.OrdinalIgnoreCase)) {
                contactToEdit = c;
                break;
            }
        }
        if (contactToEdit == null) {
            Console.WriteLine("Contact not found.");
            return;
        }
        // Display current contact info and prompt for new values
        Console.WriteLine($"Editing Contact: {contactToEdit}");
        Console.Write("Enter new name (or press Enter to keep current): ");
        string newName = Console.ReadLine();
        Console.Write("Enter new phone (or press Enter to keep current): ");
        string newPhone = Console.ReadLine();
        Console.Write("Enter new email (or press Enter to keep current): ");
        string newEmail = Console.ReadLine();
        Console.Write("Enter new address (or press Enter to keep current): ");
        string newAddress = Console.ReadLine();
        // Update fields if new values provided
        if (!string.IsNullOrWhiteSpace(newName))   contactToEdit.Name = newName;
        if (!string.IsNullOrWhiteSpace(newPhone))  contactToEdit.Phone = newPhone;
        if (!string.IsNullOrWhiteSpace(newEmail))  contactToEdit.Email = newEmail;
        if (!string.IsNullOrWhiteSpace(newAddress)) contactToEdit.Address = newAddress;
        Console.WriteLine("Contact updated successfully.");
    }

    static void DeleteContact() {
        Console.WriteLine("\n--- Delete Contact ---");
        Console.Write("Enter the name of the contact to delete: ");
        string nameToDelete = Console.ReadLine();
        // Find the contact by name (case-insensitive match)
        Contact contactToRemove = null;
        foreach (Contact c in contacts) {
            if (c.Name.Equals(nameToDelete, StringComparison.OrdinalIgnoreCase)) {
                contactToRemove = c;
                break;
            }
        }
        if (contactToRemove == null) {
            Console.WriteLine("Contact not found.");
        } else {
            contacts.Remove(contactToRemove);
            Console.WriteLine("Contact deleted successfully.");
        }
    }

    static void SaveContactsToFile() {
        try {
            List<string> lines = new List<string>();
            foreach (Contact c in contacts) {
                // Save each contact in "Name|Phone|Email|Address" format
                lines.Add($"{c.Name}|{c.Phone}|{c.Email}|{c.Address}");
            }
            File.WriteAllLines(dataFile, lines);
        } catch (Exception e) {
            Console.WriteLine("Error saving contacts to file: " + e.Message);
        }
    }
}
