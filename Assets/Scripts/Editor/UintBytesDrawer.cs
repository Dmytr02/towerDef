using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(UintBytesAttribute))]
public class UintBytesDrawer : PropertyDrawer {
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        EditorGUI.BeginProperty(position, label, property);
        
        // Pobieramy aktualną wartość jako uint
        uint val = (uint)property.longValue;
        byte b1 = (byte)(val >> 24);
        byte b2 = (byte)(val >> 16);
        byte b3 = (byte)(val >> 8);
        byte b4 = (byte)val;

        // Rysujemy etykietę i 4 pola obok siebie
        position = EditorGUI.PrefixLabel(position, label);
        float width = position.width / 4f;
        
        Rect r1 = new Rect(position.x, position.y, width, position.height);
        Rect r2 = new Rect(position.x + width, position.y, width, position.height);
        Rect r3 = new Rect(position.x + width * 2, position.y, width, position.height);
        Rect r4 = new Rect(position.x + width * 3, position.y, width, position.height);

        b1 = (byte)EditorGUI.IntField(r1, b1);
        b2 = (byte)EditorGUI.IntField(r2, b2);
        b3 = (byte)EditorGUI.IntField(r3, b3);
        b4 = (byte)EditorGUI.IntField(r4, b4);

        // Zapisujemy nową wartość
        property.longValue = (uint)((b1 << 24) | (b2 << 16) | (b3 << 8) | b4);
        
        EditorGUI.EndProperty();
    }
}