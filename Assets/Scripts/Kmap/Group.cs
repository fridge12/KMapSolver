using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Group
{

    public readonly static string POS = "POS";

    public readonly static string SOP = "SOP";

    private static readonly bool toPrint = false;

    //theoretically it should be the number of non sign bits in coordinate variable
    //but because the List<T> class methods use int as a return type the max is 31
    public readonly static int maxVariables = 31;

    //binary of this representes the coordinates of the group
    //each bit is a different dimension
    public uint coordinate;
    //binary of this represents the direction of the group
    public uint direction;
    //number of bigger groups this group is a part of
    public uint biggerGroups;

    public Color borderColour;

    public Color previousBorderColour = Color.black;

    //this is the base constructor if I don't use subgroups
    public Group(uint coordinate, uint direction,uint biggerGroups, Color borderColour)
    {
        this.coordinate = coordinate;
        this.direction = direction;
        this.biggerGroups = biggerGroups;
        this.borderColour = borderColour;

    }

    public Group(uint coordinate, uint direction, uint biggerGroups) : this(coordinate, direction, biggerGroups, new Color32())
    { }

    //if I don't use subgroups, I will use this to create a new Group
    public Group(uint coordinate, uint direction) : this(coordinate, direction, 0)
    {
    }

    //returnes the reduced expression of the group
    public string reducedExpression(string expressionType)
    {
        System.Text.StringBuilder exp = new System.Text.StringBuilder("");
        //Debug.Log("reducing expresion");

        if (direction == Kmap.maxCoordinate) return "1";

        for(int i = Kmap.variables-1; i >= 0; i--)
        {

            //    0 0 0 0   corresponding bits (direction)
            //    A B C D   variables
            //the variable is present in the final expression if the corresponding bit in direction is 0 
            //the corresponding bit in direction shows the value of the variable at the coordinate

            if (((direction >> i) & 1) == 0)
            {

                //might need to put the expression in brackets
                if(expressionType == POS)
                {
                    //if it is a pos expression adding + in between the variables
                    if (exp.Length > 0)
                    {
                        exp = exp.Append("+");
                    }

                    // adding variable to the expression
                    exp = exp.Append( (char)(64 + Kmap.variables - i));

                    //if the value of the bit corresponding to the variable is 1 complementing it
                    if (((coordinate >> i) & 1) == 1)
                    {
                        exp = exp.Append("'");
                    }
                }
                else if(expressionType == SOP)
                {
                    //adding variable to the expression
                    exp = exp.Append((char)(64 + Kmap.variables - i));

                    //if the value of the bit corresponding to the variable is 0 complementing it
                    if (((coordinate >> i) & 1) == 0)
                    {
                        exp = exp.Append("'");
                    }
                }
                else
                {
                    throw new System.ArgumentException();
                }
            }
        }

        //returning reduced expression
        return exp.ToString();
    }

    public bool isRedundant()
    {
        bool returnVal = true ; 
        if (toPrint) Debug.Log("in isRedundant");
        //printGroup();

        Group groupSearch = new Group(0, 0);
        GroupComparer GC = new GroupComparer();

        /*
         *the coordinates of all the singles in a group can be found with
         *          coordinate | ((direction -1) & direction)
         *          coordinate | ((((direction -1) & direction) -1 ) & direction)
         *                      .
         *                      .
         *                      .
         *                      .
         *          repeat n times
         *          n = number of terms in the group
         */


        uint currentDirection = direction;
        //loops as many times as there are terms in the group
        for (int i =0;i<System.Math.Pow(2, BitOperations.countSetBits(direction)); i++)
        {
            currentDirection = (currentDirection - 1) & direction;
            groupSearch.coordinate = coordinate | currentDirection;

            //Debug.Log(BitOperations.printBits(groupSearch.coordinate));
            if (toPrint) Kmap.groupsList[Kmap.groupsList.BinarySearch(groupSearch, GC)].printGroup();
            //if any term is not a part of another bigger group then it means this group is not redundant 

            Group g = Kmap.groupsList[Kmap.groupsList.BinarySearch(groupSearch, GC)];
            g.borderColour = Color.HSVToRGB(((float)Kmap.numberOfGroupsWithoutAssignedColour / (float)Kmap.numberOfGroups), 1, 1, true);
            if (g.previousBorderColour.Equals(Color.black)) g.previousBorderColour = g.borderColour;            

            Debug.Log("colour");
            Debug.Log(g.borderColour);
            if (g.biggerGroups <= 1)returnVal = false; 
        }
        if (!returnVal) return false;
        //if it reaches till here, it means that the group is redundant and will be deleted
        //if the group is deleted all the terms will be a part of 1 less bigger group
        //the function updates all the terms, and reduces the bigger groups they are a part of

        updateBiggerGroups(-1);
        return true;
        
    }
    //this function will update the biggerGroup values for all singles in the group
    //val is the amount by which the biggerGroup of each single is changed 
    public void updateBiggerGroups(int val)
    {
        if (toPrint) Debug.Log("in updating biggerr groups " + val);
        //Kmap.printGroupsList();

        Group groupSearch = new Group(0, 0);
        GroupComparer GC = new GroupComparer();
        Group g;
        
        uint currentDirection = direction;
        //loops as many times as there are terms in the group
        for (int i = 0; i < System.Math.Pow(2, BitOperations.countSetBits(direction)); i++)
        {
            currentDirection = (currentDirection - 1) & direction;
            groupSearch.coordinate = coordinate | currentDirection;
            //Debug.Log(BitOperations.printBits(groupSearch.coordinate));


            g = Kmap.groupsList[Kmap.groupsList.BinarySearch(groupSearch, GC)];
            //updating the number of bigger groups the term is a part of
            if (val < 0)
            {
                g.borderColour = g.previousBorderColour;
                //previousBorderColour = Color.black;
                //g.borderColour = insert border colour here
                g.biggerGroups -= (uint)System.Math.Abs(val);
            }
            else
            {
                //g.borderColour = insert border colour here
                g.biggerGroups += (uint)System.Math.Abs(val);
            }
            
        }
        //Debug.Log("groups list aftre updating bigger groups");
        //Kmap.printGroupsList();
    }


    public string terms()
    {
        System.Text.StringBuilder terms = new System.Text.StringBuilder("");

        bool isNotFirst = false;

        uint currentDirection = direction;
        //loops as many times as there are terms in the group
        for (int i = 0; i < System.Math.Pow(2, BitOperations.countSetBits(direction)); i++)
        {
            if (isNotFirst) terms.Append(", ");
            terms.Append(coordinate | currentDirection);
            isNotFirst = true;
            currentDirection = (currentDirection - 1) & direction;
            
        }

        return terms.ToString();
    }

    public void printGroup()
    {
        Debug.Log(coordinate + " " + BitOperations.printBits(direction) + " "+ biggerGroups);
    }
    
    

}
