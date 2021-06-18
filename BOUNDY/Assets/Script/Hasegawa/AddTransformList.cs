using UnityEngine;

/// <summary>
/// これをつけたオブジェクトは自身をRectangleCollisionDetectionに追加する
/// </summary>
public class AddTransformList : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject.Find("GameMaster").GetComponent<RectangleCollisionDetection>().SetTransformList(transform);
    }
}
