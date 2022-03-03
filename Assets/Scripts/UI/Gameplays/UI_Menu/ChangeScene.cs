using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class ChangeScene : MonoBehaviour
{
    public bool doFade;
    
    public Image fundido;
    public string[] escenas;
    
   
    void Start()
    {
        fundido.gameObject.SetActive(true);
        fundido.CrossFadeAlpha(0,0.5f,false);
    }
    
    public void FadeOut(int s)
    {
        fundido.CrossFadeAlpha(1,0.5f, false);
        StartCoroutine(CambioEscena(escenas[s]));
    }
    IEnumerator CambioEscena(string escena)
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(escena);
    }

    public void EffectFadeOut()
    {
        
        StartCoroutine(DoEffect());
    }

    IEnumerator DoEffect()
    {
        fundido.gameObject.SetActive(true);
        fundido.CrossFadeAlpha(1,0.5f, false);
        yield return new WaitForSeconds(1f);
        fundido.CrossFadeAlpha(0,0.5f,false);
        doFade = false;
    }
}
