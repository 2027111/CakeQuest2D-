using UnityEngine;

[CreateAssetMenu]
[System.Serializable]
public class LineInfo : ScriptableObject
{
    public string portraitPath;
    public string talkername;
    public string line;

    public LineInfo(string portraitPath, string talkername, string line)
    {
        this.portraitPath = portraitPath;
        this.talkername = talkername;
        this.line = line;
    }


    public LineInfo()
    {
        talkername = "";
        portraitPath = "";
        line = "";
    }
    

}

