using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class NarCursorButton : MonoBehaviour
{
    public bool debug;
    NarCursor cursor;

    RectTransform rt;
    RectTransform c;
    Button b;

    Vector2 size;

    IEnumerator Start()
    {
        yield return new WaitForEndOfFrame();
        cursor = NarCursor.singleton;

        rt = GetComponent<RectTransform>();
        b = GetComponent<Button>();
        c = cursor.cursor;

        size = rt.rect.size;
    }

    void Update()
    {
        if (!c || !b)
            return;

        bool inp = Input.GetMouseButtonDown(1);

        bool contains = rt.Contains(size, c.transform.position, debug);
        // b.interactable = contains;

        if (inp && b.onClick != null)
        {
            if (contains && b.interactable && b.onClick != null)
                    b.onClick.Invoke();
        }
    }
}
