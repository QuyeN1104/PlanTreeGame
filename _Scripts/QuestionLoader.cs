using System.Collections.Generic;
using System;
using System.IO;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class QuestionLoader
{
    public string Question; // Câu hỏi
    public string[] Answers; // Danh sách các câu trả lời
    public int CorrectAnswers; // Chỉ số câu trả lời đúng

    /// <summary>
    /// Sắp xếp ngẫu nhiên các câu hỏi và câu trả lời.
    /// </summary>
    public void ShuffleQuestions(string inputFilePath, string outputFilePath)
    {
        if (!File.Exists(inputFilePath))
        {
            Debug.LogError("File đầu vào không tồn tại!");
            return;
        }

        // Tải danh sách câu hỏi từ file
        List<QuestionLoader> questionList = LoadQuestionsFromFile(inputFilePath);

        if (questionList == null || questionList.Count == 0)
        {
            Debug.LogError("Không có câu hỏi nào được tải!");
            return;
        }

        // Sắp xếp ngẫu nhiên các câu hỏi
        var random = new System.Random();
        questionList = questionList.OrderBy(q => random.Next()).ToList();

        // Sắp xếp ngẫu nhiên các câu trả lời trong mỗi câu hỏi
        foreach (var question in questionList)
        {
            var shuffledAnswers = question.Answers
                .Select((answer, index) => new { Answer = answer, Index = index })
                .OrderBy(x => random.Next())
                .ToList();

            // Cập nhật câu trả lời và chỉ số câu trả lời đúng
            question.Answers = shuffledAnswers.Select(x => x.Answer).ToArray();
            question.CorrectAnswers = shuffledAnswers.FindIndex(x => x.Index == question.CorrectAnswers);
        }

        // Ghi lại danh sách đã sắp xếp vào file
        var resultLines = new List<string>();
        foreach (var question in questionList)
        {
            resultLines.Add(question.Question);
            for (int i = 0; i < question.Answers.Length; i++)
            {
                string prefix = (i == question.CorrectAnswers) ? "1 " : "0 ";
                resultLines.Add(prefix + question.Answers[i]);
            }
        }

        File.WriteAllLines(outputFilePath, resultLines);
        Debug.Log($"Đã lưu file sắp xếp ngẫu nhiên tại: {outputFilePath}");
    }

    /// <summary>
    /// Tải danh sách câu hỏi từ file.
    /// </summary>
    public List<QuestionLoader> LoadQuestionsFromFile(string inputFilePath)
    {
        if (!File.Exists(inputFilePath))
        {
            Debug.LogError("File đầu vào không tồn tại!");
            return null;
        }

        // Đọc file và tách từng câu hỏi
        string[] lines = File.ReadAllLines(inputFilePath);
        List<QuestionLoader> questionList = new List<QuestionLoader>();

        string currentQuestion = "";
        List<string> currentAnswers = new List<string>();
        int correctAnswerIndex = -1;

        foreach (var line in lines)
        {
            if (line.StartsWith("0 ") || line.StartsWith("1 ")) // Câu trả lời
            {
                // Tách số khỏi câu trả lời
                string answer = line.Substring(2).Trim();

                // Kiểm tra xem đây có phải là câu trả lời đúng hay không
                if (line.StartsWith("1 "))
                {
                    correctAnswerIndex = currentAnswers.Count; // Lưu chỉ số câu trả lời đúng
                }

                currentAnswers.Add(answer);
            }
            else if (!string.IsNullOrWhiteSpace(line))
            {
                // Khi gặp câu hỏi mới, lưu câu hỏi cũ và câu trả lời vào danh sách
                if (!string.IsNullOrEmpty(currentQuestion))
                {
                    questionList.Add(new QuestionLoader
                    {
                        Question = currentQuestion.Trim(),
                        Answers = currentAnswers.ToArray(),
                        CorrectAnswers = correctAnswerIndex
                    });
                }

                // Đặt câu hỏi hiện tại và làm mới danh sách câu trả lời
                currentQuestion = line.Trim();
                currentAnswers = new List<string>();
                correctAnswerIndex = -1; // Reset chỉ số câu trả lời đúng
            }
        }

        // Lưu câu hỏi cuối cùng
        if (!string.IsNullOrEmpty(currentQuestion))
        {
            questionList.Add(new QuestionLoader
            {
                Question = currentQuestion.Trim(),
                Answers = currentAnswers.ToArray(),
                CorrectAnswers = correctAnswerIndex
            });
        }

        Debug.Log($"Đã tải {questionList.Count} câu hỏi từ file.");
        return questionList;
    }
}
