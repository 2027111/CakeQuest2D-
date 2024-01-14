using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
[System.Serializable]
public class ChoiceInfo : LineInfo
{

    public List<string> choices = new List<string>();
    public List<List<LineInfo>> choiceBranches = new List<List<LineInfo>>();
    public List<LineInfo> defaultBranch = new List<LineInfo>();








   

}

