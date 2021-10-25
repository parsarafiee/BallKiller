using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoundManager : MonoBehaviour
{

    public UnityEngine.UI.Text roundOverText;
    public UnityEngine.UI.Text scoreText;
    public UnityEngine.UI.Text RestartText;
    static int roundNumber = 1;
    static int luigiScore;
    static int marioScore;

    bool roundOver;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && roundOver)
        {
            SceneManager.LoadScene("SampleScene");
        }
    }

    public void RoundEnds(Character deadCharactor)
    {
        if (!roundOver)
        {
            roundOver = true;

            if (deadCharactor == Character.Luigi)
            {
                marioScore++;
            }
            else
                luigiScore++;

            roundOverText.text = (deadCharactor == Character.Mario ? "Luigi" : "Mario") + ("is the winer of round " + roundNumber);
            scoreText.text = "Score \n Mario :" + marioScore + "\nLuigi" + luigiScore;
            RestartText.enabled = true;
            scoreText.enabled = true;
            roundOverText.enabled = true;

            roundNumber++;

        }
    }
}
