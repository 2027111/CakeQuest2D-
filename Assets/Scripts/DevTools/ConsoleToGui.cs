using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ConsoleToGui : MonoBehaviour
{
    string myLog = "*begin log";
    string filename = "";
    public static bool doShow = false;
    [SerializeField] List<KeyCode> CheatCode = new List<KeyCode>();
    int cheatIndex = 0;
    int kChars = 700;
    public static UnityEvent<bool> OnShowChange = new UnityEvent<bool>();
    void OnEnable() { Application.logMessageReceived += Log; }
    void OnDisable() { Application.logMessageReceived -= Log; }
    void Update()
    {
        if (Input.GetKeyDown(CheatCode[cheatIndex]))
        {
            cheatIndex++;
            if (cheatIndex == CheatCode.Count)
            {
                Show(true);
                cheatIndex = 0;
            }
        }else if (Input.GetKeyDown(KeyCode.PageDown))
        {

            if (doShow)
            {
                Show(false);
            }
        }
        else if (Input.anyKeyDown)
        {
            if (cheatIndex != 0)
            {
                cheatIndex = 0;
                Show(false);
            }
        }

    }


    public void Show(bool show)
    {

        doShow = show;
        OnShowChange?.Invoke(doShow);

    }
    public void Log(string logString, string stackTrace, LogType type)
    {
        // for onscreen...
        myLog = myLog + "\n" + logString;
        if (myLog.Length > kChars) { myLog = myLog.Substring(myLog.Length - kChars); }

        // for the file ...
        if (filename == "")
        {
            string d = System.Environment.GetFolderPath(
               System.Environment.SpecialFolder.Desktop) + "/YOUR_LOGS";
            System.IO.Directory.CreateDirectory(d);
            string r = Random.Range(1000, 9999).ToString();
            filename = d + "/log-" + r + ".txt";
        }
        try
        {
            System.IO.File.AppendAllText(filename, logString + "\n");
        }
        catch { }
    }

    void OnGUI()
    {
        if (!doShow) { return; }
        GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity,
           new Vector3(Screen.width / 1200.0f, Screen.height / 800.0f, 1.0f));
        GUI.TextArea(new Rect(10, 10, 540, 370), myLog);
    }
}