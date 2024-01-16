using UnityEngine;

[CreateAssetMenu]
[System.Serializable]
public class LineInfo : ScriptableObject
{
    public Sprite portrait;
    public string talkername;
    public string line;

    public LineInfo(Sprite portrait, string talkername, string line)
    {
        this.portrait = portrait;
        this.talkername = talkername;
        this.line = line;
    }


    public LineInfo()
    {
        portrait = null;
        talkername = "";
        line = "";
    }
    

}

