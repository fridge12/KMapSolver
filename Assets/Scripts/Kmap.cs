using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Kmap : MonoBehaviour
{
    //lookup table for number of bits in uint
    static int[] BitsSetTable256 = new int[256];

    //list that holds all the groups in the kmap
    static List<Group> groupList = new List<Group>();

    //number of variables in the kmap
    public static int variables;

    //this is the input field where the coordinates are inputed
    public GameObject CoordinatesInput;

    public GameObject VariablesInput;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("this is a test");
        lookupTable();

        Group g = new Group(13, 2);
            ;
/**
        groupList.Add(new Group(16,0));
        groupList.Add(new Group(9,2));
        groupList.Add(new Group(4,3));
        groupList.Add(new Group(7,8));
        groupList.Add(new Group(0, 15));
        groupList.Add(new Group(14,1));
        groupList.Add(new Group(8,7));
        groupList.Add(new Group(5,10));
        groupList.Add(new Group(3,12));
        groupList.Add(new Group(13,2));
        groupList.Add(new Group(4,1));
        groupList.Add(new Group(4, 2));


        GroupComparer GC = new GroupComparer();
        groupList.Sort(GC);
        printGroups();

        Debug.Log("found at "+groupList.BinarySearch(g, GC));
*/
        /**
                variables = 4;
                Group ob = new Group(1, 8);
                Debug.Log(ob.reducedExpression(Group.POS));
                Debug.Log(ob.reducedExpression(Group.SOP));
*/
    }

    //this method takes input for both the number of variables and the terms of the kmap
    public void inputs()
    {
        Debug.Log("in inputs functions");
        
        //text of input field
        string vars = VariablesInput.GetComponent<InputField>().text;
        //removing all non digits characters
        vars = System.Text.RegularExpressions.Regex.Replace(vars, @"\D", "");

        //if vars is empty setting it to 0 so that it'll give 0 variables erro
        if (vars.Length == 0) vars = "0";

        variables = System.Int32.Parse(vars);

        Debug.Log("Variables" + variables);

        if (variables > Group.maxVariables)
        {
            // TODO add popup saying too many variables or something 
            Debug.Log("too many variables");
            return;
        }
        else if(variables == 0)
        {
            //TODO add popup saying 0 variables does not work
            Debug.Log("0 variables");
            return;
        }

        //
        uint maxCoordinate = (uint)System.Math.Pow(2, variables);

        //getting text of the InputField
        //the + "." is so that all the coordinates can be found in the for loop, without the "." if the string ended in digits it would lead the the last number not being found
        string inputs = CoordinatesInput.GetComponent<InputField>().text + ".";


        Debug.Log(inputs);

        //setting it null so that it doesn't detect 0 accidently and so we can use all coordinates up to 2^32 -1
        uint? coordinate = null;
        //seperating the numbers, and creating single groups with each number
        for (int i = 0; i < inputs.Length; i++)
        {
            //if current character is a digit, adding it to the current number (coordinate)
            if (System.Char.IsDigit(inputs[i]))
            {
                if(coordinate == null)
                {
                    coordinate = 0;
                }
                //adding digit to the number
                coordinate = (coordinate*10) + System.UInt32.Parse(inputs[i]+"");
            }
            //only if number exists create new single group
            else if(coordinate != null)
            {
                //only creating new gruop if coordinate is valid
                if (coordinate > maxCoordinate)
                {
                    Debug.Log("invalid input (value too high )");
                    //TODO add popup to say value too high
                }
                else
                {
                    //TODO create new single group and add to the list
                    groupList.Add(new Group((uint)coordinate,0));
                    Debug.Log(coordinate);
                }

                //resetting coordinate
                coordinate = null;
            }

        }
        printGroups();
    }

    static void lookupTable()
    {
        //filling values in lookup table
        BitsSetTable256[0] = 0;
        for (int i = 0; i < 256; i++)
        {
            BitsSetTable256[i] = (i & 1) +
            BitsSetTable256[i / 2];
        }
    }


    //prints the coordinates and directions of the groups in the list.
    public static void printGroups()
    {
        groupList.ForEach(delegate (Group g) {
            Debug.Log(g.coordinate+" "+ g.direction);
        });
    }

    // Function to return the number of set bits in n
    public static int countSetBits(int n)
    {
        // the & 0xff is to get only the last byte
        return (BitsSetTable256[n & 0xff] +
                BitsSetTable256[(n >> 8) & 0xff] +
                BitsSetTable256[(n >> 16) & 0xff] +
                BitsSetTable256[n >> 24]);
    }
    public static int countSetBits(uint n)
    {
        // the & 0xff is to get only the last byte
        return (BitsSetTable256[n & 0xff] +
                BitsSetTable256[(n >> 8) & 0xff] +
                BitsSetTable256[(n >> 16) & 0xff] +
                BitsSetTable256[n >> 24]);
    }

    //these functions print the bits of an int and uint
    public static  void printBits(int n)
    {

        //print bits

        string s = "";

        for(int i = 31; i >= 0; i--)
        {
            if(((n >> i)& 1) == 1)
            {
                s = s + "1";
            }
            else
            {
                s = s + "0";
            }
        }
        Debug.Log(s);
    }
    public static void printBits(uint n)
    {

        //print bits

        string s = "";

        for (int i = 31; i >= 0; i--)
        {
            if (((n >> i) & 1) == 1)
            {
                s = s + "1";
            }
            else
            {
                s = s + "0";
            }
        }
        Debug.Log(s);
    }

}
