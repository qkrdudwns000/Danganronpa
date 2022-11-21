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
    bool isNext = false; // 특정키 입력대기.

    [Header("텍스츠 출력 딜레이")]
    [SerializeField] float textDelay;

    int lineCount = 0; // 대화 카운트.
    int contextCount = 0;//대사카운트

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
                        contextCount = 0; // 다음대화상대로 넘어갔으니 대화 인덱스초기화
                        if(++lineCount < dialogues.Length)
                        {
                            StartCoroutine(CameraTargettingType());
                        }
                        else // 모든 대화가 끝났을경우
                        {
                            EndDialogue();
                        }
                    }
                }
            }
        
    }

    public void ShowDialogue(Dialogue[] p_dialogues)
    {
        isDialogue = true; //대화시작됨의 bool값.
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
                yield return new WaitUntil(() => SplashManager.isfinished); // 스플래쉬매니저의 isfinished가 true가될때까지 정체.
                break;
            case CameraType.FadeOut:
                SettingUI(false);
                SplashManager.isfinished = false;
                StartCoroutine(theSplashManager.FadeOut(false, true));
                yield return new WaitUntil(() => SplashManager.isfinished); // 스플래쉬매니저의 isfinished가 true가될때까지 정체.
                break;
            case CameraType.FlashIn:
                SettingUI(false);
                SplashManager.isfinished = false;
                StartCoroutine(theSplashManager.FadeIn(true, true));
                yield return new WaitUntil(() => SplashManager.isfinished); // 스플래쉬매니저의 isfinished가 true가될때까지 정체.
                break;
            case CameraType.FlashOut:
                SettingUI(false);
                SplashManager.isfinished = false;
                StartCoroutine(theSplashManager.FadeOut(true, true));
                yield return new WaitUntil(() => SplashManager.isfinished); // 스플래쉬매니저의 isfinished가 true가될때까지 정체.
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
        if (dialogues[lineCount].spriteName[contextCount] != "") // 해당칸이 여백이아닐때만
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
        t_ReplaceText = t_ReplaceText.Replace("'", ","); // 액셀에서 임의로 콤마자리에썻던 (') 를 (,) 로 치환해주는 함수.
        t_ReplaceText = t_ReplaceText.Replace("\\n", "\n"); // text문을 string으로 변환하기위함. 역슬래쉬 앞에 역슬래쉬를 한번더써주면 text형식임을 인지.

        bool t_white = false, t_yellow = false, t_cyan = false;
        bool t_ignore = false; // 특수문자를 생략하기위한 bool값.

        for (int i = 0; i < t_ReplaceText.Length; i++)
        {
            switch (t_ReplaceText[i])
            {
                case 'ⓦ': t_white = true; t_yellow = false; t_cyan = false; t_ignore = true; break;
                case 'ⓨ': t_white = false; t_yellow = true; t_cyan = false; t_ignore = true; break;
                case 'ⓒ': t_white = false; t_yellow = false; t_cyan = true; t_ignore = true; break;
                case '①': StartCoroutine(theSplashManager.Splash()); SoundManager.instance.PlaySound("Emotion1", 1); t_ignore = true; break;
                case '②': StartCoroutine(theSplashManager.Splash()); SoundManager.instance.PlaySound("Emotion2", 1); t_ignore = true; break;
            }

            string t_letter = t_ReplaceText[i].ToString();
            if(!t_ignore) // 특수문자 ⓦ , ⓨ 같은것들이 시작되면 그뒤로는 색을변경하라는 신호이므로 해당 특수문자는안보이게끔.처리를하고 특수문자뒤의글자 색변경.
            {
                if (t_white) { t_letter = "<color=#ffffff>" + t_letter + "</color>"; }
                else if (t_yellow) { t_letter = "<color=#FFFF00>" + t_letter + "</color>"; }
                else if (t_cyan) { t_letter = "<color=#42DEE3>" + t_letter + "</color>"; }
                txt_Dialogue.text += t_letter; // 실질적 글씨 출력구간 (한글자식(타사식으로보이게끔))
            }
            t_ignore = false; // 특수문자만 글자출력생략되고 그뒤에는 정상출력이 되야하므로 bool값변경.

            yield return new WaitForSeconds(textDelay);
        }

        isNext = true;
    }

    void SettingUI(bool p_flag)
    {
        go_DialogueBar.SetActive(p_flag);

        if (p_flag)
        {
            if (dialogues[lineCount].name == "") // 이름부분이 공백일경우(독백)
            {
                go_DialogueNameBar.SetActive(false); 
            }
            else // 이름부분 공백이아닐경우 (대화)
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
