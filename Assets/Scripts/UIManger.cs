using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIManger : MonoBehaviour
{
    public RectTransform generalInfo, background;
    private float genInfo_y;
    // Start is called before the first frame update
    void Start()
    {
        genInfo_y = generalInfo.transform.position.y;
        Debug.Log(genInfo_y);
    }

    public void GeneralInfoButton()
    {
        var x = 0.0f;
        if (generalInfo.transform.position.y == 530.0f)
        {
            x = genInfo_y;
        }
        generalInfo.DOAnchorPos(new Vector2(0, x), 0.25f).SetDelay(0.25f);
        background.DOAnchorPos(new Vector2(0, x), 0.25f).SetDelay(0.25f);
    }
}
