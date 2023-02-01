using System.Collections.Generic;

public class Bishop : BasePiece
{
    public static Coordinate[] bishopDirections
    {
        get
        {
            return new Coordinate[]
            {
                Coordinate.NW,
                Coordinate.NE,
                Coordinate.SW,
                Coordinate.SE
            };
        }
    }

    public Bishop(PlayerColor color) : base("Bishop", PieceType.Bishop, color) { }

    public override List<Coordinate> GetPseudoLegalMoves(Board board)
    {
        List<Coordinate> legalMoves = new List<Coordinate>();

        foreach (Coordinate dir in bishopDirections)
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
