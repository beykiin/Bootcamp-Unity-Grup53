using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;

using narrenschlag.extension;

namespace narrenschlag.dialoguez
{
    public class DialogueZStyle : MonoBehaviour
    {
        public string debug_start;
        [Range(0, 1f)]
        public float typing_speed;

        [Header("Uppercase Settings")]
        public bool message_always_up;
        public bool follow_always_up;

        [Header("User Interface")]
        public DialogueZFollowInstance fprefab;
        public RectTransform froot;
        [Space]
        public Text message;

        DialogueZ d;
        public virtual void Start()
        {
            // Get the DialogueZ singleton
            d = DialogueZ.singleton;

            // Error Log if no singleton
            if (!d) Debug.LogError("No DialogueZ Manager found!");

            // Set PlayerPref playername to "Max"
            PlayerPrefs.SetString("playername", "Max");

            // Return if no DialogueZ Manager
            if (!d) return;

            // Init debug dialogue of the string is not empty
            if (!string.IsNullOrEmpty(debug_start))
            {
                DialogueZElement e;
                if (DialogueZ.TryGetDialogue_Ids(debug_start, out e)) Init(e);
            }
            // Else Quit Dialogue
            else Quit();
        }

        public delegate void StyleCall(DialogueZStyle s, DialogueZElement e);
        public static StyleCall onFollowInit;
        public static StyleCall onInit;

        DialogueZElement cur;
        public virtual void Init(DialogueZElement e, DialogueZBase new_db = null)
        {
            // Call the destroy children of the follow root here
            // so there are no issues!
            foreach (Transform t in froot.GetChildren()) Destroy(t.gameObject);

            // Set new database if assigned
            if (new_db) DialogueZ.SetDatabase(new_db);

            // Return if no DialogueZ Manager or database
            if (!d || !d.db) return;

            // Quit if the assigned e == null
            if(e == null)
            {
                Quit();
                return;
            }

            // Enable
            CheckForRoot(true);

            //Enable/Disable Parts
            SetParts_Message(true);
            SetParts_Choice(false);

            // Set cur to assigned e
            cur = e;

            // Draw the message
            DrawMessage(e);
            // Play the audio
            if (e.audio) PlayAudio(e.audio);
            // Call onInit delegate
            if (onInit != null) onInit(this, e);
        }

        [HideInInspector] public bool next_quit;
        public virtual void DrawMessage(DialogueZElement e)
        {
            bool quit_mes = false;
            TypeCoroutine = StartCoroutine(Type(message, Modify(e.message, message_always_up, out quit_mes)));
            //message.text = Modify(e.message, out quit_mes);

            if (quit_mes) next_quit = true;
        }

        public virtual void Next()
        {
            if (!done_typing)
            {
                done_typing = true;
                return;
            }

            if (!d) return;
            if (!Root().gameObject.activeInHierarchy) return;
            if (cur == null) return;

            // Check for quit commands
            bool quit = next_quit;
            next_quit = false;
            if (quit)
            {
                Quit();
                return;
            }

            // Return if the choices are already activated
            if (froot.gameObject.activeInHierarchy) return;

            // If there is only one dialogue in the database or less, quit
            if (d.db.elements.Length < 2) Quit();
            else
            {
                // Check how many follows there are and if none use the auto next
                if (cur.follows != null && cur.follows.Length > 0)
                {
                    if (!InitFollows(cur)) InitAutoNext();
                }
                else InitAutoNext();
            }
        }

        public virtual void InitAutoNext()
        {
            DialogueZElement e;
            if (DialogueZ.TryGetDialogue_Idi(cur.auto_next_idi, out e)) Init(e);
        }

        public virtual bool InitFollows(DialogueZElement e)
        {
            SetParts_Message(false);
            SetParts_Choice(true);

            foreach (DialogueZFollow f in e.follows)
            {
                bool cannot = false;
                foreach (string args in f.requ.Replace(" ", "").ToLower().Split(new string[] { special_sep }, System.StringSplitOptions.RemoveEmptyEntries)) if (!RequirementMet(args)) cannot = true;
                if (cannot) continue;

                GameObject g = GameObject.Instantiate(fprefab.gameObject);
                RectTransform rt = g.GetComponent<RectTransform>();
                rt.SetParent(froot);

                rt.localRotation = Quaternion.identity;
                rt.localScale = Vector3.one;

                g.GetComponent<DialogueZFollowInstance>().Init(f, this);
            }

            if(onFollowInit != null) onFollowInit(this, e);
            return froot.childCount > 0;
        }

        public virtual void Quit()
        {
            // Disable
            CheckForRoot(false);
        }

        #region Enable / Disable parts
        public virtual void SetParts_Message(bool b)
        {
            message.gameObject.SetActive(b);
        }

        public virtual void SetParts_Choice(bool b)
        {
            froot.gameObject.SetActive(b);
        }
        #endregion

        #region root
        public void CheckForRoot(bool b = true)
        {
            RectTransform rt = Root();

            if (transform.childCount != 1)
            {
                Transform[] childs = transform.GetChildren();

                foreach (Transform c in childs)
                    if (c != transform) c.SetParent(rt);
            }

            rt.gameObject.SetActive(b);
        }

        RectTransform _root;
        public RectTransform Root()
        {
            if (_root)
                return _root;

            RectTransform t = GetComponent<RectTransform>();
            GameObject g = new GameObject();
            g.name = this.name + "_root";

            RectTransform rt = g.AddComponent<RectTransform>();
            rt.SetParent(transform);
            rt.localRotation = Quaternion.identity;
            rt.localPosition = Vector3.zero;
            rt.localScale = Vector3.one;

            rt.anchorMin = t.anchorMin;
            rt.anchorMax = t.anchorMax;
            rt.offsetMin = t.offsetMin;
            rt.offsetMax = t.offsetMax;

            _root = rt;
            return rt;
        }
        #endregion

