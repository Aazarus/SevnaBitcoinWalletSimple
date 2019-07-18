// <copyright file="IBitcoinLibrary.cs" company="Sevna Software LTD">
// Copyright (c) Sevna Software LTD. All rights reserved.
// </copyright>

namespace SevnaBitcoinWallet.Wrapper
{
  using System.Collections.Generic;
  using System.Security;
  using SevnaBitcoinWallet.Exceptions;

  /// <summary>
  /// Defines the interface for the BitcoinLibrary.
  /// </summary>
  public interface IBitcoinLibrary
  {
    /// <summary>
    /// Identifies the command and forwards to correct method for processing.
    /// </summary>
    /// <param name="commandWithArguments">The command to process with any available arguments.</param>
    /// <param name="additionalCommandArguments">Any additional arguments required by a command. i.e. SecureString for GenerateWallet.</param>
    /// <returns>Result of running command.</returns>
    string ProcessCommand(List<string> commandWithArguments, object additionalCommandArguments = null);

    /// <summary>
    /// Generates a wallet using the provided argument.
    /// </summary>
    /// <param name="args">The arguments for the command.</param>
    /// <param name="password">Bitcoin wallet password.</param>
    /// <returns>Comma separated wallet mnemonic.</returns>
    /// <exception cref="WalletAlreadyExistsException">The requested wallet name already exists.</exception>
    /// <exception cref="GenerateWalletFailedException">Failed to generate wallet.</exception>
    string GenerateWallet(string[] args, SecureString password);

    /// <summary>
    /// Recreates a wallet based on the provided mnemonic.
    /// </summary>
    /// <param name="args">[0]"wallet-file=*Wallet file name*", [1]"mnemonic=*Comma separated mnemonic*.</param>
    /// <param name="password">SecureString password.</param>
    /// <returns>Location of generated wallet.</returns>
    /// <exception cref="WalletAlreadyExistsException">The requested wallet name already exists.</exception>
    /// <exception cref="GenerateWalletFailedException">Failed to generate wallet.</exception>
    string ProcessRecoverWallet(string[] args, SecureString password);
  }
}