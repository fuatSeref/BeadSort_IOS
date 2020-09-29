using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    [SerializeField]
    private int[] score;   

    [SerializeField]
    private Text[] scoreText;

  

    int eachPercentage;
    public bool isHundread;


    void Awake()
    {

        for (int i = 0; i < scoreText.Length; i++)
        {
            score[i] = 0;           
            scoreText[i] = scoreText[i].GetComponent<Text>();
            scoreText[i].text = "0%";
        }
    }
    private void Start()
    {       
        isHundread = false;
        eachPercentage = 100 / FindObjectOfType<BeadPrefab>().poolSize;
    }
    void OnEnable()
    {
        BlueBeadControl.BlueScore += BlueScore;
        RedBeadControl.RedScore += RedScore;

    }
    private void OnDisable()
    {
        BlueBeadControl.BlueScore -= BlueScore;
        RedBeadControl.RedScore -= RedScore;

    }

    private void Update()
    {
        if (score[0] == 100 && score[1] == 100)
        {
            isHundread = true;
        }
        else return;
    }
    void BlueScore(bool b_IsOut)
    {
        
       if(!b_IsOut)
        {
            score[0] += eachPercentage;
            scoreText[0].text = score[0].ToString() + "%";
        }else
        {
            score[0] -= eachPercentage;
            scoreText[0].text = score[0].ToString() + "%";
        }                           
          
    }

    void RedScore(bool r_IsOut)
    {
        if(!r_IsOut)
        {
            score[1] += eachPercentage;
            scoreText[1].text = score[1].ToString() + "%";
        }else
        {
            score[1] -= eachPercentage;
            scoreText[1].text = score[1].ToString() + "%";
        }
         
    }
}
