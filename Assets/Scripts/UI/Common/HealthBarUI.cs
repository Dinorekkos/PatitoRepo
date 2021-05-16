using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    public Image HealthBarImage;

    public Sprite FullHealthSprite;
    public Sprite ThreeHealthSprite;
    public Sprite TwoHealthSprite;
    public Sprite OneHealthSprite;
    public Sprite ZeroHealthSprite;

    public HealthEntity healthEntity;

    public void Update()
    {
        switch (healthEntity.Health)
        {
            case 4:
                HealthBarImage.sprite = FullHealthSprite;
                break;
            case 3:
                HealthBarImage.sprite = ThreeHealthSprite;
                break;
            case 2:
                HealthBarImage.sprite = TwoHealthSprite;
                break;
            case 1:
                HealthBarImage.sprite = OneHealthSprite;
                break;
            case 0:
                HealthBarImage.sprite = ZeroHealthSprite;
                break;
        }
    }
}
