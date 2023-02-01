using System;
using System.Collections.Generic;

public class Board
{
    // fields
    /// <summary>
    /// Underlying data for pieces. Location of piece in grid is the location of the piece on the board.
    /// </summary>
    private readonly BasePiece[,] PieceGrid;

    /// <summary>
    /// Precomputed list of pieces. Useful for retrieving the pieces without scouring the whole grid.
    /// </summary>
    private HashSet<Coordinate> pieceLocations;

    public readonly int Width;
    public readonly int Height;

    public static Board Standard
    {
        get
        {
            Board newBoard = new Board(8, 8);
            newBoard.AddPiece(0, 0, new Rook(PlayerColor.White));
            newBoard.AddPiece(1, 0, new Knight(PlayerColor.White));
            newBoard.AddPiece(2, 0, new Bishop(PlayerColor.White));
            newBoard.AddPiece(3, 0, new Queen(PlayerColor.White));
            newBoard.AddPiece(4, 0, new King(PlayerColor.White));
            newBoard.AddPiece(5, 0, new Bishop(PlayerColor.White));
            newBoard.AddPiece(6, 0, new Knight(PlayerColor.White));
            newBoard.AddPiece(7, 0, new Rook(PlayerColor.White));

            for (int file = 0; file < 8; file++) { newBoard.AddPiece(file, 1, new Pawn(PlayerColor.White)); }

            newBoard.AddPiece(0, 7, new Rook(PlayerColor.Black));
            newBoard.AddPiece(1, 7, new Knight(PlayerColor.Black));
            newBoard.AddPiece(2, 7, new Bishop(PlayerColor.Black));
            newBoard.AddPiece(3, 7, new Queen(PlayerColor.Black));
            newBoard.AddPiece(4, 7, new King(PlayerColor.Black));
            newBoard.AddPiece(5, 7, new Bishop(PlayerColor.Black));
            newBoard.AddPiece(6, 7, new Knight(PlayerColor.Black));
            newBoard.AddPiece(7, 7, new Rook(PlayerColor.Black));

            for (int file = 0; file < 8; file++) { newBoard.AddPiece(file, 6, new Pawn(PlayerColor.Black)); }

            return newBoard;
        }
    }

    public static Board QueenAndKingEndGame
    {
        get
        {
            Board newBoard = new Board(8, 8);

            newBoard.AddPiece(4, 3, new King(PlayerColor.Black));
            newBoard.AddPiece(0, 0, new Queen(PlayerColor.White));
            newBoard.AddPiece(4, 0, new King(PlayerColor.White));

            return newBoard;
        }
    }

    // constructor
    public Board(int width, int height)
    {
        Width = width;
        Height = height;
        PieceGrid = new BasePiece[width, height];
        pieceLocations = new HashSet<Coordinate>();
    }

    public Board(Board board)
    {
        this.Width = board.Width;
        this.Height = board.Height;
        this.PieceGrid = new BasePiece[Width, Height];
        for (int row = 0; row < Height; row++)
        {
            for (int file = 0; file < Width; file++)
            {
                BasePiece p = board.GetPiece(file, row);
                if (p != null)
                {
                    PieceGrid[file, row] = (BasePiece) p.Clone();
                }
            }
        }
        this.pieceLocations = new HashSet<Coordinate>(board.pieceLocations);
    }


    // methods
    public override string ToString()
    {
        return $"{Width}x{Height} Board";
    }


    private void RemovePiece(int file, int row)
    {
        pieceLocations.Remove(new Coordinate(file, row));
        PieceGrid[file, row] = null;
    }

    private void RemovePiece(Coordinate coor) => RemovePiece(coor.X, coor.Y);


    private void AddPiece(int x, int y, BasePiece piece)
    {
        PieceGrid[x, y] = piece;
        piece.position = new Coordinate(x, y);
        pieceLocations.Add(piece.position);
    }

    private void AddPiece(Coordinate coor, BasePiece piece)
    {
        PieceGrid[coor.X, coor.Y] = piece;
        piece.position = coor;
        pieceLocations.Add(coor);
    }

    public BasePiece GetPiece(int x, int y)
    {
        return PieceGrid[x, y];
    }

