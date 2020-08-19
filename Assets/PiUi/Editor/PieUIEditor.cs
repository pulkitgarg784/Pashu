using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(PiUI))]
public class PiUIEditor : Editor
{
    PiUI myTarget;
    [SerializeField]
    float[] lastAngles;
    int slicesToAdd;
    System.Action addSlice;
    System.Action<PiUI.PiData> removeSlice;
    System.Action<int> angleUpdate;
    List<PiUI.PiData> slicesToRemove = new List<PiUI.PiData>( );

    List<string> itemsNotToDraw = new List<string>( );

    private void OnEnable()
    {
        myTarget = (PiUI)target;
        addSlice = AddSlice;
        removeSlice = SliceToRemove;
        angleUpdate = AngleUpdate;
    }

    public override void OnInspectorGUI()
    {
        if (lastAngles == null)
        {
            lastAngles = new float[myTarget.piData.Length];
        }
        if (GUILayout.Button("Change PiCut State"))
        {
            myTarget.piCut.gameObject.SetActive(!myTarget.piCut.gameObject.activeInHierarchy);
        }
        if (GUILayout.Button("Sync Colors"))
        {
            myTarget.SyncColor( );
        }
        if (!myTarget.dynamicallyScaleToResolution)
        {
            if (!itemsNotToDraw.Contains("defaultResolution"))
                itemsNotToDraw.Add("defaultResolution");
        }
        else
        {
            itemsNotToDraw.Remove("defaultResolution");
        }
        if (!myTarget.outline)
        {
            if (!itemsNotToDraw.Contains("outlineColor"))
                itemsNotToDraw.Add("outlineColor");
        }
        else
        {
            itemsNotToDraw.Remove("outlineColor");
        }
        if (!myTarget.syncColors)
        {
            if (!itemsNotToDraw.Contains("syncNormal"))
            {
                itemsNotToDraw.Add("syncNormal");
            }
            if (!itemsNotToDraw.Contains("syncSelected"))
            {
                itemsNotToDraw.Add("syncSelected");
            }
            if (!itemsNotToDraw.Contains("syncDisabled"))
            {
                itemsNotToDraw.Add("syncDisabled");
            }
        }
        else
        {
            itemsNotToDraw.Remove("syncNormal");
            itemsNotToDraw.Remove("syncSelected");
            itemsNotToDraw.Remove("syncDisabled");
        }
        serializedObject.Update( );
        if (itemsNotToDraw.Count == 0)
        {
            EditorUtility.SetDirty(myTarget);
        }
        else
        {
            DrawPropertiesExcluding(serializedObject, itemsNotToDraw.ToArray( ));
            EditorUtility.SetDirty(myTarget);
        }
        serializedObject.ApplyModifiedProperties( );
        serializedObject.Update( );
        myTarget.piData = myTarget.piData.ToList( ).OrderBy(x => x.order).ToArray( );
        var sprop = serializedObject.FindProperty("piData");
        for (var i = 0; i < myTarget.piData.Length; i++)
        {
            if (!slicesToRemove.Contains(myTarget.piData[i]))
            {
                myTarget.piData[i].OnInspectorGUI(sprop.GetArrayElementAtIndex(i), myTarget, addSlice, removeSlice, angleUpdate);
            }
            if(i< myTarget.piData.Length - 1)
            {
                GUILayout.Space(10);
            }
        }
        if (myTarget.piData.Length < 1)
        {
            myTarget.piData = new PiUI.PiData[1];
        }
        myTarget.sliceCount = myTarget.piData.Length;
        if (slicesToAdd > 0)
        {
            Undo.RecordObject(myTarget, "Add Slice");
            PiUI.PiData[] tempArray = new PiUI.PiData[myTarget.piData.Length + slicesToAdd];
            for (int i = myTarget.piData.Length; i < tempArray.Length; i++)
            {
                tempArray[i] = new PiUI.PiData( );
                tempArray[i].SetValues(myTarget.piData[myTarget.piData.Length - 1]);
                tempArray[i].order = i;
            }
            for (int j = 0; j < myTarget.piData.Length; j++)
            {
                tempArray[j] = myTarget.piData[j];
            }
            slicesToAdd = 0;
            myTarget.piData = tempArray;
        }
        else if (slicesToRemove.Count > 0 && myTarget.piData.Length > 1)
        {
            Undo.RecordObject(myTarget, "Removed Slice");
            PiUI.PiData[] tempArray = new PiUI.PiData[myTarget.piData.Length - slicesToRemove.Count];
            int addedSlices = 0;
            for (int i = 0; i < myTarget.piData.Length; i++)
            {
                if (!slicesToRemove.Contains(myTarget.piData[i]))
                {
                    tempArray[addedSlices] = myTarget.piData[i];
                    tempArray[addedSlices].order = addedSlices;
                    addedSlices++;
                }
            }
            myTarget.piData = tempArray;
            slicesToRemove = new List<PiUI.PiData>( );
        }
        serializedObject.ApplyModifiedProperties( );
        if(SumOfAngles() > 360)
        {
            for (int i = 0; i < myTarget.piData.Length; i++)
            {
                AngleUpdate(i);
            }
        }
        if(SumOfAngles() < 360)
        {
            myTarget.piData[myTarget.piData.Length - 1].angle = 360 - SumOfAngles( );
        }
        /*
        if (myTarget.piData.Length != myTarget.sliceCount)
        {
            if (myTarget.piData.Length < myTarget.sliceCount)
            {
                PiUI.PiData[] temp = new PiUI.PiData[myTarget.sliceCount];
                for (int j = 0; j < myTarget.piData.Length; ++)
                {
                    PiUI.PiData instance = myTarget.piData[i];
                    temp[i] = instance;
                }
                PiUI.PiData duplicateValue = myTarget.piData[myTarget.piData.Length - 1];
                for (int j = myTarget.piData.Length - 1; j < myTarget.sliceCount; j++)
                {
                    PiUI.PiData instance = new PiUI.PiData( );
                    if (duplicateValue != null)
                        instance.SetValues(duplicateValue);
                    temp[j] = instance;
                }
                myTarget.piData = temp;
            }
            else if (myTarget.piData.Length > myTarget.sliceCount)
            {
                PiUI.PiData[] temp = new PiUI.PiData[myTarget.sliceCount];
                for (int i = 0; i < myTarget.sliceCount; i++)
                {
                    PiUI.PiData instance = myTarget.piData[i];
                    temp[i] = instance;
                }
                myTarget.piData = temp;
            }
            lastAngles = new float[myTarget.piData.Length];
        }
        float sumOfAngles = 0;
        for (int i = 0; i < myTarget.piData.Length; i++)
        {
            if (myTarget.piData[i].angle == 0 || myTarget.piData[i].angle < 20)
            {
                myTarget.piData[i].angle = 360 / myTarget.sliceCount;
            }
            sumOfAngles += myTarget.piData[i].angle;
          //  lastAngles[i] = myTarget.piData[i].angle;
        }
        */
    }

