using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Network : MonoBehaviour
{
    [SerializeField] Node node;
    public GameObject nodePrefab;
    [SerializeField] public static int layers = 5;
    [SerializeField] public static int nodesPerLayer = 3;    
    public static int layerCount;

    public static Network instance;
    [SerializeField] public static float xDist = 0.25f;
    [SerializeField] public static float yDist = 0.4f;
    // Start is called before the first frame update
    void Awake()
    {
        
        instance = this;
        node.Initialize();
    }

    void Initialize()
    {
        
    }
    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
