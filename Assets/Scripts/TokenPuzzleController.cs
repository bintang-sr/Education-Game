using UnityEngine;

/// <summary>
/// Penghubung antara TokenPuzzleValidator (logic) dan TokenBankRenderer (tampilan).
/// Attach ke GameObject, drag TokenBankRenderer + SpellData di Inspector.
/// </summary>
public class TokenPuzzleController : MonoBehaviour
{
    [SerializeField] private TokenBankRenderer renderer_;
    [SerializeField] private SpellData currentSpell;
    [SerializeField] private float minAccuracyToCast = 0.5f;

    private TokenPuzzleValidator validator;

    // dipanggil pas spell berhasil cast: (spell data, accuracy 0-1, final damage)
    public System.Action<SpellData, float, int> OnSpellCastSuccess;
    // dipanggil pas accuracy di bawah minAccuracyToCast (spell fizzle, no damage)
    public System.Action<SpellData, float> OnSpellCastFail;

    private void Awake()
    {
        validator = new TokenPuzzleValidator(minAccuracyToCast);
    }

    private void OnEnable()
    {
        renderer_.OnBankTokenClicked += HandleBankTokenClicked;
        SetSpell(currentSpell);
    }

    private void OnDisable()
    {
        renderer_.OnBankTokenClicked -= HandleBankTokenClicked;
    }

    /// <summary>Panggil ini tiap kali mulai spell baru.</summary>
    public void SetSpell(SpellData spell)
    {
        currentSpell = spell;

        string[] correctTokens = spell != null ? spell.GetCorrectBlocks() : new string[0];
        string[] bankPool = (spell != null && spell.tier != null) ? spell.tier.blockPool : correctTokens;

        validator.SetTarget(correctTokens, bankPool);
        RenderCurrentState();
    }

    private void HandleBankTokenClicked(int bankIndex)
    {
        validator.SelectToken(bankIndex);
        RenderCurrentState();

        if (validator.IsReadyToSubmit())
        {
            TrySubmit();
        }
    }

    /// <summary>Panggil ini dari tombol "Undo" di UI kalau pemain mau batalin pilihan terakhir.</summary>
    public void UndoLastSelection()
    {
        validator.UndoLastSelection();
        RenderCurrentState();
    }

    private void TrySubmit()
    {
        bool success = validator.TryGetCastResult(currentSpell.baseDamage, out int damage, out float accuracy);

        if (success)
            OnSpellCastSuccess?.Invoke(currentSpell, accuracy, damage);
        else
            OnSpellCastFail?.Invoke(currentSpell, accuracy);

        validator.Reset();
        RenderCurrentState();
    }

    private void RenderCurrentState()
    {
        renderer_.Render(validator.BankTokens, validator.SelectedTokens);
    }
}