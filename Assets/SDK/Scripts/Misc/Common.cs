﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

namespace ThunderRoad
{
    public enum Side
    {
        Right,
        Left,
    }

    public enum LayerType
    {
        Touch,
        Body,
        Ragdoll,
        Object,
        Stair,
        Avatar,
        AvatarObject,
        FloatingHand,
        Damage,
        Creature,
        UI,
        Player,
    }

    public enum AxisDirection
    {
        None,
        Up,
        Down,
        Left,
        Right,
    }

    public enum HueColorName
    {
        Lime,
        Green,
        Aqua,
        Blue,
        Navy,
        Purple,
        Pink,
        Red,
        Orange,
        Yellow,
        White
    }

    public enum AudioMixerName
    {
        Master,
        Effect,
        Ambient,
        UI,
        Voice,
        Music,
        SlowMotion,
    }

    public enum LayerName
    {
        None,
        Default,
        PlayerLocomotion,
        BodyLocomotion,
        Touch,
        MovingItem,
        DroppedItem,
        ItemAndRagdollOnly,
        TouchObject,
        Avatar,
        NPC,
        FPVHide,
        LocomotionOnly,
        Ragdoll,
        PostProcess,
        PlayerHandAndFoot,
        PlayerLocomotionObject,
        LiquidFlow,
        Zone,
        NoLocomotion,
        MovingObjectOnly,
        SkyDome,
        Highlighter,
    }

    public enum Cardinal
    {
        XYZ,
        XZ,
        X,
        Z,
    }

    public enum FilterLogic
    {
        AnyExcept,
        NoneExcept,
    }

    public enum EffectTarget
    {
        None,
        Main,
        Secondary,
    }

    public enum EffectLink
    {
        Intensity,
        Speed,
    }

    public enum SavedValueID
    {
        Rack,
        Holder,
        PotionFill,
        Ammo,
        LevelCastThrow,
        LevelCastSpray,
        LevelCharge,
        LevelImbue,
        LevelMergeAppart,
        LevelMergeUp,
        LevelMergeForward,
        LevelMergeDown,
        LevelCrystalShockwave,
        LevelCrystalForward,
        LevelCrystalFire,
        Grab,
    }

    public enum Finger
    {
        Thumb,
        Index,
        Middle,
        Ring,
        Little,
    }

    public enum EventTime
    {
        OnStart,
        OnEnd,
    }

    public enum DamageType
    {
        Unknown,
        Pierce,
        Slash,
        Blunt,
        Energy,
    }

    public enum Platform
    {
        Windows,
        Quest2,
        PSVR,
    }

    [Flags]
    public enum LevelSaveOptions
    {
        PlayerHolsters = 1,
        PlayerGrabbedItems = 2,
        PlayerRacks = 4,
        LevelCreatures = 8,
        PlayerGrabbedCreatures = 16,
    }

    [Serializable]
    public class CustomReference
    {
        public string name;
        public Transform transform;
    }

    public class ModData
    {
        public string Name;
        public string Description;
        public string Author;
        public string ModVersion;
        public string GameVersion;

        [NonSerialized]
        public string folderName;
    }

    public static class Common
    {
        private static int _lightProbeVolumeLayer;
        public static int lightProbeVolumeLayer
        {
            get { return _lightProbeVolumeLayer > 0 ? _lightProbeVolumeLayer : _lightProbeVolumeLayer = LayerMask.NameToLayer("LightProbeVolume"); }
            private set { _lightProbeVolumeLayer = value; }
        }

        private static int _zoneLayer;
        public static int zoneLayer
        {
            get { return _zoneLayer > 0 ? _zoneLayer : _zoneLayer = LayerMask.NameToLayer("Zone"); }
            private set { _zoneLayer = value; }
        }

