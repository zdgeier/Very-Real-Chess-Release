using UnityEngine;
using System.Collections;

public class ChessTester : MonoBehaviour {

    public string start_fen;
    public ChessnutInterface chessnut;
    public StockfishInterface stockfish;
    public ChessGame chessGame;
    public string humanColor;
    public int difficulty;
    public bool isTesting;

    bool justStarted;

    // Use this for initialization
    void Start () {
        justStarted = true;
	}
	
	// Update is called once per frame
	void Update () {
        if (justStarted && isTesting)
        {
            chessnut.SetFEN(start_fen);
            stockfish.SetFEN(start_fen);

            chessGame.SetDifficulty(difficulty);
            chessGame.SetHumanColor(humanColor);
            chessGame.StartGame();
            justStarted = false;
        }
        
    }
}
