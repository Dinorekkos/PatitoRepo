using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Touch;

public class ClotheController_PuzzleVestir : MonoBehaviour
{
    
    [SerializeField] AudioManager audioManager;
    CharacterBodyPart_PuzzleVestir bodyPart;
    #region public methods
    public void HandleSelect(LeanFinger finger)
    {
        if (state == ClothingState.Idle)
        {
            selectionFinger = finger;
            state = ClothingState.Dragging;
            audioManager.Play("PieceUp");
        }
        if (state == ClothingState.Wearing)
        {   
            Vector3 newposition = transform.position;
            this.transform.position = newposition;
            selectionFinger = finger;
            state = ClothingState.Dragging;
            
        }
    }

    public void HandleSelectUp(LeanFinger finger)
    {
        //Handle select up event
        HandleSelectUp();

    }

    public void HandleDeselect()
    {
        selectionFinger = null;
        
    }
    #endregion

    #region public variables
    public ClothingState state;

    public LayerMask detectLayermask;

    public string clotheID = "";

    public int idleSortingOrder = 5;
    public int draggingSortingOrder = 10;
    #endregion

    #region private methods
    private void Start()
    {
        originalPosition = transform.position;
        originalRotation = transform.rotation;
    }

    private void Update()
    {
        if (state == ClothingState.Idle)
        {
            transform.position = originalPosition;
            transform.rotation = originalRotation;
            mySpriteRenderer.sortingOrder = idleSortingOrder;
        }
        else 
        if (state == ClothingState.Dragging)
        {
            transform.position = GetDraggingTargetPosition(selectionFinger);
            mySpriteRenderer.sortingOrder = draggingSortingOrder;
        } else 
        if (state == ClothingState.Wearing)
        {
            mySpriteRenderer.sortingOrder = idleSortingOrder;  
        }
    }

    private Vector3 GetDraggingTargetPosition(LeanFinger finger)
    {
        Vector3 selectPosition = Camera.main.ScreenToWorldPoint(finger.StartScreenPosition);
        Vector3 targetPosition = Camera.main.ScreenToWorldPoint(finger.ScreenPosition);
        Vector3 differencePosition = targetPosition - selectPosition;
        //Fixed target position
        targetPosition = originalPosition + differencePosition;
        targetPosition.z = 0f;
        
        return targetPosition;
    }

    //On selectup, check if clothing is inside a valid body part
    private void HandleSelectUp()
    {
        //Set colliders buffer
        int numColliders = 1;
        Collider2D[] detectedColliders = new Collider2D[numColliders];
        
        //Contact filtering
        ContactFilter2D contactFilter = new ContactFilter2D();
        contactFilter.SetLayerMask(detectLayermask);

        int detectedCount = myCollider.OverlapCollider(contactFilter, detectedColliders);

        //If successful detection, we can try to set clothing at character body
        if (detectedCount > 0)
        {
            CharacterBodyPart_PuzzleVestir bodyPart = detectedColliders[0].GetComponent<CharacterBodyPart_PuzzleVestir>();
            bodyPart.SetClothing(this);

        }
        //If nothing was detected, set idle state
        else
        {
            state = ClothingState.Idle;
        }
    }

    #endregion

    #region private variables
    private Vector3 originalPosition;
    private Quaternion originalRotation;

    private LeanFinger selectionFinger;

    [SerializeField]
    private Collider2D myCollider;
    [SerializeField]
    private SpriteRenderer mySpriteRenderer;

    
    #endregion
}

public enum ClothingState
{
    Idle,
    Dragging,
    Wearing
}

