using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class TextCreature : MonoBehaviour
{
    private Agent a;
    private TextMesh t;
    public bool done;
    // Start is called before the first frame update
    void Start()
    {
        a = GetComponent<Agent>();
        t = GetComponent<TextMesh>();
    }

    // Update is called once per frame
    void Update()
    {
        if ((a.state == Agent.State.dead || transform.position.y < 2) && !done)
        {
            done = true;
            a.state = Agent.State.dead;
            StartCoroutine(Delete());
        }
    }

    IEnumerator Delete()
    {
       while(t.text.Length > 1){
           t.text = t.text.Substring(0, t.text.Length - 1);
            yield return new WaitForSeconds(Random.Range(0.2f, 0.5f));
        }
    }
    
}
