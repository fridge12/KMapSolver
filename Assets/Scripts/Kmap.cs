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

    public GameObject KmapInputs;

    // Start is called before the first frame update
    void Start()
    {
        //filling values in lookup table
        BitsSetTable256[0] = 0;
        for (int i = 0; i < 256; i++)
        {
            BitsSetTable256[i] = (i & 1) +
            BitsSetTable256[i / 2];
        }

        Debug.Log("this is a test");

        // TODO: take input for variables


        variables = 4;
        Group ob = new Group(1, 8);

        Debug.Log(ob.reducedExpression(Group.POS));
        Debug.Log(ob.reducedExpression(Group.SOP));


    }

    public void inputs()
    {
        Debug.Log("in inputs functions");

        //getting text of the InputField
        string inputs = KmapInputs.GetComponent<InputField>().text;
        //this is so that all the coordinates can be found in the for loop, without the "." if the string ended in digits it would lead the the last number not being found
        inputs = inputs + ".";
        Debug.Log(inputs);

        //setting it null so that it doesn't detect 0 accidently and so we can use all coordinate up to 2^32 -1
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
                //TODO create new single group and add to the list

                Debug.Log(coordinate);
                coordinate = null;
            }

        }
    }



    // Function to return the number of set bits in n
    static int countSetBits(int n)
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
