using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using UnityEditor;
using UnityEngine;

public class Node : MonoBehaviour
{
    [SerializeField] GameObject node;
    // Start is called before the first frame update
    private float offset;
    private int myLayer;
    private bool drawLine;
    private float myDelay;
    private float timer;
    void Start()
    {
        
        
    }

    public void Setup()
    {
        timer = 100;
        StartCoroutine(Delay());
    }
    
    public void Initialize()
    {
        if (myLayer < Network.layers && Random.Range(0, 100) > myLayer * 10f)
        {
            for (int i = 0; i < Network.nodesPerLayer; i++)
            {
                float x = Mathf.Lerp(-Network.xDist, Network.xDist, i / (float)(Network.nodesPerLayer-1)) * ((Network.layers - myLayer) * 2);
               x += Random.Range(-Network.xDist, Network.xDist) *2;
                float y = Network.yDist;
                GameObject newNode = Instantiate(Network.instance.nodePrefab, new Vector3(x, -Random.Range(y, y * 6),0) + transform.position,
                    Quaternion.identity);

                newNode.transform.parent = transform;
                newNode.GetComponent<Node>().myLayer = myLayer + 1;
                newNode.GetComponent<Node>().Setup();
            }
        }
    }

    public IEnumerator Delay()
    {
        yield return new WaitForSeconds(0.02f);
        Initialize();
        timer = Time.time;
    }
    public void DrawLines()
    {
        GetComponent<LineRenderer>().startColor = Random.ColorHSV();
        GetComponent<LineRenderer>().endColor = Random.ColorHSV();
        offset = Random.Range(0, Mathf.PI * 2);
        DrawLine d = GetComponent<DrawLine>();
        foreach (Node n in GetComponentsInParent<Node>())
        {
            d.targets.Add(n.transform);
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (!drawLine && Time.time > timer + myDelay)
        {
            DrawLines();
            drawLine = true;
        }
    }
}
