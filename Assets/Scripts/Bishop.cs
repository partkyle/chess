using UnityEngine;
using System.Collections;

public class Bishop : Chessman
{
    public override bool[,] PossibleMoves()
    {
        return MoveStyle(CurrentX, CurrentY, isWhite);
    }

    public static bool[,] MoveStyle(int CurrentX, int CurrentY, bool isWhite)
    {
        bool[,] r = new bool[8, 8];

        Chessman c;

        // up left
        for (int i = CurrentX - 1, j = CurrentY + 1; i >= 0 && j < 8; i--, j++)
        {
            Debug.Log("checking " + i + ", " + j);
            c = BoardManager.Instance.Chessmans[i, j];
            if (c != null)
            {
                if (c.isWhite != isWhite)
                {
                    r[i, j] = true;
                }

                break;
            }

            r[i, j] = true;
        }

        // up right
        for (int i = CurrentX + 1, j = CurrentY + 1; i < 8 && j < 8; i++, j++)
        {
            Debug.Log("checking " + i + ", " + j);
            c = BoardManager.Instance.Chessmans[i, j];
            if (c != null)
            {
                if (c.isWhite != isWhite)
                {
                    r[i, j] = true;
                }

                break;
            }

            r[i, j] = true;
        }

        // down left
        for (int i = CurrentX - 1, j = CurrentY - 1; i >= 0 && j >= 0; i--, j--)
        {
            Debug.Log("checking " + i + ", " + j);
            c = BoardManager.Instance.Chessmans[i, j];
            if (c != null)
            {
                if (c.isWhite != isWhite)
                {
                    r[i, j] = true;
                }

                break;
            }

            r[i, j] = true;
        }

        // down right

        for (int i = CurrentX + 1, j = CurrentY - 1; i < 8 && j >= 0; i++, j--)
        {
            Debug.Log("checking " + i + ", " + j);
            c = BoardManager.Instance.Chessmans[i, j];
            if (c != null)
            {
                if (c.isWhite != isWhite)
                {
                    r[i, j] = true;
                }

                break;
            }

            r[i, j] = true;
        }

        return r;
    }
}
