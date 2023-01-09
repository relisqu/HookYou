using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu]
public class AbyssRuleTile : RuleTile<AbyssRuleTile.Neighbor>
{
    [SerializeField]private TileBase AbyssTile;
    [SerializeField]private TileBase FullTile;

    public class Neighbor : RuleTile.TilingRule.Neighbor {
        public const int Null = 3;
        public const int NotNull = 4;
        public const int IsNearWall = 5;
    }

    public override bool RuleMatch(int neighbor, TileBase tile) {
        switch (neighbor) {
            case Neighbor.Null: return tile == null;
            case Neighbor.NotNull: return tile != null;
            case Neighbor.IsNearWall: return CheckIfTileIsNearWallTile(tile);
        }
        return base.RuleMatch(neighbor, tile);
    }

    private bool CheckIfTileIsNearWallTile(TileBase tile)
    {
        return tile!=null && tile.name == AbyssTile.name;
    }
}