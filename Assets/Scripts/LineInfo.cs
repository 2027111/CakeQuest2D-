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

    public LineInfo(LineInfoDTO dto)
    {
        this.portrait = dto.portrait;
        this.talkername = dto.talkername;
        this.line = dto.line;
    }
    public LineInfo()
    {
        portrait = null;
        talkername = "";
        line = "";
    }
    public void SetData(LineInfoDTO dto)
    {

        this.talkername = dto.talkername;
        this.line = dto.line;
    }

}

