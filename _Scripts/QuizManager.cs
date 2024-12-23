using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class QuizManager : MonoBehaviour
{
    private QuestionLoader questionLoader = new QuestionLoader();
    public GameObject[] options; // Các lựa chọn trả lời
    public int currentQuestionIndex = 0;
    public TextMeshProUGUI QuestionTxt; // Text hiển thị câu hỏi
    public List<QuestionLoader> QnA; // Danh sách câu hỏi và đáp án
    public int numsWrongAnswers = 0; // Số câu trả lời sai
    public int successiveAnswers = 0; // Số câu trả lời đúng liên tiếp

    // Các sự kiện
    public UnityEvent successive;
    public UnityEvent ending; // Kết thúc màn chơi
    public UnityEvent Badending; // Kết thúc khi sai quá nhiều

    const int numSuc = 5; // Số câu đúng liên tiếp để kích hoạt sự kiện
    const int numWrong = 3; // Số câu sai tối đa trước khi kết thúc
    const int numQuestOneScence = 10; // Số câu hỏi trong một màn chơi

    private void Start()
    {
        // Load câu hỏi từ file
        QnA = questionLoader.LoadQuestionsFromFile("Assets/_Data/question.txt");
        StartCoroutine(GenerateQuestionWithDelay(0)); // Hiển thị câu hỏi đầu tiên ngay lập tức
    }

    // Đặt các câu trả lời cho câu hỏi hiện tại
    void SetAnswers()
    {
        for (int i = 0; i < options.Length; i++)
        {
            options[i].GetComponent<AnswerScript>().isCorrect = false;
            options[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = QnA[currentQuestionIndex].Answers[i];

            if (QnA[currentQuestionIndex].CorrectAnswers == i) // Nếu đây là đáp án đúng
            {
                options[i].GetComponent<AnswerScript>().isCorrect = true;
            }
        }
    }

    // Xử lý khi trả lời đúng
    public void Correct()
    {
        successiveAnswers++;

        // Kích hoạt sự kiện khi đủ số câu đúng liên tiếp
        if (successiveAnswers == numSuc)
        {
            successive.Invoke();
            successiveAnswers = 0;
            Debug.Log("Nhận phần thưởng!");
        }

        if (currentQuestionIndex < QnA.Count - 1)
        {
            currentQuestionIndex++;
            if (currentQuestionIndex - numsWrongAnswers == numQuestOneScence)
            {
                Debug.Log("Qua màn!");
                ending.Invoke();
                return;
            }

            StartCoroutine(HandleNextQuestion(1.5f)); // Thời gian chờ trước câu hỏi tiếp theo
        }
        else
        {
            Debug.Log("Hết câu hỏi!");
            ending.Invoke();
        }
    }

    // Xử lý khi trả lời sai
    public void Wrong()
    {
        numsWrongAnswers++;

        if (numsWrongAnswers == numWrong)
        {
            Badending.Invoke();
            return;
        }

        if (currentQuestionIndex < QnA.Count - 1)
        {
            successiveAnswers = 0;
            currentQuestionIndex++;
            StartCoroutine(HandleNextQuestion(1.5f)); // Thời gian chờ trước câu hỏi tiếp theo
        }
        else
        {
            Debug.Log("Hết câu hỏi!");
            Badending.Invoke();
        }
    }

    // Xử lý chuyển đổi giữa các câu hỏi
    private IEnumerator HandleNextQuestion(float delay)
    {
        // Chờ hiệu ứng từ AnswerScript hoàn thành
        yield return new WaitForSeconds(delay);

        // Gọi hàm hiển thị câu hỏi mới
        yield return StartCoroutine(GenerateQuestionWithDelay(0));
    }

    // Hiển thị câu hỏi với độ trễ
    private IEnumerator GenerateQuestionWithDelay(float delay)
    {
        // Ẩn câu hỏi và các lựa chọn hiện tại
        QuestionTxt.text = "";
        foreach (var option in options)
        {
            option.SetActive(false);
        }

        // Chờ trong khoảng thời gian được chỉ định
        yield return new WaitForSeconds(delay);

        // Hiển thị câu hỏi mới
        if (QnA.Count > currentQuestionIndex)
        {
            QuestionTxt.text = QnA[currentQuestionIndex].Question;
            SetAnswers();

            foreach (var option in options)
            {
                option.SetActive(true);
            }
        }
    }
}
