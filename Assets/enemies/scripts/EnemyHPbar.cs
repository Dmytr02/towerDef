using System;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHPbar : MonoBehaviour
{
  [SerializeField] private Image HPbarSprite;
  
  public void UpdateHPbar(float maxHealth, float currentHealth)
  {
    HPbarSprite.fillAmount = currentHealth / maxHealth;
  }
}
