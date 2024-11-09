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


    public void SpawnTextEffect(AttackInformation attackInfo)
    {

        Color color = Color.white;
        if (GetComponent<TeamComponent>().teamIndex == TeamIndex.Player)
        {
            color = Color.red;
        }

        if (attackInfo.GetAmount() > 0)
        {
            color = Color.green;
        }


        if (attackInfo.GetAmount() == 0)
        {


            Vector3 position = GetDamageTextPosition();
            SpawnTextEffect(LanguageData.GetDataById(LanguageData.INDICATION).GetValueByKey("blocked"), Color.white, position);


        }
        else
        {
            Vector3 position = GetDamageTextPosition();
            SpawnTextEffect(Mathf.Abs(attackInfo.GetAmount()).ToString(), color, position);


            if (attackInfo.source)
            {

                if (attackInfo.command != null && attackInfo.TriggersChain)
                {

                    if (attackInfo.effect != ElementEffect.Neutral && attackInfo.textEffectShown == false)
                    {
                        Vector3 newposition = GetAspectTextPosition();
                        string message = LanguageData.GetDataById(LanguageData.INDICATION).GetValueByKey("recipeStart");
                        Color textColor = Color.white;
                        switch (attackInfo.effect)
                        {
                            case ElementEffect.RecipeBoosted:

                                message = LanguageData.GetDataById(LanguageData.INDICATION).GetValueByKey("recipeStart");
                                textColor = Color.red;
                                break;

                            case ElementEffect.RecipeFailed:

                                message = LanguageData.GetDataById(LanguageData.INDICATION).GetValueByKey("recipeFailed");
                                textColor = Color.blue;
                                break;

                            case ElementEffect.RecipeSuperFailed:

                                message = LanguageData.GetDataById(LanguageData.INDICATION).GetValueByKey("recipeSuperFailed");
                                textColor = Color.blue;
                                break;
                            case ElementEffect.RecipeCompleted:

                                message = LanguageData.GetDataById(LanguageData.INDICATION).GetValueByKey("recipeSuccess");
                                textColor = Color.red;
                                break;
                        }


                        SpawnTextEffect(message, textColor, newposition, 2f);
                        attackInfo.textEffectShown = true;

                    }
                }
                lastCommand = attackInfo.source.currentCommand;

            }
        }


    }


    public Vector3 GetDamageTextPosition()
    {
        return transform.position + 1.2f * Vector3.up;
    }

    public Vector3 GetAspectTextPosition()
    {


        return transform.position + 2.1f * Vector3.up + 1.1f * Vector3.right * GetComponent<BattleCharacter>().IsFacing();
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

    public void SpawnTextEffect(int text, Color color)
    {
        SpawnTextEffect(text.ToString(), color, GetDamageTextPosition());
    }




}
