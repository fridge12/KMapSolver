using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//all the functions that operate on the bits of variables will be in this class
public class BitOperations : MonoBehaviour
{

    //lookup table for number of bits in uint
    static int[] BitsSetTable256 = new int[256];

    public static void fillLookupTable()
    {
        //filling values in lookup table
        BitsSetTable256[0] = 0;
        for (int i = 0; i < 256; i++)
        {
            BitsSetTable256[i] = (i & 1) +
            BitsSetTable256[i / 2];
        }
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
    public static void printBits(int n)
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
