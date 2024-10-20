using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static PlayerController;

public class InGameUI : MonoBehaviour
{

    [SerializeField] private CoolDownUI dashCoolDownUI;
    [SerializeField] private CoolDownUI slideCoolDownUI;

    [SerializeField] private PlayerController playerController;

    [SerializeField] private TextMeshProUGUI playerSpeedText;


    private float smoothedSpeed;
    public float smoothingFactor = 0.1f; // Adjust this for more or less smoothing


    private void Start()
    {
        playerController = FindAnyObjectByType<PlayerController>();
        playerController.OnDash += PlayerController_OnDash;
        playerController.OnSlide += PlayerController_OnSlide;
    }

    private void FixedUpdate()
    {
        dashCoolDownUI.CheckCoolDown(playerController.dashCooldownTime);
        slideCoolDownUI.CheckCoolDown(playerController.slideCooldownTime);

        //UpdateSpeed();
    }

    private void UpdateSpeed()
    {
        Vector3 horizontalVelocity = new Vector3(playerController.velocity.x, 0, playerController.velocity.z);
        float currentSpeed = horizontalVelocity.magnitude;
        smoothedSpeed = Mathf.Lerp(smoothedSpeed, currentSpeed, smoothingFactor);
        playerSpeedText.text = $"Speed: {smoothedSpeed.ToString("F1")}"; // Show with one decimal place
    }

    private void PlayerController_OnDash(object sender, SkillEventData e)
    {
        dashCoolDownUI.SetCoolDown();
    }

    private void PlayerController_OnSlide(object sender, SkillEventData e)
    {
        slideCoolDownUI.SetCoolDown();
    }



    private void OnDestroy()
    {
        playerController.OnDash -= PlayerController_OnDash;
        playerController.OnSlide -= PlayerController_OnSlide;
    }
}
