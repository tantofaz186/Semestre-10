using System;
using System.Collections;
using System.Collections.Generic;
using Managers;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Map
{
    public class TileSpawner : MonoBehaviour
    {
        [SerializeField] private Tile currentTile;
        [SerializeField] private Tile[] baseTiles;
        [SerializeField] private Tile nextTileToDestroy;
        private Queue<Tile> tileQueue = new Queue<Tile>();
        private int initialTiles = 8;

        private void Start()
        {
            for (int i = 0; i < initialTiles; i++)
            {
                Tile newTile = InstantiateRandomTile();
                EnqueueTile(newTile);
            }
            StartCoroutine(SpawnTilesAsNeeded());
        }

        private IEnumerator SpawnTilesAsNeeded()
        {
            yield return null;
            while (enabled)
            {
                yield return new WaitUntil(PlayerPassedThroughSpawnPoint());
                Tile newTile = InstantiateRandomTile();
                EnqueueTile(newTile);
                DequeueTile();
            }
        }

        private Func<bool> PlayerPassedThroughSpawnPoint()
        {
            return () => GameManager.Instance.player.transform.position.z > tileQueue.Peek().SpawnPoint.position.z;
        }

        private void EnqueueTile(Tile newTile)
        {
            tileQueue.Enqueue(newTile);
            currentTile = newTile;
        }

        private Tile InstantiateRandomTile()
        {
            return InstantiateTile(baseTiles[Random.Range(0, baseTiles.Length)]);
        }

        private Tile InstantiateTile(Tile tile)
        {
            return Instantiate(tile, currentTile.SpawnPoint.position, Quaternion.identity);
        }
        private void DequeueTile()
        {
            Tile oldTile = tileQueue.Dequeue();
            Destroy(nextTileToDestroy.gameObject);
            MarkForDestruction(oldTile);
        }

        private void MarkForDestruction(Tile objectToDestroyNext)
        {
            nextTileToDestroy = objectToDestroyNext;
        }

    }
}