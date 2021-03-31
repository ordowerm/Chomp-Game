using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

/*
 * This script is attached to the PlayerPanel prefab.
 * It contains color scheme data, as well as references to the score picture.
 * 
 * 
 * 
 */
public class ScorePanelScript : MonoBehaviour
{
    public DinoData d;

    int id; //player id
    public GameObject playerName; //reference to player's name text box
    public GameObject scoreTitle; //reference to score textbox
    public GameObject playerPortrait; //reference to the portrait
    public GameObject teeth; //reference to the teeth border image
    public GameObject scoreText; //reference to the score textbox
    public Image backpanel;

    //sets score text
    public void UpdateScore(int sc)
    {
        scoreText.GetComponent<TMP_Squiggle>().SetSource(sc.ToString());
    }

    //recolors UI objects based on DinoData
    void SetColors(DinoData data)
    {
        backpanel.color = data.primarycolor;
        playerName.GetComponentInChildren<Image>().color = data.secondarycolor;
        playerPortrait.GetComponent<Image>().sprite = data.portrait;
        teeth.GetComponent<Image>().color = data.tertiarycolor;
        try
        {
            playerName.GetComponentInChildren<TMP_SubMeshUI>().material.color = data.textcolor;
            scoreText.GetComponentInChildren<TMP_SubMeshUI>().material.color = data.textcolor;
            scoreTitle.GetComponentInChildren<TMP_SubMeshUI>().material.color = data.textcolor;


        }
        catch (System.Exception e)
        {
            Debug.Log("Error: "+e);
        }
    }

    
    // Update is called once per frame
    void Update()
    {
        SetColors(d);

    }
}
