using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using Random = UnityEngine.Random;

public class TextSpawner : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject textPrefab;
    public float indent, lineHeight, lineWidth, spacing, characterSpacing;
    
    public Transform container;
    public Vector3 cursorPos;

    public string text;

    private Vector3 startPos;
    
    private string[] words;
    private int index;

    public bool done;
    private float timer;

    
    public TextManager manager;
    
    void Start()
    {
        startPos = transform.position;
        words = text.Split(' ');
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        
        if (timer < 0)
        {
            timer = Random.Range(0.1f, 0.25f);
            SpawnWord();
        }
    }

    public void SetText(string t)
    {
        words = t.Split(' ');
    }
    public void SpawnWord()
    {

        TextMesh t = Instantiate(textPrefab, transform).GetComponent<TextMesh>();
        t.text = words[index % words.Length];
        t.text = words[Random.Range(0, words.Length)];

        t.transform.parent = container;
        t.transform.localPosition = new Vector3(cursorPos.x, t.transform.localPosition.y, 0);
        t.transform.localRotation = Quaternion.identity;
        
        MeshRenderer r = t.GetComponent<MeshRenderer>();
        cursorPos.x += (t.text.Length * characterSpacing) + spacing;
        
        if (cursorPos.x > lineWidth)
        {
            container.position -= lineHeight * Vector3.up;
            cursorPos.x = 0;
        }
        
        index++;
        if (index >= words.Length)
        {
            index = 0;
            cursorPos.x = 0;
            container.position -= lineHeight * Vector3.up;
            container.position -= lineHeight * Vector3.up;
            manager.Disable(this);
//            container.position = transform.position;
        }

    }
}
