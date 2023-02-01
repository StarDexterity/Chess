public class Move
{
    public PlayerColor Turn;
    public Coordinate Origin;
    public Coordinate Destination;

    public Move(PlayerColor turn)
    {
        this.Turn = turn;
    }

    public Move(PlayerColor turn, Coordinate origin, Coordinate destination)
    {
        this.Turn = turn;
        this.Origin = origin;
        this.Destination = destination;
    }

    public override string ToString()
    {
        string moving = Turn == PlayerColor.White ? "Whites Turn" : "Blacks Turn";
        return $"{moving}:{Origin}=>{Destination}";
    }
}
