﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    [AddComponentMenu("ThunderRoad/Levels/Spawners/Waves Spawner")]
    public class WaveSpawner : MonoBehaviour
    {
        public static List<WaveSpawner> instances = new List<WaveSpawner>();

        public List<Transform> spawns = new List<Transform>();
        public bool addAsFleepointOnStart = true;

        public bool beginWaveOnStart;
        public float beginWaveOnStartDelay;
        public string startWaveId;
        public bool pooled = true;
        public bool cleanBodiesAndItemsOnWaveStart = true;

#if ODIN_INSPECTOR
        [ValueDropdown("GetAllFactionID")]
#endif
        public int ignoredFactionId = -1;
        public float updateRate = 2;
        public float sameSpawnDelay = 3;
        public float spawnDelay = 2;

        [Header("Music")]
        public string musicWaveAddress = "Bas.Audio.Music.Gladiator";
        public float musicAudioVolume = 1;
        public string stepAudioGroupAddress = "Bas.AudioGroup.Misc.WaveRound";
        public float stepAudioVolume = 1;

        [Header("Event")]
        public UnityEvent OnWaveBeginEvent = new UnityEvent();
        public UnityEvent OnWaveEndEvent = new UnityEvent();
        public UnityEvent OnWaveLoopEvent = new UnityEvent();

    }
}