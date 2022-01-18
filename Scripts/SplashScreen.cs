using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class SplashScreen : MonoBehaviour
{
    private AsyncOperation sceneOperation;
    public VideoPlayer splash;
    private TransitionManager _transitionManager;

    void Start()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        _transitionManager = GameObject.FindObjectOfType<TransitionManager>();
        StartCoroutine(beginAnim());
    }

    private IEnumerator beginAnim()
    {
        splash.Prepare();
        yield return _transitionManager.fadeOut();
        splash.Play();
        yield return new WaitForSecondsRealtime((float)splash.length * 1.33f);
        yield return _transitionManager.transition(1);
    }
}
