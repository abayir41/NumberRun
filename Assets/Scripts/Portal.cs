using System.Collections;
using System.Collections.Generic;
using GlobalTypes;
using TMPro;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public TextMeshPro amountText;
    public int plusType;
    private int amount;

    public void SetPortal(int _amount)
    {
        amount = _amount;
        if (GetComponent<ExpEffector>().expType == ExpType.Add)
        {
            plusType = 0;
            amountText.text = "+" + _amount;
        }
        else if (GetComponent<ExpEffector>().expType == ExpType.Remove)
        {
            plusType = 1;
            amountText.text = "-" + _amount;
        }
        
    }

    public int PlusTyped()
    {
        return plusType;
    }

    public int TotalAmount()
    {
        return amount;
    }
}
