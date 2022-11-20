using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueParser : MonoBehaviour
{
    public Dialogue[] Parse(string _CSVFileName)
    {
        List<Dialogue> dialogueList = new List<Dialogue>(); // ��� ����Ʈ����.
        TextAsset csvData = Resources.Load<TextAsset>(_CSVFileName); // ��簡����.

        string[] data = csvData.text.Split(new char[] { '\n' }); // ���Ͱ����ö����� (���پ�)�ɰ�����.

        for(int i = 1; i < data.Length;) // int = 1���ͽ����ϴ������� ���� ���������Ʈ�� �ۼ��ѳ��� ��ù���� id�� �̸� ��� (���)���� ���� �ǹ̾��� �����̱⶧��.
        {
            string[] row = data[i].Split(new char[] { ',' }); // ���ٴ� id, �̸�, ���  ','�γ��������ֱ⋚���� �ѹ��� �ɰ��� �迭������.

            Dialogue dialogue = new Dialogue(); // ��� ����Ʈ ����.

            dialogue.name = row[1];

            List<string> contextList = new List<string>();
            List<string> spriteList = new List<string>();

            do
            {
                contextList.Add(row[2]); // ���� 1ȸ����.
                spriteList.Add(row[3]);

                if (++i < data.Length) // i + 1 �� ��
                {
                    row = data[i].Split(new char[] { ',' });  // ������° data[i]�� �̸��ɰ� �� ��縸������.
                }
                else
                {
                    break; //  ++i >= data.Length �ϰ�� ����������
                }
            } while (row[0].ToString() == ""); // ��簡�ƴ� id �κ��� �����ϰ�� do �� ��ݺ�. �׷��� id�κ� ����ƴҶ����� �ݺ�.

            dialogue.contexts = contextList.ToArray(); // ����Ʈ�̱⶧���� �迭�� ��ȯ�ؼ� �ٽ� �־���.
            dialogue.spriteName = spriteList.ToArray();

            dialogueList.Add(dialogue);
        }

        return dialogueList.ToArray();
    }

    //private void Start()
    //{
    //    Parse("prologue"); // �׽�Ʈ������
    //}
}
