using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField] private InputAction mouseClick;

    private Coordinate selected;
    private Coordinate destination;

    private bool isSquareSelected = false;


    // ui
    private BoardUI boardUI;
    private Camera main;

    // data
    private Board gameBoard;
    private PlayerColor turn;
    private Move lastMove;


    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        GameObject[] editorOnly = GameObject.FindGameObjectsWithTag("EditorOnly");
        foreach (var obj in editorOnly)
        {
            obj.SetActive(false);
        }

        boardUI = FindObjectOfType<BoardUI>();
        main = Camera.main;
        main.transform.position = new Vector3(width / 2 - 0.5f, height / 2 - 0.5f, -10);

        gameBoard = Board.QueenAndKingEndGame;
        turn = PlayerColor.White;

        boardUI.CreateBoardUI(gameBoard);

    }

    private void OnEnable()
    {
        mouseClick.Enable();
        mouseClick.performed += MousePressed;
    }

    private void OnDisable()
    {
        mouseClick.performed -= MousePressed;
        mouseClick.Disable();
    }

    private void MousePressed(InputAction.CallbackContext obj)
    {
        // get coordinate of square pressed
        Vector3 worldPoint = main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        Coordinate coor = boardUI.GetSquareUnderPoint(worldPoint);


        // deselect if clicks outside grid
        if (!gameBoard.WithinGrid(coor))
        {
            isSquareSelected = false;
        }
        else if (isSquareSelected)
        {
            // second selection
            SetDestination(coor);

            if (SubmitMove() != MoveStatus.Legal)
            {
                if (gameBoard.GetPiece(coor) != null)
                {
                    selected = destination;
                    StartCoroutine(DragUpdate(selected));
                }
                else
                {
                    isSquareSelected = false;
                }
            }
        }
        else
        {
            // first selection
            if (gameBoard.GetPiece(coor) != null)
            {
                // only valid first selection is a piece
                isSquareSelected = true;
                selected = coor;
                StartCoroutine(DragUpdate(selected));
            }
        }
        UpdateSquareColors();
    }

    private IEnumerator DragUpdate(Coordinate startSquare)
    {
        boardUI.PieceOnTop(startSquare);

        while (mouseClick.ReadValue<float>() != 0)
        {
            Vector3 worldSpace = main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            boardUI.DragPiece(worldSpace, startSquare);
            yield return null;
        }

        boardUI.ResetPiecePosition(startSquare);

        // check to see if piece was dragged onto a new square
        Vector3 newPos = main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        Coordinate endSquare = boardUI.GetSquareUnderPoint(newPos);

        if (startSquare != endSquare)
        {
            SetDestination(endSquare);
            SubmitMove();
        }
    }

    public void SetSelected(Coordinate coor)
    {
        isSquareSelected = true;
        selected = coor;
    }

    public void SetDestination(Coordinate coor)
    {
        destination = coor;
    }

    public MoveStatus SubmitMove()
    {
        Move move = new Move(turn, selected, destination);


        MoveStatus status = gameBoard.Validate(move);
        if (status == MoveStatus.Legal)
        {
            OnMoveAccepted(move);
        }
        else
        {
            OnMoveDenied(move, status);
        }
        return status;
    }

    public void OnMoveAccepted(Move move)
    {
        gameBoard = gameBoard.MakeMove(move);
        lastMove = move;
        turn = turn.Next();
        isSquareSelected = false;

        // check for checkmate
        if (gameBoard.IsCheckmated(turn))
        {
            Debug.Log($"{turn} king is checkmated!");
        }

        // check for stalemate
        if (gameBoard.IsStalemate(turn))
        {
            Debug.Log("Stalemate!");
        }

        // update board ui
        UpdateSquareColors();
        boardUI.UpdateBoard(gameBoard);
    }

    public void OnMoveDenied(Move move, MoveStatus status)
    {
        Debug.Log(status);
    }

    public void UpdateSquareColors()
    {
        boardUI.ResetSquareColors();


        if (isSquareSelected)
        {
            boardUI.SelectSquare(selected);
        }

        if (lastMove != null)
        {
            boardUI.HighlightLastMove(lastMove);
        }

        BasePiece piece = gameBoard.GetPiece(selected);
        if (isSquareSelected && piece.Color == turn)
        {
            foreach (Coordinate square in piece.GetLegalMoves(gameBoard))
            {
                boardUI.HighlightLegalMove(square);
            }
        }

    }
}