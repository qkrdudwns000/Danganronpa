using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionType : MonoBehaviour
{
    public bool isDoor; // ��ȣ�ۿ��� ������
    public bool isObject; // ��ȣ�ۿ��� �Ϲݰ�ü����.

    [SerializeField] string interactionName;

    public string GetName()
    {
        return interactionName;
    }

}
