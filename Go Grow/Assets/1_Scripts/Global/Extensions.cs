using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
// using Newtonsoft.Json;

namespace FluffyHippo
{
    public static class Extensions
    {
        public static Vector2 ChangeX(this Vector2 vector2, float x)
        {
            return new Vector2(x, vector2.y);
        }

        // Transform.position
        public static void ChangePosition_X(this Transform transform, float value)
        {
            transform.position = new Vector3(value, transform.position.y, transform.position.z);
        }

        public static void ChangePosition_Y(this Transform transform, float value)
        {
            transform.position = new Vector3(transform.position.x, value, transform.position.z);
        }

        public static void ChangePosition_Z(this Transform transform, float value)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, value);
        }

        // Transform.localPosition
        public static void ChangeLocalPosition_X(this Transform transform, float value)
        {
            transform.localPosition = new Vector3(value, transform.localPosition.y, transform.localPosition.z);
        }

        public static void ChangeLocalPosition_Y(this Transform transform, float value)
        {
            transform.localPosition = new Vector3(transform.localPosition.x, value, transform.localPosition.z);
        }

        public static void ChangeLocalPosition_Z(this Transform transform, float value)
        {
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, value);
        }

        // Transform.scale (global)
        private static Vector3 GetGlobalToLocalScaleFactor(Transform t)
        {
            // Found here: https://forum.unity.com/threads/solved-why-is-transform-lossyscale-readonly.363594/
            Vector3 factor = Vector3.one;

            while (true)
            {
                Transform tParent = t.parent;

                if (tParent != null)
                {
                    factor.x *= tParent.localScale.x;
                    factor.y *= tParent.localScale.y;
                    factor.z *= tParent.localScale.z;

                    t = tParent;
                }
                else
                {
                    return factor;
                }
            }
        }

        /// <summary>
        /// Remember that "global" scale is actually transform.lossyScale.
        /// </summary>
        public static void SetGlobalScale(this Transform transform, Vector3 newScale)
        {
            // Found here: https://forum.unity.com/threads/solved-why-is-transform-lossyscale-readonly.363594/

            Vector3 desiredGlobalScale = new Vector3(0.5f, 1.0f, 10.0f);//What you want the global scale to be
            Vector3 scaleFactor = GetGlobalToLocalScaleFactor(transform); //Determine the factor

            //Determine what the new scale local scale should be
            Vector3 newLocalScale = new Vector3
            (desiredGlobalScale.x / scaleFactor.x,
                desiredGlobalScale.y / scaleFactor.y,
                desiredGlobalScale.z / scaleFactor.z);

            transform.localScale = newLocalScale; //Set the new local scale
                                                  //Now the gameObject has the requested global scale
        }

        /// <summary>
        /// Remember that "global" scale is actually transform.lossyScale.
        /// </summary>
        public static void ChangeGlobalScale_X(this Transform transform, float value)
        {
            Vector3 newScale = new Vector3(value, transform.lossyScale.y, transform.lossyScale.z);
            transform.SetGlobalScale(newScale);
        }

        /// <summary>
        /// Remember that "global" scale is actually transform.lossyScale.
        /// </summary>
        public static void ChangeGlobalScale_Y(this Transform transform, float value)
        {
            Vector3 newScale = new Vector3(transform.lossyScale.x, value, transform.lossyScale.z);
            transform.SetGlobalScale(newScale);
        }

        /// <summary>
        /// Remember that "global" scale is actually transform.lossyScale.
        /// </summary>
        public static void ChangeGlobalScale_Z(this Transform transform, float value)
        {
            Vector3 newScale = new Vector3(transform.lossyScale.x, transform.lossyScale.y, value);
            transform.SetGlobalScale(newScale);
        }

        // Transform.localScale
        public static void ChangeLocalScale_X(this Transform transform, float value)
        {
            transform.localScale = new Vector3(value, transform.localScale.y, transform.localScale.z);
        }

        public static void ChangeLocalScale_Y(this Transform transform, float value)
        {
            transform.localScale = new Vector3(transform.localScale.x, value, transform.localScale.z);
        }

        public static void ChangeLocalScale_Z(this Transform transform, float value)
        {
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, value);
        }

        // ^ I discovered actual extension methods

        public static Vector3 Vector3_changeX(Vector3 vector, float val)
        {
            return new Vector3(val, vector.y, vector.z);
        }
        public static Vector3 Vector3_changeY(Vector3 vector, float val)
        {
            return new Vector3(vector.x, val, vector.z);
        }
        public static Vector3 Vector3_changeZ(Vector3 vector, float val)
        {
            return new Vector3(vector.x, vector.y, val);
        }

        public static Vector3 Vector3_copy(Vector3 vector)
        {
            return new Vector3(vector.x, vector.y, vector.z);
        }
        public static Vector2 Vector2_copy(Vector3 vector)
        {
            return new Vector2(vector.x, vector.y);
        }

        public static Color SetColorAlpha(Color color, float alpha)
        {
            return new Color(color.r, color.g, color.b, alpha);
        }

        // public static T DeepCopy<T>(T obj)
        // {
        //     if (System.Object.ReferenceEquals(obj, null))
        //     {
        //         throw new Exception("The source object must not be null");
        //     }

        //     T result = default(T);
        //     string output = JsonConvert.SerializeObject(obj);
        //     result = JsonConvert.DeserializeObject<T>(output);

        //     return result;
        // }

        /// <summary>
        /// Gets rid of the string "(Clone)" at the end of a gameObject's name
        /// </summary>
        /// <param name="gameObject"></param>
        public static void GetRidOfCloneEnding(GameObject gameObject)
        {
            if (gameObject.name.Substring(gameObject.name.Length - 7).Equals("(Clone)"))
            {
                gameObject.name = gameObject.name.Substring(0, gameObject.name.Length - 7);
            }
        }
    }
}