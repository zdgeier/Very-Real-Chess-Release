using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ChessPosition : MonoBehaviour {
    public List<ChessPiece> overlappingPieces;
    public List<ChessPiece> takenPieces;

    // Use this for initialization
    void Start()
    {
        overlappingPieces = new List<ChessPiece>();
    }

    void OnTriggerEnter(Collider collider)
    {
        ChessPiece newPiece = collider.gameObject.GetComponent<ChessPiece>();

        if (newPiece != null)
        {
            newPiece.Entered(this);
            overlappingPieces.Add(newPiece);
        }
    }

    void OnTriggerExit(Collider collider)
    {
        ChessPiece oldPiece = collider.gameObject.GetComponent<ChessPiece>();

        // Makes sure piece has successfully cast to ChessPiece
        if (oldPiece != null)
        {
            oldPiece.Left(this);
            overlappingPieces.Remove(oldPiece);
        }
    }

    public void TakeBottomPiece()
    {
        try
        {
            // Instead of deleting, just hide the piece taken
            overlappingPieces[0].gameObject.SetActive(false);
            takenPieces.Add(overlappingPieces.ToArray()[0]);
            overlappingPieces.RemoveAt(0);
            overlappingPieces.TrimExcess();

        }
        catch (MissingReferenceException e)
        {
            // Do nothing i cant fix this
        }
    }

    public void UndoPieceTaken()
    {
        if (takenPieces.ToArray().Length > 0)
        {
            ChessPiece takenPiece = takenPieces.ToArray()[0];
            takenPiece.gameObject.SetActive(true);
            overlappingPieces.Add(takenPiece);
            takenPieces.RemoveAt(0);
            takenPieces.TrimExcess();
        }
        else
        {
            // No piece taken
        }
    }

    // Update is called once per frame
    void Update () {
	
	}
}
