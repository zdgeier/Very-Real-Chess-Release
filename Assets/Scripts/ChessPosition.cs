using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ChessPosition : MonoBehaviour {
    public List<ChessPiece> overlappingPieces;

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
            Destroy(overlappingPieces[0].gameObject);
            overlappingPieces.RemoveAt(0);
            overlappingPieces.TrimExcess();
        }
        catch (MissingReferenceException e)
        {
            // Do nothing i cant fix this
        }
    }

    // Update is called once per frame
    void Update () {
	
	}
}
