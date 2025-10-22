using System;
using System.Collections;
using Collectables;
using UnityEngine;

public class PlayerEffects : MonoBehaviour
{
    [SerializeField] private Material defaultMaterial;
    [SerializeField] private Material invertMaterial;
    [SerializeField] private Material immunityMaterial;
    
    MeshRenderer playerRenderer;
    Rigidbody playerRigidbody;
    private void Awake()
    {
        playerRenderer = GetComponent<MeshRenderer>();
        playerRigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        InvertControls.OnInvertControlsCollected += HandleInvertControlsCollected;
        Star.OnStarCollected += HandleStarCollected;
    }


    private void OnDestroy()
    {
        InvertControls.OnInvertControlsCollected -= HandleInvertControlsCollected;
        Star.OnStarCollected -= HandleStarCollected;
    }

    private void HandleInvertControlsCollected(float duration)
    {
        StartCoroutine(InvertEffect(duration));
        
    }

    private IEnumerator InvertEffect(float duration)
    {
        playerRenderer.material = invertMaterial;
        yield return new WaitForSeconds(duration);
        playerRenderer.material = defaultMaterial;
    }


    private void HandleStarCollected(float duration)
    {
        StartCoroutine(ApplyImmunity(duration));
    }

    private IEnumerator ApplyImmunity(float duration)
    {
        playerRenderer.material = immunityMaterial;
        playerRigidbody.isKinematic = true;
        playerRigidbody.useGravity = false;
        gameObject.layer = LayerMask.NameToLayer("PlayerImmune");
        yield return new WaitForSeconds(duration);
        playerRenderer.material = defaultMaterial;
        playerRigidbody.isKinematic = false;
        playerRigidbody.useGravity = true;
        gameObject.layer = LayerMask.NameToLayer("Player");
        
    }
}
