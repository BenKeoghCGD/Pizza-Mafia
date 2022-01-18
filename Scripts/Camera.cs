using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    public GameObject player;
    public float smoothing = 3.0f;

    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, new Vector3(Mathf.Clamp(player.transform.position.x, -5.8f, 5.8f), Mathf.Clamp(player.transform.position.y, 0, 2.8f), -10f), Time.deltaTime * smoothing);
    }
}
