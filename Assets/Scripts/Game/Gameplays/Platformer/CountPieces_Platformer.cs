using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountPieces_Platformer : MonoBehaviour
{
    private int numPieces;
    private bool allPieces;
    public int NumPieces
    {
        get { return numPieces; }
        set { numPieces = value;}
    }

    public bool AllPieces
    {
        get { return allPieces; }
        set { allPieces = value; }
    }
    

    private void Update()
    {
        if (NumPieces > 8)
        {
            AllPieces = true;
        }
    }
}

