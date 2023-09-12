using UnityEngine;

namespace App.Scripts.Scenes.SceneFillwords.Features.ProviderLevel
{
    public class ProviderLevelGrid
    {
        public Vector2Int coordinates { get; private set; }
        public int index { get; private set; }

        public ProviderLevelGrid(Vector2Int coordinates, int index)
        {
            this.coordinates = coordinates;
            this.index = index;
        }
    }
}

