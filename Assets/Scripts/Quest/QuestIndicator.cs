using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestIndicator : MonoBehaviour
{
    QuestObject questObject;
    [SerializeField] TMP_Text questNameText;
    [SerializeField] TMP_Text questDescText;
    [SerializeField] TMP_Text questProgressText;


    public void SetQuestObject(QuestObject questo)
    {
        this.questObject = questo;

        QuestManager.Singleton?.OnQuestCheck.AddListener(UpdateIndication);
        UpdateIndication();
    }
    private void OnDestroy()
    {

        QuestManager.Singleton?.OnQuestCheck.RemoveListener(UpdateIndication);
    }
    public void UpdateIndication()
    {
        questNameText.SetText(this.questObject.GetName());
        questDescText.SetText(this.questObject.GetDescription());
        questProgressText.SetText(this.questObject.GetObjectiveProgress());
    }

}
