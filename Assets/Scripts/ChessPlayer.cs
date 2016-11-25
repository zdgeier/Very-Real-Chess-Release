using UnityEngine;
using System.Collections.Generic;

public abstract class ChessPlayer : MonoBehaviour {

    protected List<ChessPiece> myPieces;
    protected string color = "";

	// Use this for initialization
	protected void Start () {
        myPieces = GetPieces();
	}

    public List<ChessPiece> GetPieces ()
    {
        List<ChessPiece> pieces = new List<ChessPiece>();

        foreach (Transform child in transform)
        {
            if (child != transform.GetChild(0))
            {
                ChessPiece piece = child.GetComponent<ChessPiece>();
                pieces.Add(piece);
            }
        }

        return pieces;
    }

    public void setColor(string color)
    {
        this.color = color;
    }

    abstract public string Poll(List<string> previousMoves);
	
	// Update is called once per frame
	void Update () {
	
	}
}
