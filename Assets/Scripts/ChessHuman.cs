using UnityEngine;
using System.Collections.Generic;
using Valve.VR;

public class ChessHuman : ChessPlayer {
    public ChessGame chessGame;
    public ChessnutInterface chessnut;

    override public string Poll (List<string> previousMoves)
    {
        string move = null;

        if (Input.GetKeyUp(KeyCode.Space) || HasPressedController())
        {
            UnityEngine.Debug.Log("Get move");
            move = GetMove();
        }

        return move;
    }

    bool HasPressedController ()
    {
        bool rightPadPressed = false;
        bool leftPadPressed = false;

        // TODO: Access button on both controllers
        int rightControllerIndex = SteamVR_Controller.GetDeviceIndex(SteamVR_Controller.DeviceRelation.FarthestRight);
        int leftControllerIndex = SteamVR_Controller.GetDeviceIndex(SteamVR_Controller.DeviceRelation.FarthestLeft);

        // Checks if controller is valid/detected
        if (rightControllerIndex != -1)
        {
            rightPadPressed = SteamVR_Controller.Input(rightControllerIndex).GetPressUp(EVRButtonId.k_EButton_SteamVR_Touchpad);
        }
        if (leftControllerIndex != -1)
        {
            leftPadPressed = SteamVR_Controller.Input(leftControllerIndex).GetPressUp(EVRButtonId.k_EButton_SteamVR_Touchpad);
        }

        return rightPadPressed || leftPadPressed;
    }

    string GetMove ()
    {
        foreach (ChessPiece myPiece in myPieces)
        {
            if (myPiece.HasMoved())
            {
                string move = myPiece.GetMovement();
                UnityEngine.Debug.Log("Movement was: " + move);

                int moveStatus = chessnut.IsValidMove(move);
                UnityEngine.Debug.Log("Move status was: " + moveStatus);

                if (moveStatus > -1)
                {
                    myPiece.CommitMovement();
                    chessGame.SetState(moveStatus);

                    foreach (ChessPiece piece in myPieces)
                    {
                        // Checks if piece has been taken
                        if (piece != null)
                        {
                            if (color == "white")
                            {
                                piece.GetComponent<Renderer>().material.color = Color.white;
                            }
                            else if (color == "black")
                            {

                            }
                        }
                    }

                    return move;
                }
                else
                {
                    myPiece.GetComponent<Renderer>().material.color = Color.red;
                }                   
            }
        }

        return null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
