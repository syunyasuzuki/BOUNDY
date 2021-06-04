using UnityEngine;
using UnityEngine.UI;

public class ScoreSystem : MonoBehaviour
{
    /// <summary>
    /// 位置を監視する対象
    /// </summary>
    [SerializeField]Transform PlayerTransform = null;

    /// <summary>
    /// スコアを表示するテキスト
    /// </summary>
    [SerializeField] Text ScoreText = null;

    /// <summary>
    /// 最高到達地点
    /// </summary>
    private float highestpoint = 0;

    /// <summary>
    /// 最高到達地点を書き換える
    /// </summary>
    public void SetHighestPoint(float n)
    {
        highestpoint = n;
    }

    /// <summary>
    /// 最高到達地点を取得する
    /// </summary>
    public float GetHighestPoint()
    {
        return highestpoint;
    }

    void Start()
    {
        //プレイヤーが存在しない場合生成する（統合前のエラー回避用）
        if (PlayerTransform == null) 
        {
            new GameObject("Player");
        }

        //プレイヤーのトランスフォームを参照
        PlayerTransform = GameObject.Find("Player").transform;
    }

    void Update()
    {
        //スコア用のテキストUIがない場合処理をはじく
        if (ScoreText == null) return;

        //最高到達地点を超えた場合に最高到達地点を書き換える
        if (highestpoint < PlayerTransform.position.y)
        {
            highestpoint = PlayerTransform.position.y;
            ScoreText.text = highestpoint + "m";
        }
    }
}
