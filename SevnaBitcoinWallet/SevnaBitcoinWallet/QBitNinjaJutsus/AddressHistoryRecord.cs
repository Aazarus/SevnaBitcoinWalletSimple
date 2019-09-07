#pragma warning disable SA1200 // Using directives should be placed correctly
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using NBitcoin;
using QBitNinja.Client.Models;

[assembly: SuppressMessage("CSharp.DocumentationRules", "*", Scope = "Namespace", Target = "StyleCopSample.Test", Justification = "3rd party code.")]

namespace DotNetWallet.QBitNinjaJutsus
{

#pragma warning restore SA1200 // Using directives should be placed correctly
#pragma warning disable SA1520 // Use braces consistently
#pragma warning disable SA1606 // Element documentation should have summary text
#pragma warning disable SA1600 // Elements should be documented
#pragma warning disable SA1401 // Fields should be private
  public class AddressHistoryRecord
  {
    public readonly BalanceOperation Operation;
    public readonly BitcoinAddress Address;

    public AddressHistoryRecord(BitcoinAddress address, BalanceOperation operation)
    {
      this.Address = address;
      this.Operation = operation;
    }

    public Money Amount
    {
      get
      {
        var amount = (from Coin coin in this.Operation.ReceivedCoins
                      let address =
                        coin.GetScriptCode().GetDestinationAddress(this.Address.Network)
                      where address == this.Address
                      select coin.Amount).Sum();
        return (from Coin coin in this.Operation.SpentCoins
                let address =
                  coin.GetScriptCode().GetDestinationAddress(this.Address.Network)
                where address == this.Address
                select coin)
          .Aggregate(amount, (current, coin) => current - coin.Amount);
      }
    }

    public DateTimeOffset FirstSeen => this.Operation.FirstSeen;

    public bool Confirmed => this.Operation.Confirmations > 0;

    public uint256 TransactionId => this.Operation.TransactionId;
  }
}
#pragma warning restore SA1600 // Elements should be documented
#pragma warning restore SA1520 // Use braces consistently
#pragma warning restore SA1606 // Element documentation should have summary text
#pragma warning restore SA1401 // Fields should be private
