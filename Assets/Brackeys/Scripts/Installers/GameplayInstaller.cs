using Reflex.Core;
using UnityEngine;

public class GameplayInstaller : MonoBehaviour, IInstaller
{
    [SerializeField] private PlayerController playerController;

    [Space]
    [SerializeField] private TimerConfig timerConfig;

    public void InstallBindings(ContainerBuilder builder)
    {
        builder.AddSingleton<TimerManager>();
        builder.AddSingleton<BiscuitManager>();
        
        builder.AddSingleton(timerConfig);

        builder.AddSingleton(playerController);
    }
}
