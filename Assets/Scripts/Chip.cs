using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chip : MonoBehaviour
{
    private Vector2Int startingPosition = new Vector2Int(0,0);
    private int player = 0;
    private int xPosition; // horizontal position
    private int yPosition = -1; // vertical position
    private bool isDropped = false;

    [SerializeField] SpriteRenderer spriteRenderer;

    public void Awake()
    {
        this.updatePlayerColor();
    }

    public void SetPlayer(int player) 
    {
        this.player = player;
        this.updatePlayerColor();
    }

    private void updatePlayerColor()
    {
        spriteRenderer.color = player == 0 ? new Color(1,0,0,1f) : new Color(0,0,1,1f);
    }

    public int GetPlayer()
    {
        return this.player;
    }

    public void SetHorizontalPosition(int xPosition)
    {
        this.xPosition = xPosition;
        this.setLocalTransform(new Vector2(this.xPosition, this.transform.localPosition.y));
    }

    private void SetVerticalPosition(int height) 
    {
        this.setLocalTransform(new Vector2(this.transform.localPosition.x, height));
    }


    public int GetHorizontalPosition()
    {
        return this.xPosition;
    }

    public void SetStartingPosition(int x, int y)
    {
        this.startingPosition = new Vector2Int(x,y);
        this.ResetPosition();
    }

    public void ResetPosition()
    {
        this.SetHorizontalPosition(this.startingPosition.x);
        this.SetVerticalPosition(this.startingPosition.y);
    }

    public void SetDropHeight(int height)
    {
        this.SetVerticalPosition(height);
        this.yPosition = height;
        this.isDropped = true;
    }

    public int GetDropHeight()
    {
        return this.yPosition;
    }

    private void setLocalTransform(Vector2 position)
    {
        if (this.isDropped) return;
        this.transform.localPosition = position;
    }
}
