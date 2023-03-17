using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    #region Singleton
    
    public static UIManager Instance { get; private set; }
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("More than one instance of UIManager found!");
            Destroy(gameObject);
        }
    }
    
    #endregion
    
    [SerializeField] private PlayerController playerController;
    [Space]
    [SerializeField] private TMP_Text timeText;
    [Space]
    [SerializeField] private TMP_Text playerOneMetalText;
    [SerializeField] private TMP_Text playerOneCrystalText;
    [SerializeField] private TMP_Text playerTwoMetalText;
    [SerializeField] private TMP_Text playerTwoCrystalText;
    [Space]
    [SerializeField] private Image playerOneHealthBar;
    [SerializeField] private Image playerTwoHealthBar;
    [Space]
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject pauseSettingsMenu;
    [Space]
    [SerializeField] private GameObject winScreen;
    [SerializeField] private TMP_Text winScreenText;
    [Space]
    [SerializeField] private GameObject spawnableAreaIndicator;
    [SerializeField] private RectTransform spawnableAreaIndicatorRectTransform;
    [Space]
    [SerializeField] private Image[] cooldownImages;
    
    public void ShowSpawnableAreaIndicator(float width, GameManager.Player player, bool active = true)
    {
        spawnableAreaIndicatorRectTransform.sizeDelta = new Vector2(Screen.width * (1 - width), Screen.height);
        spawnableAreaIndicatorRectTransform.anchoredPosition = new Vector2(
            Screen.width * (player == GameManager.Player.PlayerOne ? 
            width - (width / 2) : - width + (width / 2)), 0);
        spawnableAreaIndicator.SetActive(active);
    }
    
    public void UpdateTimeText(int time)
    {
        string minutes = Mathf.Floor(time / 60).ToString("0");
        string seconds = (time % 60).ToString("00");
        
        timeText.text = minutes + ":" + seconds;
    }
    
    public void UpdateResourceText()
    {
        playerOneMetalText.text = GameManager.Instance.PlayerOneMetal.ToString();
        playerOneCrystalText.text = GameManager.Instance.PlayerOneCrystals.ToString();
        playerTwoMetalText.text = GameManager.Instance.PlayerTwoMetal.ToString();
        playerTwoCrystalText.text = GameManager.Instance.PlayerTwoCrystals.ToString();
    }
    
    public void UpdateMotherShipHealth(GameManager.Player player, float health)
    {
        switch (player)
        {
            case GameManager.Player.PlayerOne:
                playerOneHealthBar.fillAmount = health;
                break;
            case GameManager.Player.PlayerTwo:
                playerTwoHealthBar.fillAmount = health;
                break;
        }
    }
    
    public void UpdateCooldownImages(float cooldown)
    {
        foreach (var cooldownImage in cooldownImages)
        {
            cooldownImage.fillAmount = cooldown;
        }
    }

    public void TogglePauseMenu()
    {
        if (pauseMenu.activeSelf == false)
        {
            pauseMenu.SetActive(true);
            playerController.enabled = false;
            Time.timeScale = 0;
        }
        else
        {
            pauseMenu.SetActive(false);
            pauseSettingsMenu.SetActive(false);
            playerController.enabled = true;
            Time.timeScale = 1;
        }
    }

    public void DisplayWinScreen(GameManager.Player winner)
    {
        winScreen.SetActive(true);
        
        switch (winner)
        {
            case GameManager.Player.PlayerOne:
                winScreenText.text = "Player One Won!";
                break;
            case GameManager.Player.PlayerTwo:
                winScreenText.text = "Player Two Won!";
                break;
        }
    }
}
