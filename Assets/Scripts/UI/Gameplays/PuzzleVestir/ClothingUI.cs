using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClothingUI : MonoBehaviour
{
    #region public methods

    #endregion

    #region public variables

    #endregion

    #region private methods
    private void Update()
    {
        if (head.IsValid)
        {
            headCheckImage.color = Color.green;
        } else
        {
            headCheckImage.color = Color.red;
        }

        if (body.IsValid)
        {
            bodyCheckImage.color = Color.green;
        }
        else
        {
            bodyCheckImage.color = Color.red;
        }
    }
    #endregion

    #region private variables
    [SerializeField]
    private CharacterBodyPart_PuzzleVestir head;
    [SerializeField]
    private CharacterBodyPart_PuzzleVestir body;

    [SerializeField]
    private Image headCheckImage;
    [SerializeField]
    private Image bodyCheckImage;
    #endregion
}
