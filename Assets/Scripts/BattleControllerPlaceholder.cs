using UnityEngine;

/// <summary>
/// Contoh paling basic buat nyambungin TokenPuzzleController ke battle system.
/// Ini placeholder - ganti logic damage/monster health sesuai sistem battle kamu.
/// </summary>
public class BattleControllerPlaceholder : MonoBehaviour
{
    [SerializeField] private TokenPuzzleController tokenPuzzleController;
    [SerializeField] private int monsterHealth = 100;

    private void OnEnable()
    {
        tokenPuzzleController.OnSpellCastSuccess += HandleSpellSuccess;
        tokenPuzzleController.OnSpellCastFail += HandleSpellFail;
    }

    private void OnDisable()
    {
        tokenPuzzleController.OnSpellCastSuccess -= HandleSpellSuccess;
        tokenPuzzleController.OnSpellCastFail -= HandleSpellFail;
    }

    private void HandleSpellSuccess(SpellData spell, float accuracy, int damage)
    {
        monsterHealth -= damage;
        Debug.Log($"[CAST OK] {spell.spellName} | Accuracy: {accuracy:P0} | Damage: {damage} | Monster HP: {monsterHealth}");
    }

    private void HandleSpellFail(SpellData spell, float accuracy)
    {
        Debug.Log($"[FIZZLE] {spell.spellName} | Accuracy terlalu rendah: {accuracy:P0} - spell gagal, turn hilang");
    }
}
