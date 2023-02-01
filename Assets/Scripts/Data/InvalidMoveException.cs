[System.Serializable]
public class InvalidMoveException : System.Exception
{
    public Board AttemptedOn;
    public Move AttemptedMove;
    public MoveStatus Reason;

    public InvalidMoveException() { }

    public InvalidMoveException(Board board, Move move, MoveStatus reason) 
        : base($"Invalid move: {move} on {board}. {GetMessage(reason)}")
    {
        this.AttemptedOn = board;
        this.AttemptedMove = move;
        this.Reason = reason;
    }

    public InvalidMoveException(string message, System.Exception inner) : base(message, inner) { }

    protected InvalidMoveException(
      System.Runtime.Serialization.SerializationInfo info,
      System.Runtime.Serialization.StreamingContext context) : base(info, context) { }

    private static string GetMessage(MoveStatus reason)
    {
        switch (reason)
        {
            case MoveStatus.NotValidated:
                return "Move has not been validated";
            case MoveStatus.Legal:
                return "Move is Legal";
            case MoveStatus.OutOfBounds:
                return "Origin and Destination tiles must be within the bounds of the grid";
            case MoveStatus.PieceMovementRule:
                return "Attempted move does not respect the moving pieces movement patterns and rules";
            case MoveStatus.FriendlyPieceCapture:
                return "Attempted move results in a friendly piece captured";
            case MoveStatus.WrongColorMoved:
                return "Attempted to move the wrong piece color";
            case MoveStatus.NoPieceToMove:
                return "No piece selected to move";
            case MoveStatus.NoMovement:
                return "Cannot Move a piece in place";
            case MoveStatus.KingIsInCheck:
                return "Move is not legal if it results in a position where the moving players king is in check and can be captured";
            default:
                return "Huh??!?";
        }
    }
}