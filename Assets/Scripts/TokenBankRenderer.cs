using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Stateless renderer: hanya urus tampilan word bank (token yang bisa di-tap)
/// dan daftar urutan yang sudah dipilih. Tidak menyimpan data sendiri -
/// semua dikirim lewat parameter Render().
/// </summary>
public class TokenBankRenderer : MonoBehaviour
{
    [Header("Containers")]
    [SerializeField] private Transform bankContainer;      // parent dengan Layout Group, tempat tombol word bank
    [SerializeField] private Transform selectedContainer;   // parent dengan Layout Group, tempat urutan terpilih

    [Header("Prefab")]
    [SerializeField] private Button tokenButtonPrefab;      // prefab tombol sederhana berisi TextMeshProUGUI

    // Dipanggil renderer setiap tombol bank di-klik, bawa index token di bank.
    public System.Action<int> OnBankTokenClicked;

    private List<GameObject> spawnedButtons = new List<GameObject>();

    /// <summary>Render ulang word bank dan daftar urutan terpilih berdasarkan data terbaru.</summary>
    public void Render(IReadOnlyList<string> bankTokens, IReadOnlyList<string> selectedTokens)
    {
        ClearSpawned();

        for (int i = 0; i < bankTokens.Count; i++)
        {
            int capturedIndex = i; // penting: capture index, lihat penjelasan di bawah
            Button btn = Instantiate(tokenButtonPrefab, bankContainer);
            btn.GetComponentInChildren<TextMeshProUGUI>().text = bankTokens[i];
            btn.onClick.AddListener(() => OnBankTokenClicked?.Invoke(capturedIndex));
            spawnedButtons.Add(btn.gameObject);
        }

        for (int i = 0; i < selectedTokens.Count; i++)
        {
            Button slot = Instantiate(tokenButtonPrefab, selectedContainer);
            slot.GetComponentInChildren<TextMeshProUGUI>().text = selectedTokens[i];
            slot.interactable = false; // slot terpilih tidak perlu di-klik lagi
            spawnedButtons.Add(slot.gameObject);
        }
    }

    private void ClearSpawned()
    {
        foreach (GameObject obj in spawnedButtons)
        {
            Destroy(obj);
        }
        spawnedButtons.Clear();
    }
}
