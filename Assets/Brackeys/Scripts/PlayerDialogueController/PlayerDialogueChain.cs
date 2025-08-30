using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(
    fileName = "PlayerDialogueChain",
    menuName = "Brackeys2025/Config/PlayerDialogueChain"
)]
public class PlayerDialogueChain : ScriptableObject
{
    [field: SerializeField]
    public bool ShouldBlockInteraction { get; private set; } = false;

    [field: SerializeField]
    public bool ShouldBeSkippable { get; private set; } = true;

    [field: SerializeField]
    public PlayerDialogueChainType ChainType { get; private set; }

    [field: SerializeField]
    public List<PlayerDialogue> Dialogues { get; private set; }
}
