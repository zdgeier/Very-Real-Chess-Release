using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Diagnostics;
using System.Collections.Generic;

public class ChessGame : MonoBehaviour {
    List<string> previousMoves;

    public StockfishInterface stockfish;
    public ChessnutInterface chessnut;

    public ChessPlayer white;
    public ChessPlayer black;
    public ChessPiece[] chessPieces;
    public ChessPosition[] chessPositions;

    public GameObject EndGameWin;
    public GameObject EndGameLose;
    public GameObject EndGameStalemate;

    bool whiteHasMove;
    bool blackHasMove;
    bool running;

    int state = 0;
    string humanColor = "white";
    string aiColor = "black";

    // Use this for initialization
    void Start() {
        previousMoves = new List<string>();

        running = false;
    }

    private void WhiteMove(string move)
    {
        UnityEngine.Debug.LogError("White moved: " + move);
        WhiteDestroyTakenPiece(move); 
        previousMoves.Add(move);
    }

    private void BlackMove(string move)
    {
        UnityEngine.Debug.LogError("Black moved: " + move);
        BlackDestroyTakenPiece(move);
        previousMoves.Add(move);
    }

    private void BlackDestroyTakenPiece(string move)
    {
        ChessPosition destination = GameObject.Find(move.Substring(2)).GetComponent<ChessPosition>();

        int threshold = 0;
        if (humanColor == "black") threshold = 1;

        // TODO: Verify that movement will only occur after bottom piece is taken
        if (destination.overlappingPieces.Count > threshold)
        {
            destination.TakeBottomPiece();
        }
    }

    private void WhiteDestroyTakenPiece(string move)
    {
        ChessPosition destination = GameObject.Find(move.Substring(2)).GetComponent<ChessPosition>();

        int threshold = 0;
        if (humanColor == "white") threshold = 1;

        // TODO: Verify that movement will only occur after bottom piece is taken
        if (destination.overlappingPieces.Count > threshold)
        {
            destination.TakeBottomPiece();
        }
    }

    public void StartGame()
    {
        GameObject human = this.transform.Find("Human").gameObject;
        GameObject ai = this.transform.Find("AI").gameObject;

        Transform whiteTemp = this.transform.Find("White").gameObject.transform;
        Transform blackTemp = this.transform.Find("Black").gameObject.transform;

        List<Transform> humanPieces = new List<Transform>();
        List<Transform> aiPieces = new List<Transform>();

        foreach (Transform whiteChild in whiteTemp)
        {
            if (humanColor == "white")
            {
                humanPieces.Add(whiteChild);
            }
            else
            {
                aiPieces.Add(whiteChild);
            }
        }

        foreach (Transform blackChild in blackTemp)
        {
            if (humanColor == "black")
            {
                humanPieces.Add(blackChild);
            }
            else
            {
                aiPieces.Add(blackChild);
            }
        }

        foreach(Transform piece in humanPieces)
        {
            piece.SetParent(human.transform);
        }

        foreach (Transform piece in aiPieces)
        {
            piece.SetParent(ai.transform);
        }

        human.SetActive(true);
        ai.SetActive(true);

        chessPieces = GameObject.FindObjectsOfType<ChessPiece>();
        chessPositions = GameObject.FindObjectsOfType<ChessPosition>();

        if (humanColor == "white")
        {
            white = GameObject.FindObjectOfType<ChessHuman>();
            black = GameObject.FindObjectOfType<ChessAI>();

            white.setColor("white");
            black.setColor("black");
        }
        else
        {
            black = GameObject.FindObjectOfType<ChessHuman>();
            white = GameObject.FindObjectOfType<ChessAI>();

            white.setColor("white");
            black.setColor("black");
        }

        whiteHasMove = true;
        blackHasMove = false;

        running = true;
    }

