using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;
using TMPro;
using UnityEngine;

public class Number : MonoBehaviour
{
    public Transform spawnPos;
    public int plusType;
    private int amount;
    private int matTypes;

    public void SetPlus(int _amount, int _type)
    {
        amount = _amount;
        plusType = _type;
        matTypes = matType();
        SetNumber();
    }

    #region SpawnPlayer
   
    public void SetNumber()
    {
        List<int> tempList = new List<int>();

        int tempNumber = amount;
        int length = 0;
        while (tempNumber > 0)
        {
            int numberToAdd = tempNumber % 10;
            
            tempList.Add(numberToAdd);
            
            tempNumber /= 10; 
        }

        bool IsSingle = true;
        int reverseCount = tempList.Count + 1;

        if (reverseCount % 2 == 0)
            IsSingle = false;

        int totalThree = reverseCount / 2;

        float zeroLeftCount = (totalThree) * GameManager.Instance.numberBetweenSpace;

        float totalTwo = zeroLeftCount + (Mathf.Abs(GameManager.Instance.numberBetweenSpace) / 2f);

        float increaseRate = Mathf.Abs(GameManager.Instance.numberBetweenSpace);
        
        for (int i = 0; i < tempList.Count+1; i++)
        {
            reverseCount--;
            if (i == 0)
            {
                var temObj = Instantiate(Resources.Load<GameObject>("Pluses/" + plusType));
                temObj.transform.SetParent(transform);
                temObj.GetComponent<Letter>().SetColor(matTypes);
            
                if (!IsSingle)
                {
                    temObj.transform.localPosition = new Vector3(totalTwo, spawnPos.localPosition.y, spawnPos.localPosition.z);
                    totalTwo += increaseRate;
                }
                else
                {
                    temObj.transform.localPosition = new Vector3(zeroLeftCount, spawnPos.localPosition.y, spawnPos.localPosition.z);
                    zeroLeftCount += increaseRate;
                }
            }
            else
            {
                var temObj = Instantiate(Resources.Load<GameObject>("Numbers/" + tempList[reverseCount]));
                temObj.transform.SetParent(transform);
                temObj.GetComponent<Letter>().SetColor(matTypes);
            
                if (!IsSingle)
                {
                    temObj.transform.localPosition = new Vector3(totalTwo, spawnPos.localPosition.y, spawnPos.localPosition.z);
                    totalTwo += increaseRate;
                }
                else
                {
                    temObj.transform.localPosition = new Vector3(zeroLeftCount, spawnPos.localPosition.y, spawnPos.localPosition.z);
                    zeroLeftCount += increaseRate;
                }
            }
        }
    }
    #endregion

    #region Calculators

    private int matType()
    {
        int plusTypes = PlusTyped();
        if (plusTypes == 0 || plusTypes == 2)
            return 1;
        return 2;
    }

    public int PlusTyped()
    {
        return plusType;
    }

    public int TotalAmount()
    {
        return amount;
    }
    #endregion

}
