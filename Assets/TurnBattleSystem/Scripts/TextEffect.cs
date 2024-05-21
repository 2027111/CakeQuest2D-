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
    public void SpawnTextEffect(int amount, BattleCharacter source)
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
        SpawnTextEffect(Mathf.Abs(amount).ToString(), color);

    }


    public void SpawnTextEffect(string text, Color color)
    {
        TextObject textObject = Instantiate(TextObjectPrefab, transform.position + Vector3.up, Quaternion.identity).GetComponent<TextObject>();
        textObject.Setup(text, color);

    }




}
