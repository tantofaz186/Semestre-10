using System.Collections.Generic;
using System.Linq;
using Collectables;
using Map;
using UnityEngine;

namespace Managers
{
    public class CollectableSpawnManager : MonoBehaviour
    {
        public List<Collectable> collectablePrefabs;
        [Range(0, 1)] public float spawnProbability = 0.03f;

        private void Start()
        {
            TileSpawner.OnTileSpawned += SpawnCollectableOnTile;
        }

        private void OnDestroy()
        {
            TileSpawner.OnTileSpawned -= SpawnCollectableOnTile;
        }

        private void SpawnCollectableOnTile(Tile tile)
        {
            foreach (var spawnPoint in tile.CollectableSpawnPoints)
            {
                if (Random.value <= spawnProbability)
                {
                    foreach (Collectable collectablePrefab in collectablePrefabs.OrderBy(c => c.chanceToSpawn))
                    {
                        if (Random.value <= collectablePrefab.chanceToSpawn)
                        {
                            Instantiate(collectablePrefab, spawnPoint);
                            break;
                        }
                    }
                }
            }
        }
    }
}