    public void Reset()
    {
        SceneManager.LoadScene(0);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void SetDifficulty (int difficulty)
    {
        UnityEngine.Debug.LogError("Difficulty set to " + difficulty);
        stockfish.SetSkillLevel(difficulty);
    }

    public void SetHumanColor (string humanColor)
    {
        UnityEngine.Debug.LogError("Human Color set to " + humanColor);
        if(humanColor == "white")
        {
            this.humanColor = humanColor;
            this.aiColor = "black";
        }
        else {
            this.humanColor = humanColor;
            this.aiColor = "white";
        }
            
    }

    public void SetState(int flag)
    {
        UnityEngine.Debug.LogError("State: " + flag);

        /*  
            Flags: 
            NORMAL = 0
            CHECK = 1
            CHECKMATE = 2
            STALEMATE = 3 
            ENPASSANT_FLAG = 4
            CASTLE_FLAG = 5 
        */

        this.state = flag;

        switch (flag)
        {
            case 0:
                // Normal, Do nothing
                break;
            case 1:
                CheckState();
                break;
            case 2:
                CheckmateState();
                break;
            case 3:
                StalemateState();
                break;
            case 4:
                EnPassantState();
                break;
            case 5:
                CastleState();
                break;
            case 6:
                PromoteState();
                break;
        }
    }

    private void CheckState()
    {

    }

    private void CheckmateState()
    {
        if (whiteHasMove)
        {
            whiteHasMove = false;
            blackHasMove = false;

            if (humanColor == "black")
            {
                EndGameWin.gameObject.SetActive(true);
            }
            else
            {
                EndGameLose.gameObject.SetActive(true);
            }
        }
        else if (blackHasMove)
        {
            whiteHasMove = false;
            blackHasMove = false;

            if (humanColor == "white")
            {
                EndGameWin.gameObject.SetActive(true);
            }
            else
            {
                EndGameLose.gameObject.SetActive(true);
            }
        }
    }

    private void StalemateState()
    {
        EndGameStalemate.SetActive(true);
    }

    private void EnPassantState()
    {

    }

    private void CastleState()
    {
        UnityEngine.Debug.LogError("Castling!");

        if (whiteHasMove)
        {
            ChessPiece whiteKing = GameObject.Find("King e1").gameObject.GetComponent<ChessPiece>();
            string whiteKingPosition = whiteKing.currentPosition.gameObject.name;

            // Moved left
            if (whiteKingPosition == "c1")
            {
                ChessPiece rightRook = GameObject.Find("Rook a1").gameObject.GetComponent<ChessPiece>();

                rightRook.transform.position = GameObject.Find("d1").gameObject.transform.position;
                rightRook.transform.Translate(.10F, 0, 0, Space.World);
                rightRook.CommitMovement();
            }
            // Moved right
            else
            {
                ChessPiece leftRook = GameObject.Find("Rook h1").gameObject.GetComponent<ChessPiece>();

                leftRook.transform.position = GameObject.Find("f1").gameObject.transform.position;
                leftRook.transform.Translate(.10F, 0, 0, Space.World);
                leftRook.CommitMovement();
            }
        }
        else
        {
            ChessPiece blackKing = GameObject.Find("King e8").gameObject.GetComponent<ChessPiece>();
            string blackKingPosition = blackKing.currentPosition.gameObject.name;

            // Moved left
            if (blackKingPosition == "c8")
            {
                ChessPiece rightRook = GameObject.Find("Rook a8").gameObject.GetComponent<ChessPiece>();

                rightRook.transform.position = GameObject.Find("d8").gameObject.transform.position;
                rightRook.transform.Translate(-.10F, 0, 0, Space.World);
                rightRook.CommitMovement();
            }
            // Moved right
            else
            {
                ChessPiece leftRook = GameObject.Find("Rook h8").gameObject.GetComponent<ChessPiece>();

                leftRook.transform.position = GameObject.Find("f8").gameObject.transform.position;
                leftRook.transform.Translate(-.10F, 0, 0, Space.World);
                leftRook.CommitMovement();
            }
        }
    }

    void PromoteState()
    {
        string[] firstRank = { "a1", "b1", "c1", "d1", "e1", "f1", "g1", "h1" };
        string[] lastRank = { "a8", "b8", "c8", "d8", "e8", "f8", "g8", "h8" };

        if (whiteHasMove)
        {
            // Get pawn in highest rank and change to queen model
            foreach (string locationName in lastRank)
            {
                ChessPosition pos = GameObject.Find(locationName).gameObject.GetComponent<ChessPosition>();
                ChessPiece piece = pos.overlappingPieces[0];

                if (piece.name.Contains("Pawn"))
                {
                    MeshFilter filter = piece.gameObject.GetComponent<MeshFilter>();
                    Mesh queenMesh = Resources.Load<Mesh>("obj_14");

                    filter.mesh = queenMesh;
                }
            }
        }
        else
        {
            // Get pawn in highest rank and change to queen model
            foreach (string locationName in firstRank)
            {
                ChessPosition pos = GameObject.Find(locationName).gameObject.GetComponent<ChessPosition>();
                ChessPiece piece = pos.overlappingPieces[0];

                if (piece.name.Contains("Pawn"))
                {
                    MeshFilter filter = piece.gameObject.GetComponent<MeshFilter>();
                    Mesh queenMesh = Resources.Load<Mesh>("obj_18");

                    filter.mesh = queenMesh;
                }
            }
        }
    }

    // Update is called once per frame
    void Update () {
        if (running)
        {
            if (whiteHasMove == true)
            {
                var move = white.Poll(previousMoves);

                if (move != null)
                {
                    WhiteMove(move);

                    whiteHasMove = false;
                    blackHasMove = true;

                    SetState(chessnut.CheckStatus());
                }
            }
            else if (blackHasMove == true)
            {
                var move = black.Poll(previousMoves);

                if (move != null)
                {
                    BlackMove(move);

                    blackHasMove = false;
                    whiteHasMove = true;

                    SetState(chessnut.CheckStatus());
                }
            }
        }
	}
}
