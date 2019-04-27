using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MouthDefinition")]
public class MouthDefinition : ScriptableObject
{
    [SerializeField] private List<string> _teethTexts = new List<string>();

    public IEnumerable<ToothController> Apply(List<ToothController> teeth)
    {
        int count = Mathf.Min(teeth.Count, _teethTexts.Count);
        for (int i = 0; i < count; i++)
        {
            ToothController tooth = teeth[i];
            string text = _teethTexts[i];

            if (string.IsNullOrEmpty(text))
                continue;

            if (tooth == null)
            {
                Debug.LogError("ToothController is null." +
                               " The MouthController probably has some empty teeth entries in the teeth list." +
                               " Please remove or fill those.");
                continue;
            }

            tooth.Text = text;
            tooth.gameObject.SetActive(true);
            yield return tooth;
        }
    }
}
