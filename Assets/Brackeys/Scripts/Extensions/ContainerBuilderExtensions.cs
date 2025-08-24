using Reflex.Core;

public static class ContainerBuilderExtensions
{
    public static void AddSingleton<T>(this ContainerBuilder builder)
        where T : new()
    {
        builder.AddSingleton(typeof(T));
    }

    public static void AddSingleton<T>(this ContainerBuilder builder, T instance)
        where T : new()
    {
        builder.AddSingleton(instance, typeof(T));
    }

    public static void AddScoped<T>(this ContainerBuilder builder)
        where T : new()
    {
        builder.AddScoped(typeof(T));
    }
}
