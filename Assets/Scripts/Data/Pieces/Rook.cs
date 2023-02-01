using System;
using System.Collections.Generic;

public class Rook : BasePiece
{
    public Rook(PlayerColor color) : base("Rook", PieceType.Rook, color) { }

    public override List<Coordinate> GetPseudoLegalMoves(Board board)
    {
        List<Coordinate> legalMoves = new List<Coordinate>();
        var directions = new (int, int)[] { (1, 0), (-1, 0), (0, 1), (0, -1) };

        foreach ((int, int) dir in directions)
        {
            int i = 1;
            bool pathClear = true;
            while (board.WithinGrid(position.GetLocal(dir.Item1 * i, dir.Item2 * i, out Coordinate t)) && pathClear)
            {
                BasePiece p = board.GetPiece(t);
                if (p == null)
                {
                    legalMoves.Add(t);
                }
                else
                {
                    if (this.Color != p.Color)
                    {
                        legalMoves.Add(t);
                    }
                    pathClear = false;
                }
                i += 1;
            }
        }
        

        return legalMoves;
    }

}