using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionEffect : MonoBehaviour
{

    [SerializeField] float moveSpeed;
    Vector3 targetPos = new Vector3(); // ���� �����ġ

    [SerializeField] ParticleSystem ps_Effect;

    public static bool isCollide = false; // ��� �ε������� �Ⱥε������� �������˸������ϴ��Լ�.

    public void SetTarget(Vector3 _target)
    {
        targetPos = _target;
    }

    // Update is called once per frame
    void Update()
    {
        if(targetPos != Vector3.zero)
        {
            if ((this.transform.position - targetPos).sqrMagnitude >= 0.1f) // sqrmagnitude => �ΰŸ������� �Ÿ��� ������. ��Ȯ�Ѱ��� �˼��¾����� �񱳽��϶� ����.
            {
                this.transform.position = Vector3.Lerp(this.transform.position, targetPos, moveSpeed);
            }
            else
            {
                ps_Effect.gameObject.SetActive(true);
                ps_Effect.transform.position = this.transform.position;
                ps_Effect.Play();
                isCollide = true;
                targetPos = Vector3.zero;
                this.gameObject.SetActive(false);
            }
        }
    }
}
