using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Kmap : MonoBehaviour
{
    

    //list that holds all the groups in the kmap
    static List<Group> groupsList = new List<Group>();

    //number of variables in the kmap
    public static int variables;

    //this is the input field where the coordinates are inputed
    public GameObject CoordinatesInput;

    public GameObject VariablesInput;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("this is a test");
        BitOperations.fillLookupTable();

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
        //resetting group list and variables so that previous input is not saved
        groupsList.Clear();
        variables = 0;

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
                    //creates a new single gorup and adds it to the list
                    groupsList.Add(new Group((uint)coordinate,0));
                    Debug.Log(coordinate);
                }

                //resetting coordinate
                coordinate = null;
            }

        }
        //printGroupsList();
        //sorting the list 
        groupsList.Sort(new GroupComparer());
        //printGroupsList();
    }



    public static void findingPairs()
    {
        //group object which will be show which group is being searched for
        //creating object and modifying it so that a new object isn't created everytime an object is being searched for
        Group groupSearch = new Group(0, 0);

        // don't want to create new object each time
        GroupComparer GC = new GroupComparer();

        //iterating through the list
        groupsList.ForEach  (delegate (Group g){
        
            //all the neighbouring coordinates check 
            for( int i = 0; i < variables; i++)
            {
                /*
                 * a bigger group can be formed if there are two groups with
                 *      1. same number of terms
                 *      2. same direction
                 *      3. a difference in only 1 dimension of the coordinates
                */


                //this is used to check the coordinates that are one away from the coordinate of g
                uint shiftDirection = (uint) 1 << i;

                //setting the coordinate to a neighbouring coordinate in the direction of shiftDirection
                groupSearch.coordinate = g.coordinate | shiftDirection;

                //directions of both the groups should be same
                groupSearch.direction = g.direction;

                //if the coordinates didn't change it would be searching for the current group g
                if (groupSearch.coordinate == g.coordinate) continue;

                //index of the pair which can combine with g to make a bigger group
                int pairIndex = groupsList.BinarySearch(groupSearch, GC);

                //adding a new group to the list whose coordinate is the lower coordinate of the two groups
                // and whose direction is the combination of the directino of both the groups
                groupsList.Add(new Group(g.coordinate , g.direction | groupsList[pairIndex].direction));

                //incrementing bigger groups
                g.biggerGroups++;
                groupsList[pairIndex].biggerGroups++;
                }

            //TODO delete group if it meets condition
        
        });

    } 


    //prints the coordinates and directions of the groups in the list.
    public static void printGroupsList()
    {
        groupsList.ForEach(delegate (Group g) {
            Debug.Log(g.coordinate+" "+ g.direction);
        });
    }

 

}
