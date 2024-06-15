using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Leguar.TotalJSON;
using UnityEngine;

public class GlobalObject : Singleton<GlobalObject>
{
    public Vector2 screenResolution;
    public Vector2 rawMouse;
    public Vector2 mouse;
    public Vector2 prevMouse = Vector2.zero;
    public Vector2 mouseMoved;
    public float mouseSpeed = 5f;
    public PlayerInputSystem playerInputSystem;
    public string entityDataPath;
    public List<EntityData> entityDatas;
    public string playerSkillDataPath;
    public BinarySearchTree<CustomMonoBehavior> customMonoBehaviorBinarySearchTree = new();
    CustomMonoBehaviorComparer customMonoBehaviorComparer = new();
    UtilObject utilObject = new UtilObject();
    public Coroutine nullCoroutine;
    private void Awake() 
    {
        playerInputSystem = new PlayerInputSystem();
        entityDataPath = Application.dataPath + "/JsonData/EntityData.json";
        entityDatas = new List<EntityData>();
        string json = File.ReadAllText(GlobalObject.Instance.entityDataPath);
        entityDatas = FromJson<EntityData>(json);
        playerSkillDataPath = Application.dataPath + "/JsonData/PlayerSkillData.json";
        nullCoroutine = StartCoroutine(Null());
    }

    public void UpdateCustomonoBehaviorHealth(float value, GameObject gameObject)
    {
        customMonoBehaviorBinarySearchTree.Search((CustomMonoBehavior customMonoBehavior) => {return customMonoBehavior.gameObject.GetInstanceID();}, gameObject.GetInstanceID())?.UpdateHealth(value);
        
        // var searched = utilObject.CombatEntityInfoBinarySearch(combatEntityInfos, gameObject.GetInstanceID()).CombatEntity;
        // Debug.Log(searched.CurentHealth + "-" + searched.gameObject.name);
        // searched.UpdateHealth(10);
    }

    public CustomMonoBehavior GetCustomMonoBehavior(GameObject gameObject)
    {
        return customMonoBehaviorBinarySearchTree.Search((CustomMonoBehavior customMonoBehavior) => {return customMonoBehavior.gameObject.GetInstanceID();}, gameObject.GetInstanceID());
    }

    // Start is called before the first frame update
    void Start()
    {
        screenResolution = new Vector2(Screen.currentResolution.width, Screen.currentResolution.height);
        
        #region search all custom monobehavior

        FindObjectsByType<CustomMonoBehavior>(FindObjectsSortMode.InstanceID).ToList().ForEach((customMonoBehavior) => 
        {
            customMonoBehaviorBinarySearchTree.Insert(customMonoBehavior);
        });

        // sort the list by object instance id
        // combatEntityInfos.Sort(combatEntityInfoComparer);
        #endregion
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate() 
    {
        rawMouse = playerInputSystem.Control.View.ReadValue<Vector2>();
        mouse = new Vector2(rawMouse.x/screenResolution.x, rawMouse.y/screenResolution.y);
        mouseMoved = mouse - prevMouse; mouseMoved *= mouseSpeed;    
    }

    public List<T> FromJson<T> (string json)
    {
        JSON JsonObject = JSON.ParseString(json);
        Wrapper<T> wrapper = JsonObject.Deserialize<Wrapper<T>>();
        JsonObject.Deserialize<Wrapper<T>>();
        return wrapper.items;
    }

    public List<T> FromJson<T> (string path, bool use_path)
    {
        string json = File.ReadAllText(path);
        JSON JsonObject = JSON.ParseString(json);
        Wrapper<T> wrapper = JsonObject.Deserialize<Wrapper<T>>();
        JsonObject.Deserialize<Wrapper<T>>();
        return wrapper.items;
    }


    public IEnumerator Null()
    {
        yield return new WaitForSeconds(0);
    }

    void OnEnable()
    {
        playerInputSystem.Control.Enable();
    }

    void OnDisable()
    {
        playerInputSystem.Control.Disable();
    }
}

public class Wrapper<T>
{
    public List<T> items;
}

public class CustomMonoBehaviorComparer : IComparer<CustomMonoBehavior>
{
    public int Compare(CustomMonoBehavior x, CustomMonoBehavior y)
    {
        if (x == null || y == null) 
        {
            return 0;
        }
        return x.gameObject.GetInstanceID().CompareTo(y.gameObject.GetInstanceID());
    }
}



