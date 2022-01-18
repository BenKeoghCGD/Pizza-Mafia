using UnityEngine;

public class UIFloat : MonoBehaviour
{
    Vector3 defaultPos;
    public float speed, amplitude;
    private float index;

    void Start()
    {
        defaultPos = transform.localPosition;
    }

    void Update()
    {
        index += Time.deltaTime;
        transform.localPosition = defaultPos + new Vector3(0, amplitude * Mathf.Sin(index * speed));
    }
}