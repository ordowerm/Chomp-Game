using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputScript : MonoBehaviour
{
    //user array indices to get player number
    public KeyCode[] left;
    public KeyCode[] right;
    public KeyCode[] jump;
    public KeyCode[] special;
    public KeyCode[] start;


    /*
     * This function takes a player's id number.
     * Using the player's ID number, access the corresponding assigned input
     * Return the direction of the player's horizontal movement
     * -1 means left. +1 means right.
     */
    public int GetAxis(int player_id)
    {
        int val = 0;
        if (Input.GetKey(left[player_id]))
        {
            val -= 1;
        }

        if (Input.GetKey(right[player_id]))
        {
            val += 1;
        }

        return val;
    }
    //returns whether a player's jump button is pushed down
    public bool GetJump(int player_id)
    {
        return Input.GetKeyDown(jump[player_id]);
    }


}
