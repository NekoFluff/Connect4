using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CanvasManager : MonoBehaviour
{
    [SerializeField] public TMP_Text title;
    [SerializeField] public Button button;

    public void Start()
    {
        title.text = "Game In-Progress";
        button.gameObject.SetActive(false);
    }

    public void Draw()
    {
        title.text = "Draw!";
        button.gameObject.SetActive(true);
        // button.GetComponent<TMP_Text>().text = "Play again";
    }

    public void PlayerWon(int player)
    {
        title.text = $"Player {player} won!";
        button.gameObject.SetActive(true);
    }

    public void Restart()
    {
        title.text = "Game In-Progress";
        button.gameObject.SetActive(false);
    }
}
