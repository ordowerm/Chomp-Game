using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Class containing information about meat
[CreateAssetMenu(fileName ="MeatData",menuName ="Meat")]
public class MeatData : ScriptableObject
{
    public string meatname = "none";
    public int pointvalue=1;
    public float fallspeed=1;
    public float fadetime = 0.2f;
    public int copies; //number of array slots occupied by this meat
    public Sprite pic;
}
