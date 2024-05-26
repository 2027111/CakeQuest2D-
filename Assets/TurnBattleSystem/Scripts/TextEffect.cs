using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextEffect : MonoBehaviour
{
    [SerializeField] GameObject TextObjectPrefab;
    Command lastCommand = null;
    private void Start()
    {
        GetComponent<Entity>().OnDamageTaken += SpawnTextEffect;
    }
    public void SpawnTextEffect(int amount, ElementEffect elementEffect = ElementEffect.Neutral, BattleCharacter source = null)
    {

        Color color = Color.white;
        if (GetComponent<TeamComponent>().teamIndex == TeamIndex.Player)
        {
            color = Color.red;
        }

        if (amount > 0)
        {
            color = Color.green;
        }


        if(amount == 0) {


            Vector3 position = GetDamageTextPosition();
            SpawnTextEffect(LanguageData.GetDataById("Indications").GetValueByKey("blocked"), Color.white, position);


        } else
        {
            Vector3 position = GetDamageTextPosition();
            SpawnTextEffect(Mathf.Abs(amount).ToString(), color, position);
      

        if (source)
        {

        if(source.currentCommand != null)
        {

            if (source.currentCommand != lastCommand)
            {

                if (elementEffect != ElementEffect.Neutral)
                {
                    Vector3 newposition = GetAspectTextPosition();

                    switch (elementEffect)
                    {
                        case ElementEffect.Weak:
                                    
                            SpawnTextEffect(LanguageData.GetDataById("Indications").GetValueByKey("weak"), Color.red, newposition, 2f);
                            break;

                        case ElementEffect.Resistant:
                            SpawnTextEffect(LanguageData.GetDataById("Indications").GetValueByKey("resist"), Color.blue, newposition, 2f);
                            break;
                    }

                }
            }
            lastCommand = source.currentCommand;
            }
            }
        }


    }

    public Vector3 GetDamageTextPosition()
    {
        return transform.position + 1.2f * Vector3.up;
    }

    public Vector3 GetAspectTextPosition()
    {
        return transform.position + 1.2f * Vector3.up + 1.1f * Vector3.right * GetComponent<BattleCharacter>().IsFacing();
    }
    public void SpawnTextEffect(string text, Color color, Vector3 position, float duration = 1.2f)
    {
        TextObject textObject = Instantiate(TextObjectPrefab, position, Quaternion.identity).GetComponent<TextObject>();
        textObject.Setup(text, color, duration);

    }

    public void SpawnTextEffect(string text, Color color)
    {
        SpawnTextEffect(text, color, GetAspectTextPosition());
    }




}