        #region typing
        public Coroutine TypeCoroutine;
        [HideInInspector] public bool done_typing;
        public virtual IEnumerator Type(Text t, string s)
        {
            StopType();

            char[] array = s.ToCharArray();
            done_typing = false;
            // Set the text to the target string depending on the typing speed
            t.text = typing_speed <= 0 ? s : "";

            foreach (char c in array)
            {
                if (t.text == s) done_typing = true;

                // Return if done_typing is modified outside
                if (done_typing)
                {
                    t.text = s;
                    yield break;
                }

                yield return new WaitForSeconds(typing_speed);
                t.text += c;
            }

            done_typing = true;
        }

        public void StopType()
        {
            if (TypeCoroutine == null) return;

            StopCoroutine(TypeCoroutine);
            TypeCoroutine = null;
        }
        #endregion

        #region commands and requirements
        public static string special_sep = "//";
        public static char argument_sep = ',';

        public string Modify(string inp, bool uppercase, out bool con_quit, bool call_commands = true)
        {
            string s = "";

            con_quit = false;

            string[] arguments = inp.Split(new string[] { special_sep }, System.StringSplitOptions.None);
            for (int i = 0; i < arguments.Length; i++)
                if (i % 2 == 0) s += arguments[i];
                else if(call_commands)
                {
                    string args = arguments[i].Replace(" ", "").ToLower();

                    #region Check for special functions
                    switch (args)
                    {
                        case "quit":
                            con_quit = true;
                            break;

                        default:
                            s += CheckForCommand(args).result;
                            break;
                    }
                    #endregion
                }

            return uppercase ? s.ToUpper() : s;
        }

        public virtual Command CheckForCommand(string arguments)
        {
            Command c = new Command() { result = "" };
            c.arguments = arguments.Split(new char[] { argument_sep });

            if (c.arguments.Length < 2) return c;

            // Arguments without the first argument
            // because the first is used for the switch
            string[] _a = c.arguments.Remove(c.arguments[0]);

            switch (c.arguments[0])
            {
                case "call":
                    c_Call(_a);
                    break;

                case "get":
                    c.result = c_Get(_a);
                    break;

                default:
                    break;
            }

            // Debug.Log("Command: " + arguments + " -> " + c.result);
            return c;
        }

        #region explicit calls
        // "c_" stands for command 
        // -> just so I know that it is explicitly for the command functions
        void c_Call(string[] args)
        {
            foreach (string arg in args)
            {
                int i;
                Event e;
                // if (int.TryParse(arg, out i))
                if (DialogueZ.TryGetEvent(arg, out e))
                    if (e.onCast != null) e.onCast.Invoke();
            }
        }

        string c_Get(string[] args)
        {
            string s = "";

            for (int i = 0; i < args.Length; i++)
            {
                s += ReadData(args[i]);

                // Add seperating space for multiple get arguments
                if (args.Length > 1 && i < args.Length - 1) s += " ";
            }

            return s;
        }
        #endregion

        public static string custom_first = "*";
        public virtual bool RequirementMet(string s)
        {
            string[] args = s.Split(new char[] { argument_sep }, System.StringSplitOptions.RemoveEmptyEntries);
            if (args.Length != 3) return true;

            #region Get the 2 values
            string v1 = args[0];
            bool v1_custom = v1.FirstLettersAre(custom_first, out v1);
            if (v1_custom) v1 = ReadData(v1);

            string v2 = args[2];
            bool v2_custom = v2.FirstLettersAre(custom_first, out v2);
            if (v2_custom) v2 = ReadData(v2);

            float i1 = 0;
            float i2 = 0;
            bool both_float = float.TryParse(v1, out i1) && float.TryParse(v2, out i2);
            #endregion

            bool b = true;
            switch (args[1])
            {
                case "!=":
                    if (v1 == v2)
                        b = false;
                    break;

                #region ==
                case "=":
                    if (v1 != v2)
                        b = false;
                    break;

                case "==":
                    if (v1 != v2)
                        b = false;
                    break;
                #endregion

                case "<":
                    if (both_float)
                        if (i1 >= i2) b = false;
                    break;

                case "<=":
                    if (both_float)
                        if (i1 > i2) b = false;
                    break;

                case ">":
                    if (both_float)
                        if (i1 <= i2) b = false;
                    break;

                case ">=":
                    if (both_float)
                        if (i1 < i2) b = false;
                    break;

                default:
                    break;
            }

            // Debug.Log("Require: " + v1 + args[1] + v2 + " -> " + b);
            return b;
        }
        #endregion

        #region get data
        public virtual string ReadData(string id)
        {
            string result = PlayerPrefs.GetString(id);
            // Debug.Log("Read: " + id + " -> " + result);
            return result;
        }
        #endregion

        #region play audio
        AudioSource audio_instance;
        public void PlayAudio(AudioClip clip)
        {
            StopAudio();
            if (!clip) return;

            GameObject g = new GameObject("Audio Clip - " + this);
            audio_instance = g.AddComponent<AudioSource>();
            audio_instance.PlayOneShot(clip);

            // Destroy the audio source after audioclip.length + .1f (puffering)
            Destroy(g, clip.length + .1f);
        }

        public void StopAudio()
        {
            if (audio_instance) Destroy(audio_instance.gameObject);
        }
        #endregion

        #region singleton
        public static DialogueZStyle singleton;
        private void Awake()
        {
            if (!this.enabled) return;

            if (!singleton) singleton = this;
            else
            {
                gameObject.SetActive(false);
            }
        }
        #endregion
    }

    [System.Serializable]
    public class Command
    {
        public string[] arguments;
        public string result;
    }
}