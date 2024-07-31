using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace narrenschlag.dialoguez
{
    public class DialogueZStyle_Nntndo : DialogueZStyle
    {
        public override void SetParts_Message(bool b)
        {
            // Never disable the message components
            base.SetParts_Message(true);
        }
    }
}