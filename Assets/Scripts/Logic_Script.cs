using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Logic_Script : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text liveText;
    [SerializeField] private TMP_Text countdown;
    int lives = 5;

    // Start is called before the first frame update
    void Start()
    {
        scoreText.text = GameData.score.ToString();
        liveText.text = lives.ToString();

        StartCoroutine(StartCountdown());
    }

    void OnZombieContact()
    {
        liveText.text = (--lives).ToString();

        if (lives == 0)
        {
            SceneManager.LoadScene("EndScene");
        }
    }

    void OnZombieKill()
    {
        scoreText.text = (++GameData.score).ToString();
    }

    IEnumerator StartCountdown()
    {
        for (int i = 5; i >= 0; i--)
        {
            if (i == 0)
            {
                countdown.text = "GO!";
                yield return new WaitForSeconds(1);
            }
            else
            {
                countdown.text = i.ToString();
                yield return new WaitForSeconds(1);
            }
            
        }
        countdown.gameObject.SetActive(false);
    }
}
