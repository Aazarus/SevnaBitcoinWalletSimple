// <copyright file="BitcoinLibraryTests.cs" company="Sevna Software LTD">
// Copyright (c) Sevna Software LTD. All rights reserved.
// </copyright>

namespace SevnaBitcoinWallet.Tests.Wrapper
{
  using System;
  using System.IO;
  using System.Security;
  using FluentAssertions;
  using SevnaBitcoinWallet.Exceptions;
  using SevnaBitcoinWallet.Wrapper;
  using Xunit;

  /// <summary>
  /// Tests the BitcoinLibrary class.
  /// </summary>
  public class BitcoinLibraryTests
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="BitcoinLibraryTests"/> class.
    /// </summary>
    public BitcoinLibraryTests()
    {
      try
      {
        // Ensure Wallets directory doesn't already exist
        Directory.Delete("Wallets", true);
      }
      catch (DirectoryNotFoundException)
      {
        // This is not a problem as we are deleting the directory anyway.
      }
    }

    /// <summary>
    /// Tests the GenerateWallet method successfully generates a wallet.
    /// </summary>
    [Fact]
    public void ProcessGenerateWallet_ShouldSuccessfullyGenerateAWallet()
    {
      // Arrange
      var args = new[]
      {
        "wallet-file=wallet.json",
      };
      var expectedPathToFile = Path.Combine("Wallets", "wallet.json");
      var password = new SecureString();
      password.AppendChar('p');
      password.AppendChar('a');
      password.AppendChar('s');
      password.AppendChar('s');
      password.AppendChar('w');
      password.AppendChar('o');
      password.AppendChar('r');
      password.AppendChar('d');

      // Act
      var mnemonic = BitcoinLibrary.GenerateWallet(args, password);

      // Assert
      mnemonic.Should().NotBeNull();
      mnemonic.Split(",").Length.Should().Be(12);
      File.Exists(expectedPathToFile).Should().BeTrue();

      // Cleanup
      // Ensure Wallets directory doesn't already exist
      Directory.Delete("Wallets", true);
    }

    /// <summary>
    /// Tests the GenerateWallet method should throw an exception if the wallet file already exists.
    /// </summary>
    [Fact]
    public void ProcessGenerateWallet_ShouldThrowAnExceptionIfWalletAlreadyExists()
    {
      // Arrange
      const string walletFileName = "wallet_generate_test.json";
      using (File.Create(walletFileName))
      {
      }

      var args = new[]
      {
        $"wallet-file={walletFileName}",
      };
      var password = new SecureString();
      password.AppendChar('p');
      password.AppendChar('a');
      password.AppendChar('s');
      password.AppendChar('s');
      password.AppendChar('w');
      password.AppendChar('o');
      password.AppendChar('r');
      password.AppendChar('d');

      try
      {
        // Act
        BitcoinLibrary.GenerateWallet(args, password);
      }
      catch (WalletAlreadyExistsException ex)
      {
        // Assert
        ex.Message.Should().Be("The request to generate a new wallet failed because the wallet already exists.");
      }
      catch (Exception ex)
      {
        // Should not get here.
        ex.Should().BeNull();
      }

      // Cleanup
      Directory.Delete("Wallets", true);
      File.Delete(walletFileName);  // Default results in locally created wallet file.
    }

    /// <summary>
    /// Tests the GenerateWallet method should throw an exception if the argument is invalid.
    /// </summary>
    [Fact]
    public void ProcessGenerateWallet_ShouldThrowAnExceptionIfArgumentIsInvalid()
    {
      // Arrange
      var args = new[]
      {
        "wallet-file=",
      };
      var password = new SecureString();
      password.AppendChar('p');
      password.AppendChar('a');
      password.AppendChar('s');
      password.AppendChar('s');
      password.AppendChar('w');
      password.AppendChar('o');
      password.AppendChar('r');
      password.AppendChar('d');

      try
      {
        // Act
        BitcoinLibrary.GenerateWallet(args, password);
      }
      catch (GenerateWalletFailedException ex)
      {
        // Assert
        ex.Message.Should().Be("Could not generate wallet 'wallet-file' argument is invalid.");
      }
      catch (Exception ex)
      {
        // Should not get here.
        ex.Should().BeNull();
      }

      // Cleanup
      File.Delete("BitcoinWallet.json");
    }

    /// <summary>
    /// Tests the ProcessShowHistory method correctly processes a valid Wallet File for address details.
    /// </summary>
    [Fact]
    public void ProcessShowHistory_ShouldReturnTheDetailsOfAnAddressWhenGivenValidWalletFile()
    {
      // Arrange

      // Act
      
      // Assert
      Assert.True(false);
    }
  }
}
