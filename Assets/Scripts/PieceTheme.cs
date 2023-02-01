using UnityEngine;
using System;

[CreateAssetMenu]
public class PieceTheme : ScriptableObject
{
    public SpritePieceSet whitePieces;
    public SpritePieceSet blackPieces;

    [Serializable]
    public class SpritePieceSet
    {
        public Sprite Pawn, Rook, Knight, Bishop, Queen, King;

        public Sprite GetSpriteFromPieceType(PieceType type) => type switch
        {
            PieceType.None => null,
            PieceType.Pawn => Pawn,
            PieceType.Rook => Rook,
            PieceType.Knight => Knight,
            PieceType.Bishop => Bishop,
            PieceType.Queen => Queen,
            PieceType.King => King,
            _ => null,
        };
    }

    public SpritePieceSet GetColorSet(PlayerColor color) => color switch
    {
        PlayerColor.None => null,
        PlayerColor.White => whitePieces,
        PlayerColor.Black => blackPieces,
        _ => null,
    };
}