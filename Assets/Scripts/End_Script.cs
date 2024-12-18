using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class End_Script : MonoBehaviour
{
    [SerializeField] private TMP_Text finalScore;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = true;
        finalScore.text = GameData.score.ToString();
    }
}
