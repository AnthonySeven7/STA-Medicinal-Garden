using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchSprite : MonoBehaviour
{
    public bool on = true;
    public Image target;
    public Sprite onSprite;
    public Sprite offSprite;

    public void switchSprite()
    {
        on = !on;
        if (on)
        {
            target.sprite = onSprite;
        }
        else
        {
            target.sprite = offSprite;
        }
    }

    public void cleanUp()
    {
        on = true;
        target.sprite = onSprite;
        this.GetComponent<Toggle>().isOn = true;
    }
}
