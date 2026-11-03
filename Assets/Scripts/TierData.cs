using UnityEngine;

[CreateAssetMenu(fileName = "TierData", menuName = "Spellbook/Tier Data")]
public class TierData : ScriptableObject
{
    public int tierNumber;

    [Tooltip("Semua block unik yang dipakai bareng-bareng oleh spell-spell di tier ini. Tiap spell cuma ambil sebagian, sesuai urutan yang benar.")]
    public string[] blockPool;
}
