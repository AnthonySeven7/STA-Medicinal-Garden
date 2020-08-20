using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LockModel : MonoBehaviour
{
    public Canvas canvasUI;
    public bool lockModel;
    public float pos_x = 0.0f;
    public float pos_y = 0.0f;
    public float pos_z = 0.0f;

    void Update()
    {
        if (gameObject.GetComponent<MeshRenderer>().enabled)
        {
            lockModel = (canvasUI.transform.Find("ToggleLock").GetComponent<Toggle>().isOn);
            if (!lockModel)
            {
                pos_x = ((2 * (transform.parent.position.x)) / 3);
                pos_y = ((2 * (transform.parent.position.y)) / 3);
                pos_z = ((2 * (transform.parent.position.z)) / 3);
                transform.position = new Vector3(pos_x, pos_y, pos_z);
                transform.localScale = new Vector3(transform.parent.position.z, transform.parent.position.z, transform.parent.position.z);
            }
        }
    }
}
