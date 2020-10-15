using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Threading;

namespace TestAnimatedGraph.Model
{
    public class Broker
    {
        public BrokerBalance BrokerBalance;
        public int CountOfSharesForOperation;
        public Broker()
        {
            CountOfSharesForOperation = 0;
            BrokerBalance = new BrokerBalance
            {
                BalanceValue = 1000,
                CountOfShares = 0
            };
        }

        public void Buy(int buyValue, int countOfShares)
        {
            if(BrokerBalance.BalanceValue - buyValue * countOfShares > 0)
            {
                BrokerBalance.CountOfShares += countOfShares;
                BrokerBalance.BalanceValue -= buyValue * countOfShares;
            }
        }
        public void Sale(int saleValue, int countOfShares)
        {
            if(BrokerBalance.CountOfShares - countOfShares > 0)
            {
                BrokerBalance.CountOfShares -= countOfShares;
                BrokerBalance.BalanceValue += saleValue * countOfShares;
            }
        }
    }
}
