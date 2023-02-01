using System;
using System.Collections.Generic;

public class Pawn : BasePiece
{
    public Pawn(PlayerColor color) : base("Pawn", PieceType.Pawn, color) { }

    public Coordinate Forward
    {
        get
        {
            return Color == PlayerColor.White ? new Coordinate(0, 1) : new Coordinate(0, -1);
        }
    }

    public Coordinate DoubleForward
    {
        get
        {
            return Color == PlayerColor.White ? new Coordinate(0, 2) : new Coordinate(0, -2);
        }
    }

    public override List<Coordinate> GetPseudoLegalMoves(Board board)
    {
        List<Coordinate> legalMoves = new List<Coordinate>();

        if (board.WithinGrid(position.GetLocal(Forward, out Coordinate c)) && board.GetPiece(c) == null)
        {
            legalMoves.Add(c);
        }

        if ((position.Y == 1 || position.Y == 6)
            && board.GetPiece(position.GetLocal(Forward)) == null
            && board.GetPiece(position.GetLocal(DoubleForward, out Coordinate c1)) == null)
        {
            legalMoves.Add(c1);
        }

        foreach (Coordinate dir in new Coordinate[]{ new Coordinate(1, 0), new Coordinate(-1, 0)})
        {
            if (board.WithinGrid(position.GetLocal(Forward + dir, out Coordinate c2))
            && board.GetPiece(c2, out BasePiece p) != null
            && p.Color != Color)
            {
                legalMoves.Add(c2);
            }
        }
        
        return legalMoves;
    }
}