using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System.Linq;

using UnityEditor;

namespace narrenschlag.extension
{
    #region Normal Functions
    public static class F   // Functions
    {
        //
        // To-List for everything that is an array
        public static List<TSource> ToList<TSource>(this IEnumerable<TSource> source)
        {
            List<TSource> list = new List<TSource>();
            foreach (var i in source)
                list.Add(i);
            return list;
        }

        //
        // Add for everything that is an array
        public static TSource[] Add<TSource>(this IEnumerable<TSource> source, TSource add)
        {
            List<TSource> list = source.ToList();
            list.Add(add);

            return list.ToArray();
        }

        //
        // Remove for everything that is an array
        public static TSource[] Remove<TSource>(this IEnumerable<TSource> source, TSource rem)
        {
            List<TSource> list = source.ToList();
            list.Remove(rem);

            return list.ToArray();
        }

        public static Transform[] GetChildren(this Transform t)
        {
            List<Transform> list = new List<Transform>();
            for (int i = 0; i < t.childCount; i++) list.Add(t.GetChild(i));
            return list.ToArray();
        }

        public static bool FirstLettersAre(this string s, string first_letters, out string new_s)
        {
            string s1 = first_letters.Trim();

            string rest;
            char[] first_array = s1.ToArray();
            string first = s.GetFirstLetters(first_array.Length, out rest);

            bool b = s1 == first.Trim();
            new_s = b ? rest : s;
            return b;
        }

        public static string GetFirstLetters(this string s, int count, out string rest)
        {
            char[] cur = s.ToCharArray();
            count = Mathf.Clamp(count, 0, cur.Length);
            rest = "";

            char[] array = new char[count];
            for (int i = 0; i < cur.Length; i++)
                if (i < count) array[i] = cur[i];
                else rest += cur[i];

            return array.BackToString();
        }

        public static string BackToString(this char[] chars)
        {
            string s = "";

            foreach (char c in chars) s += c;

            return s;
        }

        public static string ToFixedLength(this string s, int i)
        {
            char[] chars = s.ToCharArray();
            string _ = "";

            for (int x = 0; x < i; x++)
                if (x < chars.Length) _ += chars[x];
                else _ += " ";
            // Debug.Log(s+ " -> " + chars.Length + ": " + _);

            return _;
        }

        public static Vector2 GetChildSize(this RectTransform rt, out int count, bool mustbeactive = true)
        {
            Vector2 v2 = new Vector2();
            foreach (RectTransform r in rt)
                if (mustbeactive) { if (r.gameObject.activeInHierarchy) v2 += r.sizeDelta; }
                else v2 += r.sizeDelta;

            count = rt.childCount;
            return v2;
        }

        public static bool ToBool(this int i) { return i > 0 ? true : false; }
        public static int ToInt(this bool b) { return b ? 1 : 0; }
    }
    #endregion

    #region Editor Functions
    public static class Fe  // Functions for the Editor
    {
        #region basic
        public static void h_begin() { GUILayout.BeginHorizontal(); }
        public static void h_end() { GUILayout.EndHorizontal(); }

        public static void v_begin() { GUILayout.BeginVertical(); }
        public static void v_end() { GUILayout.EndVertical(); }

        public static void space(float f) { GUILayout.Space(f); }
        #endregion

        public static void section_begin(float f = 10f)
        {
            h_begin();
            space(f);
            v_begin();
        }

        public static void section_end()
        {
            v_end();
            h_end();
        }

        public static bool DrawButton(string s, Color c, Color old, float width = -1f)
        {
            GUI.backgroundColor = c;

            bool b = false;
            if (width >= 0)
                b = GUILayout.Button(s, GUILayout.Width(width));
            else
                b = GUILayout.Button(s);

            GUI.backgroundColor = old;

            return b;
        }

        public static Color GetButtonColor(this Color c) { return Color.white + (c + new Color(0, 0, 0, -.25f)); }
    }
    #endregion
}