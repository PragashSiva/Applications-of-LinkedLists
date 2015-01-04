// -------------------------------------------------------------------	
// Department of Electrical and Computer Engineering
// University of Waterloo
//
// Student Name:     PRAGASH SIVASUNDARAM
// Userid:           psivasun
//
// Assignment:       PROGRAMMING ASSIGNMENT 4
// Submission Date:  NOVERMBER 22ND 2014
// 
// I declare that, other than the acknowledgements listed below, 
// this program is my original work.
//
// Acknowledgements:
// NONE
// -------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;

// -----------------------------------------------------------------------------
// A Drug object holds information about one fee-for-service outpatient drug 
// reimbursed by Medi-Cal (California's Medicaid program) to pharmacies.
class Drug
{
    string name;            // brand name, strength, dosage form
    string id;              // national drug code number
    double size;            // package size
    string unit;            // unit of measurement
    double quantity;        // number of units dispensed
    double lowest;          // price Medi-Cal is willing to pay
    double ingredientCost;  // estimated ingredient cost
    int numTar;             // number of claims with a treatment auth. request
    double totalPaid;       // total amount paid
    double averagePaid;     // average paid per prescription
    int daysSupply;         // total days supply
    int claimLines;         // total number of claim lines

    // Properties providing read-only access to every field.
    public string Name { get { return name; } }
    public string Id { get { return id; } }
    public double Size { get { return size; } }
    public string Unit { get { return unit; } }
    public double Quantity { get { return quantity; } }
    public double Lowest { get { return lowest; } }
    public double IngredientCost { get { return ingredientCost; } }
    public int NumTar { get { return numTar; } }
    public double TotalPaid { get { return totalPaid; } }
    public double AveragePaid { get { return averagePaid; } }
    public int DaysSupply { get { return daysSupply; } }
    public int ClaimLines { get { return claimLines; } }

    public Drug(string name, string id, double size, string unit,
        double quantity, double lowest, double ingredientCost, int numTar,
        double totalPaid, double averagePaid, int daysSupply, int claimLines)
    {
        this.name = name;
        this.id = id;
        this.size = size;
        this.unit = unit;
        this.quantity = quantity;
        this.lowest = lowest;
        this.ingredientCost = ingredientCost;
        this.numTar = numTar;
        this.totalPaid = totalPaid;
        this.averagePaid = averagePaid;
        this.daysSupply = daysSupply;
        this.claimLines = claimLines;
    }

    // Simple string for debugging purposes, showing only selected fields.
    public override string ToString()
    {
        return string.Format(
            "{0}: {1}, {2}", id, name, size);
    }
}

// -----------------------------------------------------------------------------
// Linked list of Drugs.  A list object holds references to its head and tail
// Nodes and a count of the number of Nodes.
class DrugList
{
    // Nodes form the singly linked list.  Each node holds one Drug item.
    class Node
    {
        Node next;
        Drug data;

        public Node(Drug data) { next = null; this.data = data; }

        public Node Next { get { return next; } set { next = value; } }
        public Drug Data { get { return data; } }
    }

    Node tail;
    Node head;
    int count;

    public int Count { get { return count; } }

    // Constructors:
    public DrugList() { tail = null; head = null; count = 0; }
    public DrugList(string drugFile) : this() { AppendFromFile(drugFile); }

    // Methods which add elements:
    // Build this list from a specified drug file.
    public void AppendFromFile(string drugFile)
    {
        using (StreamReader sr = new StreamReader(drugFile))
        {
            while (!sr.EndOfStream)
            {
                string line = sr.ReadLine();

                // Extract drug information
                string name = line.Substring(7, 30).Trim();
                string id = line.Substring(37, 13).Trim();
                string temp = line.Substring(50, 14).Trim();
                double size
                    = double.Parse(temp.Substring(0, temp.Length - 2));
                string unit = temp.Substring(temp.Length - 2, 2);
                double quantity = double.Parse(line.Substring(64, 16));
                double lowest = double.Parse(line.Substring(80, 10));
                double ingredientCost
                    = double.Parse(line.Substring(90, 12));
                int numTar = int.Parse(line.Substring(102, 8));
                double totalPaid = double.Parse(line.Substring(110, 14));
                double averagePaid = double.Parse(line.Substring(124, 10));
                int daysSupply
                    = (int)double.Parse(line.Substring(134, 14));
                int claimLines = int.Parse(line.Substring(148));

                // Put drug onto this list of drugs.
                Append(new Drug(name, id, size, unit, quantity, lowest,
                    ingredientCost, numTar, totalPaid, averagePaid,
                    daysSupply, claimLines));
            }
            Console.WriteLine(count);
        }
    }

    // Add a new Drug item to the end of this linked list.
    public void Append(Drug data)
    {
		// In case data is null
		if (data == null)
			return; 
			
        // If the list is empty 
        if (head == null)
        {
            head = new Node(data);
            tail = head;
        }

        // If the list is not empty 
        else
        {
            tail.Next = new Node(data);
            tail = tail.Next;
        }

        // Add to the counter
        count++;
    }

    // Add a new Drug in order based on a user-supplied comparison method.
    // The new Drug goes just before the first one which tests greater than it.
    public void InsertInOrder(Drug data, Func<Drug, Drug, int> userCompare)
    {
        Node current = head;
        Node previous = null;

        // Runs until tail is reached
        while ( data!= null )
        {

            // Inserting into an empty list (initiates head)
            if (head == null)
            {
                head = new Node(data);
                tail = head;
            }

            // Inserting at the beginning of the list (prepends)
            else if (userCompare(data, current.Data) < 0 && current == head)
            {
                Node node = new Node(data);
                node.Next = head;
                head = node;
            }

            // Inserting in the list body
            else if (userCompare(data, current.Data) < 0)
            {
                Node node = new Node(data);
                previous.Next = node;
                node.Next = current;
            }

            // Inserting at the end of the list (appends)
            else if (current == tail)
            {
                tail.Next = new Node(data);
                tail = tail.Next;
            }

            // Continues onto the next element  
            else
            {
                previous = current;
                current = current.Next;
                // Go to next element
                continue;
            }

            // Adds to the counter
            count++;
            break;
        }
    }

