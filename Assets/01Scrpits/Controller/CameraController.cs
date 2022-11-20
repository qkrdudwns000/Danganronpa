using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    Vector3 originPos;
    Quaternion originRot;

    InteractionController theIC;
    PlayerController thePlsyer;

    Coroutine coroutine;


    private void Start()
    {
        theIC = FindObjectOfType<InteractionController>();
        thePlsyer = FindObjectOfType<PlayerController>();
    }

    public void CamOriginSetting()
    {
        originPos = transform.position;
        originRot = transform.rotation;
    }

    public void CameraTargetting(Transform p_Target, float p_CamSpeed = 0.05f, bool p_isReset = false, bool p_isFinish =false)
    {
        if (!p_isReset)
        {
            if (p_Target != null)
            {
                StopAllCoroutines();
                coroutine = StartCoroutine(CameraTargettingCoroutine(p_Target, p_CamSpeed));
            }
        }
        else
        {
            if(coroutine != null)
            {
                StopCoroutine(coroutine);
            }
            StartCoroutine(CameraResetCoroutine(p_CamSpeed, p_isFinish));
        }
    }
    IEnumerator CameraTargettingCoroutine(Transform p_Target, float p_CamSpeed = 0.05f)
    {
        Vector3 t_TargetPos = p_Target.position;
        Vector3 t_targetForntPos = t_TargetPos + (p_Target.forward * 1.2f);
        Vector3 t_Direction = (t_TargetPos - t_targetForntPos).normalized;

        while (transform.position != t_targetForntPos || Quaternion.Angle(transform.rotation, Quaternion.LookRotation(t_Direction)) >= 0.5f)
        {
            transform.position = Vector3.MoveTowards(transform.position, t_targetForntPos, p_CamSpeed);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(t_Direction), p_CamSpeed);
            yield return null;
        }
    }

    IEnumerator CameraResetCoroutine(float p_CamSpeed = 0.1f, bool p_isFinish = false)
    {
        yield return new WaitForSeconds(0.5f);

        while (transform.position != originPos || Quaternion.Angle(transform.rotation, originRot) >= 0.5f)
        {
            transform.position = Vector3.MoveTowards(transform.position, originPos, p_CamSpeed);
            transform.rotation = Quaternion.Lerp(transform.rotation, originRot, p_CamSpeed);
            yield return null;
        }
        transform.position = originPos;

        if (p_isFinish)
        {
            thePlsyer.Reset();
            theIC.SettingUI(true);
        }
    }
}
