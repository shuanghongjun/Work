using Pathfinding.Util;
using UnityEngine;

namespace Pathfinding
{
    [RequireComponent(typeof(BoxCollider))]
    public class NavmeshCutWithCollider : NavmeshCut
    {
        private BoxCollider cutCollider;

        protected override void Awake()
        {
            base.Awake();
            cutCollider = GetComponent<BoxCollider>();
        }

        protected override void Reset()
        {
            base.Reset();
            useRotationAndScale = true;
        }

        internal override Rect GetBounds(GraphTransform transform)
        {
            rectangleSize = new Vector2(cutCollider.size.x, cutCollider.size.z);
            height = cutCollider.size.y * this.transform.localScale.y;
            center = cutCollider.center;
            return base.GetBounds(transform);
        }
    }
}
