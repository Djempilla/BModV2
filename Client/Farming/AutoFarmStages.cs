namespace BModv2.Client.Farming;

public class AutoFarmStages
{
    public enum Stages
    {
        IDLE,
        
        PLACE_FARMABLE,
        BREAK_FARMABLE,
        COLLECT_FARMABLE, // includes dropping trash
        
        FIND_PLATFORM,
        PATHFIND_TO_TREE,
        BREAK_TREE,
        COLLECT_TREE,
        PLANT_TREE,
        
        WARP_PORTAL,
        DROP_TREE_LOOT
    }
}