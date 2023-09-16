using UnityEngine;

namespace App.Scripts.Scenes.SceneChess.Features.GridNavigation.Navigator
{
    public class Cell
    {
        public Vector2Int coordinates;

        public Cell parent = null;

        public bool isAvailable = true;

        public int stepNumber = -1;

        public Cell(Vector2Int coordinates)
        {
            this.coordinates = coordinates;
        }
    }
}