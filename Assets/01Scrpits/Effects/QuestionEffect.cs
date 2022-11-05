using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionEffect : MonoBehaviour
{

    [SerializeField] float moveSpeed;
    Vector3 targetPos = new Vector3(); // 날라갈 대상위치

    [SerializeField] ParticleSystem ps_Effect;

    public static bool isCollide = false; // 대상에 부딪혔는지 안부딪혔는지 전역에알리고자하는함수.

    public void SetTarget(Vector3 _target)
    {
        targetPos = _target;
    }

    // Update is called once per frame
    void Update()
    {
        if(targetPos != Vector3.zero)
        {
            if ((this.transform.position - targetPos).sqrMagnitude >= 0.1f) // sqrmagnitude => 두거리사이의 거리의 제곱값. 정확한값을 알수는없으나 비교식일때 적합.
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
