using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[CustomPropertyDrawer(typeof(UintBytesAttribute))]
public class UintBytesDrawer : PropertyDrawer {
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        UintBytesAttribute _attribute = attribute as UintBytesAttribute;
        EditorGUI.BeginProperty(position, label, property);
        
        uint val = (uint)property.longValue;
        byte b1 = (byte)(val >> 24);
        byte b2 = (byte)(val >> 16);
        byte b3 = (byte)(val >> 8);
        byte b4 = (byte)val;

        position = EditorGUI.PrefixLabel(position, label);
        Rect pos = position;
        float width = pos.width / 4f;
        
        if (!string.IsNullOrEmpty(_attribute.Name1) || !string.IsNullOrEmpty(_attribute.Name2) ||
            !string.IsNullOrEmpty(_attribute.Name3) || !string.IsNullOrEmpty(_attribute.Name4))
        {
            EditorGUI.LabelField(new Rect(pos.x, pos.y,  width, EditorGUIUtility.singleLineHeight), _attribute.Name1);   
            EditorGUI.LabelField(new Rect(pos.x+width, pos.y,  width, EditorGUIUtility.singleLineHeight), _attribute.Name2);   
            EditorGUI.LabelField(new Rect(pos.x+width*2, pos.y,  width, EditorGUIUtility.singleLineHeight), _attribute.Name3);   
            EditorGUI.LabelField(new Rect(pos.x+width*3, pos.y,  width, EditorGUIUtility.singleLineHeight), _attribute.Name4);
            pos.position = new Vector2(pos.position.x, pos.position.y+EditorGUIUtility.singleLineHeight);
        }
        
        
        Rect r1 = new Rect(pos.x, pos.y, width, EditorGUIUtility.singleLineHeight);
        Rect r2 = new Rect(pos.x + width, pos.y, width, EditorGUIUtility.singleLineHeight);
        Rect r3 = new Rect(pos.x + width * 2, pos.y, width, EditorGUIUtility.singleLineHeight);
        Rect r4 = new Rect(pos.x + width * 3, pos.y, width, EditorGUIUtility.singleLineHeight);
        
        b1 = (byte)EditorGUI.IntField(r1, b1);
        b2 = (byte)EditorGUI.IntField(r2, b2);
        b3 = (byte)EditorGUI.IntField(r3, b3);
        b4 = (byte)EditorGUI.IntField(r4, b4);
        
        property.isExpanded = EditorGUI.Foldout(new Rect(pos.x, pos.y, width, EditorGUIUtility.singleLineHeight), property.isExpanded, GUIContent.none);
        if (property.isExpanded)
        {
            for (int i = 0; i < 8; i++)
            {
                if (EditorGUI.Toggle(
                        new Rect(pos.x, pos.y + EditorGUIUtility.singleLineHeight * (i + 1), width, EditorGUIUtility.singleLineHeight),
                        (b1 & (1 << i)) != 0)) b1 = (byte)(b1 | (1 << i));
                else b1 = (byte)(b1 & ~(1 << i));
            }

            for (int i = 0; i < 8; i++)
            {
                if (EditorGUI.Toggle(
                        new Rect(pos.x + width, pos.y + EditorGUIUtility.singleLineHeight * (i + 1), width,
                            EditorGUIUtility.singleLineHeight), (b2 & (1 << i)) != 0)) b2 = (byte)(b2 | (1 << i));
                else b2 = (byte)(b2 & ~(1 << i));
            }

            for (int i = 0; i < 8; i++)
            {
                if (EditorGUI.Toggle(
                        new Rect(pos.x + width * 2, pos.y + EditorGUIUtility.singleLineHeight * (i + 1), width,
                            EditorGUIUtility.singleLineHeight), (b3 & (1 << i)) != 0)) b3 = (byte)(b3 | (1 << i));
                else b3 = (byte)(b3 & ~(1 << i));
            }

            for (int i = 0; i < 8; i++)
            {
                if (EditorGUI.Toggle(
                        new Rect(pos.x + width * 3, pos.y + EditorGUIUtility.singleLineHeight * (i + 1), width,
                            EditorGUIUtility.singleLineHeight), (b4 & (1 << i)) != 0)) b4 = (byte)(b4 | (1 << i));
                else b4 = (byte)(b4 & ~(1 << i));
            }
        }

        property.longValue = (uint)((b1 << 24) | (b2 << 16) | (b3 << 8) | b4);
        
        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        UintBytesAttribute _attribute = attribute as UintBytesAttribute;
        int result = 1;
        if (!string.IsNullOrEmpty(_attribute.Name1) || !string.IsNullOrEmpty(_attribute.Name2) ||
            !string.IsNullOrEmpty(_attribute.Name3) || !string.IsNullOrEmpty(_attribute.Name4)) result += 1;
    
        if(property.isExpanded) result += 8;
        return EditorGUIUtility.singleLineHeight*result;
    }
}