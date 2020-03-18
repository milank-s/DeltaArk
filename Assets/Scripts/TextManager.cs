using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TextManager : MonoBehaviour
{

    public List<TextSpawner> spawners;
    // Start is called before the first frame update

    private int index;
    public string[] texts;
    void Start()
    {
        foreach (TextSpawner s in spawners)
        {
            s.manager = this;
            s.enabled = false;
        }
        spawners[Random.Range(0, spawners.Count)].enabled = true;
        
    }

    public void Disable(TextSpawner s)
    {
        s.enabled = false;
        s.complete = false;
        //s.container = new GameObject().transform;
        //s.container.transform.position = s.transform.position;
        //s.transform.localPosition = Random.Range(-3f, 6f) * Vector3.up;
        
        StartCoroutine(ChangeActive());
    } 
    
    
    IEnumerator ChangeActive()
    {
        yield return new WaitForSeconds(2);

        TextSpawner s = spawners[Random.Range(0, spawners.Count)];
        s.enabled = true;
        s.SetText(texts[index % texts.Length]);
        index++;

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
