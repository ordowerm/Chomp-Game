using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    int[] score;
    public float timer;
    public bool paused;
    public TimerPanelScript timetextbox;
    public ScorePanelScript[] scoretext;

    //Extra text for content:
    public string timer_title;
    public string timer_post_text;
    


    // Start is called before the first frame update
    void Start()
    {
        score = new int[4];

        //display scores
        try
        {
            for (int i = 0; i < 4; i++)
            {
                score[i] = 0;
                scoretext[i].UpdateScore(score[i]);
            }
        }
        catch (System.Exception e)
        {

        }
        

        UpdateTimerText();
    
    
    }

    void UpdateTimerText()
    {
        timetextbox.SetTime(Mathf.Ceil(timer));
    }

    public void AddPoints(int id, int points)
    {
        Debug.Log("Adding " + points + " points to player " + id);
        score[id] += points;
        for (int i = 0; i < score.Length; i++)
        {
            scoretext[i].UpdateScore(score[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!paused)
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime;
            }
            if (timer < 0)
            {
                timer = 0;
            }
        }
        UpdateTimerText();
    }
}
