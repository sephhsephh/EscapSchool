using UnityEngine;
using TMPro;
using UnityEditor.VersionControl;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Unity.Netcode;

public class QuizBlackboard : NetworkBehaviour
{
    public GameObject player;
    public GameObject quizPanel;
    public TMP_Text quizTextTMP;
    public TMP_InputField answerInput;
    public GameObject door;
    public TMP_Text scoreText;
    public Button submitResult;
    public TMP_Text livesText;
    public GameObject nextTarget;

    [TextArea]
    public string[] questions;
    public string[] answers;

    private int questionIndex = 0;
    private int correctAnswers = 0;
    private int lives = 3;


    public void shuffleQuestionsAndAnswers(string[] questions, string[] answers)
    {
        for (int i = 0; i < questions.Length; i++)
        {
            int randomIndex = Random.Range(i, questions.Length);
            string tempQuestion = questions[i];
            questions[i] = questions[randomIndex];
            questions[randomIndex] = tempQuestion;
            string tempAnswer = answers[i];
            answers[i] = answers[randomIndex];
            answers[randomIndex] = tempAnswer;
        }
    }


    public void StartQuiz(GameObject plr)
    {
        GetComponent<InteractableObject>().MarkAsInteracted(true); // Mark as interacted prevent press e to intearact
        if (quizPanel != null && questions.Length > 0)
        {
            shuffleQuestionsAndAnswers(questions, answers); // Shuffle questions and answers
            quizPanel.SetActive(true);
            questionIndex = 0;
            correctAnswers = 0;
            lives = 3;
            DisplayQuestion();
            scoreText.text = "Score: " + correctAnswers + "/" + questions.Length;
            livesText.text = "Lives: " + lives;

            // 👇 Set up the button listener here
            submitResult.onClick.RemoveAllListeners();
            submitResult.onClick.AddListener(SubmitAnswer);
        }

        player = plr;
     
    }

    public void SubmitAnswer()
    {

        string playerAnswer = answerInput.text.Trim().ToLower();
        string correctAnswer = answers[questionIndex].ToLower();

        if (playerAnswer == correctAnswer)
        {
            print("Correct answer: " + playerAnswer);
            correctAnswers++;
            scoreText.text = "Score: " + correctAnswers + "/" + questions.Length;

        }
        else
        {
            //print("Wrong answer: " + playerAnswer);
            //lives--;
            //livesText.text = "Lives: " + lives;
            //if (lives <= 0)
            //{
            //    quizPanel.SetActive(false);
            //    questionIndex = 0;
            //    correctAnswers = 0;
            //    lives = 3;
            //    GetComponent<InteractableObject>().MarkAsInteracted(false);
            //    EventSystem.current.SetSelectedGameObject(null);
            //    return;
            //}
        }

        questionIndex++;
        DisplayQuestion();
    }

    private void DisplayQuestion()
    {
        if (questionIndex < questions.Length)
        {
            quizTextTMP.text = questions[questionIndex];
            answerInput.text = "";
        }
        else
        {
            CheckQuizCompletion();
        }
    }

    private void CheckQuizCompletion()
    {
        quizPanel.SetActive(false);

        if (correctAnswers >= 0)
        {
            print("Quiz passed!");
            door.SetActive(false);  // Unlock door

            
            player.GetComponentInChildren<ArrowPointer>().SetVisible(true); // Point to the door
            player.GetComponentInChildren<ArrowPointer>().SetTarget(nextTarget.transform); // Point to the door


        }
        else
        {
            GetComponent<InteractableObject>().MarkAsInteracted(false);
            EventSystem.current.SetSelectedGameObject(null);
            questionIndex = 0;
            correctAnswers = 0;

        }

    }
}