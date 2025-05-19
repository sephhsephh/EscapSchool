using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Unity.Netcode;
using System.Collections.Generic;

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

    public List<Question> currentQuestions = new List<Question>();
    private int questionIndex = 0;
    private int correctAnswers = 0;
    private int lives = 3;
    private string difficulty = "easy"; // Can be "easy", "medium", "hard"

    public void StartQuiz(GameObject plr)
    {
        GetComponent<InteractableObject>().MarkAsInteracted(true);
        player = plr;

        // Load questions from server
        PHPRequestHandler.Instance.GetQuestions(difficulty, (success, message, questions) =>
        {
            if (success && questions != null && questions.Count > 0)
            {
                currentQuestions = questions;
                ShuffleQuestions();

                quizPanel.SetActive(true);
                questionIndex = 0;
                correctAnswers = 0;
                lives = 3;
                DisplayQuestion();
                scoreText.text = $"Score: {correctAnswers}/{currentQuestions.Count}";
                livesText.text = $"Lives: {lives}";

                submitResult.onClick.RemoveAllListeners();
                submitResult.onClick.AddListener(SubmitAnswer);
            }
            else
            {
                Debug.LogError($"Failed to load questions: {message}");
                // Fallback to local questions if needed
                GetComponent<InteractableObject>().MarkAsInteracted(false);
            }
        });
    }

    private void ShuffleQuestions()
    {
        for (int i = 0; i < currentQuestions.Count; i++)
        {
            int randomIndex = Random.Range(i, currentQuestions.Count);
            Question temp = currentQuestions[i];
            currentQuestions[i] = currentQuestions[randomIndex];
            currentQuestions[randomIndex] = temp;
        }
    }

    public void SubmitAnswer()
    {
        string playerAnswer = answerInput.text.Trim().ToLower();
        string correctAnswer = currentQuestions[questionIndex].answer.ToLower();

        if (playerAnswer == correctAnswer)
        {
            Debug.Log("Correct answer: " + playerAnswer);
            correctAnswers++;
            scoreText.text = $"Score: {correctAnswers}/{currentQuestions.Count}";
        }
        else
        {
            lives--;
            livesText.text = $"Lives: {lives}";

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
        if (questionIndex < currentQuestions.Count)
        {
            quizTextTMP.text = currentQuestions[questionIndex].question;
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

        if (correctAnswers >= currentQuestions.Count / 2) // You can adjust this threshold
        {
            print("Quiz passed!");
            door.SetActive(false);  // Unlock door

            player.GetComponentInChildren<ArrowPointer>().SetVisible(true);
            player.GetComponentInChildren<ArrowPointer>().SetTarget(nextTarget.transform);
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