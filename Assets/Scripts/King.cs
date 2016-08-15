using UnityEngine;
using System.Collections;
using System;

public class King : Chessman
{
    public override bool[,] PossibleMoves()
    {
        bool[,] r = new bool[8, 8];

        Chessman c;
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                // skip own space
                if (i == 0 && j == 0)
                    continue;

                int newX = CurrentX + i;
                int newY = CurrentY + j;

                if (newX >= 0 && newX < 8 && newY >= 0 && newY < 8)
                {
                    c = BoardManager.Instance.Chessmans[newX, newY];

                    if (c == null || c.isWhite != isWhite)
                    {
                        r[newX, newY] = true;
                    }
                }
            }
        }

        return r;
    }
}
