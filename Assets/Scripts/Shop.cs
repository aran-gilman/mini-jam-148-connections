using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Shop : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> Structures = new List<GameObject>();
    [SerializeField]
    private List<int> Costs = new List<int>();
    [SerializeField]
    private List<TextMeshProUGUI> PriceTags = new List<TextMeshProUGUI>();

    private int _money = 50;
    [SerializeField]
    private TextMeshProUGUI MoneyText;

    [SerializeField]
    PlayerController PlayerController;

    private int currentSelection = -1;

    private void Awake()
    {
        for(int i = 0; i < PriceTags.Count; i++)
        {
            PriceTags[i].text = Costs[i].ToString();
        }

        MoneyText.text = _money.ToString();
    }

    public void SelectItem(int buttonID)
    {
        if(PlayerController.GetCurrentPlaceable() == Structures[buttonID])
        {
            currentSelection = -1;
            PlayerController.SetCurrentPlaceable(null);
            return;
        }

        if(_money >= Costs[buttonID])
        {
            currentSelection = buttonID;
            PlayerController.SetCurrentPlaceable(Structures[buttonID]);
        }
    }

    public void BuyItem()
    {
        _money -= Costs[currentSelection];
        MoneyText.text = _money.ToString();
        if(_money < Costs[currentSelection])
        {
            currentSelection = -1;
            PlayerController.SetCurrentPlaceable(null);
        }
    }


}
