using Reflex.Core;
using UnityEngine;

public class PlayerDialogueInstaller : MonoBehaviour, IInstaller
{
    [SerializeField]
    private PlayerDialogueConfig config;

    [SerializeField]
    private PlayerDialogueController controller;

    public void InstallBindings(ContainerBuilder containerBuilder)
    {
        containerBuilder.AddSingleton(controller);
        containerBuilder.AddSingleton(config);
    }
}
