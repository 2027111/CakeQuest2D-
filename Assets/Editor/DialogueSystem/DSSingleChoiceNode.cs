using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class DSSingleChoiceNode : DSNode
{

    public override void Initialize(string nodeName, DialogueSystemGraphView dsGraphView, Vector2 position)
    {
        base.Initialize(nodeName, dsGraphView, position);
        DialogueType = DSDialogueType.SingleChoice;
        DSChoiceSaveData choiceData = new DSChoiceSaveData()
        {
            Text = "Next Dialogue"
        };
        Choices.Add(choiceData);
    }

    public override void Draw()
    {
        base.Draw();


        foreach(DSChoiceSaveData choice in Choices)
        {
            Port choicePort = this.CreatePort(choice.Text);

            choicePort.userData = choice;

            outputContainer.Add(choicePort);


        }


        RefreshExpandedState();
    }


}
