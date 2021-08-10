using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerBoard : MonoBehaviour
{
    public bool isPicked;
    public Color origColor;
    public Color pickedColor;

    private void Start()
    {
        origColor = GetComponent<MeshRenderer>().material.color;
    }

    public void Picked()
    {
        GetComponent<MeshRenderer>().material.color = pickedColor;
    }

    public void Unpicked()
    {
        GetComponent<MeshRenderer>().material.color = origColor;
    }
}
