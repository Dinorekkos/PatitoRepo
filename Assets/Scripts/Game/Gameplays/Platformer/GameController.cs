using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using Gameplays.Platformer;
using UnityEngine;
using UnityEngine.Events;

public class GameController : MonoBehaviour
{   
    [Header("Eventos")]
    public UnityEvent FinishedEvent;

    public UnityEvent PausedEvent;
    
    [Header("Booleans")]
    [SerializeField]private bool gameStart;
    [SerializeField]private bool gameFinished;
    [SerializeField]private bool gamePaused;

    [Header("Player")] 
    [SerializeField] private CharacterController_Platformer player;

    [Header("FadeOut")] 
    [SerializeField] private GameObject panelGameOver;
    [SerializeField] private ChangeScene fadeout;
    
    public bool GameStart
    {
        get { return gameStart; }
        set { gameStart = value; }
    }

    public bool GameFinished
    {
        get { return gameFinished; }
        set { gameFinished = value; }
    }

    public bool GamePaused
    {
        get { return gamePaused; }
        set { GamePaused = value; }
    }
    
    void Start()
    {
        GameStart = true;
        gameFinished = false;
        panelGameOver.SetActive(false);
    }

    
    void Update()
    {
        
    }

    public void MakeGameFinished()
    {
        fadeout.EffectFadeOut();
        StartCoroutine(GameFinishedCorutine());

    }

    IEnumerator GameFinishedCorutine()
    {
        player.CanMove = false;
        player.PauseAnimation();
        yield return new WaitForSeconds(1);
        GameFinished = true;
        
        FinishedEvent.Invoke();
        print(GameFinished);
        
    }
    
    
}
