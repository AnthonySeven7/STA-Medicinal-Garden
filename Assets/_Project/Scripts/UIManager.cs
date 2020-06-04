using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    public GameObject  gen, molA, molB, molC, more, bkg1, bkg2;
    private RectTransform gen_Rect, molA_Rect, molB_Rect, molC_Rect, more_Rect, bkg1_Rect, bkg2_Rect, sel_Rect, sel_bkg_Rect;
    public string activePanel;
    private float panel_up, panel_down;

    // Start is called before the first frame update
    void Start()
    {
        panel_up = 0.0f;
        panel_down = -730;
        activePanel = "";
        gen_Rect = gen.GetComponent<RectTransform>();
        molA_Rect = molA.GetComponent<RectTransform>();
        molB_Rect = molB.GetComponent<RectTransform>();
        molC_Rect = molC.GetComponent<RectTransform>();
        more_Rect = more.GetComponent<RectTransform>();
        bkg1_Rect = bkg1.GetComponent<RectTransform>();
        bkg2_Rect = bkg2.GetComponent<RectTransform>();
        sel_bkg_Rect = bkg1_Rect;
    }

    public void GeneralInfoButton()
    {
        CheckPanel(gen_Rect, "gen_info");
    }

    public void MoleAButton()
    {
        CheckPanel(molA_Rect, "moleA");
    }

    public void MoleBButton()
    {
        CheckPanel(molB_Rect, "moleB");
    }

    public void MoleCButton()
    {
        CheckPanel(molC_Rect, "moleC");
    }

    public void MoreButton()
    {
        CheckPanel(more_Rect, "more");
    }
    public void Options()
    {
        CheckPanel(null,"opt");
    }
    public void cleanUp()
    {
        var delay = 0.25f;
        if (activePanel != "") // if there's a panel open
        {
            if (sel_Rect != null) sel_Rect.DOAnchorPos(new Vector2(0, panel_down), 0.25f).SetDelay(delay);
            if (activePanel != "opt")
            {
                sel_bkg_Rect.DOAnchorPos(new Vector2(0, panel_down), 0.25f).SetDelay(delay);
                delay = 0.5f;
                if (sel_bkg_Rect == bkg1_Rect) sel_bkg_Rect = bkg2_Rect;
                else sel_bkg_Rect = bkg1_Rect;
            }
        }
        sel_Rect = null;
        activePanel = "";
    }

    public void CheckPanel(RectTransform pointer, string name)
    {
        var delay = 0.25f;
        if (activePanel != "") // if there's a panel open
        {
            if (sel_Rect != null) sel_Rect.DOAnchorPos(new Vector2(0, panel_down), 0.25f).SetDelay(delay);
            if (activePanel != "opt")
            {
                sel_bkg_Rect.DOAnchorPos(new Vector2(0, panel_down), 0.25f).SetDelay(delay);
                delay = 0.5f;
                if (sel_bkg_Rect == bkg1_Rect) sel_bkg_Rect = bkg2_Rect;
                else sel_bkg_Rect = bkg1_Rect;
            }
        }

        if (activePanel != name) // if active panel is not what we just clicked
        {
            sel_Rect = pointer;
            if (name != "opt")
            {
                sel_Rect.DOAnchorPos(new Vector2(0, panel_up), 0.25f).SetDelay(delay);
                sel_bkg_Rect.DOAnchorPos(new Vector2(0, panel_up), 0.25f).SetDelay(delay);
            }
            activePanel = name;
        }

        else // if we closed the open panel
        {
            sel_Rect = null;
            activePanel = "";
        }
    }
}
