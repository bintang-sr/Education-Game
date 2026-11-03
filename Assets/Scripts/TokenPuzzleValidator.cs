using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Plain C# class (bukan MonoBehaviour) yang nyimpen state puzzle token:
/// token mana yang masih ada di "bank" (belum dipilih) dan urutan yang sudah dipilih pemain.
/// Tidak tahu apa-apa soal tampilan (UI).
/// </summary>
public class TokenPuzzleValidator
{
    private string[] targetTokens = new string[0];
    private string[] fullBankPool = new string[0];
    private float minAccuracyToCast;

    // Bank = token yang masih bisa dipilih pemain, sudah diacak urutannya.
    private List<string> bankTokens = new List<string>();

    // Urutan yang sudah dipilih pemain sejauh ini.
    private List<string> selectedTokens = new List<string>();

    public IReadOnlyList<string> BankTokens => bankTokens;
    public IReadOnlyList<string> SelectedTokens => selectedTokens;

    public TokenPuzzleValidator(float minAccuracyToCast)
    {
        this.minAccuracyToCast = minAccuracyToCast;
    }

    /// <summary>
    /// Set target tokens (urutan yang benar untuk spell ini) dan bankPool (seluruh
    /// shared pool milik tier, biasanya lebih besar dari target - sisanya otomatis
    /// jadi pengecoh). Word bank yang ditampilkan = bankPool, diacak.
    /// </summary>
    public void SetTarget(string[] tokens, string[] bankPool)
    {
        targetTokens = tokens ?? new string[0];
        fullBankPool = bankPool ?? targetTokens;
        selectedTokens.Clear();

        bankTokens = new List<string>(fullBankPool);
        ShuffleBank();
    }

    private void ShuffleBank()
    {
        // Fisher-Yates shuffle sederhana.
        for (int i = bankTokens.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            (bankTokens[i], bankTokens[randomIndex]) = (bankTokens[randomIndex], bankTokens[i]);
        }
    }

    /// <summary>
    /// Pemain tap token di posisi "bankIndex" pada word bank.
    /// Token itu dipindah dari bank ke daftar selected (paling belakang).
    /// </summary>
    public void SelectToken(int bankIndex)
    {
        if (bankIndex < 0 || bankIndex >= bankTokens.Count) return;

        string token = bankTokens[bankIndex];
        bankTokens.RemoveAt(bankIndex);
        selectedTokens.Add(token);
    }

    /// <summary>
    /// Undo: ambil token terakhir yang dipilih, kembalikan ke word bank.
    /// Berguna karena interaksinya tap-only (gak ada drag buat reorder bebas).
    /// </summary>
    public void UndoLastSelection()
    {
        if (selectedTokens.Count == 0) return;

        int lastIndex = selectedTokens.Count - 1;
        string token = selectedTokens[lastIndex];
        selectedTokens.RemoveAt(lastIndex);
        bankTokens.Add(token);
    }

    /// <summary>True kalau semua token sudah dipilih (bank kosong).</summary>
    public bool IsReadyToSubmit()
    {
        return targetTokens.Length > 0 && selectedTokens.Count == targetTokens.Length;
    }

    /// <summary>Hitung persentase token yang posisinya benar (0.0 - 1.0).</summary>
    public float CalculateAccuracy()
    {
        if (targetTokens.Length == 0) return 0f;

        int correctCount = 0;
        int compareLength = Mathf.Min(selectedTokens.Count, targetTokens.Length);

        for (int i = 0; i < compareLength; i++)
        {
            if (selectedTokens[i] == targetTokens[i])
                correctCount++;
        }

        return (float)correctCount / targetTokens.Length;
    }

    /// <summary>
    /// Hitung hasil cast: return true kalau accuracy >= minimum threshold.
    /// finalDamage dan accuracy dikirim balik lewat parameter out.
    /// </summary>
    public bool TryGetCastResult(int baseDamage, out int finalDamage, out float accuracy)
    {
        accuracy = CalculateAccuracy();
        bool success = accuracy >= minAccuracyToCast;
        finalDamage = success ? Mathf.RoundToInt(baseDamage * accuracy) : 0;
        return success;
    }

    /// <summary>Reset total: seluruh bank pool balik lagi, diacak ulang.</summary>
    public void Reset()
    {
        selectedTokens.Clear();
        bankTokens = new List<string>(fullBankPool);
        ShuffleBank();
    }
}
