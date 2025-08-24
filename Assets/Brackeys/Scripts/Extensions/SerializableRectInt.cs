using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

/// <summary>
/// Variant of RectInt that supports custom de-/serialization to use as a key in a dictionary.
/// </summary>
[Serializable]
public struct SerializableRectInt : IEquatable<SerializableRectInt>
{
    private struct CodingKeys
    {
        public static string xKey = "x";
        public static string yKey = "y";
        public static string widthKey = "width";
        public static string heightKey = "height";
    }

    [JsonProperty]
    public int x;

    [JsonProperty]
    public int y;

    [JsonProperty]
    public int width;

    [JsonProperty]
    public int height;

    [JsonIgnore]
    public readonly RectInt AsRectInt => new(x, y, width, height);

    [JsonConstructor]
    public SerializableRectInt(string encoded)
    {
        var jsonObject = JObject.Parse(encoded);

        x = (int)jsonObject[CodingKeys.xKey];
        y = (int)jsonObject[CodingKeys.yKey];
        width = (int)jsonObject[CodingKeys.widthKey];
        height = (int)jsonObject[CodingKeys.heightKey];
    }

    public SerializableRectInt(RectInt rectInt)
    {
        x = rectInt.x;
        y = rectInt.y;
        width = rectInt.width;
        height = rectInt.height;
    }

    public static implicit operator RectInt(SerializableRectInt serializableRect)
    {
        return serializableRect.AsRectInt;
    }

    public static implicit operator SerializableRectInt(RectInt rectInt)
    {
        return new SerializableRectInt(rectInt);
    }

    public override bool Equals(object obj)
    {
        return obj is SerializableRectInt other && Equals(other);
    }

    public bool Equals(SerializableRectInt other)
    {
        return x == other.x && y == other.y && width == other.width && height == other.height;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(x, y, width, height);
    }

    public static bool operator ==(SerializableRectInt a, SerializableRectInt b)
    {
        return a.Equals(b);
    }

    public static bool operator !=(SerializableRectInt a, SerializableRectInt b)
    {
        return !a.Equals(b);
    }

    public override string ToString()
    {
        return JsonConvert.SerializeObject(this);
    }
}
