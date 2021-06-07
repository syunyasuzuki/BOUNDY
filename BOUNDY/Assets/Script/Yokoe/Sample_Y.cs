using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sample_Y : MonoBehaviour
{
    Vector3 startmouse_pos = new Vector3(0f, 0f, 0f);
    Vector3 endmouse_pos = new Vector3(0f, 0f, 0f);
    float flick_ct = 0;
    float flick_start_ct = 0;
    float flick_end_ct = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //フリック処理
        if(Input.GetMouseButton(0))
        {
            startmouse_pos = new Vector3(Input.mousePosition.x,Input.mousePosition.y, Mathf.Abs(Camera.main.transform.position.z));
            flick_ct += Time.deltaTime;
        }

        if (Input.GetMouseButtonUp(0))
        {
            endmouse_pos= new Vector3(Input.mousePosition.x, Input.mousePosition.y, Mathf.Abs(Camera.main.transform.position.z));
            flick_end_ct = flick_ct;

            if (flick_end_ct - flick_start_ct < 1.0f)
            {
                Debug.Log("フリック");
            }
        }
    }
}
