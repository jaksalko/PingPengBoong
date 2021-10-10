using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx.Triggers;
using UniRx;
public class StageRoadUIPrefab : UIPrefab
{
    public const int maxStageButtonCount = 6;//최대 버튼 보유 개수
    public const int maxHorizontalButtonCount = 3;
    private const float horizontalSpacing = 210;
    private const float verticalSpacing = 170;
    public float GetVerticalSpacing() { return verticalSpacing; }
    private Dictionary<int, Vector3> stageButtonPosition;
   
    private void Awake()
    {
        stageButtonPosition = new Dictionary<int, Vector3>();
        stageButtonPosition.Add(1, new Vector3(210,-190,0));
        stageButtonPosition.Add(2, new Vector3(210 - horizontalSpacing, -190, 0));
        stageButtonPosition.Add(3, new Vector3(210 - (horizontalSpacing * 2), -190, 0));
        stageButtonPosition.Add(4, new Vector3(210 - (horizontalSpacing * 2), -190 + verticalSpacing, 0));
        stageButtonPosition.Add(5, new Vector3(210 - horizontalSpacing, -190 + verticalSpacing, 0));
        stageButtonPosition.Add(6, new Vector3(210, -190 + verticalSpacing, 0));
    }
    public override void SetAsParentOfChild(Transform child)
    {
        base.SetAsParentOfChild(child);
        child.GetComponent<RectTransform>().localPosition = stageButtonPosition[transform.childCount];
    }
}
