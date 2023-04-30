using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Group : MonoBehaviour
{

    public readonly static string POS = "POS";

    public readonly static string SOP = "SOP";

    //number of bits in coordinate variable
    public readonly static int maxVariables = 32;

    //binary of this representes the coordinates of the group
    public uint coordinate;
    //binary of this represents the direction of the group
    public uint direction;
    //number of bigger groups this group is a part of
    public uint biggerGroups;

    //probably won't use these 2
    public uint subGroup1Index;
    public uint subGroup2Index;

    //all the info is there, including subgroups which i might not use
    public Group(uint coordinate, uint direction, uint biggerGroups, uint subGroup1Index,uint subGroup2Index)
    {
        this.coordinate = coordinate;
        this.direction = direction;
        this.biggerGroups = biggerGroups;
        this.subGroup1Index = subGroup1Index;
        this.subGroup2Index = subGroup2Index;
    }
    //if i am going to use subgroups, then I will use this to create a new Group
    public Group(uint coordinate, uint direction, uint subGroup1Index, uint subGroup2Index) : this(coordinate, direction, 0, subGroup1Index, subGroup2Index)
    {}


    //this is the base constructor if I don't use subgroups
    public Group(uint coordinate, uint direction,uint biggerGroups)
    {
        this.coordinate = coordinate;
        this.direction = direction;
        this.biggerGroups = biggerGroups;
    }
    //if I don't use subgroups, I will use this to create a new Group
    public Group(uint coordinate, uint direction) : this(coordinate, direction, 0)
    {
    }

    public string reducedExpression(string expressionType)
    { 
        string exp = "";
        Debug.Log("reducing expresion");
        
        
        for(int i = Kmap.variables-1; i >= 0; i--)
        {

            //    0 0 0 0   corresponding bits
            //    A B C D   variables
            //the variable is present in the final expression if the corresponding bit is 0 
            //the corresponding bit shows the value of the variable at the coordinate

            if (((direction >> i) & 1) == 0)
            {

                //might need to put the expression in brackets
                if(expressionType == POS)
                {
                    //if it is a pos expression adding + in between the variables
                    if (exp.Length > 0)
                    {
                        exp = exp + "+";
                    }

                    // adding variable to the expression
                    exp = exp + (char)(64 + Kmap.variables - i);

                    //if the value of the bit corresponding to the variable is 1 complementing it
                    if (((coordinate >> i) & 1) == 1)
                    {
                        exp = exp + "'";
                    }
                }
                else if(expressionType == SOP)
                {
                    //adding variable to the expression
                    exp = exp + (char)(64 + Kmap.variables - i);

                    //if the value of the bit corresponding to the variable is 0 complementing it
                    if (((coordinate >> i) & 1) == 0)
                    {
                        exp = exp + "'";
                    }
                }
                else
                {
                    throw new System.ArgumentException();
                }
            }
        }

        return exp;
    }

}
