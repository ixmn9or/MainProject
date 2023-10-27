﻿using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace HyperCasualPack.LevelGeneration
{
    [CreateAssetMenu(fileName = "Level Generator Data", menuName = "HyperCasualPack/Level Generator Data", order = 0)]
    public class LevelGeneratorData : ScriptableObject
    {
        [SerializeField] List<SpawnableGroups> _spawnableGroups;
        [SerializeField] SpawnableGroups _levelEndGroup;
        [SerializeField] Tile _tile;

        public int MaxLevel => _spawnableGroups.Count - 1;

        public Tile GetTile()
        {
            return _tile;
        }

        public Group GetGroup(int difficulty)
        {
            if (_spawnableGroups.Count > difficulty)
            {
                var groups = _spawnableGroups[difficulty].Groups;
                int rnd = Random.Range(0, groups.Length);
                return groups[rnd];
            }

            return null;
        }

        public Group GetRandomGroup(out int randomizedValue)
        {
            randomizedValue = Random.Range(0, MaxLevel);
            return GetGroup(randomizedValue);
        }

        /// <summary>
        /// This is usually a boss that only spawns at the end of level
        /// </summary>
        public Group GetLevelEndGroup()
        {
            var groups = _levelEndGroup.Groups;
            int rnd = Random.Range(0, groups.Length);
            return groups[rnd];
        }
    }

    [Serializable]
    public struct SpawnableGroups
    {
        public Group[] Groups;
    }
}