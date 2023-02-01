public enum PlayerColor
{
    None,
    White,
    Black
}

static class PlayerColorMethods
{
    public static PlayerColor Next(this PlayerColor color)
    {
        return color == PlayerColor.White ? PlayerColor.Black : PlayerColor.White;
    }
}