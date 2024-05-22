using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextEffect : MonoBehaviour
{
    [SerializeField] GameObject TextObjectPrefab;
    private void Start()
    {
        GetComponent<Entity>().OnDamageTaken += SpawnTextEffect;
    }
    public void SpawnTextEffect(int amount, ElementEffect elementEffect = ElementEffect.Neutral)
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
        Vector3 position = GetDamageTextPosition();
        SpawnTextEffect(Mathf.Abs(amount).ToString(), color, position);



        if(elementEffect != ElementEffect.Neutral)
        {
            Vector3 newposition = GetAspectTextPosition();

            switch (elementEffect)
            {
                case ElementEffect.Weak:
                    SpawnTextEffect("Weak!!", Color.red, newposition, 2f);
                    break;

                case ElementEffect.Resistant:
                    SpawnTextEffect("Resist!!", Color.blue, newposition, 2f);
                    break;
            }

        }
        

    }

    public Vector3 GetDamageTextPosition()
    {
        return transform.position + Vector3.up;
    }

    public Vector3 GetAspectTextPosition()
    {
        return transform.position + 2 * Vector3.up + 2 * Vector3.right;
    }
    public void SpawnTextEffect(string text, Color color, Vector3 position, float duration = .4f)
    {
        TextObject textObject = Instantiate(TextObjectPrefab, position, Quaternion.identity).GetComponent<TextObject>();
        textObject.Setup(text, color, duration);

    }




}
