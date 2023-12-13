using System.Collections.Generic;
using UnityEngine;

public class UnitHandler : ICombatStatHandler
{
    public CombatStat CombatStat => _unit.Stat;
    public Unit Unit => _unit;
    private readonly Unit _unit;

    public UnitState State;

    private readonly IBattleHandler _battleHandler;
    public bool IsTargetOnMyLeftSide { get; private set; }
    // BFS
    private Queue<Vector2Int> _queue;
    private bool[,] _visit;
    public UnitHandler(IBattleHandler battleHandler, Unit unit)
    {
        _unit = unit;
        _battleHandler = battleHandler;

        _queue = new();
    }

    public void AddHP(int hp)
    {
        if (State == UnitState.Dead) return;
        CombatStat.CurHealth += hp;

        if (CombatStat.CurHealth > CombatStat.Stat[(int)Stats.MaxHealth])
            CombatStat.CurHealth = CombatStat.Stat[(int)Stats.MaxHealth];
        else if (CombatStat.CurHealth <= 0)
        {
            State = UnitState.Dead;
            _battleHandler.Map[_unit.Pos.x, _unit.Pos.y] = -1;
        }
    }

    public void Modify(StatValue statValue, ModifyType modifyType)
    {
        switch (modifyType)
        {
            case ModifyType.Add:
                if (statValue.Stat == Stats.CurHealth) AddHP((int)statValue.Value);
                else if (statValue.Stat == Stats.CurMana) return;
                else _unit.Stat.Stat[(int)statValue.Stat] = (int)(_unit.Stat.Stat[(int)statValue.Stat] + statValue.Value);
                break;
            case ModifyType.Override:
                if (statValue.Stat == Stats.CurHealth) _unit.Stat.CurHealth = (int)statValue.Value;
                else if (statValue.Stat == Stats.CurMana) _unit.Stat.CurMana = (int)statValue.Value;
                else _unit.Stat.Stat[(int)statValue.Stat] = (int)statValue.Value;
                break;
            case ModifyType.Multiple:
                if (statValue.Stat == Stats.CurHealth) return;
                else if (statValue.Stat == Stats.CurMana) return;
                else _unit.Stat.Stat[(int)statValue.Stat] = (int)(_unit.Stat.Stat[(int)statValue.Stat] * statValue.Value);
                break;
        }
    }

    public void Tick()
    {
        if (IsTargetNull())
        {
            FindTarget();

            if (IsTargetNull()) return;
        }

        if (IsTargetInAttackRange())
        {
            if (HasEnoughManaForSkill()) Skill();
            else Attack();
        }
        else
        {
            Vector2Int actDirection = FindPath();

            Move(actDirection);
        }
    }

