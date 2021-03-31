using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movable : MonoBehaviour
{
    protected bool active = true;
    public void Activate()
    {
        active = true;
    }
    public void Deactivate()
    {
        active = false;
    }

}
