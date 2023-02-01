using System;
using UnityEngine;

public class BoardUI : MonoBehaviour
{
    [SerializeField] Sprite squareSprite;
    [SerializeField] PieceTheme pieceTheme;
    [SerializeField] BoardTheme boardTheme;

    private int width;
    private int height;
    private SpriteRenderer[,] squareSRenderers;
    private SpriteRenderer[,] pieceSRenderers;

    public void CreateBoardUI(Board board)
    {
        this.width = board.Width;
        this.height = board.Height;

        squareSRenderers = new SpriteRenderer[width, height];
        pieceSRenderers = new SpriteRenderer[width, height];

        for (int row = 0; row < height; row++)
        {
            for (int file = 0; file < width; file++)
            {

                // render tile
                SpriteRenderer t = new GameObject().AddComponent<SpriteRenderer>();
                squareSRenderers[file, row] = t;
                t.transform.parent = transform;
                t.transform.localPosition = new Vector3(file, row);
                Coordinate c = new Coordinate(file, row);
                t.sprite = squareSprite;
                t.color = c.isLightSquare() ? boardTheme.inactiveLightSquare : boardTheme.inactiveDarkSquare;
                t.name = c.GetGridName();


                // render piece
                BasePiece piece = board.GetPiece(c);
                if (piece != null)
                {
                    CreatePiece(c, piece);
                }
            }
        }
    }

    public Coordinate GetSquareUnderPoint(Vector3 point)
    {
        Vector3 boardSpace = transform.InverseTransformPoint(point);
        int x = Mathf.RoundToInt(boardSpace.x);
        int y = Mathf.RoundToInt(boardSpace.y);

        return new Coordinate(x, y);
    }

    public void DeselectSquare(Coordinate coor)
    {
        squareSRenderers[coor.X, coor.Y].color = boardTheme.GetInactiveColor(coor.GetSquareColor());
    }

    public void SelectSquare(Coordinate coor)
    {
        squareSRenderers[coor.X, coor.Y].color = boardTheme.GetSelectedColor(coor.GetSquareColor());
    }

    public void ResetSquareColors()
    {
        for (int row = 0; row < height; row++)
        {
            for (int file = 0; file < width; file++)
            {
                DeselectSquare(new Coordinate(file, row));
            }
        }
    }

    public void ResetPiecePosition(Coordinate coor)
    {
        SpriteRenderer piece = pieceSRenderers[coor.X, coor.Y];
        piece.transform.localPosition = new Vector3(coor.X, coor.Y);
        piece.sortingOrder = 1;
    }

    public void PieceOnTop(Coordinate coor)
    {
        pieceSRenderers[coor.X, coor.Y].sortingOrder = 2;
    }

    internal void DragPiece(Vector3 to, Coordinate from)
    {
        SpriteRenderer pieceToDrag = pieceSRenderers[from.X, from.Y];
        to.z = 0;
        if (pieceToDrag != null)
        {
            pieceSRenderers[from.X, from.Y].transform.position = to;
        }
    }

    public void UpdateBoard(Board board)
    {

        for (int row = 0; row < height; row++)
        {
            for (int file = 0; file < width; file++)
            {
                // first clear piece
                SpriteRenderer s = pieceSRenderers[file, row];
                if (s != null)
                {
                    Destroy(s.gameObject);
                    pieceSRenderers[file, row] = null;
                }


                Coordinate c = new Coordinate(file, row);

                // render piece
                BasePiece piece = board.GetPiece(c);
                if (piece != null)
                {
                    CreatePiece(c, piece);
                }
            }
        }
    }

    private void CreatePiece(Coordinate c, BasePiece piece)
    {
        SpriteRenderer p = new GameObject().AddComponent<SpriteRenderer>();
        pieceSRenderers[c.X, c.Y] = p;
        p.transform.parent = transform;
        p.transform.localPosition = new Vector3(c.X, c.Y);
        p.sprite = pieceTheme.GetColorSet(piece.Color).GetSpriteFromPieceType(piece.Type);
        p.sortingOrder = 1;
        p.name = $"{piece.Name}:{c.ToString()}";
    }

    public void HighlightLastMove(Move lastMove)
    {
        Coordinate destination = lastMove.Destination;
        squareSRenderers[destination.X, destination.Y].color = boardTheme.lastMoveSquare;
    }

    internal void HighlightLegalMove(Coordinate coor)
    {
        squareSRenderers[coor.X, coor.Y].color = boardTheme.GetLegalMoveColor(coor.GetSquareColor());
    }
}
