using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drone : MonoBehaviour
{
    // Start is called before the first frame update

    private float xOffset, yOffset;
    public float speed = 0.2f;
    void Start()
    {
        xOffset = Random.Range(-100f, 100f);
        yOffset = Random.Range(-100, 100f);
    }
    // Update is called once per frame
    void Update()
    {
        float time = Time.time * speed;
        float x = Mathf.PerlinNoise(xOffset + time, yOffset + time)- 0.5f;
        float y = Mathf.PerlinNoise(-xOffset + time, -yOffset +time) - 0.4f;
        float z = Mathf.PerlinNoise(xOffset + -time, yOffset + -time) - 0.5f;
        transform.position += new Vector3(x, y/2, z) * Time.deltaTime;
        transform.RotateAround(Vector3.zero, Vector3.up, Time.deltaTime);
    }
}