    public void TakeDamage(int damage)
    {
        if (CombatStat.Stat[(int)Stats.DodgeChance] > Random.Range(0, 100))
        {
            // dodge
            return;
        }

        damage = damage * 100 / (100 + CombatStat.Stat[(int)Stats.Armor]);
        AddHP(-damage);
    }
    private bool IsTargetNull()
    {
        return _unit.TargetId == -1 || !_battleHandler.Units.ContainsKey(_unit.TargetId) || _battleHandler.Units[_unit.TargetId].State == UnitState.Dead;
    }
    private bool IsTargetInAttackRange()
    {
        Vector2Int targetPos = _battleHandler.Units[_unit.TargetId].Unit.Pos;
        int dx = targetPos.x - _unit.Pos.x;
        int dy = targetPos.y - _unit.Pos.y;
        dx = dx > 0 ? dx : -dx;
        dy = dy > 0 ? dy : -dy;

        return CombatStat.Stat[(int)Stats.AtkRange] >= dx + dy;
    }
    private void Move(Vector2Int moveDirect)
    {
        if (moveDirect == Vector2Int.zero) return;
        State = UnitState.Move;

        _battleHandler.Map[_unit.Pos.x, _unit.Pos.y] = -1;
        _unit.Pos += moveDirect;
        _battleHandler.Map[_unit.Pos.x, _unit.Pos.y] = _unit.Id;
    }
    private void FindTarget()
    {
        int width = _battleHandler.Map.GetLength(0);
        int height = _battleHandler.Map.GetLength(1);
        _queue.Clear();
        _visit = new bool[width, height];

        Vector2Int agent = _unit.Pos;

        _queue.Enqueue(agent); // start
        _visit[agent.x, agent.y] = true;

        while (IsTargetNull() && _queue.Count > 0)
        {
            agent = _queue.Dequeue();

            for(int range = 1; range <= _unit.Stat.Stat[(int)Stats.AtkRange]; range++)
            {
                foreach (Vector2Int originalDir in Const.Dirs[range])
                {
                    Vector2Int dir = originalDir;
                    if(!_unit.IsTeamHero) dir *= -1;
                    Vector2Int pos = agent + dir;

                    if (pos.x < width && pos.y < height && pos.x >= 0 && pos.y >= 0 && !_visit[pos.x, pos.y])
                    {
                        int id = _battleHandler.Map[pos.x, pos.y];

                        if ((id != -1) && (_unit.IsTeamHero != _battleHandler.Units[id].Unit.IsTeamHero))
                        {
                            _unit.TargetId = id;
                            IsTargetOnMyLeftSide = _battleHandler.Units[id].Unit.Pos.x - _unit.Pos.x < 0;
                            return;
                        }
                        else if (id == -1)
                        {
                            if(range == 1)
                            {
                                _queue.Enqueue(pos);
                                _visit[pos.x, pos.y] = true;
                            }
                        }
                    }
                }
            }
        }
    }
    private Vector2Int FindPath()
    {
        int width = _battleHandler.Map.GetLength(0);
        int height = _battleHandler.Map.GetLength(1);
        // simple A*
        float min = width + height;
        Vector2Int path = Vector2Int.zero;
        Vector2Int targetPos = _battleHandler.Units[_unit.TargetId].Unit.Pos;

        foreach (Vector2Int originalDir in Const.Dirs[1])
        {
            Vector2Int dir = originalDir;
            if (!_unit.IsTeamHero) dir *= -1;
            Vector2Int pos = _unit.Pos + dir;

            if (pos.x < width && pos.y < height && pos.x >= 0 && pos.y >= 0 && _battleHandler.Map[pos.x, pos.y] == -1)
            {
                float distance = Vector2Int.Distance(targetPos, pos);
                if (min > distance)
                {
                    min = distance;
                    path = dir;
                }
            }
        }

        return path;
    }

    private bool HasEnoughManaForSkill()
    {
        return CombatStat.CurMana >= CombatStat.Stat[(int)Stats.MaxMana];
    }
    private int GetDamage()
    {
        // 스킬은 고려하지 않음
        int damage;

        damage = CombatStat.Stat[(int)Stats.AtkPower];

        int validCritical = CombatStat.Stat[(int)Stats.CriticalChance] / 100;

        if (CombatStat.Stat[(int)Stats.CriticalChance] % 100 > Random.Range(0, 100))
        {
            validCritical++;
        }

        damage *= (100 + validCritical * CombatStat.Stat[(int)Stats.CriticalDamage]);
        damage /= 100;

        return damage;
    }

    public void Attack()
    {
        if (State == UnitState.Attack)
        {
            int damage;
            damage = GetDamage();

            _battleHandler.Units[_unit.TargetId].TakeDamage(damage);
        }
        else
        {
            IsTargetOnMyLeftSide = _battleHandler.Units[_unit.TargetId].Unit.Pos.x - _unit.Pos.x < 0;
            CombatStat.CurMana += CombatStat.Stat[(int)Stats.GenMana];
            State = UnitState.Attack;
        }
    }
    public void Skill()
    {
        if (State == UnitState.Skill)
        {
            int damage;
            damage = GetDamage();

            damage = (int)(damage * (1 + CombatStat.Stat[(int)Stats.MagPower] * 0.01f));

            _battleHandler.Units[_unit.TargetId].TakeDamage(damage);
        }
        else
        {
            IsTargetOnMyLeftSide = _battleHandler.Units[_unit.TargetId].Unit.Pos.x - _unit.Pos.x < 0;
            CombatStat.CurMana = 0;
            State = UnitState.Skill;
        }
    }
}