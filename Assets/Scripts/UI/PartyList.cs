using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyList : MonoBehaviour
{
    [SerializeField] GameObject MemberCardPrefab;
    [SerializeField] Transform container;
    [SerializeField] Party playerParty;
    public bool Affiched = false;


    List<MemberCard> cards = new List<MemberCard>();

    private void Start()
    {
        playerParty.OnAddToParty.AddListener(ResetList);
        playerParty.OnAddToParty?.Invoke();
    }
    public void CreateIndicator(CharacterObject qobject)
    {
        GameObject t = Instantiate(MemberCardPrefab, container);
        MemberCard qi = t.GetComponent<MemberCard>();
        qi.SetCharObject(qobject);
        cards.Add(qi);
    }

    public void ResetList()
    {
        foreach (Transform t in container)
        {
            Destroy(t.gameObject);
        }
        cards.Clear();
        foreach (CharacterObject q in playerParty.PartyMembers)
        {
            CreateIndicator(q);
        }
        UpdateList();
        Appear(true);
    }


    public void UpdateList()
    {
        foreach (MemberCard card in cards)
        {
            card.UpdateHealthBar();
        }
    }

    public void Appear(bool on)
    {

        if (container.childCount > 0)
        {
            if (Affiched != on)
            {
                GetComponent<Animator>().SetTrigger(on ? "Appear" : "Disappear");
                Affiched = on;
            }
        }
        else
        {
            if (Affiched)
            {
                GetComponent<Animator>().SetTrigger("Disappear");
            }
        }
    }
}
