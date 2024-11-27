using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.Interaction.Toolkit.AR;
using static UnityEngine.XR.ARSubsystems.XRCpuImage;

public class toggleAllForPlay : MonoBehaviour
{
    public GameObject gameUI;
    public ARRaycastManager acm;
    //public ARPlacementInteractable arpi;
    public GameObject playUI;
    public Button expandPanelBtn;
    public Button expandPptBtn;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToggleUI()
    {
        //arpi.enabled = false;
        acm.enabled = false;
        gameUI.SetActive(false);
        playUI.SetActive(true);
        if (!expandPanelBtn.gameObject.activeSelf)
        {
            expandPanelBtn.gameObject.SetActive(true);
        }
    }

    public void UntoggleUI()
    {
        //arpi.enabled = true;
        acm.enabled = true;
        gameUI.SetActive(true);
        playUI.SetActive(false);
        if (!expandPptBtn.gameObject.activeSelf)
        {
            expandPptBtn.gameObject.SetActive(true);
        }
    }

    public void PlayBegin()
    {
        Debug.Log("Play Start");
        SceneManager.LoadScene("Play");
    }

    public void BackToGame()
    {
        Debug.Log("Back to Game");
        SceneManager.LoadScene("Game");
    }
}
