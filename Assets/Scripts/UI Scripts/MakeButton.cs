using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MakeButton : MonoBehaviour//, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    /*
     * This script generates the buttons for the title screen.
     * 
     * It also keeps track of each button. When one is clicked, the selected button should change color, and the other buttons should reset.
     * This is accomplished through the Observer Pattern. The click listener of the selected button notifies this script of the selected button's ID.
     * This script in turn notifies the other buttons to change colors to the deselected color scheme.
     * 
     * Originally, this script was used to control each individual button. Its functionality was replaced by the TitleButtonScript class. 
     * Much of the original code was left in for reference while I made the replacement script, but I commented it out.
     */
    public GameObject buttontemplate; //attach button prefab

    public float animationtime; //time it takes to interpolate between two colors
    float animationtimer=0;

    int idselected = -1; //index of selected button; -1 denotes no button selected
    public string[] texts; //array of strings used to generate buttons
    List<GameObject> buttons;

    public bool debug;
    const string TAG = "MakeButton: ";

    public Color bgStd;
    public Color bgHover;
    public Color bgClicked;
    public Color bgPostClick;
    public Color fontTintStd;
    public Color fontTintHover;
    public Color fontTintClicked;
    public Color fontTintPostClick;

    public GameObject panel; //background to modify
    public GameObject textmesh; //reference to text mesh
    Image image;
    TMPro.TMP_SubMeshUI sm;
    bool selected = false;

    //tells this button manager that the id was clicked. also resets state of every other button
    public void NotifyClicked(int id)
    {
        idselected = id;
        for (int i = 0; i < buttons.Count; i++)
        {
            TitleButtonScript tb = buttons[i].GetComponent<TitleButtonScript>();
            try
            {
                if (tb.GetId() != id)
                {
                    tb.SetUnclicked();
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError("Bad button scripts when trying to update button states");
            }
        }
    }


    //instantiates buttons
    void BuildButtons()
    {
        buttons = new List<GameObject>();
        for (int i = 0; i<texts.Length;i++)
        {
            GameObject gb = Instantiate(buttontemplate);
            try
            {
                TitleButtonScript tb = gb.GetComponent<TitleButtonScript>();
                tb.SetController(this, i);
                TMP_Squiggle sq = gb.GetComponent<TMP_Squiggle>();
                sq.SetSource(texts[i]);
                gb.GetComponent<RectTransform>().SetParent(gameObject.GetComponent<RectTransform>(), false); //call to GetComponent may be replaceable by directly accessing transform.
                gb.GetComponent<RectTransform>().SetAsLastSibling(); //might be unnecessary

                buttons.Add(gb);
            }
            catch(System.Exception e)
            {
                Debug.LogError("Error when instantiating buttons");
            }
        }
    }



    void Start()
    {
        BuildButtons();
        //image = panel.GetComponent<Image>();
        //sm = textmesh.GetComponent<TMPro.TMP_SubMeshUI>();
    }

    /*
    public void OnPointerClick(PointerEventData eventData)
    {
        if (debug)
        {
            Debug.Log(TAG + "button clicked.");
        }

        selected = true;
        image.color = bgClicked;
        sm.material.color = fontTintClicked;
        //throw new System.NotImplementedException();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (debug)
        {
            Debug.Log(TAG + "hovered over button.");
        }

        if (!selected)
        {
            image.color = bgHover;
            sm.material.color = fontTintHover;

        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (debug)
        {
            Debug.Log(TAG + "stopping hover.");
        }

        if (!selected)
        {
            image.color = bgStd;
            sm.material.color = fontTintStd;
        }
    }
    */
}
