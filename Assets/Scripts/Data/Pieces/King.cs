using System.Collections.Generic;

public class King : BasePiece
{
    public King(PlayerColor color) : base("King", PieceType.King, color) { }

    public override List<Coordinate> GetPseudoLegalMoves(Board board)
    {
        List<Coordinate> legalMoves = new List<Coordinate>();

        foreach (Coordinate dir in Coordinate.Directions)
        {
            if (board.WithinGrid(position.GetLocal(dir, out Coordinate c))
                && (board.GetPiece(c, out BasePiece p) == null || p.Color != Color))
            {
                legalMoves.Add(c);
            }
        }
        return legalMoves;
    }
}