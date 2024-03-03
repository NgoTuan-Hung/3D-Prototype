using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Leguar.TotalJSON;
using UnityEditor.ShaderGraph.Serialization;
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
    public String entityDataPath;
    public List<EntityData> entityDatas;

    private void Awake() 
    {
        playerInputSystem = new PlayerInputSystem();
        entityDataPath = Application.dataPath + "/JsonData/EntityData.json";
        entityDatas = new List<EntityData>();
        string json = File.ReadAllText(GlobalObject.Instance.entityDataPath);
        entityDatas = FromJson<EntityData>(json);
    }
    // Start is called before the first frame update
    void Start()
    {
        screenResolution = new Vector2(Screen.currentResolution.width, Screen.currentResolution.height);
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

