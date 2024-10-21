using System;
using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    private int maxhealth = 100;
    [SerializeField] private int currentHealth;

    [SerializeField] private bool invensible;

    private PlayerController playerController;

    private void Awake()
    {
        currentHealth = maxhealth;
    }

    private void Start()
    {
        playerController = GetComponent<PlayerController>();
        playerController.OnSlide += PlayerController_OnSlide;
    }

    private void PlayerController_OnSlide(object sender, SkillEventData e)
    {
        StartCoroutine(nameof(InvnesibleFor), e.coolDownTime);
    }

    public void TakeDamage(int damage)
    {
        if (invensible) return;

        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Debug.Log("Dead");
        }
    }
    public IEnumerator InvnesibleFor(float _time)
    {
        invensible = true;
        yield return new WaitForSeconds(_time);
        invensible = false;
    }

    private void OnDestroy()
    {
        playerController.OnSlide -= PlayerController_OnSlide;
    }
}
