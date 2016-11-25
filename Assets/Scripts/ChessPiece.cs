using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ChessPiece : MonoBehaviour {
    public ChessHuman chessHuman;
    
    public ChessPosition currentPosition;
    public ChessPosition pendingPosition;

	// Use this for initialization
	void Start () {
        currentPosition = null;
        pendingPosition = null;
	}

    public void Entered (ChessPosition position)
    {
        // When game begins
        if (currentPosition == null)
        {
            Debug.Log(this.name + " starts at " + position.name);
            currentPosition = position;
        }
        // When placed back in original location
        else if (currentPosition == position)
        {
            Debug.Log(this.name + " has returned to " + position.name);
            pendingPosition = null;
        }
        else
        {
            Debug.Log(this.name + " has entered " + position.name);
            pendingPosition = position;
        }
    }

    public void Left (ChessPosition position)
    {
        // Avoids case where piece has entered into another position already
        if (pendingPosition == position)
        {
            pendingPosition = null;
        }
        else
        {
            Debug.Log("current pos: " + currentPosition);
        }

        Debug.Log(this.name + " has left " + position.name);
    }

    public bool HasMoved ()
    {
        return pendingPosition != null;
    }

    public void CommitMovement ()
    {
        Debug.Log("CommitMovement()" + currentPosition + " " + pendingPosition);
        currentPosition = pendingPosition;
        pendingPosition = null;
    }

    public string GetMovement()
    {
        string movement = currentPosition.name + pendingPosition.name;

        return movement;
    }
    
	// Update is called once per frame
	void Update () {
	
	}
}
