using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(
    fileName = "PlayerDialogueConfig",
    menuName = "Brackeys2025/Config/PlayerDialogueConfig"
)]
public class PlayerDialogueConfig : ScriptableObject
{
    [field: SerializeField]
    public List<PlayerDialogueChain> PlayerDialogueChains { get; private set; }

    [field: SerializeField]
    [field: Range(1, 200)]
    public float CharactersPerSecond { get; private set; } = 20f;

    [field: SerializeField]
    public Sprite DefaultIcon { get; private set; }

#if UNITY_EDITOR
    [EditorAttributes.Button]
    public void GatherAll()
    {
        var dialogueChains = AssetDatabase.FindAssetGUIDs("t:PlayerDialogueChain");

        PlayerDialogueChains.Clear();
        foreach (var dialogueChain in dialogueChains)
        {
            PlayerDialogueChains.Add(AssetDatabase.LoadAssetByGUID<PlayerDialogueChain>(dialogueChain));
        }
    }
#endif
}
