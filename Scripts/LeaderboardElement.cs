using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardElement : MonoBehaviour
{
    public Text number, user, score;
    int num, sc;
    string us;

    // Update is called once per frame
    void Update()
    {
        number.text = "#" + num;
        user.text = us;
        score.text = sc.ToString();

        if (num == 1) number.color = Color.green;
        if (num == 2) number.color = Color.yellow;
        if (num == 3) number.color = Color.red;
    }

    public void super(int number, string user, int score)
    {
        num = number;
        us = user;
        sc = score;
    }
}
