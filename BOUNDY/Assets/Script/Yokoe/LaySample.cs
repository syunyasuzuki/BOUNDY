using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaySample : MonoBehaviour
{
    int x = 0;

    SpringCon sp_con;

    // Start is called before the first frame update
    void Start()
    {
        sp_con = GameObject.Find("GameDirector").GetComponent<SpringCon>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        x = 1;
    }

    public int dontCreate()
    {
        return x;
    }
}
