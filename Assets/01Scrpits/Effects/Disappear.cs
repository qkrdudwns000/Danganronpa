using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disappear : MonoBehaviour
{

    [SerializeField] float disappearTime;

    // Start is called before the first frame update
    void OnEnable() // 해당스크립트가 부착되어있는 객체가 활성화될때마다 호출
    {
        StartCoroutine(DisappearCoroutine());
    }

    IEnumerator DisappearCoroutine()
    {
        yield return new WaitForSeconds(disappearTime);

        gameObject.SetActive(false);
    }
}
