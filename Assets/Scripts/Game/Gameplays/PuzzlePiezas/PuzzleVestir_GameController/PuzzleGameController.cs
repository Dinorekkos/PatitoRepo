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
    
        if (puzzlePiece[0].IsValid && puzzlePiece[1].IsValid)
        {
            puzzleCheckImage.color = Color.green;
            
        } else
        {
            puzzleCheckImage.color = Color.red;
        }
    }
    #endregion

    void Start() 
    {  
        
        
    }

    void CheckPuzzlesID()
    {
    
    }
   
   

    
}