    private void OnSceneGUI()
    {
        Color temp = Color.blue;
        temp.a = .25f;
        Handles.color = temp;
        Handles.DrawSolidDisc(myTarget.transform.position, Vector3.back, myTarget.innerRadius);
        temp = Color.red;
        temp.a = .25f;
        Handles.color = temp;
        Handles.DrawSolidDisc(myTarget.transform.position, Vector3.back, myTarget.outerRadius);
    }

    public void AddSlice()
    {
        slicesToAdd++;
    }

    public void SliceToRemove(PiUI.PiData sliceToRemove)
    {
        if (!slicesToRemove.Contains(sliceToRemove))
            slicesToRemove.Add(sliceToRemove);
    }
    
    public float SumOfAngles()
    {
        float sum = 0;
        for(int i = 0; i< myTarget.piData.Length;i++)
        {
            sum += Mathf.Abs(myTarget.piData[i].angle);
        }
        return sum;

    }

    public void AngleUpdate(int order)
    {
        float sumBefore = 0;
        for (int i = 0; i <= order; i++)
        {
            sumBefore += myTarget.piData[i].angle;
        }
        float remainder = (360 - sumBefore) / (myTarget.piData.Length - order - 1);
        for (int i = order + 1; i < myTarget.piData.Length; i++)
        {
            myTarget.piData[i].angle = remainder;
        }

    }

}
