using UnityEngine;
using System.Collections;
using System.Diagnostics;
using System.Collections.Generic;
using System.IO;

public class ChessAI : ChessPlayer {
    public ChessGame chessGame;
    public StockfishInterface stockfish;
    public ChessnutInterface chessnut;

    override public string Poll (List<string> previousMoves)
    {
        try
        {
            string move = stockfish.GetNextMove(previousMoves);

            ChessPosition from = GameObject.Find(move.Substring(0, 2)).GetComponent<ChessPosition>();
            ChessPosition to = GameObject.Find(move.Substring(2)).GetComponent<ChessPosition>();

            GameObject moving = from.overlappingPieces[0].gameObject;

            moving.transform.position = to.gameObject.transform.position;
            moving.transform.Translate(.10F, 0, 0, Space.World); // TODO: Remove/Fix offset hack

            if (moving.transform.name.Contains("Knight"))
            {
                moving.transform.Translate(-.10F, 0, 0, Space.World);
            }
            else if (this.color == "black")
            {
                moving.transform.Translate(-.20F, 0, 0, Space.World);
            }

            int moveStatus = chessnut.IsValidMove(move);

            if (moveStatus > -1)
            {
                chessGame.SetState(moveStatus);

                return move;
            }
            else
            {
                return null;
            }
        }
        catch
        {
            return null;
        }
    }
}
