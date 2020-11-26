using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] Board board;
    [SerializeField] Chip playerChip;
    [SerializeField] CanvasManager canvasManager;

    [SerializeField] public PlayerInput playerInput;

    // Start is called before the first frame update
    void Start()
    {
        board.SetChip(playerChip);
    }

    public void DisableUserInput()
    {
        playerInput.enabled = false;
    }

    public void EnableUserInput()
    {  
        playerInput.enabled = true;
    }

    public void EndGamePlayerWon(int player)
    {
        canvasManager.PlayerWon(player);
        this.DisableUserInput();
    }

    public void EndGameDraw()
    {
        canvasManager.Draw();
        this.DisableUserInput();
    }

    public void RestartGame()
    {
        print("Restart game");
        canvasManager.Restart();
        this.board.RestartGame();
        this.EnableUserInput();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
