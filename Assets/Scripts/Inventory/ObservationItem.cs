using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Observation Item", menuName = "Inventory/Items/Observation Items")]
public class ObservationItem : BattleItem
{
    public override void BattleUse(List<BattleCharacter> Target, CharacterInventory inventory = null)
    {
        foreach (BattleCharacter target in Target)
        {
            if (GetHitEffect() != null)
            {
                Instantiate(GetHitEffect(), target.transform.position + Vector3.up, Quaternion.identity);
            }
            if (GetSoundEffect() != null)
            {
                target.PlaySFX(GetSoundEffect());
            }
            target?.RevealRecipe();
        }

        base.BattleUse(Target, inventory);
    }


}
