using System;

namespace ITLIBRIUM.BddToolkit.Examples
{
    public class PrePaidAccount
    {
        public PrePaidAccountId Id { get; } 
        
        private Money _amountAvailable;
        private Money _debtLimit;
        private Money _debt;
        
        public static PrePaidAccount New(Currency currency) => new PrePaidAccount(
            PrePaidAccountId.New(), 
            Money.Of(0, currency),
            Money.Of(0, currency),
            Money.Of(0, currency));
        
        public static PrePaidAccount Restore(Snapshot snapshot) => new PrePaidAccount(
            PrePaidAccountId.Of(snapshot.Id), 
            snapshot.AmountAvailable,
            snapshot.DebtLimit,
            snapshot.Debt);

        private PrePaidAccount(PrePaidAccountId id, Money amountAvailable, Money debtLimit, Money debt)
        {
            Id = id;
            _amountAvailable = amountAvailable;
            _debtLimit = debtLimit;
            _debt = debt;
        }
        
        public void Charge(Money amount)
        {
            if (amount <= _amountAvailable)
            {
                _amountAvailable -= amount;
            }
            else if (amount <= _amountAvailable + (_debtLimit - _debt))
            {
                _debt += (amount - _amountAvailable);
                _amountAvailable -= _amountAvailable;
            }
            else
            {
                throw new DomainException();
            }
        }
        
        public void Recharge(Money amount)
        {
            var amountToRecharge = amount - _debt;
            if (amountToRecharge.IsNegative)
            {
                _debt -= amount;
            }
            else
            {
                _debt -= _debt;
                _amountAvailable += amountToRecharge;
            }
        }

        public void GrantDebtLimit(Money debtLimit) => _debtLimit = debtLimit;
        
        public Snapshot GetSnapshot() => new Snapshot(Id.Value, _amountAvailable, _debtLimit, _debt);
        
        public readonly struct Snapshot
        {
            public Guid Id { get; }
            public Money AmountAvailable { get; }
            public Money DebtLimit { get; }
            public Money Debt { get; }

            public Snapshot(Guid id, Money amountAvailable, Money debtLimit, Money debt)
            {
                Id = id;
                AmountAvailable = amountAvailable;
                DebtLimit = debtLimit;
                Debt = debt;
            }
        }
    }
}