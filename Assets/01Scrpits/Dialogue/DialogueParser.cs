using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueParser : MonoBehaviour
{
    public Dialogue[] Parse(string _CSVFileName)
    {
        List<Dialogue> dialogueList = new List<Dialogue>(); // 대사 리스트생성.
        TextAsset csvData = Resources.Load<TextAsset>(_CSVFileName); // 대사가져옴.

        string[] data = csvData.text.Split(new char[] { '\n' }); // 엔터가나올때마다 (한줄씩)쪼개서들어감.

        for(int i = 1; i < data.Length;) // int = 1부터시작하는이유는 구글 스프레드시트에 작성한내용 맨첫줄이 id와 이름 대사 (명사)들을 넣은 의미없는 내용이기때문.
        {
            string[] row = data[i].Split(new char[] { ',' }); // 한줄당 id, 이름, 대사  ','로나뉘어져있기떄문에 한번더 쪼개서 배열에저장.

            Dialogue dialogue = new Dialogue(); // 대사 리스트 생성.

            dialogue.name = row[1];

            List<string> contextList = new List<string>();
            List<string> spriteList = new List<string>();

            do
            {
                contextList.Add(row[2]); // 최초 1회실행.
                spriteList.Add(row[3]);

                if (++i < data.Length) // i + 1 한 후
                {
                    row = data[i].Split(new char[] { ',' });  // 다음번째 data[i]를 미리쪼갠 후 대사만가져옴.
                }
                else
                {
                    break; //  ++i >= data.Length 일경우 빠져나오게
                }
            } while (row[0].ToString() == ""); // 대사가아닌 id 부분이 공백일경우 do 문 재반복. 그래서 id부분 공백아닐때까지 반복.

            dialogue.contexts = contextList.ToArray(); // 리스트이기때문에 배열로 변환해서 다시 넣어줌.
            dialogue.spriteName = spriteList.ToArray();

            dialogueList.Add(dialogue);
        }

        return dialogueList.ToArray();
    }

    //private void Start()
    //{
    //    Parse("prologue"); // 테스트용으로
    //}
}
