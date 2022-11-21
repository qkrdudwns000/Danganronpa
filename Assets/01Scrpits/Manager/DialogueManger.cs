using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManger : MonoBehaviour
{
    [SerializeField] GameObject go_DialogueBar;
    [SerializeField] GameObject go_DialogueNameBar;

    [SerializeField] TMPro.TMP_Text txt_Dialogue;
    [SerializeField] TMPro.TMP_Text txt_Name;

    Dialogue[] dialogues;

    bool isDialogue = false;
    bool isNext = false; // Ư��Ű �Է´��.

    [Header("�ؽ��� ��� ������")]
    [SerializeField] float textDelay;

    int lineCount = 0; // ��ȭ ī��Ʈ.
    int contextCount = 0;//���ī��Ʈ

    InteractionController theIC;
    CameraController theCam;
    SplashManager theSplashManager;
    SpriteManager theSpriteManager;

    private void Start()
    {
        theIC = FindObjectOfType<InteractionController>();
        theCam = FindObjectOfType<CameraController>();
        theSplashManager = FindObjectOfType<SplashManager>();
        theSpriteManager = FindObjectOfType<SpriteManager>();
    }
    private void Update()
    {
        if (isDialogue)
            if (isNext)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    isNext = false;
                    txt_Dialogue.text = "";
                    if (++contextCount < dialogues[lineCount].contexts.Length)
                    {
                        StartCoroutine(TypeWriter());
                    }
                    else
                    {
                        contextCount = 0; // ������ȭ���� �Ѿ���� ��ȭ �ε����ʱ�ȭ
                        if(++lineCount < dialogues.Length)
                        {
                            StartCoroutine(CameraTargettingType());
                        }
                        else // ��� ��ȭ�� ���������
                        {
                            EndDialogue();
                        }
                    }
                }
            }
        
    }

    public void ShowDialogue(Dialogue[] p_dialogues)
    {
        isDialogue = true; //��ȭ���۵��� bool��.
        txt_Dialogue.text = "";
        txt_Name.text = "";
        theIC.SettingUI(false);
        dialogues = p_dialogues;
        theCam.CamOriginSetting();

        //SettingUI(false);
        StartCoroutine(CameraTargettingType());
    }

    IEnumerator CameraTargettingType()
    {
        switch (dialogues[lineCount].cameraType)
        {
            case CameraType.FadeIn:
                SettingUI(false);
                SplashManager.isfinished = false;
                StartCoroutine(theSplashManager.FadeIn(false, true));
                yield return new WaitUntil(() => SplashManager.isfinished); // ���÷����Ŵ����� isfinished�� true���ɶ����� ��ü.
                break;
            case CameraType.FadeOut:
                SettingUI(false);
                SplashManager.isfinished = false;
                StartCoroutine(theSplashManager.FadeOut(false, true));
                yield return new WaitUntil(() => SplashManager.isfinished); // ���÷����Ŵ����� isfinished�� true���ɶ����� ��ü.
                break;
            case CameraType.FlashIn:
                SettingUI(false);
                SplashManager.isfinished = false;
                StartCoroutine(theSplashManager.FadeIn(true, true));
                yield return new WaitUntil(() => SplashManager.isfinished); // ���÷����Ŵ����� isfinished�� true���ɶ����� ��ü.
                break;
            case CameraType.FlashOut:
                SettingUI(false);
                SplashManager.isfinished = false;
                StartCoroutine(theSplashManager.FadeOut(true, true));
                yield return new WaitUntil(() => SplashManager.isfinished); // ���÷����Ŵ����� isfinished�� true���ɶ����� ��ü.
                break;
            case CameraType.ObjectFront:
                theCam.CameraTargetting(dialogues[lineCount].tf_Target);
                break;
            case CameraType.Reset:
                theCam.CameraTargetting(null, 0.05f, true, false);
                break;
        }
        StartCoroutine(TypeWriter());
    }

    void EndDialogue()
    {
        isDialogue = false;
        contextCount = 0;
        lineCount = 0;
        dialogues = null;
        isNext = false;
        theCam.CameraTargetting(null, 0.05f, true, true);
        SettingUI(false);
    }

    void ChangeSprite()
    {
        if (dialogues[lineCount].spriteName[contextCount] != "") // �ش�ĭ�� �����̾ƴҶ���
        {
            StartCoroutine(theSpriteManager.SpriteChangeCoroutine(dialogues[lineCount].tf_Target, dialogues[lineCount].spriteName[contextCount].Trim()));
        }
    }

    void PlaySound()
    {
        if (dialogues[lineCount].voiceName[contextCount] != "")
        {
            SoundManager.instance.PlaySound(dialogues[lineCount].voiceName[contextCount].Trim(), 2) ;
        }
    }

    IEnumerator TypeWriter()
    {
        SettingUI(true);
        ChangeSprite();
        PlaySound();

        string t_ReplaceText = dialogues[lineCount].contexts[contextCount];
        t_ReplaceText = t_ReplaceText.Replace("'", ","); // �׼����� ���Ƿ� �޸��ڸ������� (') �� (,) �� ġȯ���ִ� �Լ�.
        t_ReplaceText = t_ReplaceText.Replace("\\n", "\n"); // text���� string���� ��ȯ�ϱ�����. �������� �տ� ���������� �ѹ������ָ� text�������� ����.

        bool t_white = false, t_yellow = false, t_cyan = false;
        bool t_ignore = false; // Ư�����ڸ� �����ϱ����� bool��.

        for (int i = 0; i < t_ReplaceText.Length; i++)
        {
            switch (t_ReplaceText[i])
            {
                case '��': t_white = true; t_yellow = false; t_cyan = false; t_ignore = true; break;
                case '��': t_white = false; t_yellow = true; t_cyan = false; t_ignore = true; break;
                case '��': t_white = false; t_yellow = false; t_cyan = true; t_ignore = true; break;
                case '��': StartCoroutine(theSplashManager.Splash()); SoundManager.instance.PlaySound("Emotion1", 1); t_ignore = true; break;
                case '��': StartCoroutine(theSplashManager.Splash()); SoundManager.instance.PlaySound("Emotion2", 1); t_ignore = true; break;
            }

            string t_letter = t_ReplaceText[i].ToString();
            if(!t_ignore) // Ư������ �� , �� �����͵��� ���۵Ǹ� �׵ڷδ� ���������϶�� ��ȣ�̹Ƿ� �ش� Ư�����ڴ¾Ⱥ��̰Բ�.ó�����ϰ� Ư�����ڵ��Ǳ��� ������.
            {
                if (t_white) { t_letter = "<color=#ffffff>" + t_letter + "</color>"; }
                else if (t_yellow) { t_letter = "<color=#FFFF00>" + t_letter + "</color>"; }
                else if (t_cyan) { t_letter = "<color=#42DEE3>" + t_letter + "</color>"; }
                txt_Dialogue.text += t_letter; // ������ �۾� ��±��� (�ѱ��ڽ�(Ÿ������κ��̰Բ�))
            }
            t_ignore = false; // Ư�����ڸ� ������»����ǰ� �׵ڿ��� ��������� �Ǿ��ϹǷ� bool������.

            yield return new WaitForSeconds(textDelay);
        }

        isNext = true;
    }

    void SettingUI(bool p_flag)
    {
        go_DialogueBar.SetActive(p_flag);

        if (p_flag)
        {
            if (dialogues[lineCount].name == "") // �̸��κ��� �����ϰ��(����)
            {
                go_DialogueNameBar.SetActive(false); 
            }
            else // �̸��κ� �����̾ƴҰ�� (��ȭ)
            {
                go_DialogueNameBar.SetActive(true);
                txt_Name.text = dialogues[lineCount].name;
            }
        }
        else
        {
            go_DialogueNameBar.SetActive(false);
        }
    }


}
