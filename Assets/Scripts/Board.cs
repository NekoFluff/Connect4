using UnityEngine;
using UnityEngine.Events;

public class Board : MonoBehaviour, IInputHandler
{
    [SerializeField] public UnityEvent drawEvent;
    [SerializeField] public UnityEvent<int> playerWonEvent;

    private int boardWidth = 7;
    private int boardHeight = 6;
    private int[,] board;

    private int totalChipPositions; // The total number of chips that can be placed on the board (boardWidth * boardHeight)

    private Chip chipCursor; // Determined whose turn it is and where the chip will be dropped
    [SerializeField] public Chip chipPrefab;
    [SerializeField] public GameObject chipsContainer; // Contains all the 'Chip' game objects that have been dropped on the board

    // Start is called before the first frame update
    void Start()
    {
        this.RestartGame();

        if (drawEvent == null) {
            drawEvent = new UnityEvent();
        }

        if (playerWonEvent == null) {
            playerWonEvent = new UnityEvent<int>();
        }
    }

    public void SetChip(Chip chip)
    {
        this.chipCursor = chip;
        this.chipCursor.SetStartingPosition((int)boardWidth/2, this.boardHeight+1); // Start at the center-top
    }

    private void SetChipPosition(int position)
    {
        int calculatedPosition = Mathf.Min(Mathf.Max(0, position), boardWidth - 1);
        this.chipCursor.SetHorizontalPosition(calculatedPosition);
        // TODO: Update visual position of chip
    }

    public void MoveRight()
    {
        this.SetChipPosition(this.chipCursor.GetHorizontalPosition() + 1);
    }

    public void MoveLeft()
    {
        this.SetChipPosition(this.chipCursor.GetHorizontalPosition() - 1); 
    }

    public void Drop()
    {
        int chipPosition = this.chipCursor.GetHorizontalPosition();
        print("Drop");
        int dropHeight = GetColumnHeight(chipPosition);
        if (dropHeight != this.boardHeight) {
            board[dropHeight, chipPosition] = this.chipCursor.GetPlayer();

            // Drop visual chip
            Chip newChip = Instantiate(chipPrefab, Vector3.zero, Quaternion.identity);
            newChip.transform.SetParent(this.chipsContainer.transform);
            newChip.transform.position = this.chipCursor.transform.position;
            newChip.SetPlayer(this.chipCursor.GetPlayer());
            newChip.SetHorizontalPosition(this.chipCursor.GetHorizontalPosition());
            newChip.SetDropHeight(dropHeight);
            this.totalChipPositions -= 1;

            // Check if the player won
            bool playerWon = this.DidPlayerWin(new Vector2Int(this.chipCursor.GetHorizontalPosition(), dropHeight));
            if (playerWon) {
                this.EndGamePlayerWon();
                return;
            }

            print(this.totalChipPositions);
            if (this.totalChipPositions == 0) {
                this.EndGameDraw();
            }

            // Next player...
            this.AlternatePlayerTurn();
            this.chipCursor.ResetPosition();
        } else {
            print("Column full");
        }
    }

    private void EndGamePlayerWon()
    {
        print("Player Won!");
        print(this.chipCursor.GetPlayer());
        print("Play again?");
        playerWonEvent.Invoke(this.chipCursor.GetPlayer());
        // this.RestartGame();
    }

    private void EndGameDraw()
    {
        print("Draw");
        print("Play again?");
        drawEvent.Invoke();

    }

    private void AlternatePlayerTurn()
    {
        this.chipCursor.SetPlayer(this.chipCursor.GetPlayer() == 0 ? 1 : 0);
    }

    private bool CanDrop(int column)
    {
        return GetColumnHeight(column) != this.boardHeight;
    }

    private int GetColumnHeight(int column)
    {
        for (int row = 0; row < boardHeight; row++)
        {
            if (board[row, column] == -1) {
                return row;
            }
        }

        return boardHeight;
    }

    public void RestartGame()
    {
        this.totalChipPositions = this.boardHeight * this.boardWidth;
        // Reset the board data by setting all values in the board to -1 (no player has put a chip there yet)
        this.board = new int[boardHeight, boardWidth];

        for (int row = 0; row < boardHeight; row++)
        {
            for (int column = 0; column < boardWidth; column++)
            {
                this.board[row, column] = -1;
            }
        }

        // Delete all chips in chip container
        foreach (Transform child in this.chipsContainer.transform)
        {
            Destroy(child.gameObject);
        }
    }

    private bool DidPlayerWin(Vector2Int dropPosition)
    {        
        // Check horizontally
        Vector2Int[] directions = new Vector2Int[4];
        directions[0] = (new Vector2Int(1, 0)); // Check horizontally
        directions[1] = (new Vector2Int(0, 1)); // Check vertically
        directions[2] = (new Vector2Int(1, 1)); // Check diagonally (bottom left/top right)
        directions[3] = (new Vector2Int(1, -1)); // Check diagonally (bottom right/top left)

        foreach (var direction in directions) {
            int connectedCount = this.CountConnectedChips(dropPosition, direction) 
                                 + this.CountConnectedChips(dropPosition, -direction) // -direction == Opposite direction 
                                 - 1; // Subtract 1 since we double count the same spot twice.

            if (connectedCount >= 4) {
                return true;
            }
        }

        return false;
    }

    private int CountConnectedChips(Vector2Int dropPosition, Vector2Int direction)
    {
        int playerAtDropPoint = this.PlayerAtPoint(dropPosition);
        int connectedCount = 0;

        for (Vector2Int position = dropPosition; 
             position.x < boardWidth && position.y < boardHeight && position.x >= 0 && position.y >=0;
             position += direction) {

            if (this.PlayerAtPoint(position) == playerAtDropPoint) {
                connectedCount += 1;
            } else {
                break;
            }
        }

        return connectedCount;
    }

    private int PlayerAtPoint(Vector2Int position)
    {
        return board[position.y, position.x];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
