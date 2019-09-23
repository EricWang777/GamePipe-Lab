using UnityEngine;
using System.Collections;

public class BeatingDemoControls : MonoBehaviour
{
    public IconProgressBar healthBar, subtleHealthBar;

    void OnGUI()
    {
        GUILayout.BeginVertical(GUILayout.Width(250));
        GUILayout.Space(25);

        GUILayout.BeginHorizontal();
        GUILayout.Space(25);

        GUILayout.Label("Current Value");
        healthBar.currentValue = GUILayout.HorizontalSlider(healthBar.currentValue, 0, healthBar.maxValue);
        subtleHealthBar.currentValue = healthBar.currentValue;


        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Space(25);
        GUILayout.Label(healthBar.currentValue.ToString());

        GUILayout.EndHorizontal();

        GUILayout.Space(10);

        GUILayout.BeginHorizontal();
        GUILayout.Space(25);

        GUILayout.Label("Icon Count");
        healthBar.MaxIcons = (int)GUILayout.HorizontalSlider(healthBar.MaxIcons, 5, 10);
        healthBar.maxValue = healthBar.MaxIcons;
        subtleHealthBar.MaxIcons = healthBar.MaxIcons;
        subtleHealthBar.maxValue = healthBar.MaxIcons;

        GUILayout.EndHorizontal();


        GUILayout.EndVertical();
    }
}
