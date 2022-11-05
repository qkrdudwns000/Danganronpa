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

    bool isDialogue = false;

    InteractionController theIC;

    private void Start()
    {
        theIC = FindObjectOfType<InteractionController>();
    }

    public void ShowDialogue()
    {
        txt_Dialogue.text = "";
        txt_Name.text = "";
        theIC.HideUI();

        SettingUI(true);
    }

    void SettingUI(bool p_flag)
    {
        go_DialogueBar.SetActive(p_flag);
        go_DialogueNameBar.SetActive(p_flag);
    }


}
