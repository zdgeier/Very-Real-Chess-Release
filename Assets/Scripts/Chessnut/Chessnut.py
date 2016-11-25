from game import Game
from game import InvalidMove
import sys

chess_game = Game()
sys.stdout.write("Chessnut C# Interface - Zachary Geier - June 2016\n")
sys.stdout.flush()

running = True

while running:
    text = sys.stdin.readline()

    if "get" in text:
        sys.stdout.write("get " + str(chess_game.get_moves()) + '\n')

    elif "move" in text:
        try:
            move_index = text.find("move")
            location = text[(move_index + 5):(move_index + 9)].strip()

            flag = chess_game.apply_move(location)
            sys.stdout.write("moved " + str(flag) + '\n')
        except InvalidMove as invalid_move:
            sys.stdout.write("move " + str(invalid_move) + '\n')
        except Exception as error:
            sys.stdout.write("move " + str(error) + '\n')

    elif "reset" in text:
        chess_game.reset()
        sys.stdout.write("reset\n")

    elif "print" in text:
        sys.stdout.write("print " + str(chess_game) + '\n')

    elif "status" in text:
        sys.stdout.write("status " + str(chess_game.status) + '\n')

    elif "set_fen" in text:
        set_index = text.find("set_fen")        
        fen = text[(set_index + 8): (text.find("\n"))]
        chess_game.set_fen(fen)
        
        sys.stdout.write("set_fen\n")

    else:
        sys.stdout.write("Invalid Command: " + text)

    sys.stdout.flush()
