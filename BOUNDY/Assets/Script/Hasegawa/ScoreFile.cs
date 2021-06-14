using UnityEngine;
using System.IO;

public class ScoreFile : MonoBehaviour
{
    //スコアを書き出す
    public void WriteScore(float score)
    {
        string path = Path.Combine(Application.persistentDataPath, "scoredata");
        StreamWriter sw = new StreamWriter(path, false);
        sw.WriteLine(score.ToString());
        sw.Dispose();
    }

    public float ReadScore()
    {
        string path = Path.Combine(Application.persistentDataPath, "scoredata");
        if (File.Exists(path))
        {
            StreamReader sr = new StreamReader(path);
            float score = float.Parse(sr.ReadToEnd());
            sr.Dispose();
            return score;
        }
        return 0;
    }
}
