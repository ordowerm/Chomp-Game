using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimerPanelScript : MonoBehaviour
{
    public TMP_Text title;
    public TMP_SubMeshUI sm;
    public TMP_Text numbers;
    float time;

    public void SetTime(float f)
    {
        numbers.gameObject.GetComponent<TMP_Squiggle>().SetSource(f.ToString());
    }

    void Awake()
    {
      sm.material.color=Color.black;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
