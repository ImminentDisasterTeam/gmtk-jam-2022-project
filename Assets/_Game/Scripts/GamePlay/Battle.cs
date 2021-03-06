using System;
using System.Linq;
using _Game.Scripts.Data;
using _Game.Scripts.UI;
using DG.Tweening;
using GeneralUtils;

namespace _Game.Scripts.GamePlay {
    public class Battle {
        private readonly EnemyData _enemy;
        private readonly Rng _rng;
        private readonly Action _reload;
        private readonly EnemyUI _enemyUI;
        private readonly Action<bool> _allowContinue;
        private readonly Event _onContinue;
        private readonly Func<(EActionType type, int result)> _getPlayerRoll;
        private readonly Action _disableInput;
        private readonly Action<bool> _finishBattle;

        private int _enemyHealth;
        private int _enemyCharge;
        private int _enemyActionPointer = -1;
        private int _chargeToRemove;
        private EnemyActionData CurrentAction => Action(_enemy.actionLoop[_enemyActionPointer]);

        private EnemyActionData _performedAction;
        private ValueWaiter<int> _syncWaiter;

        // I HAVENT SLEPT FOR SO LONG
        public Battle(RoomData data, Rng rng, Action reload, EnemyUI enemyUI, out Action<bool> onPlayerReady, Action<bool> allowContinue,
            Event onContinue, Func<(EActionType type, int result)> getPlayerRoll, Action disableInput, Action<bool> finishBattle) {
            var enemyName = rng.NextWeightedChoice(data.contentPool[0].WeightedItems);
            _enemy = DataHolder.Instance.GetEnemies().First(e => e.name == enemyName);
            _enemyHealth = _enemy.health;

            _rng = rng;
            _reload = reload;
            _enemyUI = enemyUI;
            _allowContinue = allowContinue;
            _onContinue = onContinue;
            _getPlayerRoll = getPlayerRoll;
            _disableInput = disableInput;
            _finishBattle = finishBattle;

            _enemyUI.Load(_enemy.health, SpriteHolder.Instance.GetSprite(_enemy.image));

            onPlayerReady = ready => _syncWaiter.Value += ready ? 1 : -1;

            _allowContinue(false);
            StartTurn();
        }

        private void StartTurn() {
            _reload();
            _onContinue.Subscribe(OnContinue);

            IncrementPointer();

            _chargeToRemove = 0;
            _performedAction = CurrentAction;
            while (_performedAction.HasCondition) {
                var condition = _performedAction.condition;
                var chargeHigherOK = condition.chargeHigher == 0 || _enemyCharge >= condition.chargeHigher;
                var chargeLowerOK = condition.chargeLower == 0 || _enemyCharge < condition.chargeLower;
                var healthHigherOK = condition.healthHigher == 0 || _enemyHealth >= condition.healthHigher;
                var healthLowerOK = condition.healthLower == 0 || _enemyHealth < condition.healthLower;
                if (chargeHigherOK && chargeLowerOK && healthHigherOK && healthLowerOK) {
                    _chargeToRemove = condition.chargeHigher;
                    break;
                }

                _performedAction = Action(condition.elseAction);
            }

            _syncWaiter = new ValueWaiter<int>();
            _syncWaiter.WaitForChange(WaitForChange);
            _enemyUI.SetAction(_performedAction.subActions, () => _syncWaiter.Value++);

            void WaitForChange() {
                _allowContinue(_syncWaiter.Value == 2);
                _syncWaiter.WaitForChange(WaitForChange);
            }
        }

        private void OnContinue() {
            _allowContinue(false);
            _onContinue.Unsubscribe(OnContinue);

            var enemyAttack = 0;
            var enemyDefence = 0;

            _enemyCharge += -_chargeToRemove;
            _enemyUI.SetCharge(_enemyCharge);

            var rolls = _enemyUI.Roll(_rng);
            for (var i = 0; i < _performedAction.subActions.Length; i++) {
                var type = _performedAction.subActions[i].Type;
                var result = rolls[i];

                switch (type) {
                    case EActionType.Attack:
                        enemyAttack += result;
                        break;
                    case EActionType.Defend:
                        enemyDefence += result;
                        break;
                    case EActionType.Charge:
                        _enemyCharge += result;
                        _enemyUI.SetCharge(_enemyCharge);
                        break;
                    case EActionType.Heal:
                        _enemyHealth += result;
                        _enemyUI.SetHealth(_enemyHealth);
                        break;
                    case EActionType.Wait:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            var (playerType, playerResult) = _getPlayerRoll();
            var playerAttack = 0;
            var playerDefense = 0;
            switch (playerType) {
                case EActionType.Attack:
                    playerAttack = playerResult;
                    SoundHolder.Instance.PlaySound("atk");
                    break;
                case EActionType.Defend:
                    playerDefense = playerResult;
                    SoundHolder.Instance.PlaySound("def");
                    break;
                case EActionType.UseItem:
                    Player.Instance.ChangeHealth(playerResult);
                    break;
                case EActionType.ChickenOut:
                    var escaped = playerResult >= Player.Instance.EscapeThreshold;
                    Player.Instance.RaiseEscapeThreshold();
                    InfoPanelUI.Instance.LoadEscapeResult(escaped);
                    if (escaped) {
                        FinishTurn(true, false);
                        return;
                    }
                    SoundHolder.Instance.PlaySound("escape");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            var playerDeltaHealth = Math.Min(0, playerDefense - enemyAttack);
            if (Player.Instance.ChangeHealth(playerDeltaHealth)) {
                FinishTurn(true, true);
                return;
            }

            var enemyDeltaHealth = Math.Min(0, enemyDefence - playerAttack);
            _enemyHealth = Math.Max(0, _enemyHealth + enemyDeltaHealth);
            _enemyUI.SetHealth(_enemyHealth);
            if (_enemyHealth == 0) {
                FinishTurn(true, false);
                return;
            }

            FinishTurn(false);

            void FinishTurn(bool finishBattle, bool finishByDeath = false) {
                _disableInput();
                DOVirtual.DelayedCall(0.8f, () => {
                    if (!finishBattle) {
                        StartTurn();
                        return;
                    }

                    _finishBattle(finishByDeath);
                });
            }
        }

        private void IncrementPointer() {
            _enemyActionPointer = (_enemyActionPointer + 1) % _enemy.actionLoop.Length;
        }
        
        private static EnemyActionData Action(string actionName)
            => DataHolder.Instance.GetEnemyActions().First(a => a.name == actionName);
    }
}