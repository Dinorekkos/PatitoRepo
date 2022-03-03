using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class CharacterBodyPart_PuzzleVestir : MonoBehaviour
{
    [SerializeField] private AudioManager audioManager;
    [SerializeField]private Collider2D collider2D;
    [SerializeField] private Collider2D collOhter;
    #region public methods
    public void SetClothing(ClotheController_PuzzleVestir clothing)
    {
        //If we had already a clothing
        if (myClothing != null && IsValid)
        {
            //Set idle state and remove clothing reference
            myClothing.state = ClothingState.Idle;
           
        }
        
        myClothing = clothing;

        myClothing.transform.position = this.transform.position;
        myClothing.transform.rotation = Quaternion.Euler(0,0,0);
        
        audioManager.Play("PieceDown");

        myClothing.state = ClothingState.Wearing;
    }
    #endregion
    #region public variables
    public string validID = "";
    public bool IsValid
    {
        get
        {
            if (myClothing == null)
            {
                
                return false;  
            }
                
            if (myClothing.clotheID.Equals(validID))
            {
                return true;
            } else
            {
                
                return false;
            }
        }
    }
    #endregion

    #region private methods
    private void Start() 
    {
        collider2D = this.GetComponent<Collider2D>();
    }

    private void Update()
    {
        if (IsValid)
        {
            collider2D.enabled = false;
            collOhter.enabled = false;
        }
        else
        {
            collider2D.enabled = true;
            collOhter.enabled = true;
        }
    }

    #endregion

    #region private variables
    //Current clothing for this body part
    private ClotheController_PuzzleVestir myClothing;

    private bool _isValid = false;
    #endregion
    
    
}



[System.Serializable]
public enum BodyPartType
{
    Head,
    Body
}