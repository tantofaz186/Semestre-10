using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Combat
{
    public class CombatManager : MonoBehaviour
    {
        public AssetReference PlayerUIPrefab;
        public AssetReference EnemyUIPrefbab;
        public GridLayoutGroup playerUIGrid;
        public GridLayoutGroup enemyUIGrid;
        private int turn;

        private bool finishLoadingPlayerAsset;
        private bool finishLoadingEnemyAsset;
        private void Awake()
        {
            PlayerUIPrefab.LoadAssetAsync<ActorUI>().Completed += _ =>
            {
                finishLoadingPlayerAsset = true;
            };
            EnemyUIPrefbab.LoadAssetAsync<ActorUI>().Completed += _ =>
            {
                finishLoadingEnemyAsset = true;
            };
        }

        [Button(isPlayModeOnly:true)]
        public void Setup(CombatScene scene)
        {
            turn = 0;
            foreach (var actor in scene.players)
            {
                PlayerUIPrefab.InstantiateAsync(playerUIGrid.transform).Completed  += handle =>
                {
                    var uiObj = handle.Result.GetComponent<ActorUI>();
                    uiObj.transform.SetParent(playerUIGrid.transform, false);
                    uiObj.Setup(actor);
                };

            }
            foreach (var actor in scene.enemies)
            {
                EnemyUIPrefbab.InstantiateAsync(enemyUIGrid.transform).Completed += handle =>
                {
                    var uiObj = handle.Result.GetComponent<ActorUI>();
                    uiObj.transform.SetParent(enemyUIGrid.transform, false);
                    uiObj.Setup(actor);
                };
            }
        }
        [Button]
        public void Setup(float a, float b)
        {
            
        }        
        [Button]
        public void Test(float a, Actor b, bool c, int d, string e)
        {
            Debug.Log($"a: {a}, b: {b.actorName}, c: {c}, d: {d}, e: {e}");
        }        
        [Button]
        public void Test(Mine a, Vector3 b)
        {
            Debug.Log($"a: {a} b : {b}");
        }        
        [Button]
        public void Test2(int[] a, string[] b)
        {
            Debug.Log($"a: {a} b : {b}");
        }        
        [Button]
        public void Test2(List<float> a, List<int> b)
        {
            Debug.Log($"a: {a} b : {b}");
        }
        private void Start()
        {
            
        }

        public enum Mine
        {
            one,
            off,
            two,
            on
        }
    }
}
