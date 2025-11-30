using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

[CustomEditor(typeof(MonoBehaviour), true)]
public class ButtonBehaviourEditor : Editor
{
    MonoBehaviour targetBehaviour;
    MethodInfo[] methods;
    List<List<System.Object>> parameters = new List<List<object>>();

    private static readonly Dictionary<Type, Func<string, object, object>> drawers =
        new Dictionary<Type, Func<string, object, object>>
        {
            { typeof(int), (name, value) => EditorGUILayout.IntField(name, (int)value) },
            { typeof(float), (name, value) => EditorGUILayout.FloatField(name, (float)value) },
            { typeof(long), (name, value) => EditorGUILayout.LongField(name, (long)value) },
            { typeof(bool), (name, value) => EditorGUILayout.Toggle(name, (bool)value) },
            { typeof(Vector2), (name, value) => EditorGUILayout.Vector2Field(name, (Vector2)value) },
            { typeof(Vector3), (name, value) => EditorGUILayout.Vector3Field(name, (Vector3)value) },
            { typeof(Vector4), (name, value) => EditorGUILayout.Vector4Field(name, (Vector4)value) },
            { typeof(Vector2Int), (name, value) => EditorGUILayout.Vector2IntField(name, (Vector2Int)value) },
            { typeof(Vector3Int), (name, value) => EditorGUILayout.Vector3IntField(name, (Vector3Int)value) },
            { typeof(Color), (name, value) => EditorGUILayout.ColorField(name, (Color)value) },
            { typeof(Rect), (name, value) => EditorGUILayout.RectField(name, (Rect)value) },
            { typeof(Bounds), (name, value) => EditorGUILayout.BoundsField(name, (Bounds)value) },
        };

    private void OnEnable()
    {
        targetBehaviour = (MonoBehaviour)target;
        methods = targetBehaviour.GetType().GetMethods(
            BindingFlags.Instance |
            BindingFlags.Public |
            BindingFlags.NonPublic).Where((m) => m.GetCustomAttribute<ButtonAttribute>() != null).ToArray();

        for (int i = 0; i < methods.Length; i++)
        {
            parameters.Add(new List<System.Object>());
            var parametersInfo = methods[i].GetParameters();
            for (int j = 0; j < parametersInfo.Length; j++)
            {
                parameters[i].Add(new System.Object());
            }
        }

        foldouts = new bool[methods.Length];
    }

