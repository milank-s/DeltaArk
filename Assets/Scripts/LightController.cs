using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : MonoBehaviour
{
    // Start is called before the first frame update
    public List<GameObject> children;


    IEnumerator Start()
    {
        
        float f = 0;

        while (children.Count > 0)
        {
            while (f > 0)
            {
                f -= Time.deltaTime;
                yield return null;
            }

            GameObject child = children[Random.Range(0, children.Count)];
            child.SetActive(true);
            children.Remove(child);
            f = Random.Range(2f, 3f);
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
