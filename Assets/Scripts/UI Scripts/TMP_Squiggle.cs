using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//This script attaches to the TMP text
public class TMP_Squiggle : MonoBehaviour
{
    public bool debug;
    public bool prefab;// = false; //flag determinig whether string should immediately be populated by the source text, or if it should be populated by an external script. Default value = false for runtime instantiation
    float rate; //time after which sprites should be updated
    float timer=0;
    public string source; //source text
    string[] strings; //strings to pass into TMP text
    int frame=0; //animation frame number

    TMPro.TMP_Text tmp; //reference to TMP text

    private void Start()
    {
        if (!prefab)
        {
            if (source == null || source.Equals(""))
            {
                source = tmp.text;
            }
            strings = Squiggle_Maker.GetStrings(source);
            if (debug)
            {
                Debug.Log(source);
                Debug.Log(strings[0]);
                Debug.Log(strings[1]);
                Debug.Log(strings[2]);


            }
        }

    }


    // Start is called before the first frame update
    void Awake()
    {
        rate = Squiggle_Maker.rate;
        //Debug.Log("Squiggle rate:" + rate);
        tmp = GetComponent<TMPro.TMP_Text>();
        if (prefab)
        {
            
        }
        
    }

    //reconstructs text arrays
    public void SetSource(string s)
    {
        strings = Squiggle_Maker.GetStrings(s);
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > rate)
        {
            tmp.text = strings[frame];
            //tmp.ForceMeshUpdate();
            frame = (frame + 1) % 3;
            timer = 0;
        }
    }
}