    // Methods which remove elements:
    // Remove the first Drug.
    public Drug RemoveFirst()
    {
        // Holds the removed head
        Node first = null;

        // Removes the head from the list
        if (head != null)
        {
            first = head;
            head = head.Next;
        }

        // Decrements the counter by 1
        count--;
        return first.Data;
    }

    // Remove the minimal Drug based on a user-supplied comparison method.
    public Drug RemoveMin(Func<Drug, Drug, int> userCompare)					
    {
        Node current = head;		// Traverses list -- keeping current element
        Node previous = null; 		// Traverses list -- keeping previous element
        Node minimal = head;		// Archives the minimal element
        Node beforeMinimal = null;	// Archives the element before minimal 

        // Executes until the end of the list 
        while (current != null)
        {
            // If current is smaller than the archived minimal
            if (userCompare(current.Data, minimal.Data) < 0)
            {
                minimal = current;
                beforeMinimal = previous;
            }

            // Moves onto the next element 
            previous = current;
            current = current.Next;
        }

        // If the minimal is the head
        if (head == minimal) { head = minimal.Next; }

        // If the minimal is the tail
        else if (tail == minimal) { tail = beforeMinimal; }

        // If the minimal is in the body 
        else { beforeMinimal.Next = minimal.Next; }

        // Decrements the counter
        count--;
        return minimal.Data;
    }

    // Methods which sort the list:
    // Sort this list by selection sort with a user-specified comparison method.
    public void SelectSort(Func<Drug, Drug, int> userCompare)				
    {
        // Creates a new list to store the sorted elements
        DrugList Sorted = new DrugList();

        // Removes the minimal element from unsorted and appends to sorted
        while (count != 0)
        {
            Sorted.Append(RemoveMin(userCompare));
        }

        // Allocates the sorted list as the original(unsorted) list
        this.head = Sorted.head;
        this.tail = Sorted.tail;
        this.count = Sorted.count;
    }

    // Sort this list by insertion sort with a user-specified comparison method.
    public void InsertSort(Func<Drug, Drug, int> userCompare)				
    {
        // Creates a new list to store unsorted values
        DrugList unsorted = new DrugList();

        // Allocates the original list as the new unsorted list
        unsorted.head = this.head;
        unsorted.tail = this.tail;
        unsorted.count = this.count;

        // Detaches all elements from the original list
        count = 0;
        head = null;
        tail = null;

        // Takes first element from unsorted and adds to original list
        while (unsorted.count != 0)
        {
            InsertInOrder(unsorted.RemoveFirst(), userCompare);
        }
    }

    // Methods which extract the Drugs:
    // Return, as an array, references to all the Drug objects on the list.
    public Drug[] ToArray()
    {
        Drug[] result = new Drug[count];
        int nextIndex = 0;
        Node current = head;
        while (current != null)
        {
            result[nextIndex] = current.Data;
            nextIndex++;
            current = current.Next;
        }
        return result;
    }

    // Return, as an array, references to those Drub objects on the list meeting 
    // a condition specified by a user-supplied method.
    public Drug[] ToArray(Func<Drug, bool> userTest)
    {
        // Count the number of elements meeting the condition.
        int number = 0;
        foreach (Drug d in Enumeration) if (userTest(d)) number++;

        // Collect the elements meeting the condition.
        Drug[] result = new Drug[number];
        int nextIndex = 0;
        foreach (Drug d in Enumeration)
        {
            if (userTest(d))
            {
                result[nextIndex] = d;
                nextIndex++;
            }
        }
        return result;
    }

    // Return a collection of references to the Drug items on this list which 
    // can be used in a foreach loop.  Understanding enumerations and the 
    // 'yield return' statement is beyond the scope of the course.
    public IEnumerable<Drug> Enumeration
    {
        get
        {
            Node current = head;
            while (current != null)
            {
                yield return current.Data;
                current = current.Next;
            }
        }
    }
}

// -----------------------------------------------------------------------------
// Test the linked list of Drugs.
static class Program
{
    static void Main()
    {
		// Test initialization 
		DrugList test = new DrugList( );
		
		// Test drugs
		Drug d1 = new Drug( "a", "1", 0, null, 0, 0, 0, 0, 0, 0, 0, 0 );
		Drug d2 = new Drug( "b", "2", 0, null, 0, 0, 0, 0, 0, 0, 0, 0 );
		Drug d3 = new Drug( "c", "3", 0, null, 0, 0, 0, 0, 0, 0, 0, 0 );
		Drug d4 = new Drug( "c", "4", 0, null, 0, 0, 0, 0, 0, 0, 0, 0 );
		
		// Testing Append Method
		test.Append( null );
		test.Append( d3 );
		test.Append( d2 );

		// Testing Sorting Method
		test.SelectSort( CompareByName );

		
		// Test result printing to console
		foreach( Drug d in test.Enumeration ) Console.WriteLine( d );
		
		Console.WriteLine( "test.Count = {0}", test.Count );
		
		
    }

    static int CompareByName(Drug lhs, Drug rhs)
    { return lhs.Name.CompareTo(rhs.Name); }

    static bool TestByName(Drug d)
    { return d.Name.Contains("CORT"); }
}