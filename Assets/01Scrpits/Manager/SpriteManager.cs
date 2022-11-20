using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteManager : MonoBehaviour
{
    [SerializeField] float fadeSpeed;

    bool CheckSameSprite(SpriteRenderer p_SpriteRenderer, Sprite p_Sprite)
    {
        if (p_SpriteRenderer.sprite == p_Sprite)
            return true;
        else
            return false;
    }

    public IEnumerator SpriteChangeCoroutine(Transform p_Target, string p_SpriteName)
    {
        SpriteRenderer[] t_SpriteRenderer = p_Target.GetComponentsInChildren<SpriteRenderer>(); // �迭�� ���� �ڽİ�ü�� �޸��(Rear) �������Ƿ�
        Sprite t_Sprite = Resources.Load("Characters/" + p_SpriteName, typeof(Sprite)) as Sprite;

        if (!CheckSameSprite(t_SpriteRenderer[0], t_Sprite)) // Ÿ��sprite �� �ٲ� sprite �� �����ʴٸ����.
        {
            Color t_color = t_SpriteRenderer[0].color;
            Color t_ShadowColor = t_SpriteRenderer[1].color;
            t_color.a = 0;
            t_ShadowColor.a = 0;
            t_SpriteRenderer[0].color = t_color; // ������������Ʈ�� �����ϰ�.
            t_SpriteRenderer[1].color = t_ShadowColor;

            t_SpriteRenderer[0].sprite = t_Sprite; // �ٲｺ������Ʈ �־��ְ�
            t_SpriteRenderer[1].sprite = t_Sprite;
             
            while (t_color.a < 1) // ���İ�õõ���÷���.
            {
                t_color.a += fadeSpeed;
                t_ShadowColor.a += fadeSpeed;
                t_SpriteRenderer[0].color = t_color;
                t_SpriteRenderer[1].color = t_ShadowColor;
                yield return null;
            }
        }

    }
}
