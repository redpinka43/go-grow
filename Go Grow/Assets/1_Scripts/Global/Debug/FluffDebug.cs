using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FluffyHippo
{
    public static class FluffDebug
    {
        /// <summary>
        /// Asserts that an object exists, automatically writing out "Please assign [objectName]"
        /// if it doesn't. For the nameOfObject parameter, you can often use nameof(obj)
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="nameOfObject"></param>
        /// <param name="context">Typically 'this'. </param>
        public static void AssertObject(Object obj, string nameOfObject, Object context)
        {
            Debug.Assert(obj, "Please assign " + nameOfObject + ".", context);
        }

        /// <summary>
        /// Assert that multiple objects exist, in one function call. 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="objNamePairs">chicken</param>
        /// <example>
        /// <code>
        /// FluffDebug.AssertObjects(this, new ObjNamePair(hiddenFace, nameof(hiddenFace))
        ///                                new ObjNamePair(showingFace, nameof(showingFace)));
        /// </code>
        /// </example>
        public static void AssertObjects(Object context, params ObjNamePair[] objNamePairs)
        {
            for (int i = 0; i < objNamePairs.Length; i++)
            {
                AssertObject(objNamePairs[i].obj, objNamePairs[i].str, context);
            }
        }
    }

    public struct ObjNamePair
    {
        public Object obj;
        public string str;

        public ObjNamePair(Object obj, string str)
        {
            this.obj = obj;
            this.str = str;
        }
    }
}