    public BasePiece GetPiece(Coordinate tile) => GetPiece(tile.X, tile.Y);

    public BasePiece GetPiece(Coordinate tile, out BasePiece p) => p = GetPiece(tile.X, tile.Y);

    public List<BasePiece> GetPieces()
    {
        List<BasePiece> pieces = new List<BasePiece>();
        foreach (Coordinate c in pieceLocations)
        {
            pieces.Add(GetPiece(c));
        }
        return pieces;
    }

    public List<BasePiece> GetPieces(PlayerColor color)
    {
        List<BasePiece> pieces = new List<BasePiece>();
        foreach (Coordinate c in pieceLocations)
        {
            BasePiece piece = GetPiece(c);
            if (piece.Color == color)
            {
                pieces.Add(piece);
            }
        }
        return pieces;
    }


    public bool WithinGrid(int x, int y)
    {
        return ((x >= 0 && x < Width)
            && (y >= 0 && y < Height));
    }

    public bool WithinGrid(Coordinate tile) => WithinGrid(tile.X, tile.Y);
    

    public MoveStatus Validate(Move move)
    {
        // check both origin and destination are bound within the grid
        if (!WithinGrid(move.Origin) || !WithinGrid(move.Destination)) return MoveStatus.OutOfBounds;

        // check that there is a in fact a moving piece
        BasePiece movingPiece = GetPiece(move.Origin);
        if (movingPiece == null) return MoveStatus.NoPieceToMove;

        // cannot 'move' to the same tile
        if (move.Origin == move.Destination) return MoveStatus.NoMovement;

        // cannot move an enemies piece
        if (move.Turn != movingPiece.Color) return MoveStatus.WrongColorMoved;

        // cannot capture friendly piece
        BasePiece capturePiece = GetPiece(move.Destination);
        if (capturePiece != null && movingPiece.Color == capturePiece?.Color) return MoveStatus.FriendlyPieceCapture;

        // confirm piece rules
        if (!movingPiece.IsMoveLegal(this, move)) return MoveStatus.PieceMovementRule;
        
        // confirm new move does not result in a check for the moving player
        Board newBoard = MakeMove(move);
        if (newBoard.IsInCheck(move.Turn)) return MoveStatus.KingIsInCheck;


        return MoveStatus.Legal;
    }

    public bool IsInCheck(PlayerColor turn)
    {
        // get king of that color
        King king = (King) GetPieces(turn).Find(a => a.Type == PieceType.King);

        // get all enemy pieces
        List<BasePiece> enemyPieces = GetPieces(turn.Next());

        // get all moves
        List<Coordinate> moves = new List<Coordinate>();

        foreach (BasePiece p in enemyPieces)
        {
            moves.AddRange(p.GetPseudoLegalMoves(this));
        }

        foreach (Coordinate move in moves)
        {
            if (king.position == move)
            {
                return true;
            }
        }

        return false;
    }

    public bool hasLegalMove(PlayerColor turn)
    {
        List<BasePiece> friendlyPieces = GetPieces(turn);

        List<Coordinate> LegalMoves = new List<Coordinate>();

        foreach (BasePiece piece in friendlyPieces)
        {
            LegalMoves.AddRange(piece.GetLegalMoves(this));
        }
        return LegalMoves.Count > 0;
    }

    public bool IsCheckmated(PlayerColor turn)
    {
        return IsInCheck(turn) && !hasLegalMove(turn);
    }

    public bool IsStalemate(PlayerColor turn)
    {
        return !IsInCheck(turn) && !hasLegalMove(turn);
    }

    public List<Coordinate> GetDirectionalPath(Coordinate origin, Coordinate dir)
    {
        List<Coordinate> dirPath = new List<Coordinate>();

        while (WithinGrid(origin.GetLocal(dir, out Coordinate nextCoor)))
        {
            dirPath.Add(nextCoor);
        }
        return dirPath;
    }

    public Board MakeMove(Move move)
    {
        Board newBoard = new Board(this);
        newBoard.AddPiece(move.Destination, newBoard.GetPiece(move.Origin));
        newBoard.RemovePiece(move.Origin);
        return newBoard;
    }
}