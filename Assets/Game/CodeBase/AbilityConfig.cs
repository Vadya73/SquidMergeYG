using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Ability/AbilityConfig", fileName = "AbilityConfig", order = 0)]
public class AbilityConfig : ScriptableObject
{
    [SerializeField] private AbilityData _abilityData;
    
    public AbilityData AbilityData => _abilityData;
}

[Serializable]
public class AbilityData
{
    [Header("Bomb")]
    [SerializeField] private Sprite _bombActiveSprite;
    [SerializeField] private Sprite _bombInactiveSprite;
    [SerializeField] private int _bombCooldown;
    [Header("Swap")]
    [SerializeField] private Sprite _swapActiveSprite;
    [SerializeField] private Sprite _swapInactiveSprite;
    [SerializeField] private int _swapCooldown;
    [Header("Mix")]
    [SerializeField] private Sprite _mixActiveSprite;
    [SerializeField] private Sprite _mixInactiveSprite;
    [SerializeField] private int _mixCooldown;
    [Header("Delete")]
    [SerializeField] private Sprite _deleteActiveSprite;
    [SerializeField] private Sprite _deleteInactiveSprite;
    [SerializeField] private int _deleteCooldown;

    public Sprite BombActiveSprite => _bombActiveSprite;
    public Sprite BombInactiveSprite => _bombInactiveSprite;
    public int BombCooldown => _bombCooldown;
    
    public Sprite SwapActiveSprite => _swapActiveSprite;
    public Sprite SwapInactiveSprite => _swapInactiveSprite;
    public int SwapCooldown => _swapCooldown;
    
    public Sprite MixActiveSprite => _mixActiveSprite;
    public Sprite MixInactiveSprite => _mixInactiveSprite;
    public int MixCooldown => _mixCooldown;
    
    public Sprite DeleteActiveSprite => _deleteActiveSprite;
    public Sprite DeleteInactiveSprite => _deleteInactiveSprite;
    public int DeleteCooldown => _deleteCooldown;
}