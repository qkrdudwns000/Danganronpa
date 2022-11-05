using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionType : MonoBehaviour
{
    public bool isDoor; // 상호작용대상 문인지
    public bool isObject; // 상호작용대상 일반객체인지.

    [SerializeField] string interactionName;

    public string GetName()
    {
        return interactionName;
    }

}
