using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsSettings : MonoBehaviour
{
    public float hspeed; //normal horizontal movement speed
    public float jumpforce; //upward force applied when player jumps
    public float turbospeed; //speed when turbo is activated
    public float turbotime; //duration of turbo mode
    public float dinomass = 1;
    public float dinogravityscale = 3;

    public float GetJumpHeight()
    {
        float height;
        float grav = dinogravityscale * Physics2D.gravity.y;
        float time = -jumpforce / (grav) / dinomass;
        height = jumpforce /dinomass * time + grav * time * time / 2.0f+0.5f;
        return height;
    }

    private void Update()
    {
        Debug.Log("Height: " + GetJumpHeight());
    }
}
