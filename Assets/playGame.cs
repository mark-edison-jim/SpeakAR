using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class playGame : MonoBehaviour
{
    private bool active = false;
    public Animator animator;
    public Button expandBtn;
    // Start is called before the first frame update
    void Start()
    {   

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void popUp()
    {
        active = true;
        animator.SetTrigger("Expand");
        expandBtn.gameObject.SetActive(false);
    }
    public void popDown()
    {
        active = false;
        animator.SetTrigger("Unpand");
        expandBtn.gameObject.SetActive(true);
    }

    public void pptPopUp()
    {
        active = true;
        animator.SetTrigger("pptUp");
        expandBtn.gameObject.SetActive(false);
    }
    public void pptPopDown()
    {
        active = false;
        animator.SetTrigger("pptDown");
        expandBtn.gameObject.SetActive(true);
    }

    public void MainMenu()
    {
        Debug.Log("Main Menu");
        SceneManager.LoadScene("Start");
    }

    public void PlayGame() {
        Debug.Log("Play Game");
        SceneManager.LoadScene("Game");
    }
}
