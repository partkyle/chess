using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BoardManager : MonoBehaviour
{
    public static BoardManager Instance { set; get; }
    private bool[,] allowedMoves { set; get; }

    public GameObject WhiteKing;
    public GameObject WhiteQueen;
    public GameObject WhiteRook;
    public GameObject WhiteBishop;
    public GameObject WhiteKnight;
    public GameObject WhitePawn;
    public GameObject BlackKing;
    public GameObject BlackQueen;
    public GameObject BlackRook;
    public GameObject BlackBishop;
    public GameObject BlackKnight;
    public GameObject BlackPawn;

    public Chessman[,] Chessmans { set; get; }

    public bool isWhiteTurn = true;

    private Chessman selectedChessman;

    private Material previousMat;
    public Material selectedMat;


    private List<GameObject> activeChessman;

    private const float TILE_SIZE = 1.0f;
    private const float TILE_OFFSET = 0.5f;
    private LayerMask layerMask;

    private Quaternion orientation = Quaternion.Euler(0, 180, 0);

    private int selectionX = -1;
    private int selectionY = -1;

    void Awake()
    {
        Instance = this;
        layerMask = LayerMask.GetMask("ChessPlane");
        Chessmans = new Chessman[8, 8];
        SpawnAllChessman();
    }

    private void Update()
    {
        UpdateSelection();
        DrawChessBoard();

        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("click " + selectionX + ", " + selectionY);
            if (selectionX >= 0 && selectionY >= 0)
            {
                if (selectedChessman == null)
                {
                    // select the chessman
                    SelectChessman(selectionX, selectionY);
                }
                else
                {
                    // move the chessman
                    MoveChessman(selectionX, selectionY);
                }
            }
        }
    }

    private void SelectChessman(int x, int y)
    {
        if (Chessmans[x, y] == null) return;

        if (Chessmans[x, y].isWhite != isWhiteTurn) return;

        bool hasAtleastOneMove = false;
        
        allowedMoves = Chessmans[x, y].PossibleMoves();
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (allowedMoves[i, j])
                {
                    hasAtleastOneMove = true;
                }
            }
        }

        if (!hasAtleastOneMove) return;

        selectedChessman = Chessmans[x, y];
        BoardHightlights.Instance.HighlightAllowedMoves(allowedMoves);
        previousMat = selectedChessman.GetComponentInChildren<MeshRenderer>().material;
        selectedMat.mainTexture = previousMat.mainTexture;
        selectedChessman.GetComponentInChildren<MeshRenderer>().material = selectedMat;
        selectedChessman.GetComponentInChildren<Animator>().SetBool("Selected", true);
    }

    private void MoveChessman(int x, int y)
    {
        if (allowedMoves[x, y])
        {
            Chessman c = Chessmans[x, y];

            if (c != null)
            {
                // if this is a different piece that the current turn
                if (c.isWhite != isWhiteTurn)
                {
                    activeChessman.Remove(c.gameObject);
                    Destroy(c.gameObject);
                    // this will be done later on, but just doing it now for sanity reasons
                    Chessmans[x, y] = null;
                }
                else
                {
                    // don't allow moving on your own piece
                    return;
                }
            }

            Chessmans[selectedChessman.CurrentX, selectedChessman.CurrentY] = null;
            selectedChessman.transform.position = GetTileCenter(x, y);
            selectedChessman.SetPosition(x, y);
            Chessmans[x, y] = selectedChessman;

            // switch turns
            isWhiteTurn = !isWhiteTurn;
        }

        selectedChessman.GetComponentInChildren<MeshRenderer>().material = previousMat;
        selectedChessman.GetComponentInChildren<Animator>().SetBool("Selected", false);
        selectedChessman = null;
        BoardHightlights.Instance.HideHighlights();
    }

    private void UpdateSelection()
    {
        if (!Camera.main) return;

        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 50.0f, layerMask))
        {
            //Debug.Log(hit.point);
            selectionX = (int)hit.point.x;
            selectionY = (int)hit.point.z;
        }
        else
        {
            selectionX = selectionY = -1;
            selectedChessman = null;
        }
    }

    private void DrawChessBoard()
    {
        Vector3 widthLine = Vector3.right * 8;
        Vector3 heightLine = Vector3.forward * 8;

        for (int i = 0; i <= 8; i++)
        {
            Vector3 start = Vector3.forward * i;
            Debug.DrawLine(start, start + widthLine);
            for (int j = 0; j <= 8; j++)
            {
                start = Vector3.right * j;
                Debug.DrawLine(start, start + heightLine);
            }
        }

        // draw the selection
        if (selectionX >= 0 && selectionY >= 0)
        {
            Debug.DrawLine(
                Vector3.forward * selectionY + Vector3.right * selectionX,
                Vector3.forward * (selectionY + 1) + Vector3.right * (selectionX + 1)
            );

            Debug.DrawLine(
                Vector3.forward * (selectionY + 1) + Vector3.right * selectionX,
                Vector3.right * (selectionX + 1) + Vector3.forward * selectionY
            );
        }
    }

    private void SpawnAllChessman()
    {
        activeChessman = new List<GameObject>();

        // spawn the white team

        SpawnChessman(WhiteKing, 4, 0);

        SpawnChessman(WhiteQueen, 3, 0);

        SpawnChessman(WhiteBishop, 2, 0);
        SpawnChessman(WhiteBishop, 5, 0);

        SpawnChessman(WhiteKnight, 1, 0);
        SpawnChessman(WhiteKnight, 6, 0);

        SpawnChessman(WhiteRook, 0, 0);
        SpawnChessman(WhiteRook, 7, 0);

        for (int i = 0; i < 8; i++)
        {
            SpawnChessman(WhitePawn, i, 1);
        }

        // spawn the black team
        SpawnChessman(BlackKing, 3, 7);

        SpawnChessman(BlackQueen, 4, 7);

        SpawnChessman(BlackBishop, 2, 7);
        SpawnChessman(BlackBishop, 5, 7);

        SpawnChessman(BlackKnight, 1, 7);
        SpawnChessman(BlackKnight, 6, 7);

        SpawnChessman(BlackRook, 0, 7);
        SpawnChessman(BlackRook, 7, 7);

        for (int i = 0; i < 8; i++)
        {
            SpawnChessman(BlackPawn, i, 6);
        }
    }

    private void SpawnChessman(GameObject piecePrefab, int x, int y)
    {
        Vector3 position = GetTileCenter(x, y);
        GameObject go = Instantiate(piecePrefab, position, orientation) as GameObject;
        go.transform.SetParent(transform);
        Chessmans[x, y] = go.GetComponent<Chessman>();
        Chessmans[x, y].SetPosition(x, y);
        activeChessman.Add(go);
    }

    public static Vector3 GetTileCenter(int x, int y)
    {
        Vector3 position = Vector3.zero;
        position.x += (TILE_SIZE * x) + TILE_OFFSET;
        position.z += (TILE_SIZE * y) + TILE_OFFSET;
        return position;
    }
}
