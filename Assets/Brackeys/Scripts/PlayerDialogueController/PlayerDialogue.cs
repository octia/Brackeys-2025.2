using System;
using UnityEngine;

[Serializable]
public class PlayerDialogue
{
    [field: SerializeField]
    [field: Multiline]
    public string Message { get; private set; }

    [field: SerializeField]
    public Sprite IconOverride { get; private set; }
}
