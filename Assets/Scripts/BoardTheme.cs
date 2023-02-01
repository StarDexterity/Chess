using UnityEngine;

[CreateAssetMenu]
public partial class BoardTheme : ScriptableObject
{
    public Color inactiveLightSquare;
    public Color inactiveDarkSquare;
    public Color selectedLightSquare;
    public Color selectedDarkSquare;
    public Color lastMoveSquare;
    public Color legalMoveLight;
    public Color legalMoveDark;

    public Color GetInactiveColor(SquareColor color) => color == SquareColor.Light ? inactiveLightSquare : inactiveDarkSquare;

    public Color GetSelectedColor(SquareColor color) => color == SquareColor.Light ? selectedLightSquare : selectedDarkSquare;

    public Color GetLegalMoveColor(SquareColor color) => color == SquareColor.Light ? legalMoveLight : legalMoveDark;
}