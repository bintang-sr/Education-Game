using UnityEngine;

[CreateAssetMenu(fileName = "New Spell", menuName = "Spellbook/Spell Data")]
public class SpellData : ScriptableObject
{
    public string spellName;

    [Tooltip("Tier tempat spell ini berada. Block diambil dari TierData.blockPool milik tier ini.")]
    public TierData tier;

    [Tooltip("Urutan INDEX block yang benar, mengacu ke posisi di TierData.blockPool. Contoh: kalau blockPool = [\"print(\", \"x\", \")\"], isi [0, 2] berarti jawabannya \"print(\" lalu \")\".")]
    public int[] correctBlockIndices;

    public int baseDamage;

    /// <summary>Ambil string block yang benar, dengan nerjemahin index ke isi blockPool milik tier.</summary>
    public string[] GetCorrectBlocks()
    {
        if (tier == null || tier.blockPool == null || correctBlockIndices == null)
            return new string[0];

        string[] result = new string[correctBlockIndices.Length];
        for (int i = 0; i < correctBlockIndices.Length; i++)
        {
            int index = correctBlockIndices[i];
            bool isValidIndex = index >= 0 && index < tier.blockPool.Length;
            result[i] = isValidIndex ? tier.blockPool[index] : "";
        }
        return result;
    }
}