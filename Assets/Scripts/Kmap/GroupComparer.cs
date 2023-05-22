using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//need to create a new class because if I didn't then List.BinarySearch wouldn't work
public class GroupComparer : IComparer<Group>
{

    /*
     * sorts the list according to 
            1. number of terms in a group
            2. coordinate of group
            3. direction of group
     */
    
    //this method compares 2 groups, and returns value > 0 if x > y
    public int Compare(Group x, Group y)
    {
        //in a group 2^(number of set bits in the direction) gives the number of terms in a group
        //therefore greater the set bits, greater the number of terms in the group, greater the group
        if (BitOperations.countSetBits(x.direction) > BitOperations.countSetBits(y.direction))
        {
            return 1;
        }
        else if (BitOperations.countSetBits(x.direction) < BitOperations.countSetBits(y.direction))
        {
            return -1;
        }
        else
        {
            //the group with the greater coordinate is greater

            if (x.coordinate > y.coordinate)
            {
                return 1;
            }
            else if (x.coordinate < y.coordinate)
            {
                return -1;
            }
            else
            {
                //the greater the direction the greater the group

                if (x.direction > y.direction)
                {
                    return 1;
                }
                else if (x.direction < y.direction)
                {
                    return -1;
                }
                else
                {
                    return 0;
                }

            }
        }
    }
}
