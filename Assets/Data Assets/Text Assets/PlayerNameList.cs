using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerNameData", menuName = "ScriptableObjects/PlayerNameList", order = 1)]
public class PlayerNameList : ScriptableObject
{
    public string[] names;
}
