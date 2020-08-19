using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace PolyPerfect
{

    public class StatsTable : EditorWindow
    {
        static List<AIStats> stats = new List<AIStats>();
        static public Dictionary<AIStats, int> selectionValue = new Dictionary<AIStats, int>();

        static string pathFolder = "";
        bool toggleAnimalName = true;
        bool toggleDominance = true;
        bool toggleAgression = true;
        bool toggleAttackSpeed = true;
        bool togglePower = true;
        bool toggleStamina = true;
        bool toggleStealthy = true;
        bool toggleToughness = true;
        bool toggleTeritorial = true;


        GUIStyle folderStyle = new GUIStyle();
        GUIStyle Explanation = new GUIStyle();
        GUIStyle toggleField = new GUIStyle();


        // Add menu named "My Window" to the Window menu
        [MenuItem("PolyPerfect/Stats Table")]
        static void Init()
        {
            // Get existing open window or if none, make a new one:
            StatsTable window = (StatsTable)EditorWindow.GetWindow(typeof(StatsTable));
            window.Show();

            //If the window has been open before then clear the stats list and make a new one
            SortLists();
        }

        static void SortLists()
        {
            //Find all the animal stats in the project
            var animalStats = (AIStats[])Resources.FindObjectsOfTypeAll(typeof(AIStats));

            //If the window has been open before then clear the stats list and make a new one
            stats.Clear();
            selectionValue.Clear();

            foreach (var item in animalStats)
            {
                //Debug.Log(item.name);

                selectionValue.Add(item, -1);
                stats.Add(item);
            }
        }

        void OnDestroy()
        {
            stats.Clear();
            selectionValue.Clear();
        }

        void CreateNewAnimalStats()
        {
            var animalStats = (AIStats[])Resources.FindObjectsOfTypeAll(typeof(AIStats));

            AIStats newAnimalStats = ScriptableObject.CreateInstance<AIStats>();

            if (AssetDatabase.GetMainAssetTypeAtPath(pathFolder + "/New Animal Stats.asset") != null)
            {
                AssetDatabase.CreateAsset(newAnimalStats, pathFolder + "/New Animal Stats" + animalStats.Length.ToString() + ".asset");
            }

            else
            {
                AssetDatabase.CreateAsset(newAnimalStats, pathFolder + "/New Animal Stats.asset");
            }
        }


        void OnGUI()
        {
            folderStyle.normal.textColor = Color.black;
            Explanation.alignment = TextAnchor.MiddleCenter;

            pathFolder = "Assets";

            //Get the stats logo
            var mainTexture = Resources.Load<Texture2D>("StatsLogo");

            //Main Image    
            GUILayout.BeginHorizontal();
            GUILayout.Label(mainTexture);
            GUILayout.EndHorizontal();



            GUILayout.Label("See a side by side comparison of the animal stats, use the boxes below to re order the list into the highest value of the category", Explanation);
            var filters = new List<string>();

            filters.Add("Animal Name");
            filters.Add("Dominance");
            filters.Add("Agression");
            filters.Add("AttackSpeed");
            filters.Add("Power");
            filters.Add("Stamina");
            filters.Add("Stealthy");
            filters.Add("Toughness");
            filters.Add("territorial");

            var buttonSize = (position.width / 9.5f);
            GUILayout.Space(20f);

            GUILayout.BeginHorizontal();

            if (GUILayout.Button("Animal Name", GUILayout.Width(buttonSize)))
            {
                //Re order by the animals name
                ReOrderFloatList(0);
            }


            if (GUILayout.Button("Dominance", GUILayout.Width(buttonSize)))
            {
                ReOrderFloatList(1);
            }


            if (GUILayout.Button("Agression", GUILayout.Width(buttonSize)))
            {
                ReOrderFloatList(2);
            }

            if (GUILayout.Button("AttackSpeed", GUILayout.Width(buttonSize)))
            {
                ReOrderFloatList(3);
            }

            if (GUILayout.Button("Power", GUILayout.Width(buttonSize)))
            {
                ReOrderFloatList(4);
            }

            if (GUILayout.Button("Stamina", GUILayout.Width(buttonSize)))
            {
                ReOrderFloatList(5);
            }

            if (GUILayout.Button("Stealthy", GUILayout.Width(buttonSize)))
            {
                ReOrderFloatList(6);
            }

            if (GUILayout.Button("Toughness", GUILayout.Width(buttonSize)))
            {
                ReOrderFloatList(7);
            }


            if (GUILayout.Button("territorial", GUILayout.Width(buttonSize)))
            {
                ReOrderFloatList(8);
            }


            GUILayout.EndHorizontal();

            GUILayout.Space(20f);

            foreach (var item in stats)
            {
                BuildWindow(item);
            }

            if (GUILayout.Button("Add New Stats"))
            {
                if (AssetDatabase.IsValidFolder(pathFolder))
                {
                    CreateNewAnimalStats();
                }

                else
                {
                    Debug.Log("Please enter a valid path");
                }

                SortLists();
            }


            GUILayout.Space(20f);
        }

        void BuildWindow(AIStats animalStats)
        {
            if (animalStats == null)
            {
                stats.Remove(animalStats);
            }

            toggleField.alignment = TextAnchor.MiddleCenter;

            Repaint();

            if (selectionValue.ContainsKey(animalStats))
            {
                GUILayout.BeginHorizontal();

                var previousName = animalStats.name;

                var newName = GUILayout.TextField(animalStats.name, GUILayout.Width(position.width / 8.5f));

                animalStats.name = newName;

                if (previousName != animalStats.name)
                {
                    AssetDatabase.RenameAsset(AssetDatabase.GetAssetPath(animalStats), animalStats.name);
                    stats.Clear();
                    SortLists();
                }

                animalStats.dominance = int.Parse(GUILayout.TextField(animalStats.dominance.ToString(), GUILayout.Width(position.width / 9)));
                animalStats.agression = Mathf.Clamp(float.Parse(GUILayout.TextField(animalStats.agression.ToString(), GUILayout.Width(position.width / 9))), 0, 99);
                animalStats.attackSpeed = float.Parse(GUILayout.TextField(animalStats.attackSpeed.ToString(), GUILayout.Width(position.width / 9)));
                animalStats.power = float.Parse(GUILayout.TextField(animalStats.power.ToString(), GUILayout.Width(position.width / 9)));
                animalStats.stamina = float.Parse(GUILayout.TextField(animalStats.stamina.ToString(), GUILayout.Width(position.width / 9)));
                animalStats.stealthy = GUILayout.Toggle(animalStats.stealthy, animalStats.stealthy.ToString(), GUILayout.Width(position.width / 9));
                animalStats.toughness = float.Parse(GUILayout.TextField(animalStats.toughness.ToString(), GUILayout.Width(position.width / 9)));
                animalStats.territorial = GUILayout.Toggle(animalStats.territorial, animalStats.territorial.ToString(), toggleField, GUILayout.Width(position.width / 9));
                GUILayout.EndHorizontal();



            }
        }

        void ReOrderFloatList(int filterID)
        {
            switch (filterID)
            {
                case 0:

                    if (toggleAnimalName)
                    {
                        stats = (stats.OrderBy(p => p.name).Reverse()).ToList();
                        toggleAnimalName = !toggleAnimalName;
                    }

                    else
                    {
                        stats = stats.OrderBy(p => p.name).ToList();
                        toggleAnimalName = !toggleAnimalName;
                    }

                    break;

                case 1:
                    if (toggleDominance)
                    {
                        stats = (stats.OrderBy(p => p.dominance).Reverse()).ToList();
                        toggleDominance = !toggleDominance;
                    }

                    else
                    {
                        stats = stats.OrderBy(p => p.dominance).ToList();
                        toggleDominance = !toggleDominance;
                    }
                    break;

                case 2:

                    if (toggleAgression)
                    {
                        stats = (stats.OrderBy(p => p.agression).Reverse()).ToList();
                        toggleAgression = !toggleAgression;
                    }

                    else
                    {
                        stats = stats.OrderBy(p => p.agression).ToList();
                        toggleAgression = !toggleAgression;
                    }

                    break;

                case 3:

                    if (toggleAttackSpeed)
                    {
                        stats = (stats.OrderBy(p => p.attackSpeed).Reverse()).ToList();
                        toggleAttackSpeed = !toggleAttackSpeed;
                    }

                    else
                    {
                        stats = stats.OrderBy(p => p.attackSpeed).ToList();
                        toggleAttackSpeed = !toggleAttackSpeed;
                    }

                    break;

                case 4:

                    if (togglePower)
                    {
                        stats = (stats.OrderBy(p => p.power).Reverse()).ToList();
                        togglePower = !togglePower;
                    }

                    else
                    {
                        stats = stats.OrderBy(p => p.power).ToList();
                        togglePower = !togglePower;
                    }

                    break;

                case 5:

                    if (toggleStamina)
                    {
                        stats = (stats.OrderBy(p => p.stamina).Reverse()).ToList();
                        toggleStamina = !toggleStamina;
                    }

                    else
                    {
                        stats = stats.OrderBy(p => p.stamina).ToList();
                        toggleStamina = !toggleStamina;
                    }

                    break;

                case 6:

                    if (toggleStealthy)
                    {
                        stats = (stats.OrderBy(p => p.stealthy).Reverse()).ToList();
                        toggleStealthy = !toggleStealthy;
                    }

                    else
                    {
                        stats = stats.OrderBy(p => p.stealthy).ToList();
                        toggleStealthy = !toggleStealthy;
                    }

                    break;

                case 7:

                    if (toggleToughness)
                    {
                        stats = (stats.OrderBy(p => p.toughness).Reverse()).ToList();
                        toggleToughness = !toggleToughness;
                    }

                    else
                    {
                        stats = stats.OrderBy(p => p.toughness).ToList();
                        toggleToughness = !toggleToughness;
                    }

                    break;

                case 8:

                    if (toggleTeritorial)
                    {
                        stats = (stats.OrderBy(p => p.territorial).Reverse()).ToList();
                        toggleTeritorial = !toggleTeritorial;
                    }

                    else
                    {
                        stats = stats.OrderBy(p => p.territorial).ToList();
                        toggleTeritorial = !toggleTeritorial;
                    }

                    break;
            }
        }
    }
}