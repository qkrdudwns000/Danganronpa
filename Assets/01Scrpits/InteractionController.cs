using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractionController : MonoBehaviour
{
    [SerializeField] Camera cam;

    RaycastHit hitInfo;

    [SerializeField] GameObject go_NormalCrosshair;
    [SerializeField] GameObject go_InteractiveCrosshair;
    [SerializeField] GameObject go_Crosshair;
    [SerializeField] GameObject go_Cursor;
    [SerializeField] GameObject go_TargetNameBar;
    [SerializeField] TMPro.TMP_Text txt_TargetName;

    bool isContact = false;
    public static bool isInteract = false; // ��ȣ�ۿ������� �ƴ���.

    [SerializeField] ParticleSystem ps_QuestionEffect;

    [SerializeField] Image img_Interaction;
    [SerializeField] Image img_InteractionEffect;

    DialogueManger theDM;

    public void HideUI()
    {
        go_Crosshair.SetActive(false);
        go_Cursor.SetActive(false);
        go_TargetNameBar.SetActive(false);
    }

    private void Start()
    {
        theDM = FindObjectOfType<DialogueManger>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isInteract)
        {
            CheckObject();
            ClickLeftBtn();
        }
    }

    void CheckObject()
    {
        Vector3 t_MousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);

        if (Physics.Raycast(cam.ScreenPointToRay(t_MousePos), out hitInfo, 100))
        {
            Contact();
        }
        else
        {
            NotContact();
        }
    }

    void Contact()
    {
        if (hitInfo.transform.CompareTag("Interaction"))
        {
            go_TargetNameBar.SetActive(true);
            txt_TargetName.text = hitInfo.transform.GetComponent<InteractionType>().GetName();
            if (!isContact)
            {
                isContact = true;
                go_InteractiveCrosshair.SetActive(true);
                go_NormalCrosshair.SetActive(false);
                StopCoroutine("Interaction");
                StopCoroutine(InteractionEffect());
                StartCoroutine(Interaction(true)); // �ڷ�ƾ �Ű������ִ� ȣ����1
                StartCoroutine(InteractionEffect());
            }
        }
        else
        {
            NotContact();
        }
    }
    void NotContact()
    {
        if(isContact)
        {
            go_TargetNameBar.SetActive(false);
            isContact = false;
            go_InteractiveCrosshair.SetActive(false);
            go_NormalCrosshair.SetActive(true);
            StopCoroutine("Interaction");
            StartCoroutine("Interaction", false); // �ڷ�ƾ �Ű������ִ� ȣ����2
        }
    }

    IEnumerator Interaction(bool p_Appear)
    {
        Color color = img_Interaction.color;
        if (p_Appear)
        {
            color.a = 0;
            while (color.a < 1)
            {
                color.a += 0.1f;
                img_Interaction.color = color;
                yield return null;
            }
        }
        else
        {
            while(color.a > 0)
            {
                color.a -= 0.1f;
                img_Interaction.color = color;
                yield return null;
            }
        }
    }

    IEnumerator InteractionEffect()
    {
        while(isContact && !isInteract) // ��ȣ�ۿ��� ��ü���� ���콺�� �ӹ�����������
        {
            Color color = img_InteractionEffect.color;
            color.a = 0.5f;

            img_InteractionEffect.transform.localScale = new Vector3(1.3f, 1.3f, 1.3f);
            Vector3 t_scale = img_InteractionEffect.transform.localScale;


            while (color.a > 0)
            {
                color.a -= 0.002f;
                img_InteractionEffect.color = color;
                t_scale.Set(t_scale.x + Time.deltaTime, t_scale.y + Time.deltaTime, t_scale.z + Time.deltaTime);
                img_InteractionEffect.transform.localScale = t_scale;
                yield return null;
            }
            yield return null;
        }
    }

    void ClickLeftBtn()
    {
        if (!isInteract) // ��ȣ�ۿ��ϴµ��߿� �ߺ���ȣ�ۿ�ȵǰԲ�.
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (isContact)
                {
                    Interact();
                }
            }
        }
    }

    void Interact()
    {
        isInteract = true;

        StopCoroutine("Interaction");
        Color color = img_Interaction.color;
        color.a = 0;
        img_Interaction.color = color;

        ps_QuestionEffect.gameObject.SetActive(true);
        Vector3 t_targetPos = hitInfo.transform.position;
        ps_QuestionEffect.GetComponent<QuestionEffect>().SetTarget(t_targetPos);
        ps_QuestionEffect.transform.position = cam.transform.position;

        StartCoroutine(WaitCollision());
    }

    IEnumerator WaitCollision()
    {
        yield return new WaitUntil(()=>QuestionEffect.isCollide);
        QuestionEffect.isCollide = false; // ���ð���ٸ� �Ŀ��� �ٽ� false�ιٲ���.

        

        theDM.ShowDialogue(hitInfo.transform.GetComponent<InteractionEvent>().GetDialogue());
    }
}
