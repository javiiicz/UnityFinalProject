using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu_Script : MonoBehaviour
{
    [SerializeField] private GameObject mainView;
    [SerializeField] private GameObject instructionView;
    [SerializeField] private Button startButton;
    [SerializeField] private Button instructionButton;
    [SerializeField] private Button backButton;


    // Start is called before the first frame update
    void Start()
    {
        instructionButton.onClick.AddListener(ShowInstructions);
        backButton.onClick.AddListener(GoBack);
        startButton.onClick.AddListener(StartGame);
    }

    void ShowInstructions()
    {
        mainView.SetActive(false);
        instructionView.SetActive(true);
    }

    void GoBack()
    {
        mainView.SetActive(true);
        instructionView.SetActive(false);
    }

    void StartGame()
    {
        SceneManager.LoadScene("GameScene");
    }

}
