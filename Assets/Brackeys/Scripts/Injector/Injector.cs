using Reflex.Core;
using Reflex.Extensions;
using Reflex.Injectors;
using UnityEngine;

public static class Injector
{
    private static Container containerCache = null;

    private static UnityEngine.SceneManagement.Scene cachedScene = default;

    public static T Inject<T>(T instanceToInject)
        where T : MonoBehaviour
    {
        TryRecacheContainer(instanceToInject);

        GameObjectInjector.InjectObject(instanceToInject.gameObject, containerCache);
        return instanceToInject;
    }

    public static T InjectWithChildren<T>(T instanceToInject)
        where T : MonoBehaviour
    {
        TryRecacheContainer(instanceToInject);

        GameObjectInjector.InjectRecursive(instanceToInject.gameObject, containerCache);
        return instanceToInject;
    }

    public static T New<T>(T prefab, Transform parent = null)
        where T : MonoBehaviour
    {
        var instanceToInject = Object.Instantiate(prefab, parent);

        TryRecacheContainer(instanceToInject);

        InjectWithChildren(instanceToInject);
        return instanceToInject;
    }

    private static void TryRecacheContainer<T>(T instanceToInject)
        where T : MonoBehaviour
    {
        if (instanceToInject.gameObject.scene != cachedScene)
        {
            Recache(instanceToInject.gameObject);
        }
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void ResetInjector()
    {
        containerCache = null;
        cachedScene = default;
    }

    private static void Recache(GameObject gameObject)
    {
        cachedScene = gameObject.scene;
        containerCache = cachedScene.GetSceneContainer();
    }
}
