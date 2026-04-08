using UnityEngine;

public class UintBytesAttribute : PropertyAttribute
{
    public string Name1;
    public string Name2;
    public string Name3;
    public string Name4;

    public UintBytesAttribute()
    { }

    public UintBytesAttribute(string name1, string name2, string name3, string name4)
    {
        Name1 = name1;
        Name2 = name2;
        Name3 = name3;
        Name4 = name4;
    }
}