using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Watering : MonoBehaviour
{
    public GameObject particlePrefab; // Prefab của Particle System
    public Animator animator;
    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    void SpawnParticle()
    {
        if (particlePrefab != null)
        {
            // Tạo một bản sao của particlePrefab tại spawnPosition
            particlePrefab.SetActive(true);
            Debug.Log("Tao ban sao");
        }
        else
        {
            Debug.LogWarning("Chưa gán Prefab cho particle!");
        }
    }
    public void OnAnimationEnd()
    {
        gameObject.SetActive(false); // Tắt GameObject sau khi Animation kết thúc
        particlePrefab.SetActive(false);
    }
    public void Water()
    { 
     
        gameObject.SetActive(true);
        animator.Play("watering");
    }
  
}
