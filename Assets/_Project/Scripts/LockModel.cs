using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LockModel : MonoBehaviour
{
    public Canvas canvasUI;
    public Camera ARCamera;
    public bool lockModel;
    public float pos_x = 0.0f;
    public float pos_y = 0.0f;
    public float pos_z = 0.0f;

    void Start()
    {
        ARCamera = Camera.main;
    }

    void Update()
    {
        if (this.gameObject.GetComponent<MeshRenderer>().enabled)
        {
            lockModel = (canvasUI.transform.Find("ToggleLock").GetComponent<Toggle>().isOn);
            if (!lockModel)
            {
                pos_x = ((2 * (this.transform.parent.position.x)) / 3);
                pos_y = ((2 * (this.transform.parent.position.y)) / 3);
                pos_z = ((2 * (this.transform.parent.position.z)) / 3);
                this.transform.position = new Vector3(pos_x, pos_y, pos_z);
                this.transform.localScale = new Vector3(this.transform.parent.position.z, this.transform.parent.position.z, this.transform.parent.position.z);
            }
        }
    }
}
