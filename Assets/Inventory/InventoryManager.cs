using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{



    [Header("Inventory Information")]
    [SerializeField] private GameObject blankInventorySlot;
    [SerializeField] public CharacterInventory characterInventory;

    [Header("UI")]
    [SerializeField] private GameObject InventoryContainer;
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private GameObject useButton;
    [SerializeField] private ScrollRect scroll;

    private InventorySlot LastButton;


    [SerializeField] Button LeftButton;
    [SerializeField] Button RightButton;

    

    InventoryItem currentItem;
    private void Start()
    {
        SetTextAndButton("");
        MakeInventorySlots();
    }

    private void MakeInventorySlots()
    {
        if (characterInventory)
        {
            foreach(Transform transform in InventoryContainer.transform)
            {
                Destroy(transform.gameObject);
            }


            foreach(InventoryItem item in characterInventory.ReturnUniqueInventory())
            {



                CreateInventorySlot(item);
            }



        }
    }

    public void SetTextAndButton(string description, bool buttonActive = false)
    {
        descriptionText.text = description;
        useButton.SetActive(buttonActive);
    }


    public void SelectFirstItem()
    {
        if(InventoryContainer.transform.childCount > 0)
        {

        InventoryContainer.transform.GetChild(0).GetComponent<Button>().Select();
        }
    }
    public void SnapTo(RectTransform target)
    {
        Canvas.ForceUpdateCanvases();

        InventoryContainer.GetComponent<RectTransform>().anchoredPosition =
                (Vector2)scroll.transform.InverseTransformPoint(InventoryContainer.transform.position)
                - (Vector2)scroll.transform.InverseTransformPoint(target.position);
    }
    private Vector2 GetWorldPointFromViewport(RectTransform rect, Vector2 viewportPosition)
    {
        Vector3[] corners = new Vector3[4];
        rect.GetWorldCorners(corners);

        Vector3 min = corners[0];
        Vector3 max = corners[2];

        return new Vector2(Mathf.Lerp(min.x, max.x, viewportPosition.x), Mathf.Lerp(min.y, max.y, viewportPosition.y));
    }

    public void CreateInventorySlot(InventoryItem item)
    {


        InventorySlot Slot = Instantiate(blankInventorySlot, InventoryContainer.transform).GetComponent<InventorySlot>();
        Slot.OnSelect.AddListener(delegate{ SnapTo(Slot.GetComponent<RectTransform>()); }) ;
        if(LastButton == null)
        {
            if (LeftButton)
            {
                Navigation customNav = new Navigation
                {
                    mode = Navigation.Mode.Explicit,
                    selectOnRight = Slot.GetComponent<Selectable>()
                };
                LeftButton.navigation = customNav;
            }

            if (RightButton)
            {
                Navigation customNav = new Navigation
                {

                    mode = Navigation.Mode.Explicit,
                    selectOnLeft = Slot.GetComponent<Selectable>()
                };
                RightButton.navigation = customNav;
            }
            


        }
        Navigation nav = new Navigation();
        nav.mode = Navigation.Mode.Explicit;
        if (LastButton)
        {
            nav.selectOnUp = LastButton.GetComponent<Selectable>();
            Navigation n = LastButton.GetComponent<Selectable>().navigation;
            n.selectOnDown = Slot.GetComponent<Selectable>();
            LastButton.GetComponent<Selectable>().navigation = n;
        }
        if (LeftButton)
        {

            nav.selectOnLeft = LeftButton;
        }

        if (RightButton)
        {

            nav.selectOnRight = RightButton;
        }





        Slot.GetComponent<Selectable>().navigation = nav;
        Slot.Setup(item, this, characterInventory.AmountObject(item));
        LastButton = Slot;


    }


    public void SetupDescriptionAndButton(InventoryItem newItem)
    {
        currentItem = newItem;
        descriptionText.text = currentItem.itemDescription;
        useButton.SetActive(currentItem.usable);
        if (useButton.activeSelf)
        {
            useButton.GetComponent<Button>().Select();
        }

    }


    public void UseButtonPress()
    {
        currentItem.OverWorldUse(null);
    }


}
