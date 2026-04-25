using BoardTower.Common.Application;
using UnityEngine;

namespace BoardTower.Common.Data.DataStore
{
    [CreateAssetMenu(fileName = nameof(SeData), menuName = "DataTable/" + nameof(SeData))]
    public sealed class SeData : ScriptableObject
    {
        [SerializeField] private SeType seType = default;
        [SerializeField] private AudioClip audioClip = default;

        public SeType type => seType;
        public SeAudioVO ToVO() => new(seType, audioClip);
    }
}