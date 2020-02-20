﻿using UnityEngine;
using System.Collections.Generic;
using System;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace BS
{
    public class EffectSpawner : MonoBehaviour
    {
#if ODIN_INSPECTOR
        [ValueDropdown("GetAllEffectID")]
#endif
        public string effectId;

        public bool spawnOnStart = true;

        [Range(0, 1)]
        public float intensity;

        public bool useMainGradient;
        [GradientUsage(true)]
        public Gradient mainGradient;

        public bool useSecondaryGradient;
        [GradientUsage(true)]
        public Gradient secondaryGradient;

        public Transform target;

        public Mesh mesh;
        public Renderer mainRenderer;
        public Renderer secondaryRenderer;
        public new Collider collider;

        protected EffectInstance effectInstance;

#if ODIN_INSPECTOR
        public List<ValueDropdownItem<string>> GetAllEffectID()
        {
            return Catalog.GetDropdownAllID(Catalog.Category.Effect);
        }
#endif
        private void OnValidate()
        {
            if (effectInstance != null)
            {
                effectInstance.SetIntensity(intensity);
                if (useMainGradient) effectInstance.SetMainGradient(mainGradient);
                if (useSecondaryGradient) effectInstance.SetSecondaryGradient(secondaryGradient);
            }
        }

        protected void Start()
        {
            if (spawnOnStart) Spawn();
        }

        [Button]
        public void Spawn()
        {
            if (!Application.isPlaying)
            {
                Debug.LogError("Press play to use the effect Spawner!");
                return;
            }

            if (effectInstance != null && effectInstance.effects.Count > 0)
            {
                effectInstance.Despawn();
            }

            if (effectId != "" && effectId != null)
            {
                EffectData effectData = Catalog.GetData<EffectData>(effectId);
                effectInstance = effectData.Spawn(this.transform.position, this.transform.rotation, this.transform);
                effectInstance.SetIntensity(intensity);
                if (useMainGradient) effectInstance.SetMainGradient(mainGradient);
                if (useSecondaryGradient) effectInstance.SetSecondaryGradient(secondaryGradient);
                if (target) effectInstance.SetTarget(target);
                if (mesh) effectInstance.SetMesh(mesh);
                if (mainRenderer) effectInstance.SetRenderer(mainRenderer, false);
                if (secondaryRenderer) effectInstance.SetRenderer(secondaryRenderer, true);
                if (collider) effectInstance.SetCollider(collider);
                effectInstance.Play();
            }
        }

        [Button]
        public void Stop()
        {
            if (!Application.isPlaying) return;
            effectInstance.Stop();
        }

    }
}
