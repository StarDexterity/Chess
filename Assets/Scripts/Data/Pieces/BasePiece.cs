using System;
using System.Collections.Generic;


public abstract class BasePiece : ICloneable
{
    public readonly string Name;
    public readonly PlayerColor Color;
    public readonly PieceType Type;
    public Coordinate position;

    protected BasePiece(string name, PieceType type, PlayerColor color)
    {
        this.Name = name;
        this.Color = color;
        this.Type = type;
    }

    public object Clone()
    {
        return MemberwiseClone();
    }

    public virtual List<Coordinate> GetPseudoLegalMoves(Board board)
    {
        List<Coordinate> legalMoves = new List<Coordinate>();

        for (int row = 0; row < board.Height; row++)
        {
            for (int file = 0; file < board.Width; file++)
            {
                legalMoves.Add(new Coordinate(file, row));
            }
        }
        return legalMoves;
    }

    public List<Coordinate> GetLegalMoves(Board board)
    {
        List<Coordinate> legalMoves = new List<Coordinate>();

        List<Coordinate> pseudoLegal = GetPseudoLegalMoves(board);

        foreach (Coordinate move in pseudoLegal)
        {
            if (!board.MakeMove(new Move(Color, position, move)).IsInCheck(Color))
            {
                legalMoves.Add(move);
            }
        }
        return legalMoves;
    }

    public bool IsMoveLegal(Board board, Move move)
    {
        return GetPseudoLegalMoves(board).Contains(move.Destination);
    }
}