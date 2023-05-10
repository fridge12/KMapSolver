using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Kmap : MonoBehaviour
{
    
    //if false this doesn't allow debug statements to print
    public static readonly bool toPrint = false;
    //keep false unless debugging as it adds way to much time for any slightly big input


    //list that holds all the groups in the kmap
    public static List<Group> groupsList = new List<Group>();

    //number of variables in the kmap
    public static int variables;
    //highest value for a term
    public static uint maxCoordinate;

    public static string SOP_POS ="";

    //this is the input field where the coordinates are inputed
    public GameObject CoordinatesInput;

    //input field where number of variables are inputted
    public GameObject VariablesInput;

    //dropdown menu to select SOP or POS
    public GameObject Dropdown;

    //this will be either ∑ or π depending on whether SOP or POS is selected
    public GameObject termsText;

    //text object in which the final reduced expression will be displayed
    public GameObject ReducedExpressionOutput;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("this is a test");
        BitOperations.fillLookupTable();

    }

    void Update()
    {
        SOP_POS = Dropdown.GetComponent<Dropdown>().captionText.text;
        if (SOP_POS == Group.POS)
            termsText.GetComponent<Text>().text = "π";
        else
            termsText.GetComponent<Text>().text = "∑";
    }

    public void onInputButtonClick()
    {
        //for timing each method
        var watch = System.Diagnostics.Stopwatch.StartNew();
       
        
        /*
         * for(int i =0;i<100000;i++)
        if(toPrint)Debug.Log("this is another test");
        watch.Stop();
        Debug.Log("time test" + watch.ElapsedMilliseconds);
        watch.Reset();
        watch.Restart();
        */


        //taking and sorting inputs
        inputs();
        watch.Stop();
        Debug.Log("time input"+ watch.ElapsedMilliseconds);

        watch.Reset();
        watch.Restart();
        //finding pairs
        findingPairs();
        watch.Stop();
        Debug.Log("time pairs" + watch.ElapsedMilliseconds);


        watch.Reset();
        watch.Restart();
        //calling method to delete redundant groups
        deleteRedundantGroups();
        watch.Stop();
        Debug.Log("time redundant" + watch.ElapsedMilliseconds);

        watch.Reset();
        watch.Restart();
        //to print the final reduced expression

        ReducedExpressionOutput.GetComponent<Text>().text = reducedExpression();
        watch.Stop();
        Debug.Log("time reduced" + watch.ElapsedMilliseconds);
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

        // highest value for a term
        maxCoordinate = (uint)System.Math.Pow(2, variables)-1;

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
                    if (toPrint) Debug.Log(coordinate);
                }

                //resetting coordinate
                coordinate = null;
            }

        }
        //printGroupsList();
        //sorting the list 
        groupsList.Sort(new GroupComparer());
        Debug.Log("groups list after input and sorting");
        //printGroupsList();

        //getting the selection made by the user
        //have to do captionText.text because captionText returns the text component and not a string of text 
        


    }


    //creates groups
    public static void findingPairs()
    {
        Debug.Log("finding pairs");

        //group object which will be show which group is being searched for
        //creating object and modifying it so that a new object isn't created everytime an object is being searched for
        Group groupSearch = new Group(0, 0);

        // don't want to create new object each time
        GroupComparer GC = new GroupComparer();

        Group g;
        Group gPair;
        //index of g
        int index = 0;

        int size = groupsList.Count;

        //iterating through the list
        for(index =0;index < size;index++){
            g = groupsList[index];

            //all the neighbouring coordinates check 
            for (int i = 0; i < variables; i++)
            {
                /*
                 * a bigger group can be formed if there are two groups with
                 *      1. same number of terms
                 *      2. same direction
                 *      3. a difference in only 1 dimension of the coordinates
                */


                //this is used to check the coordinates that are one away from the coordinate of g
                uint shiftDirection = (uint)1 << i;

                //setting the coordinate to a neighbouring coordinate in the direction of shiftDirection
                groupSearch.coordinate = g.coordinate | shiftDirection;

                //directions of both the groups should be same
                groupSearch.direction = g.direction;


                //if the coordinates didn't change it would be searching for the current group g, so skipping this iteration
                if (groupSearch.coordinate == g.coordinate) { continue; }

                //index of the pair which can combine with g to make a bigger group
                int pairIndex = groupsList.BinarySearch(groupSearch, GC);

                //if pair does not exist go to the top of the loop
                if (pairIndex < 0) { continue; }



                groupSearch.coordinate = g.coordinate;
                groupSearch.direction = g.direction | shiftDirection;

                if (toPrint) Debug.Log("found pair");
                //groupSearch.printGroup();
                //if a duplicate group does not exist then
                if (groupsList.BinarySearch(groupSearch, GC) < 0)
                {
                    //adding a new group to the list whose coordinate is the lower coordinate of the two groups
                    // and whose direction is the combination of the direction of both the groups
                    groupsList.Add(new Group(g.coordinate, g.direction | shiftDirection));
                    size++;
                    groupSearch.updateBiggerGroups(+1);

                }

                //incrementing biggerGroups, of groups with more than one term
                //because singles are updated whenever a bigger group is created with the updateBiggerGroups function
                //so if we were to increment biggerGroups for singles we would be double counting groups
                if (BitOperations.countSetBits(g.direction) > 0) { 
                    g.biggerGroups++;
                    groupsList[pairIndex].biggerGroups++;
                }

            }

            //if it is not a single and part of a bigger group, deleting the group
            if (g.direction != 0 && g.biggerGroups > 0)
                {
                g.updateBiggerGroups(-1);
                groupsList.RemoveAt(index);
                index--;
                size--;

                //Debug.Log("final value of group");
                //g.printGroup();
                }
            
        }
        //Debug.Log("groups list after finding pairs");
        //printGroupsList();
        //printGroupsListWithoutRedundantGroups();
        
        
    } 

    public static void deleteRedundantGroups()
    {
        Debug.Log("deleting redundant groups");

        Group g;

        //index of g
        int index = 0;

        int size = groupsList.Count;
        //iterating through the list
        for (index = 0; index < size; index++)
        {
            g = groupsList[index];

            //a single is not redundant
            if (BitOperations.countSetBits(g.direction) == 0) continue;

            //if group is redundant deleting it
            if (g.isRedundant())
            {
                
                //Debug.Log("found redundant");
                //g.printGroup();
                
                //deleting group and updating the size and index to match
                groupsList.RemoveAt(index);
                size--;
                index--;
            }

        }
        //Debug.Log("groups list after removing redundant groups");
        //printGroupsList();
        //printGroupsListWithoutRedundantGroups();

        
    }


    public static string reducedExpression()
    {
        printGroupsListWithoutRedundantGroups();
        groupsList.Sort(new GroupComparer());

        //using string builder because when using a string, every modification creates a new string
        System.Text.StringBuilder expression = new System.Text.StringBuilder("");

        //tell us whether to include + or . before a term
        bool isNotStart = false;

        Group g;

        //index of g
        int index = 0;

        int size = groupsList.Count;
        //iterating through the list
        for (index = 0; index < size; index++)
        {
            g = groupsList[index];
            //if the group is a part of a bigger group then its reduced expression is not to be included
            if (g.biggerGroups != 0) continue;


            if (SOP_POS.Equals(Group.SOP))
            {
                if (isNotStart) expression.Append(" + ");
                expression.Append(g.reducedExpression(Group.SOP));
                isNotStart = true;
            }
            else
            {
                if (isNotStart) expression.Append(" . ");
                expression.Append("(" + g.reducedExpression(Group.POS) + ")");
                isNotStart = true;
                
            }

        }
        Debug.Log(expression);
        return expression.ToString();
    }


    //prints the coordinates and number of terms of the groups in the list.
    public static void printGroupsList()
    {
        Debug.Log("Groups list with redundant groups");
        groupsList.ForEach(delegate (Group g) {
            g.printGroup();
        });
    }

    //prints the coordinates and directions of the groups in the list.
    public static void printGroupsListWithoutRedundantGroups()
    {
        Debug.Log("groups list without redundant groups");
        groupsList.ForEach(delegate (Group g) {
            if(g.biggerGroups == 0) g.printGroup();
        });
    }

}