        public static bool ActiveInPrefabHierarchy(this GameObject gameObject)
        {
            if (gameObject.activeSelf)
            {
                if (gameObject.transform.parent)
                {
                    return gameObject.transform.parent.gameObject.ActiveInPrefabHierarchy();
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return false;
            }
        }

        public static bool Contains(this LayerMask mask, int layer)
        {
            return mask == (mask | (1 << layer));
        }

        public static int GetMaskAddLayer(int mask, int layer)
        {
            return (mask | ~(1 << layer));
        }

        public static int GetMaskRemoveLayer(int mask, int layer)
        {
            return (mask & ~(1 << layer));
        }

        private static bool platformCached;
        private static Platform currentPlatform;

        public static Platform GetPlatform()
        {
            if (Application.isPlaying)
            {
                if (!platformCached)
                {
                    if (Enum.TryParse(QualitySettings.names[QualitySettings.GetQualityLevel()], out currentPlatform))
                    {
                        platformCached = true;
                    }
                    else
                    {
                        Debug.LogError("Quality Settings names don't match platform enum!");
                    }
                }
                return currentPlatform;
            }
            else
            {
                if (Enum.TryParse(QualitySettings.names[QualitySettings.GetQualityLevel()], out Platform platform))
                {
                    return platform;
                }
                else
                {
                    Debug.LogError("Quality Settings names don't match platform enum!");
                    return Platform.Windows;
                }
            }
        }

        public static int GetRandomWeightedIndex(float[] weights)
        {
            if (weights == null || weights.Length == 0) return -1;

            float w;
            float t = 0;
            int i;
            for (i = 0; i < weights.Length; i++)
            {
                w = weights[i];

                if (float.IsPositiveInfinity(w))
                {
                    return i;
                }
                else if (w >= 0f && !float.IsNaN(w))
                {
                    t += weights[i];
                }
            }

            float r = UnityEngine.Random.value;
            float s = 0f;

            for (i = 0; i < weights.Length; i++)
            {
                w = weights[i];
                if (float.IsNaN(w) || w <= 0f) continue;

                s += w / t;
                if (s >= r) return i;
            }

            return -1;
        }

        public static void DrawText(GUISkin guiSkin, string text, Vector3 position, Color? color = null, int fontSize = 0, float yOffset = 0)
        {
            var prevSkin = GUI.skin;
            if (guiSkin == null)
                Debug.LogWarning("editor warning: guiSkin parameter is null");
            else
                GUI.skin = guiSkin;

            GUIContent textContent = new GUIContent(text);

            GUIStyle style = (guiSkin != null) ? new GUIStyle(guiSkin.GetStyle("Label")) : new GUIStyle();
            if (color != null)
                style.normal.textColor = (Color)color;
            if (fontSize > 0)
                style.fontSize = fontSize;

            Vector2 textSize = style.CalcSize(textContent);
            Vector3 screenPoint = Camera.current.WorldToScreenPoint(position);

            if (screenPoint.z > 0) // checks necessary to the text is not visible when the camera is pointed in the opposite direction relative to the object
            {
                var worldPosition = Camera.current.ScreenToWorldPoint(new Vector3(screenPoint.x - textSize.x * 0.5f, screenPoint.y + textSize.y * 0.5f + yOffset, screenPoint.z));
#if UNITY_EDITOR
                UnityEditor.Handles.Label(worldPosition, textContent, style);
#endif
            }
            GUI.skin = prevSkin;
        }

        private static NavMeshPath navMeshPath = new NavMeshPath();

        public static T GetClosest<T>(List<T> behaviours, Vector3 position, bool prioritizeShortestPath) where T : Behaviour
        {
            if (prioritizeShortestPath)
            {
                float shortestPathLength = Mathf.Infinity;
                Behaviour shortestPathBehaviour = null;
                foreach (Behaviour behaviour in behaviours)
                {
                    navMeshPath.ClearCorners();
                    if (NavMesh.CalculatePath(position, behaviour.transform.position, -1, navMeshPath))
                    {
                        float pathLength = GetPathLength(navMeshPath);
                        if (pathLength < shortestPathLength)
                        {
                            shortestPathLength = pathLength;
                            shortestPathBehaviour = behaviour;
                        }
                    }
                }
                if (shortestPathBehaviour)
                {
                    return shortestPathBehaviour as T;
                }
            }

            float closestDistanceSqr = Mathf.Infinity;
            Behaviour closestBehaviour = null;
            foreach (Behaviour behaviour in behaviours)
            {
                Vector3 directionToTarget = behaviour.transform.position - position;
                float dSqrToTarget = directionToTarget.sqrMagnitude;
                if (dSqrToTarget < closestDistanceSqr)
                {
                    closestDistanceSqr = dSqrToTarget;
                    closestBehaviour = behaviour;
                }
            }
            return closestBehaviour as T;
        }

        public static Transform GetClosest(List<Transform> transforms, Vector3 position, bool prioritizeShortestPath)
        {
            if (prioritizeShortestPath)
            {
                float shortestPathLength = Mathf.Infinity;
                Transform shortestPathTransform = null;
                foreach (Transform transform in transforms)
                {
                    navMeshPath.ClearCorners();
                    if (NavMesh.CalculatePath(position, transform.transform.position, -1, navMeshPath))
                    {
                        float pathLength = GetPathLength(navMeshPath);
                        if (pathLength < shortestPathLength)
                        {
                            shortestPathLength = pathLength;
                            shortestPathTransform = transform;
                        }
                    }
                }
                if (shortestPathTransform)
                {
                    return shortestPathTransform;
                }
            }

            float closestDistanceSqr = Mathf.Infinity;
            Transform closestTransform = null;
            foreach (Transform transform in transforms)
            {
                Vector3 directionToTarget = transform.transform.position - position;
                float dSqrToTarget = directionToTarget.sqrMagnitude;
                if (dSqrToTarget < closestDistanceSqr)
                {
                    closestDistanceSqr = dSqrToTarget;
                    closestTransform = transform;
                }
            }
            return closestTransform;
        }

        public static float GetPathLength(NavMeshPath path)
        {
            float lng = 0.0f;
            Vector3[] corners = path.corners;
            if ((path.status != NavMeshPathStatus.PathInvalid) && (corners.Length > 1))
            {
                for (int i = 1; i < corners.Length; ++i)
                {
                    lng += Vector3.Distance(corners[i - 1], corners[i]);
                }
            }
            return lng;
        }

        public static Component CloneComponent(Component source, GameObject destination, bool copyProperties = false)
        {
            Component destinationComponent = destination.AddComponent(source.GetType());
            if (copyProperties)
            {
                foreach (PropertyInfo property in source.GetType().GetProperties())
                {
                    if (property.CanWrite) property.SetValue(destinationComponent, property.GetValue(source, null), null);
                }
            }
            foreach (FieldInfo field in source.GetType().GetFields())
            {
                field.SetValue(destinationComponent, field.GetValue(source));
            }
            return destinationComponent;
        }

        public static string GetPathFromRoot(this GameObject obj)
        {
            string path = "/" + obj.name;
            while (obj.transform.parent != null)
            {
                obj = obj.transform.parent.gameObject;
                path = "/" + obj.name + path;
            }
            return path;
        }

        public static Vector3 GetRowPosition(Transform transform, int index, float rowCount, float rowSpace)
        {
            return transform.position + (transform.right * rowSpace * (index % rowCount)) + (transform.forward * rowSpace * Mathf.FloorToInt((index / rowCount)));
        }

        public static int GetIndexByName(this Dropdown dropDown, string name)
        {
            if (dropDown == null) { return -1; } // or exception
            if (string.IsNullOrEmpty(name) == true) { return -1; }
            List<Dropdown.OptionData> list = dropDown.options;
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].text.Equals(name)) { return i; }
            }
            return -1;
        }

        public static void SetLayerRecursively(this GameObject obj, int layer)
        {
            obj.layer = layer;
            foreach (Transform child in obj.transform)
            {
                child.gameObject.SetLayerRecursively(layer);
            }
        }

        public static string GetStringBetween(this string text, string start, string end)
        {
            int pFrom = text.IndexOf(start) + start.Length;
            int pTo = text.LastIndexOf(end);
            return text.Substring(pFrom, pTo - pFrom);
        }

        public static void SetParentOrigin(this Transform transform, Transform parent)
        {
            transform.SetParent(parent);
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            transform.localScale = Vector3.one;
        }

        public static void SetParentOrigin(this Transform transform, Transform parent, Vector3? localPosition = null, Quaternion? localRotation = null, Vector3? localScale = null)
        {
            transform.parent = parent;
            transform.localPosition = localPosition != null ? Vector3.zero : (Vector3)localPosition;
            transform.localRotation = localRotation != null ? Quaternion.identity : (Quaternion)localRotation;
            transform.localScale = localScale != null ? Vector3.one : (Vector3)localScale;
        }

        public static void MoveAlign(this Transform transform, Transform child, Transform target, Transform parent = null)
        {
            transform.MoveAlign(child, target.position, target.rotation, parent);
        }

        public static void MoveAlign(this Transform transform, Transform child, Vector3 targetPosition, Quaternion targetRotation, Transform parent = null)
        {
            // Align connector rotations
            Quaternion deltaRotation = targetRotation * Quaternion.Inverse(child.rotation);
            transform.transform.rotation = deltaRotation * transform.transform.rotation;
            // Align connector positions
            Vector3 displacement = targetPosition - child.position;
            transform.transform.position += displacement;
            // parenting
            if (parent) transform.transform.SetParent(parent, true);
        }

        public static Quaternion InverseTransformRotation(this Transform transform, Quaternion rotation)
        {
            return (Quaternion.Inverse(transform.rotation) * rotation);
        }

        public static Quaternion TransformRotation(this Transform transform, Quaternion localRotation)
        {
            return (transform.rotation * localRotation);
        }

        public static void MirrorChilds(this Transform transform, Vector3 mirrorAxis)
        {
            foreach (Transform child in transform.GetComponentsInChildren<Transform>())
            {
                if (child == transform) continue;
                Transform orgParent = child.parent;
                Transform mirror = new GameObject("Mirror").transform;
                mirror.SetParent(orgParent, false);
                mirror.localPosition = Vector3.zero;
                mirror.localRotation = Quaternion.identity;
                mirror.localScale = Vector3.one;
                child.SetParent(mirror, true);
                mirror.localScale = Vector3.Scale(mirrorAxis, transform.localScale);
                child.SetParent(orgParent, true);
                if (UnityEngine.Application.isPlaying) GameObject.Destroy(mirror.gameObject);
                else GameObject.DestroyImmediate(mirror.gameObject);
                child.localScale = Vector3.Scale(mirrorAxis, transform.localScale);
            }
        }

        public static void MirrorRelativeToParent(this Transform transform, Vector3 mirrorAxis)
        {
            Transform root = new GameObject("MirrorRoot").transform;
            root.SetParent(transform.parent);
            root.localPosition = Vector3.zero;
            root.localRotation = Quaternion.identity;
            root.localScale = Vector3.one;
            transform.SetParent(root, true);
            root.MirrorChilds(mirrorAxis);
            transform.SetParent(root.parent, true);
            if (UnityEngine.Application.isPlaying) GameObject.Destroy(root.gameObject);
            else GameObject.DestroyImmediate(root.gameObject);
        }

        private static Hashtable hueColourValues = new Hashtable{
         { HueColorName.Lime,     new Color32( 166 , 254 , 0, 255 ) },
         { HueColorName.Green,     new Color32( 0 , 254 , 111, 255 ) },
         { HueColorName.Aqua,     new Color32( 0 , 201 , 254, 255 ) },
         { HueColorName.Blue,     new Color32( 0 , 122 , 254, 255 ) },
         { HueColorName.Navy,     new Color32( 60 , 0 , 254, 255 ) },
         { HueColorName.Purple, new Color32( 143 , 0 , 254, 255 ) },
         { HueColorName.Pink,     new Color32( 232 , 0 , 254, 255 ) },
         { HueColorName.Red,     new Color32( 254 , 9 , 0, 255 ) },
         { HueColorName.Orange, new Color32( 254 , 161 , 0, 255 ) },
         { HueColorName.Yellow, new Color32( 254 , 224 , 0, 255 ) },
         { HueColorName.White, new Color32( 255 , 255 , 255, 255 ) },
        };

        public static Color32 HueColourValue(HueColorName color)
        {
            return (Color32)hueColourValues[color];
        }

        public static void DrawGizmoArrow(Vector3 pos, Vector3 direction, Color color, float arrowHeadLength = 0.25f, float arrowHeadAngle = 20.0f, bool dim3 = false)
        {
            Gizmos.color = color;
            Gizmos.DrawRay(pos, direction);
            Vector3 right = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 + arrowHeadAngle, 0) * new Vector3(0, 0, 1);
            Vector3 left = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 - arrowHeadAngle, 0) * new Vector3(0, 0, 1);
            Vector3 forward = Quaternion.LookRotation(direction) * Quaternion.Euler(180 + arrowHeadAngle, 0, 0) * new Vector3(0, 0, 1);
            Vector3 backward = Quaternion.LookRotation(direction) * Quaternion.Euler(180 - arrowHeadAngle, 0, 0) * new Vector3(0, 0, 1);
            Gizmos.DrawRay(pos + direction, right * arrowHeadLength);
            Gizmos.DrawRay(pos + direction, left * arrowHeadLength);
            if (dim3)
            {
                Gizmos.DrawRay(pos + direction, forward * arrowHeadLength);
                Gizmos.DrawRay(pos + direction, backward * arrowHeadLength);
#if UNITY_EDITOR
                UnityEditor.Handles.color = color;
                UnityEditor.Handles.DrawWireDisc(pos + new Vector3(0, arrowHeadLength * (1 - Mathf.Cos(arrowHeadAngle * Mathf.Deg2Rad)), 0), direction, arrowHeadLength * Mathf.Sin(arrowHeadAngle * Mathf.Deg2Rad));
#endif
            }
        }
        public static void DrawGizmoCapsule(Vector3 pos, Vector3 direction, Color color, float capsuleLength, float capsuleRadius)
        {
            Gizmos.color = color;
            Gizmos.DrawWireSphere(pos + (direction.normalized) * capsuleLength, capsuleRadius);
            Gizmos.DrawWireSphere(pos - (direction.normalized) * capsuleLength, capsuleRadius);

            Gizmos.DrawRay((Vector3.right) * capsuleRadius, (Vector3.up) * capsuleLength);
            Gizmos.DrawRay((Vector3.right) * capsuleRadius, (Vector3.down) * capsuleLength);

            Gizmos.DrawRay((Vector3.left) * capsuleRadius, (Vector3.up) * capsuleLength);
            Gizmos.DrawRay((Vector3.left) * capsuleRadius, (Vector3.down) * capsuleLength);

            Gizmos.DrawRay((Vector3.forward) * capsuleRadius, (Vector3.up) * capsuleLength);
            Gizmos.DrawRay((Vector3.forward) * capsuleRadius, (Vector3.down) * capsuleLength);

            Gizmos.DrawRay((Vector3.back) * capsuleRadius, (Vector3.up) * capsuleLength);
            Gizmos.DrawRay((Vector3.back) * capsuleRadius, (Vector3.down) * capsuleLength);
        }
    }
}