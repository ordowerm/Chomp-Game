using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This script animates the TextMeshPro meshes.
 * 
 * The way it works:
 * The image file containing the different text sprites has three versions of each letter.
 * Text animation should cycle through the letters to create a "squiggly"-looking text, akin to the cartoons "Ed, Edd & Eddy" and "Science Court".
 * 
 * On Awake, each letter of the source string is assigned a random number between 0 and 2, each corresponding to the frame of animation on which it should start.
 * The script then constructs a new string by fetching the corresponding sprite for each letter and concatenating the sprites into a single TMP text.
 * At a fixed interval, the sprite ID of each letter gets updated by adding the base ID for the sprite and then an offset of 0-2, cycling through it, mod 3.
 */
public class Squiggle_Maker : MonoBehaviour
{
    /*
     * The values in this script are heavily dependent on the alphabet size.
     * 
     * 
     */
    public static bool debug=false;
    public static float rate = 0.2f;
    const int offset = 42;
    static int[] id=new int[offset];
    const string TAG = "Squiggle Script: ";

    void Awake()
    {
     /*   id = new int[offset];
        Debug.Log("ID initialized to" + id.ToString());

        for (int i = 0; i<id.Length; i++)
        {
            id[i] = Random.Range(0,3);
        }
        
        if (debug)
        {
            string array = "";
            for (int i = 0; i < id.Length; i++)
            {
                array += id[i];
            }
            Debug.Log(TAG+"Initialized frame indices: " + array);
        }*/
    }


    //input: lower case letter OR valid punctuation mark
    public static string GetSpriteId(char a)
    {
        if(debug) Debug.Log("Character: " + a); 
        
        string result = "<sprite=";
        int arrayindex = 0;
        int newid = 0;
        //if it's 
        if (a >= 'a' && a <= 'z')
        {
            arrayindex = a - 'a'; //to fetch whether the letter is on frame 0, 1, or 2          
        }
        else if (a>='0' && a <= '9')
        {
            arrayindex = a - '0' + 26; //currently, the sprite for 0 is at sprite_26
        }
        else
        {
            switch (a)
            {
                case '.':
                    arrayindex = 38;
                    break;
                case ',':
                    arrayindex = 39;
                    break;
                case '!':
                    arrayindex = 36;
                    break;
                case '?':
                    arrayindex = 37;
                    break;
                case '\'':
                    arrayindex = 40;
                    break;
                case ':':
                    arrayindex = 41;
                    break;
                default:
                    return " "; //default case is an invalid letter/one for which there's no sprite, or it's a space
            }
        }
        if (debug) { Debug.Log(TAG + "Letter index calculated: " + id[arrayindex]); }
        newid = arrayindex + id[arrayindex] * offset;
        if (debug) { Debug.Log(TAG + "Sprite ID Calculated: " + newid); }

        result += newid + ">";
        return result;
    }

    //increments frame number for each letter
    static void UpdateIds()
    {
        for (int i = 0; i < id.Length; i++)
        {
            id[i] = (id[i] + 1) % 3;
        }

        if (debug)
        {
            string array = "";
            for (int i = 0; i < id.Length; i++)
            {
                array += id[i];
            }
            Debug.Log(TAG + "frame indices: " + array);
        }
    }

    //builds whole string from source text
    public static string BuildString(string source)
    {
        source=source.ToLower(); //make everything lowercase
        string result = "";

        for (int i = 0; i < source.Length; i++)
        {
            result += GetSpriteId(source[i]);
        }

        if (debug)
        {
            Debug.Log(TAG+"built string: " + result );
        }

        return result;
    }

    //constructs all three variants of a source text, then returns them as an array of strings
    public static string[] GetStrings(string source)
    {
        string[] strings = { "", "", "" };
        for (int i = 0; i < strings.Length; i++)
        {
            strings[i] = BuildString(source);
            UpdateIds();
        }

        return strings;
    }
}
