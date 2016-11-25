using UnityEngine;
using System.Collections;
using System.Diagnostics;
using System.Collections.Generic;

public class ChessnutInterface : MonoBehaviour {

    Process chessnut;

	// Use this for initialization
	public ChessnutInterface () {
	    this.chessnut = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = System.IO.Directory.GetCurrentDirectory() + "\\Assets\\Scripts\\Chessnut\\dist\\Chessnut.exe",
                    Arguments = "",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardInput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true
                }
            };
        
            this.chessnut.Start();
	}

    public void SetFEN (string fen)
    {
        var stdInput = this.chessnut.StandardInput;
        var stdOutput = this.chessnut.StandardOutput;
        stdInput.AutoFlush = true;

        var s = "set_fen " + fen;
        stdInput.WriteLine(s);

        var output = "";
        while (!output.Contains("set_fen"))
        {
            output = stdOutput.ReadLine();
        }
    }

    public int CheckStatus()
    {
        int state = 0;

        var stdInput = this.chessnut.StandardInput;
        var stdOutput = this.chessnut.StandardOutput;
        stdInput.AutoFlush = true;

        var command = "status";
        stdInput.WriteLine(command);

        var output = "";
        while (!output.Contains("status"))
        {
            output = stdOutput.ReadLine();
        }

        int.TryParse(output.Substring(output.Length - 1, 1), out state);

        return state;
    }

    public int IsValidMove(string move)
    {
        UnityEngine.Debug.Log("IsValidMove(" + move + ")");

        var stdInput = this.chessnut.StandardInput;
        var stdOutput = this.chessnut.StandardOutput;
        stdInput.AutoFlush = true;

        var s = "move " + move;
        stdInput.WriteLine(s);

        var output = "";
        while (!output.Contains("move"))
        {
            output = stdOutput.ReadLine();
        }

        string flag = output.Substring(output.Length-1, 1);

        int state = 0;
        int.TryParse(output.Substring(output.Length - 1, 1), out state);

        if (output.Contains("Illegal"))
        {
            UnityEngine.Debug.Log("Illegal Move: " + move + "\nOutput: " + output + "\nFlag: " + flag);
            return -1;
        }
        else
        {
            UnityEngine.Debug.Log("Valid Move: " + move + "\nOutput: " + output + "\nFlag: " + flag);
            return state;
        }
    }
}
