using UnityEngine;
using System.Collections;
using System.Diagnostics;
using System.Collections.Generic;

public class StockfishInterface : MonoBehaviour {

    Process stockfish;

	// Use this for initialization
	public StockfishInterface () {
        this.stockfish = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = System.IO.Directory.GetCurrentDirectory() + "\\Assets\\Scripts\\Stockfish\\stockfish.exe",
                Arguments = "",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardInput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            }
        };

        this.stockfish.Start();
    }

    public void SetFEN(string fen)
    {
        var stdInput = stockfish.StandardInput;
        var stdOutput = stockfish.StandardOutput;
        stdInput.AutoFlush = true;

        stdInput.WriteLine(""); // Fixes bug where engine doesn't recognize correct command
        stdInput.WriteLine("position fen " + fen);
    }

    public void SetSkillLevel(int skillLevel)
    {
        var stdInput = stockfish.StandardInput;
        var stdOutput = stockfish.StandardOutput;
        stdInput.AutoFlush = true;

        stdInput.WriteLine(""); // Fixes bug where engine doesn't recognize correct command
        stdInput.WriteLine("setoption name Skill Level value " + skillLevel);
    }

    public string GetNextMove(List<string> previousMoves)
    {
        string totalMessage = "Moves: ";
        foreach (string move in previousMoves)
        {
            totalMessage = totalMessage + " " + move;
        }
        UnityEngine.Debug.Log(totalMessage);

        var stdInput = stockfish.StandardInput;
        var stdOutput = stockfish.StandardOutput;
        stdInput.AutoFlush = true;

        stdInput.WriteLine(""); // Fixes bug where engine doesn't recognize correct command
        stdInput.WriteLine("position startpos moves " + string.Join(" ", previousMoves.ToArray()));


        stdInput.WriteLine("go");
        string output = "";
        while (!output.Contains("bestmove"))
        {
            output = stdOutput.ReadLine();
        }

        // Gets the bestmove position from the output
        var outputLocation = output.Split(' ')[1];

        return outputLocation;
    }
}
