using System.Collections;
using System.Collections.Generic;
using DialogSystem;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PuzzleGameController : MonoBehaviour
{
     
    #region private variables

    [SerializeField] private CharacterBodyPart_PuzzleVestir[] puzzlePiece;
    [SerializeField] private Image puzzleCheckImage;
    [SerializeField] private GameObject dialogWin;
    [SerializeField] private DialogManager dialogManager;
    [SerializeField] private ChangeScene changeScene;
    [SerializeField] private int indexScene;
    
    
    #endregion

    #region public methods

    #endregion

    #region public variables
    #endregion


    #region private methods
    private void Update()
    {
        CheckPuzzlesID();
        if (dialogManager.finished==true)
        {
            changeScene.FadeOut(indexScene);
        }
       
    }
    #endregion

    void Start() 
    {
        dialogWin.SetActive(false);
    }

    void CheckPuzzlesID()
    {
        bool allValid = true;
        for (int i = 0; i < puzzlePiece.Length; i++)
        {
            if (puzzlePiece[i].IsValid == false)
            {
                allValid = false;
                
            }
        }

        if (allValid)
        {


            StartCoroutine(Final());
        }
        
        IEnumerator Final()
        {
            yield return new WaitForSeconds(2f);
            dialogWin.SetActive(true);
        }
     
    }

    

}
