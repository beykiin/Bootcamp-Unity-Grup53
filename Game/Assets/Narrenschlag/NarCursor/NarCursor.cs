using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Canvas))]
public class NarCursor : MonoBehaviour
{
    public RectTransform cursor_prefab;
    [HideInInspector] public RectTransform cursor;

    private void Update()
    {
        // Firstly...   Check if cursor is existing
        if (!cursor)
            return;

        // Then...      Position the cursor
        Position();

        // Finally...   Clamp the cursor to the screen
        Clamp();
    }

    private void CreateCursor()
    {
        if (!cursor_prefab)
            return;

        GameObject instance = GameObject.Instantiate(cursor_prefab.gameObject);
        RectTransform rt = instance.GetComponent<RectTransform>();

        rt.SetParent(transform);

        rt.rotation = new Quaternion();
        rt.localScale = Vector3.one;
        rt.position = Vector3.zero;

        cursor = rt;
    }

    private void Position()
    {
        if (isGamepad())
        {
            // If is Gamepad
        }
        else
        {
            cursor.position = Input.mousePosition;
        }
    }

    private bool isGamepad()
    {
        return false;
    }

    private void Clamp()
    {
        Vector2 size = new Vector2(Screen.width, Screen.height);
        Vector2 p = cursor.position;

        cursor.position = new Vector2(Mathf.Clamp(p.x, 0, size.x), Mathf.Clamp(p.y, 0, size.y));
    }

    #region singleton
    public static NarCursor singleton;
    private void Awake()
    {
        if(singleton)
        {
            this.enabled = false;

            return;
        }

        singleton = this;
        CreateCursor();
    }
    #endregion
}

public static class Nar_Cursor
{
    public static bool Contains(this RectTransform rt, Vector2 size, Vector2 p, bool debug = false)
    {
        Vector2 c = rt.position;

        float w = size.x / 2;
        float h = size.y / 2;

        bool x = p.x <= c.x + w && p.x >= c.x - w;
        bool y = p.y <= c.y + h && p.y >= c.y - h;

        if(debug)
        Debug.Log(p.x + " - " + c + ": " +x + ", " + y);

        return x && y;
    }
}