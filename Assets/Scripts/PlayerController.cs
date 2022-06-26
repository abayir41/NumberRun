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
    private bool IsPortalable = true;
    public bool IsCompleted;
    public int total;
    private Animator animator;
    void Awake()
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
            if(!IsPortalable) return;
            IsPortalable = true;
            var portal = other.GetComponent<Portal>();
            SetNumber(portal.TotalAmount(),portal.PlusTyped(), true);
            Destroy(other.transform.parent.gameObject);
            IsPortalable = true;
        }
    }

    #endregion

    #region SpawnPlayer
   
    public void SetNumber(int _number, int pluses, bool IsAdd)
    {
        for (int i = 0; i < addedNumbers.Count; i++)
        {
            Destroy(addedNumbers[i]);
        }


        addedNumbers.Clear();
        addedNumbers = new List<GameObject>();
        List<int> tempList = new List<int>();

        if (!IsAdd)
            total = _number;
        else
        {
            if (pluses == 0)
            {
                if(!IsCompleted)
                    ScaleFeedBack();
                total += _number;
            }
            else if (pluses == 1)
            {
                if(!IsCompleted)
                    SpawnLostNumber(total);
                total -= _number;
            }
            else if (pluses == 2)
            {
                if(!IsCompleted)
                    ScaleFeedBack();
                total *= _number;
            }
            else if (pluses == 3)
            {
                if(!IsCompleted)
                    SpawnLostNumber(total);
                total /= _number;
            }
            _number = total;
        }
        IsCompleted = total == LevelManager.Instance.levelGoal;
        if (IsAdd)
        {
            if (pluses == 0)
            {
                if(!IsCompleted)
                    ScaleFeedBack();
            }
            else if (pluses == 1)
            {
                if(!IsCompleted)
                    SpawnLostNumber(total);
            }
            else if (pluses == 2)
            {
                if(!IsCompleted)
                    ScaleFeedBack();
            }
            else if (pluses == 3)
            {
                if(!IsCompleted)
                    SpawnLostNumber(total);
            }
        }
        if (total <= 0)
        {
            //TODO: Die
            return;
        }
        
        int length = 0;
        while (_number > 0)
        {
            int numberToAdd = _number % 10;
            
            tempList.Add(numberToAdd);
            
            _number /= 10; 
        }

        bool IsSingle = true;
        int reverseCount = tempList.Count;

        if (reverseCount % 2 == 0)
            IsSingle = false;

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
        
        if (IsCompleted)
            LevelManager.Instance.GameCompleted(true);
    }
    #endregion

    #region SpawnLostNumber

    public void SpawnLostNumber(int _number)
    {
        GameManager.Instance.BadSound();
        List<int> tempList = new List<int>();

        var DeadNumber = Instantiate(Resources.Load<GameObject>("Spawnable/DeadNumber"));
        DeadNumber.transform.position = transform.position;

        int length = 0;
        while (_number > 0)
        {
            int numberToAdd = _number % 10;
            
            tempList.Add(numberToAdd);
            
            _number /= 10; 
        }

        bool IsSingle = true;
        int reverseCount = tempList.Count;

        if (reverseCount % 2 == 0)
            IsSingle = false;

        int totalThree = reverseCount / 2;

        float zeroLeftCount = (totalThree) * GameManager.Instance.numberBetweenSpace;

        float totalTwo = zeroLeftCount + (Mathf.Abs(GameManager.Instance.numberBetweenSpace) / 2f);

        float increaseRate = Mathf.Abs(GameManager.Instance.numberBetweenSpace);

        var spawnPosNumber = DeadNumber.GetComponent<Number>().spawnPos;
        
        for (int i = 0; i < tempList.Count; i++)
        {
            reverseCount--;
            var temObj = Instantiate(Resources.Load<GameObject>("Numbers/" + tempList[reverseCount]));
            temObj.transform.SetParent(DeadNumber.transform);
            temObj.transform.localRotation = Quaternion.Euler(25f, 180, 0);
            temObj.AddComponent<Rigidbody>();
            temObj.AddComponent<BoxCollider>();
            temObj.GetComponent<Letter>().SetColor(2);
            temObj.layer = 6;
            
            if (!IsSingle)
            {
                temObj.transform.localPosition = new Vector3(totalTwo, spawnPosNumber.localPosition.y, spawnPosNumber.localPosition.z);
                totalTwo += increaseRate;
            }
            else
            {
                temObj.transform.localPosition = new Vector3(zeroLeftCount, spawnPosNumber.localPosition.y, spawnPosNumber.localPosition.z);
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
