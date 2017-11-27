using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum SourcePathType
{
    Prefabs,
}

public class ResourceMgr : MonoBehaviour
{

    public static ResourceMgr Instance;

    Dictionary<SourcePathType, string> pathTypeDict = new Dictionary<SourcePathType, string>();

    private void Awake()
    {
        Instance = this;

        pathTypeDict.Add(SourcePathType.Prefabs, "Prefabs/");
    }

    public GameObject LoadGo(SourcePathType pathType, string childPath)
    {
        string fullPath = pathTypeDict[pathType] + childPath;

        return Resources.Load(fullPath) as GameObject;
    }
}
