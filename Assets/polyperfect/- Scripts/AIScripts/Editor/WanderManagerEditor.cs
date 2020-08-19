using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace PolyPerfect
{
    [CustomEditor(typeof(WanderManager))]
    public class WanderManagerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            //Load a Texture (Assets/Resources/Textures/texture01.png)
            var mainTexture = Resources.Load<Texture2D>("ManagerLogo");
            GUILayout.BeginHorizontal();
            GUILayout.Label(mainTexture);
            GUILayout.EndHorizontal();

            WanderManager animalManager = (WanderManager)target;

            if (!Application.isPlaying)
            {
                base.OnInspectorGUI();
                return;
            }

            GUILayout.Space(10);

            animalManager.PeaceTime = EditorGUILayout.Toggle("Peace Time", animalManager.PeaceTime);

            GUILayout.Space(5);

            if (GUILayout.Button("Nuke Animals"))
            {
                animalManager.Nuke();
            }
        }
    }
}