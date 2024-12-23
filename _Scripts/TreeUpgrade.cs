using UnityEngine;
using System.Collections;
public class TreeUpgrade : MonoBehaviour
{
    public Sprite[] treeSprites; // Mảng chứa các sprites cho từng cấp độ cây
    public float fadeDuration = 1.0f; // Thời gian hiệu ứng fade
    public int currentLevel = 0; // Cấp độ hiện tại của cây
    private SpriteRenderer spriteRenderer;
    public GameObject tree;
    public QuizManager quizManager;

    void Start()
    {   
        spriteRenderer = tree.GetComponent<SpriteRenderer>();
    }
    public void UpdateTree()
    {
        int correctNums = quizManager.currentQuestionIndex - quizManager.numsWrongAnswers;
        if (correctNums < 3)
        {
            currentLevel = 0;
        }
        else if (correctNums < 6)
        {
            currentLevel = 1;
        }
        else if(correctNums< 10)
        {
            currentLevel = 2;   
        }
        spriteRenderer.sprite = treeSprites[currentLevel];

    }
   
}