using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;
using TMPro;
using UnityEngine;

public class Number : MonoBehaviour
{
    public Transform spawnPos;
    public int plusType;
    private int _amount;
    private int _matTypes;

    public void SetPlus(int amount, int type)
    {
        _amount = amount;
        plusType = type;
        _matTypes = MatType();
        SetNumber();
    }

    #region SpawnPlayer
   
    public void SetNumber()
    {
        List<int> tempList = new List<int>();

        int tempNumber = _amount;
        while (tempNumber > 0)
        {
            int numberToAdd = tempNumber % 10;
            
            tempList.Add(numberToAdd);
            
            tempNumber /= 10; 
        }

        bool isSingle = true;
        int reverseCount = tempList.Count + 1;

        if (reverseCount % 2 == 0)
            isSingle = false;

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
                temObj.GetComponent<Letter>().SetColor(_matTypes);
            
                if (!isSingle)
                {
                    var localPosition = spawnPos.localPosition;
                    temObj.transform.localPosition = new Vector3(totalTwo, localPosition.y, localPosition.z);
                    totalTwo += increaseRate;
                }
                else
                {
                    var localPosition = spawnPos.localPosition;
                    temObj.transform.localPosition = new Vector3(zeroLeftCount, localPosition.y, localPosition.z);
                    zeroLeftCount += increaseRate;
                }
            }
            else
            {
                var temObj = Instantiate(Resources.Load<GameObject>("Numbers/" + tempList[reverseCount]));
                temObj.transform.SetParent(transform);
                temObj.GetComponent<Letter>().SetColor(_matTypes);
            
                if (!isSingle)
                {
                    var localPosition = spawnPos.localPosition;
                    temObj.transform.localPosition = new Vector3(totalTwo, localPosition.y, localPosition.z);
                    totalTwo += increaseRate;
                }
                else
                {
                    var localPosition = spawnPos.localPosition;
                    temObj.transform.localPosition = new Vector3(zeroLeftCount, localPosition.y, localPosition.z);
                    zeroLeftCount += increaseRate;
                }
            }
        }
    }
    #endregion

    #region Calculators

    private int MatType()
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
        return _amount;
    }
    #endregion

}
