// -------------------------------------------------------------------	
// Department of Electrical and Computer Engineering
// University of Waterloo
//
// Student Name:     PRAGASH SIVASUNDARAM
// Userid:           psivasun
//
// Assignment:       WEEKLY ASSIGNMENT 8
// Submission Date:  NOVERMBER 16TH 2014
// 
// I declare that, other than the acknowledgements listed below, 
// this program is my original work.
//
// Acknowledgements:
// NONE
// -------------------------------------------------------------------

using System;
using System.Collections;
using System.IO;

// Hold information about one baby name.
class BabyName
{
    string name;
    int numFemales;
    int numMales;
    
    public BabyName( string name )
    {
        if( string.IsNullOrWhiteSpace( name ) ) 
            throw new ArgumentException( "Invalid baby name", "name" );
            
        this.name = name;
        numFemales = 0;
        numMales = 0;
    }
    
    public string Name { get { return name; } }
    public int NumFemales { get { return numFemales; } }
    public int NumMales { get { return numMales; } }
    
    public void IncrementFemales( int number ) { numFemales += number; }
    public void IncrementMales( int number ) { numMales += number; }
}


// A linked list carrying data of type BabyName.
class List
{
    class Node
    {
        Node link;
        BabyName data;
        
        public Node( BabyName data ) { link = null; this.data = data; }
        
        public Node Link { get { return link; } set { link = value; } }
        public BabyName Data { get { return data; } }
    }
    
    Node tail;
    Node head;
    
    public List( ) { tail = null; head = null; }
    
    // Add new baby-name data to the linked list.
    public void Add( string name, int countFemales, int countMales )
    {
        if( string.IsNullOrWhiteSpace( name ) ) 
            throw new ArgumentException( "Invalid baby name", "name" );
         
					//---------------------------								// TO DO:  Complete this method.
		// Finding existing occurrence of baby name
		;
		bool found = false;
		if ( head != null ){
			Node current = head; 
			while ( current != null )
			{
				if ( current.Data.Name.Equals( name ) ){
					current.Data.IncrementFemales(countFemales); 
					current.Data.IncrementMales(countMales);
					found = true;
				}
				current = current.Link;
			}
		}
		
		if ( !found && head == null ){
			BabyName inHand = new BabyName( name );
			head = new Node( inHand );
			head.Data.IncrementFemales(countFemales); 
			head.Data.IncrementMales(countMales);
			tail = head;
		}
		
		else if ( !found && head != null ){
			BabyName inHand = new BabyName( name );
			tail.Link = new Node( inHand );
			tail.Link.Data.IncrementFemales(countFemales); 
			tail.Link.Data.IncrementMales(countMales);
			tail = tail.Link;
		}
    }				
    
    // Convert the linked list to an array of its data elements.
    public BabyName[ ] ToArray( )
    {
        // Collect data objects from the linked list.
        ArrayList objects = new ArrayList( );
        
        Node current = head;
        while( current != null )
        {
            objects.Add( current.Data );
            current = current.Link;
        }
        
        // Covert the collection of objects to an array of strings.
        BabyName[ ] results = new BabyName[ objects.Count ];
        
        for( int i = 0; i < results.Length; i ++ ) 
        {
            results[ i ] = ( BabyName ) objects[ i ];
        }
        
        return results;
    }
}


// Program to extract information from a file of baby-name data.
static class Program
{
    static string file = "Baby_Names__Beginning_2007.csv";
    
    static void Main( )
    {
        // Use a linked list to hold the baby-name data.
        List babies = new List( );
        
        // Read the baby-name data from the file.
        int count = 0;
        
        using( StreamReader sr = new StreamReader( file ) )
        {
            sr.ReadLine( );  // skip header line
            
            while( ! sr.EndOfStream )
            {
                string line = sr.ReadLine( );
                string[ ] words = line.Split( ',' );
                count ++;
                
                if( words.Length != 5 )
                {
                    Console.WriteLine( "{0}: {1}", count, line );
                }
                
                string year = words[ 0 ];
                string name = words[ 1 ];
                string county = words[ 2 ];
                string gender = words[ 3 ];
                int number = int.Parse( words[ 4 ] );
                
                if( gender == "M" ) babies.Add( name, 0, number );
                if( gender == "F" ) babies.Add( name, number, 0 );
            }
        }
        
        Console.WriteLine( );
        Console.WriteLine( "Read {0} records from \"{1}\"", count, file );
        
        // Display the baby-name data for names that are used for both genders.
        Console.WriteLine( );
        Console.WriteLine( 
            "The following names were popularly used for both genders:" );
            
        foreach( BabyName baby in babies.ToArray( ) )
        if( baby.NumFemales > 0 && baby.NumMales > 0 )
        {
            Console.WriteLine( "  {0}: {1} F, {2} M", 
                baby.Name, baby.NumFemales, baby.NumMales );
        }
    }
}