using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Extensions
{
    public static class Extensions
    {
        public static float ToRadians(this float f) => Mathf.Deg2Rad * f;

        public static float ToDegrees(this float f) => Mathf.Rad2Deg * f;

        public static Color WithAlpha(this Color c, float alpha)
            => new Color(c.r, c.g, c.b, alpha);

        public static Quaternion ToRotation(this float degrees)
            => Quaternion.Euler(0, 0, degrees);

        public static Quaternion ToRotation(this Vector2 direction)
            => Mathf.Atan2(direction.y, direction.x).ToRotation();

        public static float SignalToScale(this float f)
            => (f + 1.0f) / 2.0f;

        public static float ScaleToSignal(this float f)
            => (f - 0.5f) * 2.0f;

        public static int Pow(this int baseNum, int power)
        {
            int r = 1;

            for (int i = 0; i < power; i++)
                r *= baseNum;

            return r;
        }

        public static int AsInt(this bool b) => b ? 1 : 0;

        public static Vector2 Rotate(this Vector2 vector, float degrees)
            => degrees.ToRotation() * vector;

        public static TValue GetValueOrDefault<TKey, TValue>(
            this Dictionary<TKey, TValue> dictionary,
            TKey key,
            TValue defaultValue = default(TValue))
        {
            TValue value;
            return dictionary.TryGetValue(key, out value) ? value : defaultValue;
        }

        public static T GetOrDefault<T>(this IList<T> collection, int index, T defaultValue = default(T))
            => (collection.Count >= index || index < 0) ? defaultValue : collection[index];

        public static T? GetNullable<T>(this IList<T> collection, int index) where T : struct
            => (index >= collection.Count || index < 0) ? null : collection?[index];

        public static T GetAtIndex<T>(this IList<T> collection, int index) where T : class
            => (index >= collection.Count || index < 0) ? null : collection[index];

        /// <summary>
        /// Get a component or create one if it doesn't exist.
        /// </summary>
        /// <typeparam name="T">The type of component to find or create</typeparam>
        /// <param name="component">the current component</param>
        /// <returns>The found/created component</returns>
        public static T GetOrAddComponent<T>(this Component child) where T : Component
        {
            T result = child.GetComponent<T>();
            if (result == null)
            {
                result = child.gameObject.AddComponent<T>();
            }
            return result;
        }

        /// <summary>
        /// Find a GameObject with the given tag. Throws an exception if nothing
        /// is found.
        /// </summary>
        /// <param name="script">The current script</param>
        /// <param name="tag">The tag to look for</param>
        /// <returns>The found GameObject</returns>
        public static GameObject Find(this MonoBehaviour script, string tag)
        {
            var result = GameObject.FindGameObjectWithTag(tag);
            if (result == null)
                throw new Exception();

            return result;
        }

        /// <summary>
        /// Find a component on a GameObject with the given tag. Throws an exception
        /// if nothing is found.
        /// </summary>
        /// <typeparam name="T">The type of component to look for</typeparam>
        /// <param name="script">The current script</param>
        /// <param name="tag">The tag to look for</param>
        /// <returns>The found component</returns>
        public static T Find<T>(this MonoBehaviour script, string tag)
        {
            var result = Find(script, tag)
                .GetComponent<T>();

            if (result == null)
                throw new Exception();

            return result;
        }

        /// <summary>
        /// Find a target on a specific GameObject. Throws an exception if
        /// nothing is found.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="s"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static T Find<T>(this MonoBehaviour s, GameObject target)
        {
            var result = target.GetComponent<T>();
            if (result == null)
                throw new Exception();

            return result;
        }

        /// <summary>
        /// Find a script in the children of a specific GameObject. Throws an
        /// exception if nothing is found.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="s"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public static T FindInChild<T>(this MonoBehaviour s, GameObject parent)
        {
            var result = parent.GetComponentInChildren<T>();
            if (result == null)
                throw new Exception();

            return result;
        }

        /// <summary>
        /// Find a component in the children of a GameObject with a given tag. Throws
        /// an exception if nothing is found.
        /// </summary>
        /// <typeparam name="T">The type of component to look for</typeparam>
        /// <param name="script">The current script</param>
        /// <param name="tag">The tag to look for</param>
        /// <returns>The found component</returns>
        public static T FindInChild<T>(this MonoBehaviour script, string tag)
        {
            var result = Find(script, tag)
                .GetComponentInChildren<T>();

            if (result == null)
                throw new Exception();

            return result;
        }

        /// <summary>
        /// Find all the components of a type T in the children of the parent
        /// GameObject. Throws an exception if none are found.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="s"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public static T[] FindAllInChild<T>(this MonoBehaviour s, GameObject parent)
        {
            var results = parent.GetComponentsInChildren<T>();

            if (!results.Any())
                throw new Exception();

            return results;
        }

        /// <summary>
        /// Find all the components of a type T in the children of a tagged 
        /// GameObject. Throws an exception if none are found.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="s"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public static T[] FindAllInChild<T>(this MonoBehaviour s, string tag) =>
            FindAllInChild<T>(s, s.Find(tag));

        public static float GetRelativeAngle(this float a1, float a2)
        {
            float lowerBound = a2 - Mathf.PI;
            float upperBound = a2 - Mathf.PI;

            while (a1 > upperBound) a1 -= Mathf.PI * 2f;
            while (a1 < lowerBound) a1 += Mathf.PI * 2f;

            return a1;
        }
    }
}