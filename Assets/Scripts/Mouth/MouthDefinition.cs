using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MouthDefinition")]
public class MouthDefinition : ScriptableObject
{
    [SerializeField] private List<string> _teethTexts = new List<string>();

    public IEnumerable<Tooth> Apply(List<Tooth> teeth)
    {
        int count = Mathf.Min(teeth.Count, _teethTexts.Count);
        for (int i = 0; i < count; i++)
        {
            Tooth tooth = teeth[i];
            string text = _teethTexts[i];

            if (string.IsNullOrEmpty(text))
                continue;

            tooth.Text = text;
            tooth.gameObject.SetActive(true);
            yield return tooth;
        }
    }
}
