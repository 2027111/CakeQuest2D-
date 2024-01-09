
using UnityEngine;

[System.Serializable]
public class LineInfoDTO
{

    public Sprite portrait;
    public string talkername;
    public string line;


    // Other properties...

    public LineInfoDTO(LineInfo lineInfo)
    {
        this.portrait = lineInfo.portrait;
        this.talkername = lineInfo.talkername;
        this.line = lineInfo.line;
    }

    public LineInfoDTO(Sprite portrait, string talkername, string line)
    {
        this.portrait = portrait;
        this.talkername = talkername;
        this.line = line;
    }
}

[System.Serializable]
public class DialogueContentDTO
{
    public LineInfoDTO[] lines;
}