    bool[] foldouts;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        for (int i = 0; i < methods.Length; i++)
        {
            var method = methods[i];

            var buttonAttribute = method.GetCustomAttribute<ButtonAttribute>();
            string buttonName = string.IsNullOrEmpty(buttonAttribute.ButtonName) ? method.Name : buttonAttribute.ButtonName;
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            EditorGUILayout.Space();
            GUI.enabled = Application.isPlaying || !buttonAttribute.IsPlayModeOnly;
            var b = GUILayout.Button(buttonName);
            GUI.enabled = true;
            var parametersInfo = method.GetParameters();
            if (parametersInfo.Length > 0)
            {
                foldouts[i] = EditorGUILayout.Foldout(foldouts[i], "Parameters");
                if (foldouts[i])
                {
                    for (int j = 0; j < parametersInfo.Length; j++)
                    {
                        String parameterName = parametersInfo[j].Name;
                        Type parameterType = parametersInfo[j].ParameterType;
                        var holder = parameters[i][j];
                        DrawFields(parameterName, parameterType, ref holder);
                        parameters[i][j] = holder;
                    }
                }
            }


            if (b)
            {
                method.Invoke(targetBehaviour, parameters[i].ToArray());
            }
        }
    }

    private void DrawFields(String parameterName, Type parameterType, ref System.Object obj)
    {
        if (parameterType.IsValueType)
        {
            DealWithValueType(parameterName, parameterType, ref obj);
        }
        else if (parameterType == typeof(string))
        {
            DealWithString(parameterName, parameterType, ref obj);
        }
        else if (typeof(IEnumerable).IsAssignableFrom(parameterType))
        {
            DealWithArray(parameterName, parameterType, ref obj);
        }
        else
        {
            if (obj != null && obj.GetType() != parameterType)
            {
                obj = null;
            }

            obj = EditorGUILayout.ObjectField(parameterName, (Object)obj, parameterType, true);
        }
        // else
        // {
        //     Debug.LogWarning($"Unsupported parameter type: {parameterInfo.ParameterType}");
        // }
    }

    private void DealWithArray(String parameterName, Type parameterType, ref System.Object obj)
    {
        EditorGUILayout.BeginHorizontal();

        Type elementType = parameterType.GetElementType() ?? parameterType.GetGenericArguments().Single();
        Debug.Log(elementType.Name);
        if (obj == null || obj.GetType() != parameterType)
        {
            obj = Activator.CreateInstance(typeof(List<>).MakeGenericType(elementType));
        }
        
        IList array = (IList)obj;

        int newSize = EditorGUILayout.IntField(parameterName + " Size", array.Count);
        if (newSize != array.Count)
        {
            while (newSize > array.Count)
            {
                array.Add(null);
            }

            while (newSize < array.Count)
            {
                array.RemoveAt(array.Count - 1);
            }
        }
        System.Object element;
        try{element = Activator.CreateInstance(elementType);} 
        catch{element = new object();}
        // if (typeof(IEnumerable).IsAssignableFrom(paramType))
        // {
        //     System.Object subList;
        //     DealWithArray(parameterName + " Element", paramType, ref subList);
        //     ((List<System.Object>)obj).Add(subList);
        // }
        
        if (elementType == typeof(string))
        {
            DealWithString(parameterName + " Element", elementType, ref element);
            ((IList)obj).Add(element);
        }
        else if (elementType.IsValueType)
        {
            DealWithValueType(parameterName + " Element", elementType, ref element);
            ((IList)obj).Add(element);
        }
        else
        {
            element = EditorGUILayout.ObjectField(parameterName + " Element", (Object)element, elementType, true);
            ((IList)obj).Add(element);
        }
        // Type elementType = parameterInfo.ParameterType.GetElementType();
        // if (obj == null || obj.GetType() != parameterInfo.ParameterType)
        // {
        //     obj = new ArrayList();
        //     
        // }
        //
        // Array array = (Array)obj;
        // int newSize = EditorGUILayout.IntField(parameterInfo.Name + " Size", array.Length);
        // if (newSize != array.Length)
        // {
        //     Array newArray = Array.CreateInstance(elementType, newSize);
        //     Array.Copy(array, newArray, Math.Min(array.Length, newSize));
        //     array = newArray;
        //     obj = array;
        // }
        //
        // EditorGUILayout.BeginVertical();
        // for (int k = 0; k < array.Length; k++)
        // {
        //     if (elementType.IsValueType)
        //     {
        //         if (array.GetValue(k) == null || array.GetValue(k).GetType() != elementType)
        //         {
        //             array.SetValue(Activator.CreateInstance(elementType), k);
        //         }
        //
        //         if (drawers.TryGetValue(elementType, out var drawer))
        //         {
        //             array.SetValue(drawer($"{parameterInfo.Name}[{k}]", array.GetValue(k)), k);
        //         }
        //         else if (elementType.IsEnum)
        //         {
        //             array.SetValue(EditorGUILayout.EnumPopup($"{parameterInfo.Name}[{k}]", (Enum)array.GetValue(k)), k);
        //         }
        //         else
        //         {
        //             Debug.LogWarning($"Unsupported array element type: {elementType}");
        //             array.SetValue(EditorGUILayout.ObjectField($"{parameterInfo.Name}[{k}]", (Object)array.GetValue(k), elementType, true), k);
        //         }
        //     }
        //     else if (elementType == typeof(string))
        //     {
        //         if (array.GetValue(k) == null || array.GetValue(k).GetType() != typeof(string))
        //         {
        //             array.SetValue("", k);
        //         }
        //
        //         array.SetValue(EditorGUILayout.TextField($"{parameterInfo.Name}[{k}]", (string)array.GetValue(k)), k);
        //     }
        //     else
        //     {
        //         if (array.GetValue(k) != null && array.GetValue(k).GetType() != elementType)
        //         {
        //             array.SetValue(null, k);
        //         }
        //
        //         array.SetValue(EditorGUILayout.ObjectField($"{parameterInfo.Name}[{k}]", (Object)array.GetValue(k), elementType, true), k);
        //     }
        // }
        // EditorGUILayout.EndVertical();

        EditorGUILayout.EndHorizontal();
    }

    private void DealWithString(String parameterName, Type parameterType, ref System.Object obj)
    {
        if (obj.GetType() != typeof(string))
        {
            obj = "";
        }

        obj = EditorGUILayout.TextField(parameterName, (string)obj);
    }

    private void DealWithValueType(String parameterName, Type parameterType, ref System.Object obj)
    {
        if (obj.GetType() != parameterType) obj = Activator.CreateInstance(parameterType);

        if (drawers.TryGetValue(parameterType, out var drawer))
        {
            obj = drawer(parameterName, obj);
        }
        else if (parameterType.IsEnum)
        {
            obj = EditorGUILayout.EnumPopup(parameterName, (Enum)obj);
        }
        else
        {
            Debug.LogWarning($"Unsupported parameter type: {parameterType}");
            if (obj != null && obj.GetType() != parameterType)
            {
                obj = null;
            }

            obj = EditorGUILayout.ObjectField(parameterName, (Object)obj, parameterType, true);
        }
    }
}