using System;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHPbar : MonoBehaviour
{
  [SerializeField] private Image HPbarSprite;

  private Camera cam;

  private void Start()
  {
    cam = Camera.main;
  }

  public void UpdateHPbar(float maxHealth, float currentHealth)
  {
    HPbarSprite.fillAmount = currentHealth / maxHealth;
  }

  private void Update()
  {
    transform.rotation = Quaternion.LookRotation(transform.position - cam.transform.position);
  }
}
