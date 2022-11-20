using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionEvent : MonoBehaviour
{
    [SerializeField] DialogueEvent dialogueEvent;

    public Dialogue[] GetDialogue()
    {
        DialogueEvent t_DialogueEvent = new DialogueEvent(); // 바로 대입을해버리면 인스펙터창에 카메라타겟팅바인딩한게 날라가므로 임시변수설정
        t_DialogueEvent.dialogues = DatabaseManager.instance.GetDialogue((int)dialogueEvent.line.x, (int)dialogueEvent.line.y); // Vector값이기에 int로 형변환.
        for(int i= 0; i<dialogueEvent.dialogues.Length; i++)
        {
            t_DialogueEvent.dialogues[i].tf_Target = dialogueEvent.dialogues[i].tf_Target; // 임시변수에다 치환
            t_DialogueEvent.dialogues[i].cameraType = dialogueEvent.dialogues[i].cameraType; // 임시변수에다 치환.
        }
        dialogueEvent.dialogues = t_DialogueEvent.dialogues;
        
        return dialogueEvent.dialogues;
    }
}
