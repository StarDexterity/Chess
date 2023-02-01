using System;
using System.Collections.Generic;

public class Knight : BasePiece
{
    public Coordinate[] KnightSquares
    {
        get => new Coordinate[]
        {
            new Coordinate(-1, 2),
            new Coordinate(1, 2),

            new Coordinate(2, -1),
            new Coordinate(2, 1),

            new Coordinate(-2, -1),
            new Coordinate(-2, 1),

            new Coordinate(1, -2),
            new Coordinate(-1, -2),
        };
    }
    public Knight(PlayerColor color) : base("Knight", PieceType.Knight, color) { }

    public override List<Coordinate> GetPseudoLegalMoves(Board board)
    {
        List<Coordinate> legalMoves = new List<Coordinate>();

        foreach (Coordinate candidate in KnightSquares)
        {
            if (board.WithinGrid(position.GetLocal(candidate, out Coordinate c))
                && (board.GetPiece(c, out BasePiece p) == null || p.Color != Color))
            {
                legalMoves.Add(c);
            }
        }

        return legalMoves;
    }
}