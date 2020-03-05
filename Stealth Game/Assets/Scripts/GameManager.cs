using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject TitleBackgroundImage;
    public GameObject TitleText;
    public GameObject StartGameButton;
    public GameObject VictoryBackground;
    public GameObject VictoryText;

    public GameObject player;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    void Update()
    {
        Quit();
    }

    public void Quit()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public void StartGame()
    {
        TitleBackgroundImage.SetActive(false);
        TitleText.SetActive(false);
        StartGameButton.SetActive(false);
        VictoryBackground.SetActive(false);
        VictoryText.SetActive(false);
    }
}
