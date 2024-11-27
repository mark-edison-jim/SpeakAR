using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImportFile : MonoBehaviour
{
    public Text txt;
    private bool isImported = false;
    public Button begin;
    void Start()
    {
        begin.interactable = false;
        txt.text = "Import File to Begin";
    }

    public void Import()
    {
        if (!isImported)
        {
            txt.text = "spacePPT.pdf";
            isImported = true;
            begin.interactable = true;
        }
        else
        {
            txt.text = "Import File to Begin";
            isImported = false;
            begin.interactable = false;
        }
    }
}
