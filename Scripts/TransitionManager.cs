using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TransitionManager : MonoBehaviour
{
    public Image ui_Overlay, ui_Loading;
    public float fadeSpeed = 0.5f, loadingSpeed = 2.0f;
    public bool isFinished = true;

    // KEEPS [GLOBAL] ACTIVE
    private void Awake()
    {
        DontDestroyOnLoad(transform.parent.gameObject);
        ui_Loading.gameObject.SetActive(false);
        currScene = SceneManager.GetActiveScene().buildIndex;
    }


    private int currScene = -1;
    private void Update()
    {
        ui_Loading.transform.localEulerAngles += new Vector3(0, 0, Time.deltaTime * loadingSpeed);
        if (currScene != SceneManager.GetActiveScene().buildIndex) StartCoroutine(fadeOut());
        currScene = SceneManager.GetActiveScene().buildIndex;
    }

    public IEnumerator fadeOut()
    {
        while(ui_Overlay.color.a > 0)
        {
            ui_Overlay.color = new Color(ui_Overlay.color.r, ui_Overlay.color.g, ui_Overlay.color.b, ui_Overlay.color.a - (fadeSpeed * Time.deltaTime));
            yield return null;
        }
        isFinished = true;
    }

    public IEnumerator fadeIn()
    {
        while (ui_Overlay.color.a < 1)
        {
            ui_Overlay.color = new Color(ui_Overlay.color.r, ui_Overlay.color.g, ui_Overlay.color.b, ui_Overlay.color.a + (fadeSpeed * Time.deltaTime));
            yield return null;
        }
    }

    public IEnumerator transition(int sceneIndex)
    {
        if(isFinished)
        {
            isFinished = false;
            ui_Loading.gameObject.SetActive(true);
            yield return null;
            AsyncOperation _transitionOperation = SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Single);
            _transitionOperation.allowSceneActivation = false;
            yield return fadeIn();
            _transitionOperation.allowSceneActivation = true;
            ui_Loading.gameObject.SetActive(false);
        }
    }
}
