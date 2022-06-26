using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using MoreMountains.Feedbacks;
using MoreMountains.Feel;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;

    public Transform spawnPos;
    public int firstNumber = 12;
    public List<GameObject> addedNumbers;
    private bool _isPortalable = true;
    public bool isCompleted;
    public int total;
    private Animator _animator;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        SetNumber(firstNumber, 0, false);
    }

    #region Collision

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Number"))
        {
            var number = other.GetComponent<Number>();
            SetNumber(number.TotalAmount(),number.PlusTyped(), true);
            
            Destroy(other.gameObject);
        }
        if (other.CompareTag("Portal"))
        {
            if(!_isPortalable) return;
            _isPortalable = true;
            var portal = other.GetComponent<Portal>();
            SetNumber(portal.TotalAmount(),portal.PlusTyped(), true);
            Destroy(other.transform.parent.gameObject);
            _isPortalable = true;
        }
    }

    #endregion

    #region SpawnPlayer
   
    public void SetNumber(int number, int pluses, bool isAdd)
    {
        for (int i = 0; i < addedNumbers.Count; i++)
        {
            Destroy(addedNumbers[i]);
        }


        addedNumbers.Clear();
        addedNumbers = new List<GameObject>();
        List<int> tempList = new List<int>();

        if (!isAdd)
            total = number;
        else
        {
            if (pluses == 0)
            {
                if(!isCompleted)
                    ScaleFeedBack();
                total += number;
            }
            else if (pluses == 1)
            {
                if(!isCompleted)
                    SpawnLostNumber(total);
                total -= number;
            }
            else if (pluses == 2)
            {
                if(!isCompleted)
                    ScaleFeedBack();
                total *= number;
            }
            else if (pluses == 3)
            {
                if(!isCompleted)
                    SpawnLostNumber(total);
                total /= number;
            }
            number = total;
        }
        isCompleted = total == LevelManager.Instance.levelGoal;
        if (isAdd)
        {
            if (pluses == 0)
            {
                if(!isCompleted)
                    ScaleFeedBack();
            }
            else if (pluses == 1)
            {
                if(!isCompleted)
                    SpawnLostNumber(total);
            }
            else if (pluses == 2)
            {
                if(!isCompleted)
                    ScaleFeedBack();
            }
            else if (pluses == 3)
            {
                if(!isCompleted)
                    SpawnLostNumber(total);
            }
        }
        if (total <= 0)
        {
            //TODO: Die
            return;
        }

        while (number > 0)
        {
            int numberToAdd = number % 10;
            
            tempList.Add(numberToAdd);
            
            number /= 10; 
        }

        bool isSingle = true;
        int reverseCount = tempList.Count;

        if (reverseCount % 2 == 0)
            isSingle = false;

        int totalThree = reverseCount / 2;

        float zeroLeftCount = (totalThree) * GameManager.Instance.numberBetweenSpace;

        float totalTwo = zeroLeftCount + (Mathf.Abs(GameManager.Instance.numberBetweenSpace) / 2f);

        float increaseRate = Mathf.Abs(GameManager.Instance.numberBetweenSpace);
        
        for (int i = 0; i < tempList.Count; i++)
        {
            reverseCount--;
            var temObj = Instantiate(Resources.Load<GameObject>("Numbers/" + tempList[reverseCount]));
            addedNumbers.Add(temObj);
            temObj.transform.SetParent(transform);
            temObj.GetComponent<Letter>().SetColor(0);
            temObj.GetComponent<Letter>().HitColor(pluses);
            
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
        
        if (isCompleted)
            LevelManager.Instance.GameCompleted(true);
    }
    #endregion

    #region SpawnLostNumber

    public void SpawnLostNumber(int number)
    {
        GameManager.Instance.BadSound();
        List<int> tempList = new List<int>();

        var deadNumber = Instantiate(Resources.Load<GameObject>("Spawnable/DeadNumber"));
        deadNumber.transform.position = transform.position;

        while (number > 0)
        {
            int numberToAdd = number % 10;
            
            tempList.Add(numberToAdd);
            
            number /= 10; 
        }

        bool isSingle = true;
        int reverseCount = tempList.Count;

        if (reverseCount % 2 == 0)
            isSingle = false;

        int totalThree = reverseCount / 2;

        float zeroLeftCount = (totalThree) * GameManager.Instance.numberBetweenSpace;

        float totalTwo = zeroLeftCount + (Mathf.Abs(GameManager.Instance.numberBetweenSpace) / 2f);

        float increaseRate = Mathf.Abs(GameManager.Instance.numberBetweenSpace);

        var spawnPosNumber = deadNumber.GetComponent<Number>().spawnPos;
        
        for (int i = 0; i < tempList.Count; i++)
        {
            reverseCount--;
            var temObj = Instantiate(Resources.Load<GameObject>("Numbers/" + tempList[reverseCount]));
            temObj.transform.SetParent(deadNumber.transform);
            temObj.transform.localRotation = Quaternion.Euler(25f, 180, 0);
            temObj.AddComponent<Rigidbody>();
            temObj.AddComponent<BoxCollider>();
            temObj.GetComponent<Letter>().SetColor(2);
            temObj.layer = 6;
            
            if (!isSingle)
            {
                var localPosition = spawnPosNumber.localPosition;
                temObj.transform.localPosition = new Vector3(totalTwo, localPosition.y, localPosition.z);
                totalTwo += increaseRate;
            }
            else
            {
                var localPosition = spawnPosNumber.localPosition;
                temObj.transform.localPosition = new Vector3(zeroLeftCount, localPosition.y, localPosition.z);
                zeroLeftCount += increaseRate;
            }
        }
    }

    #endregion

    #region FeedBack

    void ScaleFeedBack()
    {
        GameManager.Instance.SplashSound();
        GameManager.Instance.ScalePlayer(false);
        transform.localRotation = Quaternion.Euler(0,0,0);
        GameManager.Instance.ScalePlayer(true);
    }

    #endregion


}
