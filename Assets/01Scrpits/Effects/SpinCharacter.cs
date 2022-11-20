using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinCharacter : MonoBehaviour
{
    [SerializeField] Transform tf_Target;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(tf_Target != null)
        {
            Quaternion t_Rotation = Quaternion.LookRotation(tf_Target.position);
            Vector3 t_Euler = new Vector3(0, t_Rotation.eulerAngles.y, 0);
            transform.eulerAngles = t_Euler;
        }
    }
}
