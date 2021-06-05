using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class File_test : MonoBehaviour
{
    //メモ
    //801SH
    //1440 * 2992 (1:2.077777)
    //
    //One S1
    //1080 * 1920 (1:1.777777)


    [SerializeField] Text text = null;

    void SmartPhone_desktop()
    {
        string path = Path.Combine(Application.persistentDataPath, "testdata");
        if (!File.Exists(path))
        {
            Debug.Log("create");
            StreamWriter sw = new StreamWriter(path);
            sw.WriteLine("あいうえおかきくけこさしすせそ");
            sw.Dispose();
        }
        else
        {
            Debug.Log("delete");
            File.Delete(path);
        }
    }

    void Desktop()
    {
        string path = Path.Combine(Application.dataPath, "testdata");
        text.text = path;
    }


    private void Start()
    {
        Desktop();
    }
}
