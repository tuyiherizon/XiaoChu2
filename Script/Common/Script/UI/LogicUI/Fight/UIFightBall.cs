using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class UIFightBall : MonoBehaviour,IDragHandler,IEndDragHandler, IBeginDragHandler, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    private RectTransform _RectTransform;
    public RectTransform RectTransform
    {
        get
        {
            if (_RectTransform == null)
            {
                _RectTransform = gameObject.GetComponent<RectTransform>();
            }

            return _RectTransform;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        FallUpdate();
        BombMoveUpdate();
    }

    #region show

    public RectTransform _FightBallAnchor;
    public Vector2 _Pos;

    public UIBallInfo _NormalBallInfo;
    public UIBallInfo _SPBallInfo;
    public UIBallInfo _InnerSPBallInfo;

    public UIBallInfo _BallIce;
    public UIBallInfo _BallClod;
    public UIBallInfo _BallStone;
    public UIBallInfo _BallPosion;
    public UIBallInfo _BallIron;

    public UIBallInfo _BallBombSmallNormal;
    public UIBallInfo _BallBombBigNormal;
    public UIBallInfo _BallBombSmallEnlarge;
    public UIBallInfo _BallBombBigEnlarge;
    public UIBallInfo _BallBombSmallHitTrap;
    public UIBallInfo _BallBombBigHitTrap;
    public UIBallInfo _BallBombSmallReact;
    public UIBallInfo _BallBombBigReact;
    public UIBallInfo _BallBombSmallLighting;
    public UIBallInfo _BallBombBigLighting;
    public UIBallInfo _BallBombSmallAuto;
    public UIBallInfo _BallBombBigAuto;

    public UIBallInfo _BallLineRowNormal;
    public UIBallInfo _BallLineClumnNormal;
    public UIBallInfo _BallLineCrossNormal;
    public UIBallInfo _BallLineRowEnlarge;
    public UIBallInfo _BallLineClumnEnlarge;
    public UIBallInfo _BallLineCrossEnlarge;
    public UIBallInfo _BallLineRowHitTrap;
    public UIBallInfo _BallLineClumnHitTrap;
    public UIBallInfo _BallLineCrossHitTrap;
    public UIBallInfo _BallLineRowReact;
    public UIBallInfo _BallLineClumnReact;
    public UIBallInfo _BallLineCrossReact;
    public UIBallInfo _BallLineRowLighting;
    public UIBallInfo _BallLineClumnLighting;
    public UIBallInfo _BallLineCrossLighting;
    public UIBallInfo _BallLineRowAuto;
    public UIBallInfo _BallLineClumnAuto;
    public UIBallInfo _BallLineCrossAuto;
    public UIBallInfo _BallRPGHP;

    private BallInfo _BallInfo;
    public BallInfo BallInfo
    {
        get
        {
            return _BallInfo;
        }
    }

    public void SetBallInfo(BallInfo ballInfo)
    {
        _BallInfo = ballInfo;
    }

    public void ShowBall()
    {
        _FightBallAnchor.gameObject.SetActive(true);
        ShowNormal();
        ShowSPInner();
        ShowSP();
        _BombMoveVec = Vector2.zero;
    }

    private void ShowNormal()
    {
        if (_BallInfo == null)
            return;

        _NormalBallInfo.SetBallNormalType(_BallInfo);
        _NormalBallInfo.ShowBallInfo(_BallInfo);
    }

    private void ClearSPBall()
    {
        if (_SPBallInfo != null)
        {
            _SPBallInfo.OnClearInfo();
            ResourcePool.Instance.RecvIldeUIItem(_SPBallInfo.gameObject);
            _SPBallInfo = null;
        }
    }

    private void ClearInnerSpBall()
    {
        if (_InnerSPBallInfo != null)
        {
            _InnerSPBallInfo.OnClearInfo();
            ResourcePool.Instance.RecvIldeUIItem(_InnerSPBallInfo.gameObject);
            _InnerSPBallInfo = null;
        }
    }

    private void ShowSP()
    {
        if (_BallInfo == null || _BallInfo.BallSPType == BallType.None)
        {
            ClearSPBall();
            return;
        }

        if (_SPBallInfo != null && _SPBallInfo.BallSPType == _BallInfo.BallSPType)
        {
            _SPBallInfo.ShowBallInfo(_BallInfo);
            return;
        }

        ClearSPBall();

        _SPBallInfo = GetSPBallInfo(_BallInfo.BallSPType);

        if (_SPBallInfo != null)
        {
            _SPBallInfo.SetBallSPType(_BallInfo);
            _SPBallInfo.ShowBallInfo(_BallInfo);
        }
    }

    private void ShowSPInner()
    {
        if (_BallInfo == null || _BallInfo.IncludeBallSPType == BallType.None)
        {
            ClearInnerSpBall();
            return;
        }

        if (_InnerSPBallInfo != null && _InnerSPBallInfo.BallSPType == _BallInfo.IncludeBallSPType)
        {
            _InnerSPBallInfo.ShowBallInfo(_BallInfo, true);
            return;
        }

        ClearInnerSpBall();

        _InnerSPBallInfo = GetSPBallInfo(_BallInfo.IncludeBallSPType);

        if (_InnerSPBallInfo != null)
        {
            _InnerSPBallInfo.SetBallSPType(_BallInfo, true);
            _InnerSPBallInfo.ShowBallInfo(_BallInfo, true);
        }
    }

    private UIBallInfo GetSPBallInfo(BallType ballType)
    {
        UIBallInfo spBallInfo = null;
        switch (ballType)
        {
            case BallType.Ice:
                spBallInfo = ResourcePool.Instance.GetIdleUIItem<UIBallInfo>(_BallIce.gameObject, _FightBallAnchor);
                break;
            case BallType.Clod:
                spBallInfo = ResourcePool.Instance.GetIdleUIItem<UIBallInfo>(_BallClod.gameObject, _FightBallAnchor);
                break;
            case BallType.Stone:
                spBallInfo = ResourcePool.Instance.GetIdleUIItem<UIBallInfo>(_BallStone.gameObject, _FightBallAnchor);
                break;
            case BallType.Posion:
                spBallInfo = ResourcePool.Instance.GetIdleUIItem<UIBallInfo>(_BallPosion.gameObject, _FightBallAnchor);
                break;
            case BallType.Iron:
                spBallInfo = ResourcePool.Instance.GetIdleUIItem<UIBallInfo>(_BallIron.gameObject, _FightBallAnchor);
                break;
            case BallType.BombSmall1:
                spBallInfo = ResourcePool.Instance.GetIdleUIItem<UIBallInfo>(_BallBombSmallNormal.gameObject, _FightBallAnchor);
                break;
            case BallType.BombBig1:
                spBallInfo = ResourcePool.Instance.GetIdleUIItem<UIBallInfo>(_BallBombBigNormal.gameObject, _FightBallAnchor);
                break;
            case BallType.BombSmallEnlarge1:
                spBallInfo = ResourcePool.Instance.GetIdleUIItem<UIBallInfo>(_BallBombSmallEnlarge.gameObject, _FightBallAnchor);
                break;
            case BallType.BombBigEnlarge:
                spBallInfo = ResourcePool.Instance.GetIdleUIItem<UIBallInfo>(_BallBombBigEnlarge.gameObject, _FightBallAnchor);
                break;
            case BallType.BombSmallHitTrap:
                spBallInfo = ResourcePool.Instance.GetIdleUIItem<UIBallInfo>(_BallBombSmallHitTrap.gameObject, _FightBallAnchor);
                break;
            case BallType.BombBigHitTrap:
                spBallInfo = ResourcePool.Instance.GetIdleUIItem<UIBallInfo>(_BallBombBigHitTrap.gameObject, _FightBallAnchor);
                break;
            case BallType.BombSmallReact:
                spBallInfo = ResourcePool.Instance.GetIdleUIItem<UIBallInfo>(_BallBombSmallReact.gameObject, _FightBallAnchor);
                break;
            case BallType.BombBigReact:
                spBallInfo = ResourcePool.Instance.GetIdleUIItem<UIBallInfo>(_BallBombBigReact.gameObject, _FightBallAnchor);
                break;
            case BallType.BombSmallLighting:
                spBallInfo = ResourcePool.Instance.GetIdleUIItem<UIBallInfo>(_BallBombSmallLighting.gameObject, _FightBallAnchor);
                break;
            case BallType.BombBigLighting:
                spBallInfo = ResourcePool.Instance.GetIdleUIItem<UIBallInfo>(_BallBombBigLighting.gameObject, _FightBallAnchor);
                break;
            case BallType.BombSmallAuto:
                spBallInfo = ResourcePool.Instance.GetIdleUIItem<UIBallInfo>(_BallBombSmallAuto.gameObject, _FightBallAnchor);
                break;
            case BallType.BombBigAuto:
                spBallInfo = ResourcePool.Instance.GetIdleUIItem<UIBallInfo>(_BallBombBigAuto.gameObject, _FightBallAnchor);
                break;

            case BallType.LineRow:
                spBallInfo = ResourcePool.Instance.GetIdleUIItem<UIBallInfo>(_BallLineRowNormal.gameObject, _FightBallAnchor);
                break;
            case BallType.LineClumn:
                spBallInfo = ResourcePool.Instance.GetIdleUIItem<UIBallInfo>(_BallLineClumnNormal.gameObject, _FightBallAnchor);
                break;
            case BallType.LineCross:
                spBallInfo = ResourcePool.Instance.GetIdleUIItem<UIBallInfo>(_BallLineCrossNormal.gameObject, _FightBallAnchor);
                break;
            case BallType.LineRowEnlarge:
                spBallInfo = ResourcePool.Instance.GetIdleUIItem<UIBallInfo>(_BallLineRowEnlarge.gameObject, _FightBallAnchor);
                break;
            case BallType.LineClumnEnlarge:
                spBallInfo = ResourcePool.Instance.GetIdleUIItem<UIBallInfo>(_BallLineClumnEnlarge.gameObject, _FightBallAnchor);
                break;
            case BallType.LineCrossEnlarge:
                spBallInfo = ResourcePool.Instance.GetIdleUIItem<UIBallInfo>(_BallLineCrossEnlarge.gameObject, _FightBallAnchor);
                break;
            case BallType.LineRowHitTrap:
                spBallInfo = ResourcePool.Instance.GetIdleUIItem<UIBallInfo>(_BallLineRowHitTrap.gameObject, _FightBallAnchor);
                break;
            case BallType.LineClumnHitTrap:
                spBallInfo = ResourcePool.Instance.GetIdleUIItem<UIBallInfo>(_BallLineClumnHitTrap.gameObject, _FightBallAnchor);
                break;
            case BallType.LineCrossHitTrap:
                spBallInfo = ResourcePool.Instance.GetIdleUIItem<UIBallInfo>(_BallLineCrossHitTrap.gameObject, _FightBallAnchor);
                break;
            case BallType.LineRowReact:
                spBallInfo = ResourcePool.Instance.GetIdleUIItem<UIBallInfo>(_BallLineRowReact.gameObject, _FightBallAnchor);
                break;
            case BallType.LineClumnReact:
                spBallInfo = ResourcePool.Instance.GetIdleUIItem<UIBallInfo>(_BallLineClumnReact.gameObject, _FightBallAnchor);
                break;
            case BallType.LineCrossReact:
                spBallInfo = ResourcePool.Instance.GetIdleUIItem<UIBallInfo>(_BallLineCrossReact.gameObject, _FightBallAnchor);
                break;
            case BallType.LineRowLighting:
                spBallInfo = ResourcePool.Instance.GetIdleUIItem<UIBallInfo>(_BallLineRowLighting.gameObject, _FightBallAnchor);
                break;
            case BallType.LineClumnLighting:
                spBallInfo = ResourcePool.Instance.GetIdleUIItem<UIBallInfo>(_BallLineClumnLighting.gameObject, _FightBallAnchor);
                break;
            case BallType.LineCrossLighting:
                spBallInfo = ResourcePool.Instance.GetIdleUIItem<UIBallInfo>(_BallLineCrossLighting.gameObject, _FightBallAnchor);
                break;
            case BallType.LineRowAuto:
                spBallInfo = ResourcePool.Instance.GetIdleUIItem<UIBallInfo>(_BallLineRowAuto.gameObject, _FightBallAnchor);
                break;
            case BallType.LineClumnAuto:
                spBallInfo = ResourcePool.Instance.GetIdleUIItem<UIBallInfo>(_BallLineClumnAuto.gameObject, _FightBallAnchor);
                break;
            case BallType.LineCrossAuto:
                spBallInfo = ResourcePool.Instance.GetIdleUIItem<UIBallInfo>(_BallLineCrossAuto.gameObject, _FightBallAnchor);
                break;

            case BallType.RPGHP:
                spBallInfo = ResourcePool.Instance.GetIdleUIItem<UIBallInfo>(_BallRPGHP.gameObject, _FightBallAnchor);
                break;
            default:

                break;
        }

        return spBallInfo;
    }

    public void ResetRoot()
    {
        _FightBallAnchor.SetParent(transform);
        _FightBallAnchor.position = transform.position;
        //_FightBallAnchor.localPosition = Vector3.zero;
    }

    public void Exchange(UIFightBall otherBall)
    {
        var temp = otherBall._FightBallAnchor;
        otherBall._FightBallAnchor = _FightBallAnchor;
        _FightBallAnchor = temp;

        var tempNormal = otherBall._NormalBallInfo;
        otherBall._NormalBallInfo = _NormalBallInfo;
        _NormalBallInfo = tempNormal;

        var tempSp = otherBall._SPBallInfo;
        otherBall._SPBallInfo = _SPBallInfo;
        _SPBallInfo = tempSp;

        otherBall.ResetRoot();
        ResetRoot();
    }

    public float Elimit()
    {
        StartCoroutine(OnElimit());

        return 0.2f * (BallInfo._BombPrivite - 1);
    }

    private IEnumerator OnElimit()
    {
        yield return new WaitForSeconds(0.2f * (BallInfo._BombPrivite - 1));

        if (_SPBallInfo != null)
        {
            _SPBallInfo.OnElimit();
        }
        else
        {
            _NormalBallInfo.OnElimit();
        }
    }

    public void Explore()
    {
        ShowSP();
    }

    public bool IsSPBallBomb()
    {
        if (_SPBallInfo == null)
            return false;

        return _SPBallInfo.BallSPType > BallType.SPBombStart && _SPBallInfo.BallSPType < BallType.SPBombEnd;
    }

    public bool IsSPBallLine()
    {
        if (_SPBallInfo == null)
            return false;

        return _SPBallInfo.BallSPType > BallType.SPLineStart && _SPBallInfo.BallSPType < BallType.SPLineEnd;
    }

    #endregion

    #region anim

    public float _FallSpeed = 2000;

    private UIFightBox _UIFightBox;

    public void SetFightBox(UIFightBox uiFightBox)
    {
        _UIFightBox = uiFightBox;
    }

    public void UIBallFall(List<BallInfo> anchorPath)
    {
        _FallPath = anchorPath;
        _FightBallAnchor.anchoredPosition = _UIFightBox.GetBallPosByIdx(_Pos.x, _Pos.y, anchorPath[0].Pos.x, anchorPath[0].Pos.y);
        _CurFallIdx = 0;
        FallNextPath();
    }

    private List<BallInfo> _FallPath;
    private int _CurFallIdx = 0;
    private Vector2 _MoveVec = Vector2.zero;
    private Vector2 _TargetPos = Vector2.zero;
    private float _LastTime = 0;

    private void FallNextPath()
    {
        ++_CurFallIdx;
        if (_CurFallIdx > 0 && _CurFallIdx < _FallPath.Count)
        {
            _TargetPos = _UIFightBox.GetBallPosByIdx(_Pos.x, _Pos.y, _FallPath[_CurFallIdx].Pos.x, _FallPath[_CurFallIdx].Pos.y);
            _MoveVec = _TargetPos - _FightBallAnchor.anchoredPosition;
            _LastTime = 1;
        }
        else
        {
            _TargetPos = Vector2.zero;
            _MoveVec = Vector2.zero;

            if (_CurFallIdx == _FallPath.Count - 1)
            {
                _FightBallAnchor.anchoredPosition = Vector2.zero;
            }
        }
    }

    public void FallUpdate()
    {
        if (_MoveVec == Vector2.zero)
            return;

        Vector2 moveVec = _MoveVec.normalized * _FallSpeed * Time.fixedDeltaTime;

        _MoveVec -= moveVec;
        _LastTime -= Time.fixedDeltaTime;
        if (_LastTime < 0)
        {
            _LastTime = 0;
            _MoveVec = Vector2.zero;
        }

        var destPoint = _FightBallAnchor.anchoredPosition + moveVec;
        if (_TargetPos != Vector2.zero)
        {
            var delta = _FightBallAnchor.anchoredPosition - _TargetPos;
            if (delta.magnitude < moveVec.magnitude)
            {
                destPoint = _TargetPos;
                _LastTime = 0;
                FallNextPath();
            }
        }
        _FightBallAnchor.anchoredPosition = destPoint;
    }

    public float _BombMoveSpeed = 3000;

    private Vector2 _BombTargetPos;
    private Vector2 _BombMoveVec = Vector2.zero;

    public void BombMoveTo(UIFightBall targetBall)
    {
        _BombTargetPos = targetBall.transform.position;
        Vector2 curPos = _FightBallAnchor.transform.position;
        _BombMoveVec = _BombTargetPos - curPos;
    }

    public void BombMoveUpdate()
    {
        if (_BombMoveVec == Vector2.zero)
            return;

        Vector2 curPos = _FightBallAnchor.transform.position;
        Vector2 moveVec = _BombMoveVec.normalized * _BombMoveSpeed * Time.fixedDeltaTime;

        var destPoint = curPos + moveVec;
        var delta = curPos - _BombTargetPos;
        if (delta.magnitude < moveVec.magnitude)
        {
            destPoint = _TargetPos;
            _BombMoveVec = Vector2.zero;
            _FightBallAnchor.gameObject.SetActive(false);
        }

        _FightBallAnchor.transform.position = destPoint;
    }
    
    #endregion

    #region opt

    public void OnDrag(PointerEventData eventData)
    {
        
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _FightBallAnchor.transform.localScale = Vector3.one;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _FightBallAnchor.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
        _UIFightBox.DragBall = this;
    }

    public void OnDrop(PointerEventData eventData)
    {
        //if (BallBox.Instance.IsCanMoveTo(_UIFightBox.DragBall.BallInfo, BallInfo))
        //{
        //    Debug.Log("ballA._FightBallAnchor000:" + _UIFightBox.DragBall._FightBallAnchor.position + "," + _UIFightBox.DragBall.transform.position);
        //    _UIFightBox.ExChangeBalls(this, _UIFightBox.DragBall);
        //}
        //_UIFightBox.DragBall = null;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_UIFightBox.DragBall == null)
        {
            _FightBallAnchor.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
            return;
        }

        if (BallBox.Instance.IsCanMoveTo(_UIFightBox.DragBall.BallInfo, BallInfo))
        {
            //_FightBallAnchor.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
            _UIFightBox.ExChangeBalls(this, _UIFightBox.DragBall);
            _UIFightBox.DragBall = null;
        }

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (this != _UIFightBox.DragBall)
        {
            _FightBallAnchor.transform.localScale = Vector3.one;
        }
    }

    #endregion

    #region sound

    

    #endregion
}
