using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class AnswerScript : MonoBehaviour
{
    public UnityEvent Correct;
    public UnityEvent Wrong;

    public bool isCorrect = false;

    private Button button;
    private Image buttonImage;
    public Color correctColor = Color.green; // Màu xanh khi đúng
    public Color wrongColor = Color.red;    // Màu đỏ khi sai
    private Color originalColor;           // Lưu màu gốc
    public float effectDuration = 1.0f;    // Thời gian chuyển màu
    public int blinkCount = 3;             // Số lần nhấp nháy
    public float blinkInterval = 0.2f;     // Khoảng thời gian giữa mỗi lần nhấp nháy

    void Start()
    {
        // Lấy component Button và Image
        button = GetComponent<Button>();
        buttonImage = button.GetComponent<Image>();
        if (buttonImage != null)
        {
            originalColor = buttonImage.color; // Lưu màu gốc
        }
    }

    public void Answer()
    {
        if (buttonImage != null)
        {
            StartCoroutine(ShowFeedbackWithBlink());
        }
    }

    IEnumerator ShowFeedbackWithBlink()
    {
        Color targetColor = isCorrect ? correctColor : wrongColor;

        // Nhấp nháy màu
        for (int i = 0; i < blinkCount; i++)
        {
            buttonImage.color = targetColor; // Đổi sang màu đúng/sai
            yield return new WaitForSeconds(blinkInterval);
            buttonImage.color = originalColor; // Quay về màu gốc
            yield return new WaitForSeconds(blinkInterval);
        }

        // Chuyển sang màu rõ ràng
        buttonImage.color = targetColor;

        // Gọi sự kiện
        if (isCorrect)
        {
            Correct.Invoke();
            Debug.Log("Correct Answer");
        }
        else
        {
            Wrong.Invoke();
            Debug.Log("Wrong Answer");
        }

        // Chờ thêm trước khi khôi phục lại màu gốc
        yield return new WaitForSeconds(effectDuration);
        buttonImage.color = originalColor;
    }
}