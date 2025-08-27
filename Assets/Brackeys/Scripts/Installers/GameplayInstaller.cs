using Reflex.Core;
using UnityEngine;

public class GameplayInstaller : MonoBehaviour, IInstaller
{
    [SerializeField] PlayerController playerController;

    public void InstallBindings(ContainerBuilder builder)
    {
        builder.AddSingleton<BiscuitManager>();

        builder.AddSingleton(playerController);
    }
}
