using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldPrefab : LootItem<GoldPrefab>
{
    public GoldPrefab(Object objType) : base(objType)
    {
    }

    /*public override void MoveTowards(Vector2 vec)
    {
        base.MoveTowards(vec);
    }*/
    public void Move()
    {
    }
    IEnumerator MoveC(Vector2 start, Vector2 end, float duration)
    {
        float elapsed = 0.0f;
        float ratio = 0.0f;
        while (elapsed < duration)
        {
            ratio = elapsed / duration;
            //instance.transform.position = Vector3.Lerp(start, end, ratio);
            elapsed += Time.deltaTime;
            yield return null;
        }
    }
}
