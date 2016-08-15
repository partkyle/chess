using UnityEngine;
using System.Collections;
using System;

public class Queen : Chessman
{
    public override bool[,] PossibleMoves()
    {
        bool[,] r = new bool[8, 8];

        bool[,] rookMoves = Rook.MoveStyle(CurrentX, CurrentY, isWhite);
        bool[,] bishopMoves = Bishop.MoveStyle(CurrentX, CurrentY, isWhite);

        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                r[i, j] = rookMoves[i, j] || bishopMoves[i, j];
            }
        }

        return r;
    }
}
