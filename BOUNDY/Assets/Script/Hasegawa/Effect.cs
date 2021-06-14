using UnityEngine;

public class Effect : MonoBehaviour
{
    [SerializeField] bool Roop = false;

    /// <summary>
    /// 処理を継続できるか
    /// アニメーションさせるスプライトが入ってない場合、
    /// スプライトと時間の数が同じでない場合処理をはじく
    /// </summary>
    private bool IsAnimation = false;

    /// <summary>
    /// アニメーションさせるスプライト
    /// </summary>
    [SerializeField] Sprite[] AnimationSprite = null;

    /// <summary>
    /// アニメーションさせる時間
    /// </summary>
    [SerializeField] float[] AnimationSpeed = null;

    /// <summary>
    /// 現在のアニメーションをカウント
    /// </summary>
    private int count = 0;

    /// <summary>
    /// アニメーションを遷移させるための時間
    /// </summary>
    private float time = 0;

    /// <summary>
    /// アニメーションを再生できるか
    /// </summary>
    private bool Isweit = true;

    /// <summary>
    /// アニメーションさせるとこ
    /// </summary>
    private SpriteRenderer spriterenderer = null;

    /// <summary>
    /// アニメーションを再生させる
    /// </summary>
    public void Play()
    {
        if (!IsAnimation || !Isweit) return;

        Isweit = false;
        count = 0;
        time = 0;
        spriterenderer.sprite = AnimationSprite[0];
    }

    /// <summary>
    /// 再生中のアニメーションを止める
    /// 引数をtrueにするとアニメーションを抜く
    /// </summary>
    public void Stop(bool clear)
    {
        if (!IsAnimation) return;
        
        Isweit = true;

        if (clear)
        {
            spriterenderer.sprite = null;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //スプライトと時間の数が違う場合はじく
        if (AnimationSprite.Length != AnimationSpeed.Length) return;

        //スプライトがない場合はじく
        for(int i = 0; i < AnimationSprite.Length; ++i)
        {
            if (AnimationSprite[i] == null) return;
        }

        //スプライトレンダラーをアタッチ参照
        spriterenderer = gameObject.AddComponent<SpriteRenderer>();

        IsAnimation = true;

        if (Roop)
        {
            spriterenderer.sprite = AnimationSprite[0];
            Isweit = false;
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (!IsAnimation || Isweit) return;

        time += Time.deltaTime;

        if (time >= AnimationSpeed[count])
        {
            if (++count < AnimationSprite.Length)
            {
                time = 0;
                spriterenderer.sprite = AnimationSprite[count];
            }
            else
            {
                if (Roop)
                {
                    time = 0;
                    count = 0;
                    spriterenderer.sprite = AnimationSprite[0];
                }
                else
                {
                    Isweit = true;
                    spriterenderer.sprite = null;
                }
            }
        }
    }

    void DebugMode()
    {
        if (Input.GetKeyDown(KeyCode.A)) { Play(); }
    }

}