using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCon : MonoBehaviour
{
    [SerializeField] GameObject Player = null;

    // Update is called once per frame
    void Update()
    {
        if (!Player) return;
        transform.position = new Vector3(Player.transform.position.x, Player.transform.position.y, -10);
    }
}
