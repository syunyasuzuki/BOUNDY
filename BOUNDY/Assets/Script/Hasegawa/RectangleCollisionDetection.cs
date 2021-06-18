using System.Collections.Generic;
using UnityEngine;

public class RectangleCollisionDetection : MonoBehaviour
{
    /// <summary>
    /// 当たり判定を取りたいオブジェクトのTranformのリスト
    /// </summary>
    List<Transform> transformlist = new List<Transform>();

    /// <summary>
    /// 座標をとる際の一番下の値
    /// これ以下の場合すべてをあたった判定にする
    /// </summary>
    [SerializeField] float LowestValue = 0f;

    /// <summary>
    /// 当たり判定を取らせたいオブジェクトのTransformをリストに追加
    /// </summary>
    public void SetTransformList(Transform trans)
    {
        transformlist.Add(trans);
    }

    /// <summary>
    /// リストをクリア
    /// </summary>
    public void ClearTransformList()
    {
        transformlist.Clear();
    }

    /// <summary>
    /// 当たり判定をとりたい座標を渡すと、
    /// リストの中であたっている数を返す
    /// </summary>
    public int CollisionDetection(Vector3 pos ,bool All)
    {
        if (pos.y < LowestValue) return 1;

        int count = 0;
        for(int i = 0; i < transformlist.Count; ++i)
        {
            if (Mathf.Abs(transformlist[i].position.x - pos.x) < transformlist[i].localScale.x / 2 + 0.5f && Mathf.Abs(transformlist[i].position.y - pos.y) < transformlist[i].localScale.y / 2 + 0.5f) 
            { 
                ++count;
                if (All) { return 1; }
            }
        }
        if (count > 0) { Debug.Log("Hit"); }
        return count;
    }
}
