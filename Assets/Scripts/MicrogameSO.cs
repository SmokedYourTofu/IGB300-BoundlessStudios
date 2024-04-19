using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/MiceogameSO")]
public class MicrogameSO : ScriptableObject
{
    public string microGameName;
    public GameObject microGameObject;
    public Transform prefabTransform;
}
