using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Collections;

public class FileImporter : MonoBehaviour
{
    public GameObject displayPanel; // The panel to display the slides
    public Button importButton, beginButton, leftButton, rightButton;
    public RawImage slideDisplay; // RawImage to show the slide
    public Text activeSlideText; // Text UI to show the slide number

    private List<Texture2D> slides = new List<Texture2D>(); // Stores slide images
    private int currentSlideIndex = 0;

    void Start()
    {
        // Set up button listeners
        //importButton.onClick.AddListener(ImportFile);
        //beginButton.onClick.AddListener(BeginPresentation);
        //leftButton.onClick.AddListener(PreviousSlide);
        //rightButton.onClick.AddListener(NextSlide);

        // Hide the display panel initially
        //displayPanel.SetActive(false);    
    }

    //public void ImportFile()
    //{
    //    // Opens file picker on mobile device using NativeFilePicker
    //    NativeFilePicker.Permission permission = NativeFilePicker.PickFile((filePath) =>
    //    {
    //        if (filePath != null)
    //        {
    //            StartCoroutine(LoadSlidesFromPDF(filePath)); // Load PDF slides
    //        }
    //        else
    //        {
    //            Debug.Log("File selection was canceled or failed.");
    //        }
    //    });

    //    if (permission == NativeFilePicker.Permission.Denied)
    //        Debug.Log("File permission denied");
    //}

    //private IEnumerator LoadSlidesFromPDF(string filePath)
    //{
    //    // Assuming you have a PDF Renderer or similar library to handle PDF files
    //    slides.Clear(); // Clear any previous slides

    //    PDFRenderer pdfRenderer = new PDFRenderer(); // This is pseudo-code; refer to PDF library's docs
    //    pdfRenderer.Load(filePath);

    //    int pageCount = pdfRenderer.GetPageCount();

    //    for (int i = 0; i < pageCount; i++)
    //    {
    //        Texture2D pageTexture = pdfRenderer.RenderPageToTexture(i);
    //        slides.Add(pageTexture);
    //        yield return null; // Wait for the next frame to avoid freezing
    //    }

    //    if (slides.Count > 0)
    //    {
    //        currentSlideIndex = 0;
    //        DisplaySlide(currentSlideIndex);
    //        Debug.Log("Slides loaded successfully.");
    //    }
    //}

    public void BeginPresentation()
    {
        if (slides.Count > 0)
        {
            displayPanel.SetActive(true); // Show the display panel
            DisplaySlide(currentSlideIndex); // Show the first slide
        }
        else
        {
            Debug.Log("No slides available to begin presentation.");
        }
    }

    private void DisplaySlide(int index)
    {
        slideDisplay.texture = slides[index];
        activeSlideText.text = $"Slide {index + 1} / {slides.Count}";
    }

    private void PreviousSlide()
    {
        if (currentSlideIndex > 0)
        {
            currentSlideIndex--;
            DisplaySlide(currentSlideIndex);
        }
    }

    private void NextSlide()
    {
        if (currentSlideIndex < slides.Count - 1)
        {
            currentSlideIndex++;
            DisplaySlide(currentSlideIndex);
        }
    }
}
