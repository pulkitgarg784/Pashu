using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(Group))]

public class PowerTools : Editor
{
    [MenuItem("Power Tools/Group Items %g")]
    private static void Group()
    {

        GameObject parentObj = new GameObject("Parent");
        GameObject[] objsToGroup = Selection.gameObjects;

        for (int i = 0; i < objsToGroup.Length; i++)
        {
            if (objsToGroup[i].transform.parent == null)
            {
                objsToGroup[i].transform.SetParent(parentObj.transform);

            }
            else
            {
                DestroyImmediate(parentObj);
            }
        }


    }

}