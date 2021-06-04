using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCon : MonoBehaviour
{
    private Rigidbody2D rb;

    /// <summary>
    /// 速度
    /// </summary>
    private Vector3 speed;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.RightArrow))
        {
            transform.position += new Vector3(0.1f, 0.0f, 0.0f);
        }
        if(Input.GetKey(KeyCode.LeftArrow))
        {
            transform.position += new Vector3(-0.1f, 0.0f, 0.0f);
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.tag==("Wall"))
        {
            transform.localScale = new Vector2(transform.localScale.x * -1.0f, transform.localScale.y);
        }
    }
}
