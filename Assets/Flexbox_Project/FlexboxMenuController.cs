using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

public class FlexboxMenuController : EditorWindow
{
    VisualElement container;
    [MenuItem("Testing/Test Window")]
    public static void ShowWindow()
    {
        FlexboxMenuController window = GetWindow<FlexboxMenuController>();
        window.titleContent = new GUIContent("Window");
        window.minSize = new Vector2(500, 500);
    }

    public void CreateGUI()
    {
        container = rootVisualElement; // Creating container

        VisualTreeAsset visualTreeAsset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Flexbox_Project/Flexbox.uxml");
        container.Add(visualTreeAsset.Instantiate()); // Adding uxml template

        StyleSheet styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Flexbox_Project/flexbox_styles.uss");
        container.styleSheets.Add(styleSheet); // Adding uss styles
    }
}
