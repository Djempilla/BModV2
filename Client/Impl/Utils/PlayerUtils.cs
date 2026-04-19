using Kernys.Bson;

namespace BModv2.Client.Impl.Utils;

public class PlayerUtils
{
    public static void PlaceBlock(int x, int y, World.BlockType blockId)
    {
        BSONObject bsonobject = new BSONObject();
        bsonobject["ID"] = "SB";
        bsonobject["x"] = x;
        bsonobject["y"] = y;
        bsonobject["BlockType"] = (int)blockId;
        OutgoingMessages.AddOneMessageToList(bsonobject);
    }
}