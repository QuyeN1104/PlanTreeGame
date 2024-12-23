using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{   
    public int health ; // chỉ số máu hiện tại 
    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;
    private int maxhealth = 3;
    public QuizManager quizManager;


    // Update is called once per frame
    void Update()
    {
        health = maxhealth - quizManager.numsWrongAnswers;
        for (int i = 0; i < maxhealth; i++)
        {
            if(i < health)
            {
                hearts[i].sprite = fullHeart;
            }
            else
            {
                hearts[i].sprite = emptyHeart;
            }
        }
    }
}
