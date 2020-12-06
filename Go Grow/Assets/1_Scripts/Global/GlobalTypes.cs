using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FluffyHippo
{
    // ENUMS

    /// <summary>
    /// YES / NO
    /// </summary>
    public enum YesOrNo
    {
        YES,
        NO
    }

    /// <summary>
    /// LEFT / RIGHT
    /// </summary>
    public enum HorizontalSide
    {
        LEFT,
        RIGHT
    }

    /// <summary>
    /// UP / DOWN / LEFT / RIGHT. 
    /// For only UP / DOWN, use VerticalDirection.
    /// </summary>
    public enum Direction
    {
        UP,
        DOWN,
        LEFT,
        RIGHT
    }

    /// <summary>
    /// UP / DOWN
    /// </summary>
    public enum VerticalDirection
    {
        UP,
        DOWN
    }

    /// <summary>
    /// LEFT / RIGHT
    /// </summary>
    public enum HorizontalDirection
    {
        LEFT,
        RIGHT
    }

    /// <summary>
    /// HORIZONTAL / VERTICAL
    /// </summary>
    public enum Axis
    {
        HORIZONTAL,
        VERTICAL
    }

    public enum MagicType
    {
        SPACE,
        ELEMENTAL,
        LIFE,
        LOVE,
        REALITY
    }

    public enum ButtonState
    {
        UNSELECTED,
        SELECTED,
        PRESSED
    }

    public enum CurrentHeldDownButton
    {
        NONE,
        A,
        B,
        C,
        UP,
        DOWN,
        LEFT,
        RIGHT,
        DIRECTION
    }

    public enum AttackSymbolType
    {
        NONE,
        BULLET,
        EXPLOSION,
        SONAR,
        STRAIGHT,
        CIRCLE,
        WAVE,
        HOURGLASS,
        LINES,
        BUBBLES,
        QUESTION,
        EYE
    }

    public enum AttackIconState
    {
        UNPRESSED,
        PRESSED,
        GREY
    }

    // SERIALIZABLE UNITY CLASSES
    [Serializable]
    public class SerializableVector4
    {
        public float a;
        public float b;
        public float c;
        public float d;

        // Vector2
        public static implicit operator Vector2(SerializableVector4 sv)
        {
            return new Vector2(sv.a, sv.b);
        }

        public static implicit operator SerializableVector4(Vector2 v)
        {
            return new SerializableVector4()
            {
                a = v.x,
                b = v.y
            };
        }

        // Vector3
        public static implicit operator Vector3(SerializableVector4 sv)
        {
            return new Vector3(sv.a, sv.b, sv.c);
        }

        public static implicit operator SerializableVector4(Vector3 v)
        {
            return new SerializableVector4()
            {
                a = v.x,
                b = v.y,
                c = v.z
            };
        }

        // Color
        public static implicit operator Color(SerializableVector4 sv)
        {
            return new Color(sv.a, sv.b, sv.c, sv.d);
        }

        public static implicit operator SerializableVector4(Color c)
        {
            return new SerializableVector4()
            {
                a = c.r,
                b = c.g,
                c = c.b,
                d = c.a
            };
        }

        // Quaternion
        public static implicit operator Quaternion(SerializableVector4 sv)
        {
            return new Quaternion(sv.a, sv.b, sv.c, sv.d);
        }

        public static implicit operator SerializableVector4(Quaternion q)
        {
            return new SerializableVector4()
            {
                a = q.x,
                b = q.y,
                c = q.z,
                d = q.w
            };
        }
    }
}
