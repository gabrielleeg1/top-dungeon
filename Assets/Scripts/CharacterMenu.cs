using UnityEngine;
using UnityEngine.UI;

public class CharacterMenu : MonoBehaviour
{
    public Text levelText;
    public Text healthText;
    public Text pesosText;
    public Text upgradeCostText;
    public Text xpText;

    public Image currentCharacterSprite;
    public Image weaponSprite;
    public RectTransform xpBar;

    public void OnArrowClick(bool direction)
    {
        if (direction) GameManager.Instance.currentCharacterSelection++;
        else GameManager.Instance.currentCharacterSelection--;

        if (GameManager.Instance.currentCharacterSelection >= GameManager.Instance.playerSprites.Count)
            GameManager.Instance.currentCharacterSelection = 0;

        if (GameManager.Instance.currentCharacterSelection <= 0)
            GameManager.Instance.currentCharacterSelection = GameManager.Instance.playerSprites.Count - 1;

        currentCharacterSprite.sprite =
            GameManager.Instance.playerSprites[GameManager.Instance.currentCharacterSelection];
        GameManager.Instance.player.SwapSprite(GameManager.Instance.currentCharacterSelection);
    }

    public void OnUpgradeClick()
    {
        if (!GameManager.Instance.TryUpgradeWeapon()) return;

        UpdateMenu();
    }

    public void UpdateMenu()
    {
        var gameManager = GameManager.Instance;
        var currentLevel = gameManager.CalculateCurrentLevel();

        // weapon
        weaponSprite.sprite = gameManager.weaponSprites[gameManager.weapon.weaponLevel];
        upgradeCostText.text = gameManager.weapon.weaponLevel >= gameManager.weaponPrices.Count
            ? "MAX"
            : gameManager.weaponPrices[gameManager.weapon.weaponLevel].ToString();

        // information
        healthText.text = $"{gameManager.player.health} / {gameManager.player.maxHealth}";
        pesosText.text = gameManager.pesos.ToString();
        levelText.text = currentLevel.ToString();

        // exp
        if (currentLevel >= gameManager.xpTable.Count)
        {
            xpText.text = $"{gameManager.xp} total experience points";
            xpBar.localScale = Vector3.one;
        }
        else
        {
            var previousLevelXp = gameManager.GetXpToLevel(currentLevel - 1);
            var currentLevelXp = gameManager.GetXpToLevel(currentLevel);

            var diff = currentLevelXp - previousLevelXp;
            var currentXpIntoLevel = gameManager.xp - previousLevelXp;

            var completionRatio = (float)currentXpIntoLevel / diff;
            xpBar.localScale = new Vector3(completionRatio, 1, 1);
            xpText.text = $"{currentXpIntoLevel} / {diff}";
        }
    }
}