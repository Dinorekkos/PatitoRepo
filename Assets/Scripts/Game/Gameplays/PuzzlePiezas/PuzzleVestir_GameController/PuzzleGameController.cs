using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PuzzleGameController : MonoBehaviour
{
     
    #region private variables

    [SerializeField] private CharacterBodyPart_PuzzleVestir[] puzzlePiece;
    [SerializeField] private Image puzzleCheckImage;
    
    #endregion

    #region public methods

    #endregion

    #region public variables
    #endregion


    #region private methods
    private void Update()
    {
        CheckPuzzlesID();
       
    }
    #endregion

    void Start() 
    {

    }

    void CheckPuzzlesID()
    {
        if (puzzlePiece[0].IsValid && puzzlePiece[1].IsValid && puzzlePiece[2].IsValid && puzzlePiece[3].IsValid && puzzlePiece[4].IsValid && puzzlePiece[5].IsValid 
            && puzzlePiece[6].IsValid && puzzlePiece[7].IsValid)
        {
            puzzleCheckImage.color = Color.green;
            
        } else
        {
            puzzleCheckImage.color = Color.red;
        }
    }

}
