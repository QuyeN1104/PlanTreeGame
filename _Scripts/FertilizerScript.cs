// FertilizerScript.cs
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FertilizerScript : MonoBehaviour
{
    public GameObject fertilizer; // Object fertilizer
    public int numsFertilizer = 0; // Số lượng fertilizer
    public QuizManager quizManager; // Tham chiếu đến QuizManager
    public AnswerScript answerScript; // Tham chiếu đến AnswerScript

    private void Start()
    {
        // Lấy component Button từ fertilizer
        Button fertilizerButton = fertilizer.GetComponent<Button>();

        if (fertilizerButton != null)
        {
            fertilizerButton.onClick.AddListener(TriggerCorrectEvent); // Gắn sự kiện khi nhấn nút
            fertilizerButton.onClick.AddListener(Decrease); // Gắn sự kiện khi nhấn nút
            fertilizerButton.onClick.AddListener(Display); // Gắn sự kiện khi nhấn nút
            Display();
            Debug.Log("Display", fertilizer);
        }
        else
        {
            Debug.LogError("Không tìm thấy Button trên object fertilizer!");
        }
      
    }

    public void Increase()
    {
        numsFertilizer+=2;
    }
    public void Decrease()
    {
        if (numsFertilizer > 0)
        numsFertilizer--;
    }

    public void Display()
    {
        if (numsFertilizer <= 0)
        {
            fertilizer.SetActive(false);
        }
        else
        {
            fertilizer.SetActive(true);
            fertilizer.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "x " + numsFertilizer;
        }
    }

    public void TriggerCorrectEvent()
    {
        if (answerScript != null)
        {
                answerScript.Correct.Invoke(); // Gọi sự kiện Correct
           
                Debug.Log("Correct event invoked from FertilizerScript");
        }
        else
        {
            Debug.LogError("AnswerScript chưa được gán trong FertilizerScript!");
        }
    }
}
