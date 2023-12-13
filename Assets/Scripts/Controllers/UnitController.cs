using UnityEngine;
using UnityEngine.UI;

public class UnitController : MonoBehaviour
{
    private UnitHandler _unitHandler;
    [SerializeField] private UnitAnimatorController _unitAnimatorController;

    private float _moveDelay = 1;
    private float _healthGenDelay = 1;
    private float _attackDelay;

    private float _counter;
    private float _timer;

    private Vector3 _moveVector;

    [SerializeField] private Slider _hpSlider;
    [SerializeField] private Slider _mpSlider;
    private float _maxHP;
    private float _maxMP;

    public void Init(UnitHandler unitHandler)
    { 
        _unitHandler = unitHandler;
        _unitHandler.State = UnitState.Idle;

        _counter = 0;

        _moveVector = Vector3.zero;
        Vector2Int pos = unitHandler.Unit.Pos;
        transform.localPosition = new Vector3(pos.x, 0, pos.y);

        _maxHP = _unitHandler.CombatStat.Stat[(int)Stats.MaxHealth];
        _maxMP = _unitHandler.CombatStat.Stat[(int)Stats.MaxMana];
        _attackDelay = 100f / _unitHandler.CombatStat.Stat[(int)Stats.AtkSpeed];

        _unitAnimatorController.Init(_unitHandler.Unit.SpriteId);
        _unitAnimatorController.Turn(!_unitHandler.Unit.IsTeamHero);
    }

    private void Update()
    {
        GenHealth();

        _hpSlider.value = _unitHandler.CombatStat.CurHealth / _maxHP;
        _mpSlider.value = _unitHandler.CombatStat.CurMana / _maxMP;

        switch (_unitHandler.State)
        {
            case UnitState.Idle:
                _unitHandler.Tick();
                break;
            case UnitState.Move:
                Move();
                break;
            case UnitState.Attack:
                Attack();
                break;
            case UnitState.Skill:
                Skill();
                break;
            case UnitState.Dead:
                gameObject.SetActive(false);
                break;
        }
    }

    private void Move()
    {
        if (_moveVector == Vector3.zero)
        {
            _unitAnimatorController.SetState(AnimationState.Walking);

            Vector2Int pos = _unitHandler.Unit.Pos;
            _moveVector = new Vector3(pos.x - transform.localPosition.x, 0, pos.y - transform.localPosition.z) / _moveDelay;

            if (_moveVector.x == 0) _unitAnimatorController.Turn(isTurnLeft: _unitHandler.IsTargetOnMyLeftSide);
            else _unitAnimatorController.Turn(_moveVector.x < 0);
        }

        transform.localPosition += _moveVector * Time.deltaTime;
        _counter += Time.deltaTime;
        if (_counter >= _moveDelay)
        {
            Vector2Int pos = _unitHandler.Unit.Pos;
            transform.localPosition = new Vector3(pos.x, 0, pos.y);

            _unitAnimatorController.SetState(AnimationState.Idle);

            _moveVector = Vector3.zero;
            _unitHandler.State = UnitState.Idle;
            _counter = 0;
        }
    }

    private void Attack()
    {
        if (_counter == 0)
        {
            _unitHandler.Attack();
            _unitAnimatorController.Turn(isTurnLeft: _unitHandler.IsTargetOnMyLeftSide);
            _unitAnimatorController.SetTrigger(AnimationTrigger.Jab);

            Managers.Instance.SoundManager.PlaySFX(SFXSource.Hit);
        }

        _counter += Time.deltaTime;

        if (_counter >= _attackDelay)
        {
            _unitHandler.State = UnitState.Idle;
            _counter = 0;
        }
    }

    private void Skill()
    {
        if (_counter == 0)
        {
            _unitHandler.Skill();
            _unitAnimatorController.Turn(isTurnLeft: _unitHandler.IsTargetOnMyLeftSide);
            _unitAnimatorController.SetTrigger(AnimationTrigger.Slash);

            Managers.Instance.SoundManager.PlaySFX(SFXSource.MeleeAttack);
        }

        _counter += Time.deltaTime;

        if (_counter >= _attackDelay)
        {
            _unitHandler.State = UnitState.Idle;
            _counter = 0;
        }
    }

    private void GenHealth()
    {
        _timer += Time.deltaTime;

        if (_timer >= _healthGenDelay)
        {
            _unitHandler.AddHP(_unitHandler.CombatStat.Stat[(int)Stats.GenHealth]);
            _timer = 0;
        }
            
    }
}