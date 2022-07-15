using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using MoreMountains.Feel;
using Random = UnityEngine.Random;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    
    
    [Header("Beginner")] 
    public int curGameType;
    public GameObject BeginnerPlayer;
    public GameObject pathCube;
    public GameObject[] portalPos;
    public List<GameObject> expPortals;
    [Header("Level Settings")] 
    public int levelGoal;
    [Header("Spawn")] 
    public GameObject coinSpawnParent;
    public string spawnablePath = "Spawnable/Number";
    [Header("Obs")]
    public GameObject[] resourcesObs;
    [Header("Portal")]
    public Transform PortalSpawnPosesParent;
    [Header("Final")] 
    public GameObject confetti;

    [Header("Envoriment")] 
    [SerializeField] private List<Transform> environmentNumbersParentOfParents;
    [SerializeField] private List<Transform> environmentNumbersParents;
    [SerializeField] private List<Material> materialsForEnvironmentNumber;
    private List<Vector3> cachedScales;

    public int LevelTime => (int) _timeCounter;
    private float _timeCounter;
    private bool _isGamePlaying;
    
    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    #region Beginner

    private void Start()
    {
        if (GameManager.Instance.IsEditorGoal)
        {
            levelGoal = GameManager.Instance.editorLevelGoal;
        }
        SpawnPortals();
        SpawnMobs();
        LevelGoal();

        for (var i = 0; i < environmentNumbersParents.Count; i++)
        {
            var parent = environmentNumbersParents[i];
            SpawnEnvironmentNumber(GameManager.Instance.editorLevelGoal, parent, environmentNumbersParentOfParents[i] ,materialsForEnvironmentNumber[i]);    
        }

        cachedScales = environmentNumbersParentOfParents.ConvertAll(input => input.localScale);
        environmentNumbersParentOfParents.ForEach(trans => trans.localScale = Vector3.zero);

        
        
        
        
    }

    private void SpawnEnvironmentNumber(int number, Transform parent,Transform parentOfParent, Material matOfNumber)
    {
        var tempList = new List<int>();
        
        while (number > 0)
        {
            var numberToAdd = number % 10;
            
            tempList.Add(numberToAdd);
            
            number /= 10; 
        }
        
        var reverseCount = tempList.Count;

        var isSingle = reverseCount % 2 != 0;

        var totalThree = reverseCount / 2;

        var zeroLeftCount = totalThree * GameManager.Instance.numberBetweenSpace;

        var totalTwo = zeroLeftCount + (Mathf.Abs(GameManager.Instance.numberBetweenSpace) / 2f);

        var increaseRate = Mathf.Abs(GameManager.Instance.numberBetweenSpace);
        
        for (int i = 0; i < tempList.Count; i++)
        {
            reverseCount--;
            var temObj = Instantiate(Resources.Load<GameObject>("Numbers/" + tempList[reverseCount]), parent, true);
            temObj.GetComponent<MeshRenderer>().material = matOfNumber;
            if (!isSingle)
            {
                temObj.transform.localPosition = new Vector3(totalTwo, 0, 0);
                totalTwo += increaseRate;
            }
            else
            {
                temObj.transform.localPosition = new Vector3(zeroLeftCount, 0, 0);
                zeroLeftCount += increaseRate;
            }
            
            temObj.transform.localEulerAngles = Vector3.up * 180;
            temObj.transform.localScale = Vector3.one * 100; 
        }

        
        if (tempList.Count == 3)
        {
            parentOfParent.localScale *= 4f / 5f;
        }
        else if (tempList.Count == 2)
        {
            parentOfParent.localScale *= 5f / 6f;
        }
        
    }

    private void Update()
    {
        if (_isGamePlaying)
        {
            _timeCounter += Time.deltaTime;
            ProjectEvents.TimeChanged?.Invoke(LevelTime);
        }
    }

    private void OnEnable()
    {
        ProjectEvents.GameLost += GameLost;
        ProjectEvents.GameWin += GameWin;
        ProjectEvents.UITargetTextStartedTheHideAnimation += UITargetTextStartedTheHideAnimation;
    }

    

    private void OnDisable()
    {
        ProjectEvents.GameLost -= GameLost;
        ProjectEvents.GameWin -= GameWin;
        ProjectEvents.UITargetTextStartedTheHideAnimation -= UITargetTextStartedTheHideAnimation;

    }

    public void BeginGame()
    {
        StartPlayer();
        StartCube();
        _isGamePlaying = true;
    }

    void StartPlayer()
    {
        BeginnerPlayer.GetComponent<Player>().StartPlayer(true);
    }
    
    void StartCube()
    {
        var pathFolower = pathCube.GetComponent<BeziePathFollower>();
        pathFolower.canMove = true;
        pathFolower.SetSpeed(GameManager.Instance.speed);
    }
    
    #endregion
    
    #region SpawnMobs

    private void SpawnMobs()
    {
        for (int i = 0; i < coinSpawnParent.transform.GetChild(curGameType).transform.childCount; i++)
        {
            SpawnMobs(i);
        }
    }

    void SpawnMobs(int pos)
    {
        GameObject[] currentObjs = new GameObject[2];
        for (int i = 0; i < currentObjs.Length; i++)
        {
            currentObjs[i] = coinSpawnParent.transform.GetChild(curGameType).transform.GetChild(pos).transform.GetChild(i).gameObject;
        }
        bool isForward = IsForward(currentObjs);
        var totalSpawnMob = 0;

        if (isForward)
        {
            var totalDistance = Mathf.Abs(currentObjs[0].transform.position.z - currentObjs[1].transform.position.z);
            totalSpawnMob = (int)(totalDistance / GameManager.Instance.spawnMobDistanceVertical);
            SpawnForward(currentObjs, totalSpawnMob);
        }
        else
        {
            var totalDistance = Mathf.Abs(currentObjs[0].transform.position.x - currentObjs[1].transform.position.x);
            totalSpawnMob = (int)(totalDistance / GameManager.Instance.spawnMobDistanceVertical);
            bool isRight = IsRight(currentObjs);

            if (isRight)
            {
                SpawnRight(currentObjs, totalSpawnMob);
            }
            else
            {
                SpawnLeft(currentObjs, totalSpawnMob);
            }
        }
    }

    private void SpawnForward(GameObject[] go, int totalSpawnCoin)
    {
        var distance = GameManager.Instance.distanceBetweenMobAndPortal;
        for (int i = 0; i < totalSpawnCoin; i++)
        {
            var spawnPosZ = go[0].transform.position.z + (i * GameManager.Instance.spawnMobDistanceVertical);
            int IsAnyPortal = 0;
            for (int j = 0; j < expPortals.Count; j++)
            {
                if (spawnPosZ < expPortals[j].transform.position.z + distance &&
                    spawnPosZ > expPortals[j].transform.position.z - distance)
                {
                    IsAnyPortal++;
                }
            }

            if (IsAnyPortal == 0)
            {
                for (int j = 0; j < GameManager.Instance.spawnMobDistanceHorizontal.Length; j++)
                {
                    int isSpawn = Random.Range(0, 100);
                    if (isSpawn < GameManager.Instance.spawnRate)
                    {
                        int obsSpawn = Random.Range(0, 100);
                        if (obsSpawn < GameManager.Instance.obsSpawnRate)
                        {
                            var spawnedName = Random.Range(0, resourcesObs.Length);
                            if (GameManager.Instance.isOnlyOneObs)
                                spawnedName = GameManager.Instance.spawnObsNumber;
                            var SpawnItem = 
                                Instantiate(Resources.Load<GameObject>("Obs/" + resourcesObs[spawnedName].name));
                            SpawnItem.transform.position = new Vector3(go[0].transform.position.x + GameManager.Instance.spawnMobDistanceHorizontal[j],
                                SpawnItem.transform.position.y,
                                spawnPosZ);
                            SpawnItem.transform.rotation = go[0].transform.rotation;
                        }
                        else
                        {
                            var type = ChooseType();
                            var number = GetNumber(type);
                            var SpawnItem = 
                                Instantiate(Resources.Load<GameObject>(spawnablePath));
                            SpawnItem.transform.position = new Vector3(go[0].transform.position.x + GameManager.Instance.spawnMobDistanceHorizontal[j],
                                SpawnItem.transform.position.y,
                                spawnPosZ);
                            SpawnItem.transform.rotation = go[0].transform.rotation;
                            SpawnItem.GetComponent<Number>().SetPlus(number,type);
                        }
                    }
                }
            }
        }
    }
    
    private void SpawnRight(GameObject[] go, int totalSpawnCoin)
    {
        var distance = GameManager.Instance.distanceBetweenMobAndPortal;
        for (int i = 0; i < totalSpawnCoin; i++)
        {
            var spawnPosX = go[0].transform.position.x + (i * GameManager.Instance.spawnMobDistanceVertical);
            int IsAnyPortal = 0;
            for (int j = 0; j < expPortals.Count; j++)
            {
                if (spawnPosX < expPortals[j].transform.position.x + distance &&
                    spawnPosX > expPortals[j].transform.position.x - distance)
                {
                    IsAnyPortal++;
                }
            }

            if (IsAnyPortal == 0)
            {
                for (int j = 0; j < GameManager.Instance.spawnMobDistanceHorizontal.Length; j++)
                {
                    int isSpawn = Random.Range(0, 100);
                    if (isSpawn < GameManager.Instance.spawnRate)
                    {
                        int obsSpawn = Random.Range(0, 100);
                        if (obsSpawn < GameManager.Instance.obsSpawnRate)
                        {
                            var spawnedName = Random.Range(0, resourcesObs.Length);
                            if (GameManager.Instance.isOnlyOneObs)
                                spawnedName = GameManager.Instance.spawnObsNumber;
                            var SpawnItem = 
                                Instantiate(Resources.Load<GameObject>("Obs/" + resourcesObs[spawnedName].name));
                            SpawnItem.transform.position = new Vector3(spawnPosX,
                                SpawnItem.transform.position.y,
                                go[0].transform.position.z + GameManager.Instance.spawnMobDistanceHorizontal[j]);
                            SpawnItem.transform.rotation = go[0].transform.rotation;
                        }
                        else
                        {
                            var type = ChooseType();
                            var number = GetNumber(type);
                            var SpawnItem = 
                                Instantiate(Resources.Load<GameObject>(spawnablePath));
                            SpawnItem.transform.position = new Vector3(spawnPosX,
                                SpawnItem.transform.position.y,
                                go[0].transform.position.z + GameManager.Instance.spawnMobDistanceHorizontal[j]);
                            SpawnItem.transform.rotation = go[0].transform.rotation;
                            SpawnItem.GetComponent<Number>().SetPlus(number,type);
                        }
                    }
                }
            }
        }
    }
    private void SpawnLeft(GameObject[] go, int totalSpawnCoin)
    {
        var distance = GameManager.Instance.distanceBetweenMobAndPortal;
        for (int i = 0; i < totalSpawnCoin; i++)
        {
            // pos - 12 portal -30 distance 3
            var spawnPosX = go[0].transform.position.x - (i * GameManager.Instance.spawnMobDistanceVertical);
            int IsAnyPortal = 0;
            for (int j = 0; j < expPortals.Count; j++)
            {
                if (spawnPosX < expPortals[j].transform.position.x + distance &&
                    spawnPosX > expPortals[j].transform.position.x - distance)
                {
                    IsAnyPortal++;
                }
            }

            if (IsAnyPortal == 0)
            {
                for (int j = 0; j < GameManager.Instance.spawnMobDistanceHorizontal.Length; j++)
                {
                    int isSpawn = Random.Range(0, 100);
                    if (isSpawn < GameManager.Instance.spawnRate)
                    {
                        int obsSpawn = Random.Range(0, 100);
                        if (obsSpawn < GameManager.Instance.obsSpawnRate)
                        {
                            var spawnedName = Random.Range(0, resourcesObs.Length);
                            if (GameManager.Instance.isOnlyOneObs)
                                spawnedName = GameManager.Instance.spawnObsNumber;
                            var SpawnItem = 
                                Instantiate(Resources.Load<GameObject>("Obs/" + resourcesObs[spawnedName].name));
                            SpawnItem.transform.position = new Vector3(spawnPosX,
                                SpawnItem.transform.position.y,
                                go[0].transform.position.z + GameManager.Instance.spawnMobDistanceHorizontal[j]);
                            SpawnItem.transform.rotation = go[0].transform.rotation;
                        }
                        else
                        {
                            var type = ChooseType();
                            var number = GetNumber(type);
                            var SpawnItem = 
                                Instantiate(Resources.Load<GameObject>(spawnablePath));
                            SpawnItem.transform.position = new Vector3(spawnPosX,
                                SpawnItem.transform.position.y,
                                go[0].transform.position.z + GameManager.Instance.spawnMobDistanceHorizontal[j]);
                            SpawnItem.transform.rotation = go[0].transform.rotation;
                            SpawnItem.GetComponent<Number>().SetPlus(number,type);
                        }
                    }
                }
            }
        }
    }

    private bool IsForward(GameObject[] go)
    {
        if (Mathf.Abs(go[0].transform.position.x - go[1].transform.position.x) == 0)
        {
            return true;
        }

        return false;
    }
    
    private bool IsRight(GameObject[] go)
    {
        if ((go[1].transform.position.x > go[0].transform.position.x))
        {
            return true;
        }

        return false;
    }
    
    public int GetNumber(int type)
    {
        if (type == 0)
        {
            return Random.Range( GameManager.Instance.spawnNumberLimitIncrease[0], GameManager.Instance.spawnNumberLimitIncrease[1]);
        }
        else if (type == 1)
        {
            return Random.Range(GameManager.Instance.spawnNumberLimitDecrease[0], GameManager.Instance.spawnNumberLimitDecrease[1]);
        }
        else if (type == 2)
        {
            return Random.Range(GameManager.Instance.spawnNumberLimitMultiply[0], GameManager.Instance.spawnNumberLimitMultiply[1]);
        }
        else
        {
            return Random.Range( GameManager.Instance.spawnNumberLimitDivide[0], GameManager.Instance.spawnNumberLimitDivide[1]);
        }
    }

    public int ChooseType()
    {
        var randomType = Random.Range(0, 100);
        var zero = GameManager.Instance.typeChances[0];
        var one = GameManager.Instance.typeChances[1];
        var two = GameManager.Instance.typeChances[2];
        var three = GameManager.Instance.typeChances[3];
        if (randomType <= zero)
        {
            return 0;
        }
        else if (randomType < zero + one)
        {
            return 1;
        }
        else if (randomType < zero + one + two)
        {
            return 2;
        }
        else
        {
            return 3;
        }
    }

    #endregion

    #region SpawnPortals
    
    void SpawnPortals()
    {
        for (int i = 0; i < PortalSpawnPosesParent.transform.GetChild(curGameType).transform.childCount; i++)
        {
            SpawnPortals(i);
        }
    }
    
    void SpawnPortals(int pos)
    {
        GameObject[] currentObjs = new GameObject[2];
        for (int i = 0; i < currentObjs.Length; i++)
        {
            currentObjs[i] = PortalSpawnPosesParent.transform.GetChild(curGameType).transform.GetChild(pos).transform.GetChild(i).gameObject;
        }
        bool isForward = IsForward(currentObjs);
        var totalSpawnPortal = 0;

        if (isForward)
        {
            var totalDistance = Mathf.Abs(currentObjs[0].transform.position.z - currentObjs[1].transform.position.z);
            totalSpawnPortal = (int)(totalDistance / GameManager.Instance.spawnPortalDistanceVertical);
            SpawnForwardPortal(currentObjs, totalSpawnPortal);
        }
        else
        {
            var totalDistance = Mathf.Abs(currentObjs[0].transform.position.x - currentObjs[1].transform.position.x);
            totalSpawnPortal = (int)(totalDistance / GameManager.Instance.spawnPortalDistanceVertical);
            bool isRight = IsRight(currentObjs);

            if (isRight)
            {
                SpawnRightPortal(currentObjs, totalSpawnPortal);
            }
            else
            {
                SpawnLeftPortal(currentObjs, totalSpawnPortal);
            }
        }
    }
    
    private void SpawnForwardPortal(GameObject[] go, int totalSpawnPortal)
    {
        for (int i = 0; i < totalSpawnPortal; i++)
        {
            var portalSpawnRate = Random.Range(0, 100);
            if (portalSpawnRate < GameManager.Instance.portalSpawnRate)
            {
                var spawnPosZ = go[0].transform.position.z + (i * GameManager.Instance.spawnPortalDistanceVertical);
            
                var portal = Instantiate(Resources.Load<GameObject>("Map/Gate"));
                portal.transform.position = new Vector3(go[0].transform.position.x,
                    go[0].transform.position.y, spawnPosZ);
                portal.transform.rotation = go[0].transform.rotation;

                int year0 = Random.Range(GameManager.Instance.addableExp[0], GameManager.Instance.addableExp[1]);
                int year1 = Random.Range(GameManager.Instance.addableExp[0], GameManager.Instance.addableExp[1]);

                portal.GetComponent<GateController>().SetPortals(year0, year1);
            
                expPortals.Add(portal);
            }
        }
    }
    
    private void SpawnRightPortal(GameObject[] go, int totalSpawnPortal)
    {
        for (int i = 0; i < totalSpawnPortal; i++)
        {
            var portalSpawnRate = Random.Range(0, 100);
            if (portalSpawnRate < GameManager.Instance.portalSpawnRate)
            {
                var spawnPosZ = go[0].transform.position.x + (i * GameManager.Instance.spawnPortalDistanceVertical);
            
                var portal = Instantiate(Resources.Load<GameObject>("Map/Gate"));
                portal.transform.position = new Vector3(spawnPosZ,
                    go[0].transform.position.y, go[0].transform.position.z);
                portal.transform.rotation = go[0].transform.rotation;

                int year0 = Random.Range(GameManager.Instance.addableExp[0], GameManager.Instance.addableExp[1]);
                int year1 = Random.Range(GameManager.Instance.addableExp[0], GameManager.Instance.addableExp[1]);

                portal.GetComponent<GateController>().SetPortals(year0, year1);
            
                expPortals.Add(portal);
            }
        }
    }
    
    private void SpawnLeftPortal(GameObject[] go, int totalSpawnPortal)
    {
        for (int i = 0; i < totalSpawnPortal; i++)
        {
            var portalSpawnRate = Random.Range(0, 100);
            if (portalSpawnRate < GameManager.Instance.portalSpawnRate)
            {
                var spawnPosZ = go[0].transform.position.x - (i * GameManager.Instance.spawnPortalDistanceVertical);
            
                var portal = Instantiate(Resources.Load<GameObject>("Map/Gate"));
                portal.transform.position = new Vector3(spawnPosZ,
                    go[0].transform.position.y, go[0].transform.position.z);
                portal.transform.rotation = go[0].transform.rotation;

                int year0 = Random.Range(GameManager.Instance.addableExp[0], GameManager.Instance.addableExp[1]);
                int year1 = Random.Range(GameManager.Instance.addableExp[0], GameManager.Instance.addableExp[1]);

                portal.GetComponent<GateController>().SetPortals(year0, year1);
            
                expPortals.Add(portal);
            }
        }
    }

    #endregion

    #region LevelSettings

    public void LevelGoal()
    {
        if (GameManager.Instance.IsEditorGoal)
        {
            levelGoal = GameManager.Instance.editorLevelGoal;
            UIManager.Instance.SetGoalText(levelGoal);
        }
    }

    #endregion

    #region Final

    public void GameCompleted(bool IsWin)
    {
        PlayerController.Instance.GetComponent<Player>().LevelDone();
        if (IsWin)
        {
            GameManager.Instance.CompleteSound();
            confetti.SetActive(true);
            pathCube.GetComponent<BeziePathFollower>().canMove = false;
            BeginnerPlayer.transform.parent.GetComponent<BeziePathFollower>().canMove = false;
            PlayerController.Instance.GetComponent<Animator>().enabled = true;
        }
    }
    
    private void GameLost()
    {
        pathCube.GetComponent<BeziePathFollower>().canMove = false;
        BeginnerPlayer.transform.parent.GetComponent<BeziePathFollower>().canMove = false;
        PlayerController.Instance.GetComponent<Animator>().enabled = true;
        _isGamePlaying = false;
    }
    
    private void GameWin()
    {
        _isGamePlaying = false;
    }

    #endregion

    public void PathCompleted()
    {
        
    }
    
    private void UITargetTextStartedTheHideAnimation()
    {
        
        for (var i = 0; i < environmentNumbersParentOfParents.Count; i++)
        {
            var parent = environmentNumbersParentOfParents[i];
            parent.DOScale(cachedScales[i], 1.5f).SetDelay(i * 0.5f);
            parent.DORotate(parent.eulerAngles + new Vector3(0, 360, 0) * 2, 1.5f, RotateMode.FastBeyond360).SetDelay(i * 0.5f).OnComplete(
                () =>
                {
                    var numberPosition = parent.localScale;
                    var fatScale = new Vector3(numberPosition.x * 1.2f, numberPosition.y * 0.833f, numberPosition.z);
                    var skinnyScale = new Vector3(numberPosition.x * 0.833f, numberPosition.y * 1.2f, numberPosition.z);

                    //50 50 possibility
                    if (Random.Range(0.0f, 10f) > 5)
                    {
                        parent.localScale = fatScale;
                        parent.DOScale(skinnyScale, Random.Range(1.5f,2.5f)).SetLoops(-1, LoopType.Yoyo).Goto(Random.Range(0f,5f), true);
                    }
                    else
                    {
                        parent.localScale = fatScale;
                        parent.DOScale(skinnyScale, Random.Range(1.5f,2.5f)).SetLoops(-1, LoopType.Yoyo).Goto(Random.Range(0f,5f), true);
                    }
                });
        }
    }
}
