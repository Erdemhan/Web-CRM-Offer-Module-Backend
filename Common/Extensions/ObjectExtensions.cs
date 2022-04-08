using System;
using System.Collections.Generic;
using System.Reflection;

namespace crmweb.Common.Extensions
{
    public static class ObjectExtensions
    {
        private static readonly MethodInfo CloneMethod = typeof(object).GetMethod("MemberwiseClone", BindingFlags.NonPublic | BindingFlags.Instance);


        public static T Copy<T>(this T AOriginal)
        {
            return (T)Copy((object)AOriginal);
        }

        private static bool IsPrimitive(this Type AType)
        {
            if (AType == typeof(string))
                return true;

            return (AType.IsValueType & AType.IsPrimitive);
        }

        private static object Copy(this object AOriginalobject)
        {
            return InternalCopy(AOriginalobject, new Dictionary<object, object>(new ReferenceEqualityComparer()));
        }

        private static object InternalCopy(object AOriginalobject, IDictionary<object, object> AVisited)
        {
            if (AOriginalobject == null)
                return null;

            var vTypeToReflect = AOriginalobject.GetType();
            if (IsPrimitive(vTypeToReflect))
                return AOriginalobject;

            if (AVisited.ContainsKey(AOriginalobject))
                return AVisited[AOriginalobject];

            if (typeof(Delegate).IsAssignableFrom(vTypeToReflect))
                return null;

            var vCloneobject = CloneMethod.Invoke(AOriginalobject, null);
            if (vTypeToReflect.IsArray)
            {
                var vArrayType = vTypeToReflect.GetElementType();
                if (IsPrimitive(vArrayType) == false)
                {
                    Array vClonedArray = (Array)vCloneobject;
                    vClonedArray.ForEach((Array, Indices) => Array.SetValue(InternalCopy(vClonedArray.GetValue(Indices), AVisited), Indices));
                }

            }
            AVisited.Add(AOriginalobject, vCloneobject);
            CopyFields(AOriginalobject, AVisited, vCloneobject, vTypeToReflect);
            RecursiveCopyBaseTypePrivateFields(AOriginalobject, AVisited, vCloneobject, vTypeToReflect);
            return vCloneobject;
        }

        private static void RecursiveCopyBaseTypePrivateFields(object AOriginalobject,
            IDictionary<object, object> AVisited, object ACloneobject, Type ATypeToReflect)
        {
            if (ATypeToReflect.BaseType != null)
            {
                RecursiveCopyBaseTypePrivateFields(AOriginalobject, AVisited, ACloneobject, ATypeToReflect.BaseType);
                CopyFields(AOriginalobject, AVisited, ACloneobject, ATypeToReflect.BaseType,
                    BindingFlags.Instance | BindingFlags.NonPublic, Info => Info.IsPrivate);
            }
        }

        private static void CopyFields(object AOriginalobject, IDictionary<object, object> AVisited,
            object ACloneobject, Type ATypeToReflect,
            BindingFlags ABindingFlags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.FlattenHierarchy,
            Func<FieldInfo, bool> AFilter = null)
        {
            foreach (FieldInfo vFieldInfo in ATypeToReflect.GetFields(ABindingFlags))
            {
                if (AFilter != null && AFilter(vFieldInfo) == false)
                    continue;

                if (IsPrimitive(vFieldInfo.FieldType))
                    continue;

                var vOriginalFieldValue = vFieldInfo.GetValue(AOriginalobject);
                var vClonedFieldValue = InternalCopy(vOriginalFieldValue, AVisited);
                vFieldInfo.SetValue(ACloneobject, vClonedFieldValue);
            }
        }
    }

    public class ReferenceEqualityComparer : EqualityComparer<object>
    {
        public override bool Equals(object AX, object AY)
        {
            return ReferenceEquals(AX, AY);
        }

        public override int GetHashCode(object AObj)
        {
            if (AObj == null)
                return 0;

            return AObj.GetHashCode();
        }
    }
}
