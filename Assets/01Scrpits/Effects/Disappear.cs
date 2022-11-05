using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disappear : MonoBehaviour
{

    [SerializeField] float disappearTime;

    // Start is called before the first frame update
    void OnEnable() // �ش罺ũ��Ʈ�� �����Ǿ��ִ� ��ü�� Ȱ��ȭ�ɶ����� ȣ��
    {
        StartCoroutine(DisappearCoroutine());
    }

    IEnumerator DisappearCoroutine()
    {
        yield return new WaitForSeconds(disappearTime);

        gameObject.SetActive(false);
    }
}
