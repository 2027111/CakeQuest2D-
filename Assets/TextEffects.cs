using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextEffects : MonoBehaviour
{
    [SerializeField] GameObject TextObjectPrefab;
    private void Start()
    {
        GetComponent<Entity>().OnDamageTaken += SpawnTextEffect;
    }
    public void SpawnTextEffect(int amount, BattleCharacter source)
    {
        TextObject textObject = Instantiate(TextObjectPrefab, transform.position + Vector3.up, Quaternion.identity).GetComponent<TextObject>();
        textObject.Setup(amount, source, GetComponent<BattleCharacter>());
        
    }




}
