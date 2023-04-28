using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kmap : MonoBehaviour
{
    //lookup table for number of bits in uint
    static int[] BitsSetTable256 = new int[256];

    //list that holds all the groups in the kmap
    static List<Group> groupList = new List<Group>();

    //number of variables in the kmap
     public static int variables;


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

    // Function to return the number of set bits in n
    int countSetBits(int n)
    {
        // the & 0xff is to get only the last byte
        return (BitsSetTable256[n & 0xff] +
                BitsSetTable256[(n >> 8) & 0xff] +
                BitsSetTable256[(n >> 16) & 0xff] +
                BitsSetTable256[n >> 24]);
    }

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
