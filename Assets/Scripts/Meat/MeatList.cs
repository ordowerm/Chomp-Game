using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MeatList", menuName = "MeatList")]
public class MeatList : ScriptableObject
{
    public MeatData[] list;
}
