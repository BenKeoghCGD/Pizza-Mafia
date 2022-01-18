using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class Leaderboard : MonoBehaviour
{
    List<KeyValuePair<string, int>> data = new List<KeyValuePair<string, int>>();
    List<GameObject> elements = new List<GameObject>();
    public GameObject LeaderboardCanvas, NoDataCanvas, LeaderboardElement, LeaderboardElementParent;
    private float leaderboard_Y, noData_Y;
    public float floatSpeed = 3.0f;
    private TransitionManager _transitionManager;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(getData());
        _transitionManager = GameObject.FindObjectOfType<TransitionManager>();
    }

    private IEnumerator getData()
    {
        if(elements.Count != 0)
        {
            foreach (GameObject obj in elements) Destroy(obj);
        }
        noData_Y = 670;
        leaderboard_Y = 1200;
        NoDataCanvas.transform.localPosition = new Vector3(0, noData_Y);
        LeaderboardCanvas.transform.localPosition = new Vector3(0, leaderboard_Y);

        data.Clear();
        UnityWebRequest www = UnityWebRequest.Get("http://api.benkeoghcgd.co.uk/PizzaMafia/Leaderboard/leaderboardData.txt");
        yield return www.SendWebRequest();
        if (www.result != UnityWebRequest.Result.Success)
        {
            showNoData();
        }
        else
        {
            string d = www.downloadHandler.text;
            string[] da = d.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

            foreach (string s in da)
            {
                if(s != da[da.Length - 1])
                {
                    string[] dat = s.Split(':');
                    data.Add(new KeyValuePair<string, int>(dat[0], int.Parse(dat[1])));
                }
            }
            data.Sort(Comparison);
            showLeaderboard();
        }
    }

    private void showNoData()
    {
        noData_Y = 0;
    }

    private void showLeaderboard()
    {
        leaderboard_Y = 0;
        int offset = 117, startPos = 298, index = 0;
        foreach (KeyValuePair<string, int> el in data)
        {
            if (index > 4) continue;
            GameObject obj = Instantiate(LeaderboardElement, new Vector3(), new Quaternion(), LeaderboardElementParent.transform);
            obj.transform.localPosition = new Vector3(0, startPos - (index * offset));
            index++;
            obj.GetComponent<LeaderboardElement>().super(index, el.Key, el.Value);

            elements.Add(obj);
        }
    }

    static int Comparison(KeyValuePair<string, int> a, KeyValuePair<string, int> b)
    {
        return b.Value.CompareTo(a.Value);
    }

    // Update is called once per frame
    void Update()
    {
        LeaderboardCanvas.transform.localPosition = Vector3.Lerp(LeaderboardCanvas.transform.localPosition, new Vector3(LeaderboardCanvas.transform.localPosition.x, leaderboard_Y), Time.deltaTime * floatSpeed);
        NoDataCanvas.transform.localPosition = Vector3.Lerp(NoDataCanvas.transform.localPosition, new Vector3(NoDataCanvas.transform.localPosition.x, noData_Y), Time.deltaTime * floatSpeed);
    }

    public void refresh()
    {
        StartCoroutine(getData());
    }

    public void returnToMain()
    {
        StartCoroutine(_transitionManager.transition(1));
    }
}
