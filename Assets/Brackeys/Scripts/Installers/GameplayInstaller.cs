using Reflex.Core;
using UnityEngine;

public class GameplayInstaller : MonoBehaviour, IInstaller
{
    [SerializeField] private PlayerController playerController;

    [Space]
    [SerializeField] private BiscuitManagerConfig biscuitManagerConfig;
    [SerializeField] private TimerManagerConfig timerManagerConfig;

    public void InstallBindings(ContainerBuilder builder)
    {
        builder.AddSingleton<TimerManager>();
        builder.AddSingleton<BiscuitManager>();
        
        builder.AddSingleton(timerManagerConfig);
        builder.AddSingleton(biscuitManagerConfig);

        builder.AddSingleton(playerController);
    }
}
