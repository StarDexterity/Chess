using System;
using System.Collections.Generic;

public class Queen : BasePiece
{
    public Queen(PlayerColor color) : base("Queen", PieceType.Queen, color) { }

    public static Coordinate[] queenDirections
    {
        get
        {
            return new Coordinate[]
            {
                Coordinate.NW,
                Coordinate.N,
                Coordinate.NE,
                Coordinate.W,
                Coordinate.E,
                Coordinate.SW,
                Coordinate.S,
                Coordinate.SE
            };
        }
    }


    public override List<Coordinate> GetPseudoLegalMoves(Board board)
    {
        List<Coordinate> legalMoves = new List<Coordinate>();

        foreach (Coordinate dir in queenDirections)
        {
            bool pathClear = true;
            int dist = 1;
            while (pathClear && board.WithinGrid(position.GetLocal(dir.X * dist, dir.Y * dist, out Coordinate c)))
            {
                BasePiece piece = board.GetPiece(c);
                if (piece == null || piece.Color != Color)
                {
                    legalMoves.Add(c);
                }

                if (piece != null) pathClear = false;
                dist += 1;
            }
        }

        return legalMoves;
    }
}