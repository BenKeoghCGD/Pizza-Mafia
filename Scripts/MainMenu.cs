using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject[] UIElements;
    private TransitionManager _transitionManager;
    void Start()
    {
        _transitionManager = GameObject.FindObjectOfType<TransitionManager>();
    }

    public void HowToPlay()
    {
        StartCoroutine(_transitionManager.transition(4));
    }

    public void Leaderboard()
    {
        StartCoroutine(_transitionManager.transition(3));
    }

    public void Play()
    {
        StartCoroutine(_transitionManager.transition(2));
    }

    public void ReturnToMM()
    {
        StartCoroutine(_transitionManager.transition(1));
    }
}
