using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    float _SurvivalTime = 0.0f, _SpawnDelay = 2.0f;
    int _wave = 0, _enemiesStart, _enemiesRemain;
    TransitionManager _transitionManager;

    public bool paused = false, waveInProgress = false;
    public Text ui_Timer, ui_WaveNum, ui_EnemiesRemain, ui_ShowWave, go_WaveNum, go_Seconds;
    public Transform[] spawnPoints;
    public GameObject enemyPrefab, gameOverScreen;
    public InputField go_if;

    private void Start()
    {
        gameOverScreen.SetActive(false);
        _transitionManager = GameObject.FindObjectOfType<TransitionManager>();
        ui_ShowWave.transform.localPosition = new Vector2(0, -130);
        ui_ShowWave.color = new Color(ui_ShowWave.color.r, ui_ShowWave.color.g, ui_ShowWave.color.b, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if(!paused && _transitionManager.isFinished && waveInProgress) _SurvivalTime += Time.deltaTime;
        ui_Timer.text = "Survived " + (int)_SurvivalTime + " second" + ((int)_SurvivalTime == 1 ? "" : "s");

        if (!paused && _transitionManager.isFinished && !waveInProgress) StartCoroutine(beginWave(_wave));
        if (_enemiesRemain == 0) waveInProgress = false;

        ui_WaveNum.text = "Wave " + _wave;
        ui_EnemiesRemain.text = _enemiesRemain + " Enemies Remaining";
    }

    public void endGame()
    {
        gameOverScreen.SetActive(true);
        go_WaveNum.text = "YOU SURVIVED " + _wave + " WAVE" + (_wave == 1 ? "" : "s");
        go_Seconds.text = "Survived " + (int)_SurvivalTime + " second" + ((int)_SurvivalTime == 1 ? "" : "s");
    }

    public void endGame_Retry()
    {
        StartCoroutine(_transitionManager.transition(1));
    }

    public void endGame_Submit()
    {
        if (go_if.text.Length != 0 && !go_if.text.Contains(":")) StartCoroutine(submitData(go_if.text, _SurvivalTime));
    }

    IEnumerator submitData(string name, float surviveTime)
    {
        UnityWebRequest www = UnityWebRequest.Get("api.benkeoghcgd.co.uk/PizzaMafia/Leaderboard/save.php?auth=&user=" + name + "&score=" + (int)surviveTime);
        yield return www.SendWebRequest();
        if (www.result == UnityWebRequest.Result.Success) StartCoroutine(_transitionManager.transition(3));
        else StartCoroutine(_transitionManager.transition(1));
    }

    IEnumerator beginWave(int wave)
    {
        GameObject.FindObjectOfType<Pizzeria>().health = GameObject.FindObjectOfType<Pizzeria>().maxHealth * 1.5f;
        GameObject.FindObjectOfType<Pizzeria>().maxHealth = GameObject.FindObjectOfType<Pizzeria>().health;
        _wave++;
        waveInProgress = true;
        _enemiesStart = _enemiesRemain = 5 * (wave + 1);
        yield return showWave(wave);
        int index = 0;
        yield return null;
        while(index < _enemiesStart)
        {
            index++;
            Instantiate(enemyPrefab, spawnPoints[Random.Range(0, spawnPoints.Length - 1)].position, new Quaternion());
            yield return new WaitForSeconds(_SpawnDelay - (wave * 0.05f));
        }
    }

    IEnumerator showWave(int wave)
    {
        ui_ShowWave.text = "WAVE " + (wave + 1);
        ui_ShowWave.transform.localPosition = new Vector2(0, -130);
        while (ui_ShowWave.color.a < 1) {
            ui_ShowWave.color = new Color(ui_ShowWave.color.r, ui_ShowWave.color.g, ui_ShowWave.color.b, ui_ShowWave.color.a + (0.5f * Time.deltaTime));
            ui_ShowWave.transform.localPosition += new Vector3(0, 0.1f);
            yield return null;
        }
        while (ui_ShowWave.color.a > 0)
        {
            ui_ShowWave.color = new Color(ui_ShowWave.color.r, ui_ShowWave.color.g, ui_ShowWave.color.b, ui_ShowWave.color.a - (0.5f * Time.deltaTime));
            ui_ShowWave.transform.localPosition += new Vector3(0, 0.1f);
            yield return null;
        }
        yield return null;
    }

    public void enemyKilled() { _enemiesRemain--; }
}
