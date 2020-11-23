using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIEffectLine : EffectController
{

    // Update is called once per frame
    void Update ()
    {
        if (_UP.activeSelf || _Down.activeSelf || _Left.activeSelf || _Right.activeSelf)
        {
            EffectLineFlyUpdate();
        }
	}

    #region effect

    public GameObject _UP;
    public GameObject _Down;
    public GameObject _Left;
    public GameObject _Right;

    public GameObject _UpLarge;
    public GameObject _DownLarge;
    public GameObject _LeftLarge;
    public GameObject _RightLarge;

    public GameObject _UpSmall;
    public GameObject _DownSmall;
    public GameObject _LeftSmall;
    public GameObject _RightSmall;

    public GameObject _SubEffectPanel;
    public List<GameObject> _UpSub;
    public List<GameObject> _DownSub;
    public List<GameObject> _LeftSub;
    public List<GameObject> _RightSub;

    public float _Speed = 10;
    private float _StartTime = 0;

    private BallInfo _BaseBall;
    private float _MaxUp;
    private float _MaxDown;
    private float _MaxLeft;
    private float _MaxRight;
    private Dictionary<int, SubEffectInfo> _SubEffectInfos = new Dictionary<int, SubEffectInfo>();

    public class SubEffectInfo
    {
        public Vector3 _SubEffectStart = Vector3.zero;
        public Vector3 _SubEffectEndBallUpLeft = Vector3.zero;
        public Vector3 _SubEffectEndBallDownRight = Vector3.zero;
    }

    public void ResetPos()
    {
        _UP.transform.localPosition = Vector2.zero;

        _Down.transform.localPosition = Vector2.zero;

        _Left.transform.localPosition = Vector2.zero;

        _Right.transform.localPosition = Vector2.zero;

        foreach (var subEffectGO in _UpSub)
        {
            subEffectGO.SetActive(false);
        }
        foreach (var subEffectGO in _DownSub)
        {
            subEffectGO.SetActive(false);
        }
        foreach (var subEffectGO in _LeftSub)
        {
            subEffectGO.SetActive(false);
        }
        foreach (var subEffectGO in _RightSub)
        {
            subEffectGO.SetActive(false);
        }
    }

    private void SetMaxPos(List<BallInfo> elimitBalls)
    {
        BallInfo upBall = _BaseBall;
        BallInfo downBall = _BaseBall;
        BallInfo leftBall = _BaseBall;
        BallInfo rightBall = _BaseBall;

        List<BallInfo> outLineBalls = new List<BallInfo>();

        foreach (var elimitBall in elimitBalls)
        {
            if (upBall.Pos.y < elimitBall.Pos.y && _BaseBall.Pos.x == elimitBall.Pos.x)
            {
                upBall = elimitBall;
            }

            if (downBall.Pos.y > elimitBall.Pos.y && _BaseBall.Pos.x == elimitBall.Pos.x)
            {
                downBall = elimitBall;
            }

            if (leftBall.Pos.x > elimitBall.Pos.x && _BaseBall.Pos.y == elimitBall.Pos.y)
            {
                leftBall = elimitBall;
            }

            if (rightBall.Pos.x < elimitBall.Pos.x && _BaseBall.Pos.y == elimitBall.Pos.y)
            {
                rightBall = elimitBall;
            }

            if (_BaseBall.Pos.y != elimitBall.Pos.y && _BaseBall.Pos.x != elimitBall.Pos.x)
            {
                outLineBalls.Add(elimitBall);
            }
        }

        var uiUpBall = UIFightBox.GetFightBall(upBall);
        var uiDownBall = UIFightBox.GetFightBall(downBall);
        var uiLeftBall = UIFightBox.GetFightBall(leftBall);
        var uiRightBall = UIFightBox.GetFightBall(rightBall);

        _MaxUp = uiUpBall.transform.position.y;
        _MaxDown = uiDownBall.transform.position.y;
        _MaxLeft = uiLeftBall.transform.position.x;
        _MaxRight = uiRightBall.transform.position.x;

        _StartTime = Time.time;

        foreach (var outlineBall in outLineBalls)
        {
            var uiOutLineBall = UIFightBox.GetFightBall(outlineBall);
            if ((int)outlineBall.Pos.y == (int)upBall.Pos.y && Mathf.Abs( outlineBall.Pos.x - upBall.Pos.x) < 2)
            {
                if (!_SubEffectInfos.ContainsKey(1))
                {
                    _SubEffectInfos.Add(1, new SubEffectInfo());
                    _SubEffectInfos[1]._SubEffectStart = uiUpBall.transform.position;
                }
                if (outlineBall.Pos.x < upBall.Pos.x)
                {
                    _SubEffectInfos[1]._SubEffectEndBallUpLeft = uiOutLineBall.transform.position;
                    _LeftSub[0].SetActive(true);
                }
                else
                {
                    _SubEffectInfos[1]._SubEffectEndBallDownRight = uiOutLineBall.transform.position;
                    _RightSub[0].SetActive(true);
                }
            }
            else if ((int)outlineBall.Pos.y == (int)downBall.Pos.y && Mathf.Abs(outlineBall.Pos.x - downBall.Pos.x) < 2)
            {
                if (!_SubEffectInfos.ContainsKey(2))
                {
                    _SubEffectInfos.Add(2, new SubEffectInfo());
                    _SubEffectInfos[2]._SubEffectStart = uiDownBall.transform.position;
                }
                if (outlineBall.Pos.x < downBall.Pos.x)
                {
                    _SubEffectInfos[2]._SubEffectEndBallUpLeft = uiOutLineBall.transform.position;
                    _LeftSub[1].SetActive(true);
                }
                else
                {
                    _SubEffectInfos[2]._SubEffectEndBallDownRight = uiOutLineBall.transform.position;
                    _RightSub[1].SetActive(true);
                }
            }
            else if ((int)outlineBall.Pos.x == (int)leftBall.Pos.x && Mathf.Abs(outlineBall.Pos.y - leftBall.Pos.y) < 2)
            {
                if (!_SubEffectInfos.ContainsKey(3))
                {
                    _SubEffectInfos.Add(3, new SubEffectInfo());
                    _SubEffectInfos[3]._SubEffectStart = uiLeftBall.transform.position;
                }
                if (outlineBall.Pos.y > leftBall.Pos.y)
                {
                    _SubEffectInfos[3]._SubEffectEndBallUpLeft = uiOutLineBall.transform.position;
                    _UpSub[0].SetActive(true);
                }
                else
                {
                    _SubEffectInfos[3]._SubEffectEndBallDownRight = uiOutLineBall.transform.position;
                    _DownSub[0].SetActive(true);
                }
            }
            else if ((int)outlineBall.Pos.x == (int)rightBall.Pos.x && Mathf.Abs(outlineBall.Pos.y - rightBall.Pos.y) < 2)
            {
                if (!_SubEffectInfos.ContainsKey(4))
                {
                    _SubEffectInfos.Add(4, new SubEffectInfo());
                    _SubEffectInfos[4]._SubEffectStart = uiRightBall.transform.position;
                }
                if (outlineBall.Pos.y > rightBall.Pos.y)
                {
                    _SubEffectInfos[4]._SubEffectEndBallUpLeft = uiOutLineBall.transform.position;
                    _UpSub[1].SetActive(true);
                }
                else
                {
                    _SubEffectInfos[4]._SubEffectEndBallDownRight = uiOutLineBall.transform.position;
                    _DownSub[1].SetActive(true);
                }
            }
        }
    }

    public void StartEffect(BallType ballType, BallInfo baseBall, List<BallInfo> elimitBalls, bool isSmallLine)
    {
        _BaseBall = baseBall;

        ResetPos();        
        
        SetMaxPos(elimitBalls);

        if (isSmallLine)
        {
            _UpLarge.SetActive(false);
            _DownLarge.SetActive(false);
            _LeftLarge.SetActive(false);
            _RightLarge.SetActive(false);

            _UpSmall.SetActive(true);
            _DownSmall.SetActive(true);
            _LeftSmall.SetActive(true);
            _RightSmall.SetActive(true);
        }
        else
        {
            _UpLarge.SetActive(true);
            _DownLarge.SetActive(true);
            _LeftLarge.SetActive(true);
            _RightLarge.SetActive(true);

            _UpSmall.SetActive(false);
            _DownSmall.SetActive(false);
            _LeftSmall.SetActive(false);
            _RightSmall.SetActive(false);
        }

        switch (ballType)
        {
            case BallType.LineClumn:
            case BallType.LineClumnAuto:
            case BallType.LineClumnEnlarge:
            case BallType.LineClumnHitTrap:
            case BallType.LineClumnLighting:
            case BallType.LineClumnReact:
                _UP.SetActive(true);
                _Down.SetActive(true);
                break;
            case BallType.LineRow:
            case BallType.LineRowAuto:
            case BallType.LineRowEnlarge:
            case BallType.LineRowHitTrap:
            case BallType.LineRowLighting:
            case BallType.LineRowReact:
                _Left.SetActive(true);
                _Right.SetActive(true);
                break;
            case BallType.LineCross:
            case BallType.LineCrossAuto:
            case BallType.LineCrossEnlarge:
            case BallType.LineCrossHitTrap:
            case BallType.LineCrossLighting:
            case BallType.LineCrossReact:
                _UP.SetActive(true);
                _Down.SetActive(true);
                _Left.SetActive(true);
                _Right.SetActive(true);
                break;
        }
    }

    public void EffectLineFlyUpdate()
    {
        if (_UP.activeSelf)
        {
            _UP.transform.position += new Vector3(0, _Speed * Time.deltaTime, 0);
            if (_UP.transform.position.y >= _MaxUp)
            {
                _UP.SetActive(false);
                MoveFinish();
            }
        }

        if (_Down.activeSelf)
        {
            _Down.transform.position -= new Vector3(0, _Speed * Time.deltaTime, 0);
            if (_Down.transform.position.y <= _MaxDown)
            {
                _Down.SetActive(false);
                MoveFinish();
            }
        }

        if (_Right.activeSelf)
        {
            _Right.transform.position += new Vector3(_Speed * Time.deltaTime, 0, 0);
            if (_Right.transform.position.x >= _MaxRight)
            {
                _Right.SetActive(false);
                MoveFinish();
            }
        }

        if (_Left.activeSelf)
        {
            _Left.transform.position -= new Vector3(_Speed * Time.deltaTime, 0, 0);
            if (_Left.transform.position.x <= _MaxLeft)
            {
                _Left.SetActive(false);
                MoveFinish();
            }
        }
        
        float deltaTime = Time.time - _StartTime;
        if (_SubEffectInfos.ContainsKey(1))
        {
            if (_SubEffectInfos[1]._SubEffectEndBallUpLeft != Vector3.zero)
            {
                if (_LeftSub[0].activeSelf)
                {
                    _LeftSub[0].transform.position = new Vector3(-deltaTime * _Speed,0,0) + _SubEffectInfos[1]._SubEffectStart;
                    if (_LeftSub[0].transform.position.x < _SubEffectInfos[1]._SubEffectEndBallUpLeft.x)
                    {
                        _LeftSub[0].SetActive(false);
                    }
                }
            }

            if (_SubEffectInfos[1]._SubEffectEndBallDownRight != Vector3.zero)
            {
                if (_RightSub[0].activeSelf)
                {
                    _RightSub[0].transform.position = new Vector3(deltaTime * _Speed, 0, 0) + _SubEffectInfos[1]._SubEffectStart;
                    if (_RightSub[0].transform.position.x > _SubEffectInfos[1]._SubEffectEndBallDownRight.x)
                    {
                        _RightSub[0].SetActive(false);
                    }
                }
            }
        }

        if (_SubEffectInfos.ContainsKey(2))
        {
            if (_SubEffectInfos[2]._SubEffectEndBallUpLeft != Vector3.zero)
            {
                if (_LeftSub[1].activeSelf)
                {
                    _LeftSub[1].transform.position = new Vector3(-deltaTime * _Speed, 0, 0) + _SubEffectInfos[2]._SubEffectStart;
                    if (_LeftSub[1].transform.position.x < _SubEffectInfos[2]._SubEffectEndBallUpLeft.x)
                    {
                        _LeftSub[1].SetActive(false);
                    }
                }
            }

            if (_SubEffectInfos[2]._SubEffectEndBallDownRight != Vector3.zero)
            {
                if (_RightSub[1].activeSelf)
                {
                    _RightSub[1].transform.position = new Vector3(deltaTime * _Speed, 0, 0) + _SubEffectInfos[2]._SubEffectStart;
                    if (_RightSub[1].transform.position.x > _SubEffectInfos[2]._SubEffectEndBallDownRight.x)
                    {
                        _RightSub[1].SetActive(false);
                    }
                }
            }
        }

        if (_SubEffectInfos.ContainsKey(3))
        {
            if (_SubEffectInfos[3]._SubEffectEndBallUpLeft != Vector3.zero)
            {
                if (_UpSub[0].activeSelf)
                {
                    _UpSub[0].transform.position = new Vector3(0, deltaTime * _Speed, 0) + _SubEffectInfos[3]._SubEffectStart;
                    if (_UpSub[0].transform.position.y > _SubEffectInfos[3]._SubEffectEndBallUpLeft.y)
                    {
                        _UpSub[0].SetActive(false);
                    }
                }
            }

            if (_SubEffectInfos[3]._SubEffectEndBallDownRight != Vector3.zero)
            {
                if (_DownSub[0].activeSelf)
                {
                    _DownSub[0].transform.position = new Vector3(0, -deltaTime * _Speed, 0) + _SubEffectInfos[3]._SubEffectStart;
                    if (_DownSub[0].transform.position.y < _SubEffectInfos[3]._SubEffectEndBallDownRight.y)
                    {
                        _DownSub[0].SetActive(false);
                    }
                }
            }
        }

        if (_SubEffectInfos.ContainsKey(4))
        {
            if (_SubEffectInfos[4]._SubEffectEndBallUpLeft != Vector3.zero)
            {
                if (_UpSub[1].activeSelf)
                {
                    _UpSub[1].transform.position = new Vector3(0, deltaTime * _Speed, 0) + _SubEffectInfos[4]._SubEffectStart;
                    if (_UpSub[1].transform.position.y > _SubEffectInfos[4]._SubEffectEndBallUpLeft.y)
                    {
                        _UpSub[1].SetActive(false);
                    }
                }
            }

            if (_SubEffectInfos[4]._SubEffectEndBallDownRight != Vector3.zero)
            {
                if (_DownSub[1].activeSelf)
                {
                    _DownSub[1].transform.position = new Vector3(0, -deltaTime * _Speed, 0) + _SubEffectInfos[4]._SubEffectStart;
                    if (_DownSub[1].transform.position.y < _SubEffectInfos[4]._SubEffectEndBallDownRight.y)
                    {
                        _DownSub[1].SetActive(false);
                    }
                }
            }
        }
    }

    public void MoveFinish()
    {
        if (_UP.activeSelf || _Down.activeSelf || _Left.activeSelf || _Right.activeSelf)
        {
            EffectFinish();
        }
        else
        {

        }
    }

    public void EffectFinish()
    {
        //ResourcePool.Instance.RecvIldeEffect(this);
    }

    #endregion
}
