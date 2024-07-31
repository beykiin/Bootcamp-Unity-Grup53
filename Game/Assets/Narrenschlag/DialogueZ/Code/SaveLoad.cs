using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using System.IO;
using System;

using narrenschlag.extension;

namespace narrenschlag.dialoguez
{
    // too tired to write an documentation for here
    // if you want to get your own stuff in,
    // get how it works or ask me via twitter.com/narrenschlag

    public static class SaveLoad
    {
        public const string format = ".txt";
        public const string key = "_nar!";

        public static string GetBasePath(this DialogueZBase db)
        {
            string old = AssetDatabase.GetAssetPath(db);
            char[] array = old.ToCharArray();

            string to_replace = ".asset";
            char[] remove = to_replace.ToCharArray();

            return old.ToFixedLength(array.Length - remove.Length);
        }

        public static bool FileExists(this DialogueZBase db)
        {
            string path = GetBasePath(db);

            return File.Exists(path + format);
        }

        public static void SaveAsTxt(this DialogueZBase db)
        {
            string path = GetBasePath(db);
            string _ =
                  "##############################" +
                "\n#  Narrenschlag's DialogueZ  #" +
                "\n# Professional Dialogue Tool #" +
                "\n#         for  Unity         #" +
                "\n##############################" +
                "\n#" +
                "\n";

            int i = 0;
            string pre =
                "\n#" +
                "\n################" +
                "\n# Narrenschlag #" +
                "\n################" +
                "\n#" +
                "\n";
            foreach (DialogueZElement e in db.elements)
            {
                string s = "";

                s += (i > 0 ? pre : "") + ParameterType.id + key + e.idi + ":" + e.ids;
                s += "\n" + ParameterType.title + key + e.title;
                s += "\n" + ParameterType.message + key; s += "\n" + e.message;
                s += "\n" + ParameterType.auto_next + key + e.auto_next_idi;
                s += "\n" + ParameterType.color + key + e.color_type.r + ":" + e.color_type.g + ":" + e.color_type.b + ":" + e.color_type.a;
                s += "\n" + ParameterType.audio + key + (e.audio ? AssetDatabase.GetAssetPath(e.audio) : "");

                foreach (DialogueZFollow f in e.follows) s += "\n" + ParameterType.follow + key + f.next_idi + ":" + f.message.Replace("\n", " ") + ":" + f.requ;

                _ += s;
                i++;
            }

            File.WriteAllText(path + format, _);
        }

        /*
         * TO FIX
         * -> message detection
         * -> integration for follows
         */

        public static bool LoadFromTxt(this DialogueZBase db, out DialogueZElement[] new_db)
        {
            new_db = null;
            if (!FileExists(db)) return false;

            int i = 0;
            string path = GetBasePath(db) + format;
            string[] lines = File.ReadAllLines(path);

            List<DialogueZElement> elements = new List<DialogueZElement>();

            // Get the start line ids of the elements
            i = 0;
            List<int> element_starts = new List<int>();
            foreach (string _ in lines) { if (_.Contains(ParameterType.id + key)) element_starts.Add(i); i++; }

            // Get the elements
            i = 0;
            foreach (int start in element_starts)
            {
                bool is_last = start == element_starts[element_starts.Count - 1];
                int max_line = is_last ?
                    (lines.Length - 1) :
                    (element_starts[i + 1] - 1);

                // Get the lines of the element
                List<string> ls = new List<string>();
                for (int x = start; x < max_line + 1; x++) ls.Add(lines[x]);
                // Debug.Log(lines[start] + ": " + ls.Count);

                // New element
                DialogueZElement e = new DialogueZElement();

                // Get Ids and Idi
                foreach (string s in ls)
                {
                    string _1;
                    if (s.FirstLettersAre(ParameterType.id + key, out _1))
                    {
                        string[] _2 = _1.Split(new string[] { ":" }, StringSplitOptions.None);
                        e.idi = int.Parse(_2[0]);
                        e.ids = _2[1];

                        ls.Remove(s);
                        break;
                    }
                }

                // Get title
                foreach (string s in ls)
                {
                    string _1;
                    if (s.FirstLettersAre(ParameterType.title + key, out _1))
                    {
                        e.title = _1;

                        ls.Remove(s);
                        break;
                    }
                }

                // End of the message string
                int max = 0;
                // Get auto next
                foreach (string s in ls)
                {
                    string _1;
                    if (s.FirstLettersAre(ParameterType.auto_next + key, out _1))
                    {
                        e.auto_next_idi = int.Parse(_1);

                        ls.Remove(s);
                        break;
                    }

                    max++;
                }

                // Get list for which ints are obsolete now
                List<int> to_remove = new List<int>();

                // Get message
                int y = 1; // +1 because it starts without the first line at message
                foreach (string s in ls)
                {
                    string _1 = "";
                    if (s.FirstLettersAre(ParameterType.message + key, out _1))
                    {
                        for (int z = y; z < max; z++)
                        {
                            _1 += (z > y ? "\n" : "") + ls[z];
                            to_remove.Add(z);
                        }

                        e.message = _1;
                        break;
                    }

                    y++;
                }

                // Get Requirements
                List<DialogueZFollow> follows = new List<DialogueZFollow>();
                y = 0;
                foreach (string s in ls)
                {
                    string _1;
                    if (s.FirstLettersAre(ParameterType.follow + key, out _1))
                    {
                        string[] args = _1.Split(new char[] { ':' });
                        if (args.Length != 3) continue;

                        follows.Add(new DialogueZFollow()
                        {
                            next_idi = int.Parse(args[0]),
                            message = args[1],
                            requ = args[2]
                        });

                        if (!to_remove.Contains(y)) to_remove.Add(y);
                    }

                    y++;
                }
                e.follows = follows.ToArray();

                #region remove obsolete lines
                int off = 0;
                for (int x = 0; x < to_remove.Count; x++)
                {
                    int __i = to_remove[x] - off;
                    if (ls.Count > __i)
                    {
                        ls.Remove(ls[__i]);
                        off++;
                    }
                }
                #endregion

                // Get type_color
                foreach (string s in ls)
                {
                    string _1;
                    if (s.FirstLettersAre(ParameterType.color + key, out _1))
                    {
                        string[] args = _1.Split(new char[] { ':' });
                        if (args.Length != 4) continue;

                        // Debug.Log(args[0] + ", " + args[1] + ", " + args[2] + ", " + args[3]);

                        e.color_type = new Color()
                        {
                            r = float.Parse(args[0]),
                            g = float.Parse(args[1]),
                            b = float.Parse(args[2]),
                            a = float.Parse(args[3])
                        };

                        break;
                    }
                }

                // Get audio
                foreach (string s in ls)
                {
                    string _1;
                    if (s.FirstLettersAre(ParameterType.audio + key, out _1))
                    {
                        if (string.IsNullOrEmpty(_1) || !File.Exists(_1)) continue;

                        e.audio = AssetDatabase.LoadAssetAtPath(_1, typeof(AudioClip)) as AudioClip;

                        ls.Remove(s);
                        break;
                    }
                }

                elements.Add(e);
                i++;
            }

            new_db = elements.ToArray();
            return true;
        }

        public enum ParameterType { id, title, message, auto_next, follow, color, audio }
    }
}