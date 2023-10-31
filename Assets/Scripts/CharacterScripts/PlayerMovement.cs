using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public float speed = 1.0f; // Daha yavaş bir hareket hızı
    public float runSpeed = 2f; // Koşma hızı

    public float jumpHeight = 0.5f; // Daha düşük bir zıplama yüksekliği

    public float gravity = 20.0f;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    // Stamina ile ilgili değişkenler
    public float maxStamina = 100f;
    public float currentStamina;
    public float staminaDecreaseRate = 5f;
    public float staminaRegenRate = 2f;
    public Slider staminaSlider;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;
    private bool isRunning = false;

    public CanvasGroup staminaCanvasGroup; // Slider'ın Canvas Group bileşenine referans

    void Start()
    {
        controller = GetComponent<CharacterController>();
        currentStamina = maxStamina;

        if (staminaSlider)
        {
            staminaSlider.maxValue = maxStamina;
            staminaSlider.value = currentStamina;
        }

        if (staminaCanvasGroup)
        {
            staminaCanvasGroup.alpha = 0f; // Bu satırı ekleyin.
        }



        controller = GetComponent<CharacterController>();
        currentStamina = maxStamina;
        if (staminaSlider)
        {
            staminaSlider.maxValue = maxStamina;
            staminaSlider.value = currentStamina;
        }
    }

    void Update()
    {
        UpdateStaminaUI();

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 move = transform.right * x + transform.forward * z;

        if (Input.GetKey(KeyCode.LeftShift) && currentStamina > 0)
        {
            isRunning = true;
            currentStamina -= staminaDecreaseRate * Time.deltaTime;
            controller.Move(move * runSpeed * Time.deltaTime);
        }
        else
        {
            isRunning = false;
            if (currentStamina < maxStamina)
            {
                currentStamina += staminaRegenRate * Time.deltaTime;
            }
            controller.Move(move * speed * Time.deltaTime);
        }

        currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
        if (staminaSlider)
        {
            staminaSlider.value = currentStamina;
        }

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * 2f * gravity);
        }

        velocity.y -= gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    void UpdateStaminaUI()
    {
        if (currentStamina >= maxStamina)
        {
            staminaCanvasGroup.alpha = Mathf.MoveTowards(staminaCanvasGroup.alpha, 0f, Time.deltaTime);
        }
        else
        {
            staminaCanvasGroup.alpha = Mathf.MoveTowards(staminaCanvasGroup.alpha, 1f, Time.deltaTime);
        }
    }


}