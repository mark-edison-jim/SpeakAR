using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainEventManager : MonoBehaviour {

    public delegate void MainEventSignal(bool forward);
    public static MainEventSignal OnMainEventSignalRequest;


    [SerializeField]
    public GameObject homePage;
    [SerializeField]
    private GameObject parentSlides;

    //Do not initialize this List unity will do that for us
    [SerializeField]
    public List<GameObject> Slides;

    private GameObject currentSlide;

    [SerializeField]
    private int SlideIndex = 0;
    private int SlideMax = 0;
    private int SlideMIn = 0;

    [SerializeField]
    public GameObject MyCanvas;

    public Text SlideName;

    void Start () {

        if (homePage != null)
        {
            NewSlide(Slides[SlideIndex], Vector2.zero, Vector2.zero);
        }
        else
        {
            Debug.LogError("Referance to homepage missing!, MainEventManager.cs:16");
        }
	}
    
    public void ResetSlides()
    {
        SlideIndex=0;
        Start();
    }
   
    void OnEnable()
    {
        SubscribeEvents();
        SlideMax = Slides.Capacity - 1;
    }	
    void OnDisable()
    {
        UnsubscribeEvents();
    }

    public void SignalTest()
    {
        Debug.Log("Signal recived");
    }

    private void SubscribeEvents()
    {
        MainEventMessageSender.OnStartSignal += SignalTest;
        slideEventCallBack.OnSlideFinish += NextSlide;
        slideEventCallBack.OnSlideReverse += PrevSlide;

    }
    private void UnsubscribeEvents()
    {
        MainEventMessageSender.OnStartSignal -= SignalTest;
        slideEventCallBack.OnSlideFinish -= NextSlide;
        slideEventCallBack.OnSlideReverse -= PrevSlide;
    }

    //Old function now replaced by more generic NewSlide function
    private void OpenHomepage()
    {
        GameObject go = GameObject.Instantiate(homePage);
        go.transform.SetParent(MyCanvas.transform);
        go.transform.SetParent(parentSlides.transform);
        go.GetComponent<RectTransform>().offsetMax = Vector2.zero;
        go.GetComponent<RectTransform>().offsetMin = Vector2.zero;
        go.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        try
        {
            SlideName.text = go.GetComponent<slideEventCallBack>().SlideName;
            Debug.Log("setting slide name to " + go.GetComponent<slideEventCallBack>().SlideName);
        }
        catch
        {
            SlideName.text = "N/A";
        }
        currentSlide = go;
        
    }

    private void NewSlide(GameObject slide, Vector2 offsetMax, Vector2 offsetMin)
    {
        // Debug logging for current slide
        if (currentSlide != null)
        {
            Debug.Log($"Destroying current slide: {currentSlide.name}");
            Destroy(currentSlide);
        }
        else
        {
            Debug.Log("No current slide to destroy.");
        }

        // Instantiate the new slide
        GameObject go = Instantiate(slide);
        go.transform.SetParent(MyCanvas.transform);
        go.GetComponent<RectTransform>().offsetMax = offsetMax;
        go.GetComponent<RectTransform>().offsetMin = offsetMin;
        go.GetComponent<RectTransform>().localScale = Vector3.one;

        // Attempt to set the slide name
        try
        {
            SlideName.text = go.GetComponent<slideEventCallBack>().SlideName;
            Debug.Log($"Slide name set to: {go.GetComponent<slideEventCallBack>().SlideName}");
        }
        catch
        {
            SlideName.text = "N/A";
            Debug.LogWarning("Slide name could not be set. Component might be missing.");
        }

        // Update current slide reference
        currentSlide = go;
        Debug.Log("CurrentSlide: " + currentSlide);
    }


    //input handlign


    public void InputForward()
    {

        if (OnMainEventSignalRequest != null)
            OnMainEventSignalRequest(true);
        else
            Debug.Log("No Response");
    }
    public void InputBackward()
    {
        Debug.Log("Input backward");
        if (OnMainEventSignalRequest != null)
            OnMainEventSignalRequest(false);
        else
            Debug.Log("No Response");
    }

    public void NextSlide()
    {
        Debug.Log("NextSlide");
        if(SlideIndex < SlideMax)
        {
            SlideIndex++;
            NewSlide(Slides[SlideIndex], Vector2.zero, Vector2.zero);
        }
        else
        {
            Debug.Log("End of List");
        }
    }
    public void PrevSlide()
    {
        Debug.Log("PrevSlide");
        if (SlideIndex > SlideMIn)
        {
            SlideIndex--;
            NewSlide(Slides[SlideIndex], Vector2.zero, Vector2.zero);
        }
        else
        {
            Debug.Log("Start of List");
        }
    }
    public void DestroyAllInParentSlides()
    {
        foreach (Transform child in parentSlides.transform)
        {
            Destroy(child.gameObject);
        }
    }
}
