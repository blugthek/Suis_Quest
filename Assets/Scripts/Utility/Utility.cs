using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ScriptableObjects;
using UnityEngine;

namespace Utility
{
    public static class Utility
    {
        public static Vector3 LerpVector3(Vector3 a, Vector3 b,float s)
        {
            var targetVector = a;

            targetVector.z = Mathf.Lerp(targetVector.z, b.z, s);
            targetVector.x = Mathf.Lerp(targetVector.x, b.x, s);
            targetVector.y = Mathf.Lerp(targetVector.y, b.y, s);

            return targetVector;
        }
        public static Vector3 SlerpVector3(Vector3 a, Vector3 b,float s)
        {
            return Vector3.Slerp(a, b, s);
        }

        public static IEnumerable<Vector3> EvaluateSlerpPoints(Vector3 start, Vector3 end,float centerOffset)
        {
            var centerPivot = (start + end) * .05f;
            centerPivot -= new Vector3(0, -centerOffset);
            var startRelativeCenter = start - centerPivot;
            var endRelativeCenter = end - centerPivot;

            var f = 1f / 10;

            for (var i = 0f; i < 1 + f; i += f)
            {
                yield return Vector3.Slerp(startRelativeCenter, endRelativeCenter, i) + centerPivot;
            }
        }

        public static void CheckDirectory(string fileName,string filePath)
        {
            if (!Directory.Exists(filePath)) Directory.CreateDirectory(filePath);
            if (!File.Exists(fileName)) File.Create(fileName);
        }

        public static void ExportNodeDialogs(string fileName,List<NodeDialogs> nodeDialogsList)
        {
            var temp = nodeDialogsList.Aggregate("{ \"ThisDialos\" : [", (current, dialog) => current + (JsonUtility.ToJson(dialog, true) + ","));
            temp = temp[..^1];
            temp += "]}";
            
            File.WriteAllText(fileName,temp);
        }
    }
}