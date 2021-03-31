using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveOrb : MonoBehaviour
{
    public float yperiod; //period of bigger circle
    public float xperiod;
    public float yrad; //radius of bigger circle
    public float xrad; //radius of smaller circle
    public float resetchance; //chance of re-randomizing at the end of a cycle
    float xtimer;
    float ytimer;
    public float force;

    Transform t; //local transform
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        ytimer = Random.Range(0, yperiod);
        xtimer = Random.Range(0, xperiod);
        t = GetComponent<Transform>();
        rb = GetComponent<Rigidbody2D>();
    }

    //
    void ApplyRandomForce()
    {
        if (Random.value > resetchance)
        {
            rb.AddForce(new Vector2(force * Mathf.Sin(Random.Range(0, 2 * Mathf.PI)), force * Mathf.Sin(Random.Range(0, 2 * Mathf.PI))));     
        }
    }

    void RerandomizePos()
    {
        if (Random.value >= 1 - resetchance)
        {
            ytimer = Random.Range(0, yperiod);
            xtimer = Random.Range(0, xperiod);
        }
    }

    //sets transform position to parent's position + offset
    void MoveFixed()
    {
        float xth = (xtimer / xperiod) * 2*Mathf.PI;
        float yth = (ytimer / yperiod) * 2 * Mathf.PI;
        t.localPosition = new Vector3(xrad*Mathf.Sin(xth),yrad*Mathf.Sin(yth),0);
    }

    // Update is called once per frame
    void Update()
    {
        //update timer 
        if (xtimer > xperiod)
        {
            xtimer = 0;
        }
        else
        {
            xtimer += Time.deltaTime;
        }

        if (ytimer > yperiod)
        {
            ytimer = 0;
        }
        else
        {
            ytimer += Time.deltaTime;
        }
        //RerandomizePos();
        //MoveFixed();
        ApplyRandomForce();
    }
}
