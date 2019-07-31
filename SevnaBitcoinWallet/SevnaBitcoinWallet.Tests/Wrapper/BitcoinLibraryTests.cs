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
      IBitcoinLibrary bitcoinLibrary = new BitcoinLibrary();

      // Act
      var mnemonic = bitcoinLibrary.GenerateWallet(args, password);

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
      const string walletDirectory = "Wallets";
      Directory.CreateDirectory(walletDirectory);
      const string walletFileName = "wallet_generate_test.json";
      using (File.Create(Path.Combine(walletDirectory, walletFileName)))
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

      IBitcoinLibrary bitcoinLibrary = new BitcoinLibrary();

      try
      {
        // Act
        bitcoinLibrary.GenerateWallet(args, password);

        // Should not get here - Force a fail if we do
        bitcoinLibrary.Should().BeNull();
      }
      catch (WalletAlreadyExistsException ex)
      {
        // Assert
        ex.Message.Should().Be("The request to generate a wallet failed because the wallet already exists.");
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
        "wallet-fi=", // Invalid spelling of 'wallet-file'
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

      IBitcoinLibrary bitcoinLibrary = new BitcoinLibrary();

      try
      {
        // Act
        bitcoinLibrary.GenerateWallet(args, password);

        // Should not get here - Force a fail if we do
        bitcoinLibrary.Should().BeNull();
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
    /// Tests the ProcessRecoverWallet correctly recovers a wallet using the given mnemonic.
    /// </summary>
    [Fact]
    public void ProcessRecoverWallet_ShouldRecoverAWalletWhenGivenValidArguments()
    {
      // Arrange
      const string walletFileName = "wallet_recover_test.json";
      const string mnemonic = "dash destroy twelve twice labor patch embrace embody chronic inch install term";
      var args = new[]
      {
        $"wallet-file={walletFileName}",
        $"mnemonic={mnemonic}",
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

      const string expectedEncryptedSeed = "6PYM714zxNRpmx7WRaCLNJyAreYx2BU5GkbCx5jF5QQNKeZExYqrNHzK8L";
      const string expectedChainCode = "CarQU+owbi2iML7Fy5vf+6O0Lpc/V//NFkk7WLVkh70=";
      IBitcoinLibrary bitcoinLibrary = new BitcoinLibrary();

      // Act
      var recoveredWalletLocation = string.Empty;
      try
      {
        recoveredWalletLocation = bitcoinLibrary.ProcessRecoverWallet(args, password);
      }
      catch (Exception ex)
      {
        ex.Should().BeNull();
      }

      // Assert
      File.Exists(recoveredWalletLocation).Should().BeTrue();
      var fileContents = File.ReadAllText(recoveredWalletLocation);

      // Check our Encrypted Seed exists
      fileContents.Should().Contain(expectedEncryptedSeed);

      // Check our Chain Code exists
      fileContents.Should().Contain(expectedChainCode);

      // Cleanup
      Directory.Delete("Wallets", true);
      File.Delete(walletFileName);
    }

    /// <summary>
    /// Tests the ProcessRecoverWallet throws a custom exception if the wallet to generate already exists
    /// by wallet name.
    /// </summary>
    [Fact]
    public void ProcessRecoverWallet_ShouldThrowCustomExceptionIfWalletExists()
    {
      // Arrange
      const string walletDirectoryName = "Wallets";
      const string walletFileName = "wallet_recover_exists_test.json";

      Directory.CreateDirectory(walletDirectoryName);
      using (File.Create(Path.Combine(walletDirectoryName, walletFileName)))
      {
      }

      const string mnemonic = "dash,destroy,twelve,twice,labor,patch,embrace,embody,chronic,inch,install,term";
      var args = new[]
      {
        $"wallet-file={walletFileName}",
        $"mnemonic={mnemonic}",
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
      IBitcoinLibrary bitcoinLibrary = new BitcoinLibrary();

      try
      {
        // Act
        bitcoinLibrary.ProcessRecoverWallet(args, password);

        // Should not get here - force a fail if we do
        bitcoinLibrary.Should().BeNull();
      }
      catch (WalletAlreadyExistsException ex)
      {
        // Assert
        ex.Message.Should().Be("The request to generate a wallet failed because the wallet already exists.");
      }

      // Cleanup
      Directory.Delete(walletDirectoryName, true);
      File.Delete(walletFileName);
    }

    /// <summary>
    /// Tests the ProcessRecoverWallet throws an InvalidMnemonicException on an invalid Mnemonic.
    /// </summary>
    [Fact]
    public void ProcessRecoverWallet_ShouldThrowCustomExceptionOnInvalidMnemonic()
    {
      // Arrange
      const string walletFileName = "wallet_recover_invalid_mnemonic_test.json";

      var args = new[]
      {
        $"wallet-file={walletFileName}",
        "mnemonic=InvalidMnemonic",
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

      IBitcoinLibrary bitcoinLibrary = new BitcoinLibrary();

      try
      {
        // Act
        bitcoinLibrary.ProcessRecoverWallet(args, password);

        // Should not get here - force a fail if we do
        bitcoinLibrary.Should().BeNull();
      }
      catch (InvalidMnemonicException ex)
      {
        // Assert
        ex.Message.Should().Be("Mnemonic is invalid.");
      }
    }

    /// <summary>
    /// Tests the ProcessShowBalances method returns the correct balance for a given wallet.
    /// </summary>
    [Fact]
    public void ProcessShowBalances_ShouldReturnBalanceForAGivenWallet()
    {
      // Arrange
      const string walletContents = "{ \"EncryptedSeed\": \"6PYM714zxNRpmx7WRaCLNJyAreYx2BU5GkbCx5jF5QQNKeZExYqrNHzK8L\",\"ChainCode\": \"CarQU+owbi2iML7Fy5vf+6O0Lpc/V//NFkk7WLVkh70=\",\"Network\": \"Main\",\"CreationTime\": \"2019-07-18\"}";

      const string walletDirectoryName = "Wallets";
      const string walletFileName = "wallet_show_balances_test.json";
      var walletFilePath = Path.Combine(walletDirectoryName, walletFileName);

      Directory.CreateDirectory(walletDirectoryName);
      using (File.Create(walletFilePath))
      {
      }

      File.WriteAllText(walletFilePath, walletContents);
      File.Exists(walletFilePath).Should().BeTrue();

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
      IBitcoinLibrary bitcoinLibrary = new BitcoinLibrary();

      try
      {
        // Act
        var result = bitcoinLibrary.ShowBalances(args, password);

        // Should not get here - force a fail if we do
        result.Should().NotBeNull();
      }
      catch (IncorrectWalletPasswordException ex)
      {
        // Assert
        ex.Message.Should().Contain("Provided password is incorrect.");
      }

      password.Should().NotBeNull();

      // Clean up
      Directory.Delete(walletDirectoryName, true);
      File.Delete(walletFileName);
    }

    /// <summary>
    /// Tests the ProcessShowBalances method throws an IncorrectWalletPasswordException.
    /// </summary>
    [Fact]
    public void ProcessShowBalances_ShouldThrowCustomExceptionForAnIncorrectPassword()
    {
      // Arrange
      const string walletContents = "{ \"EncryptedSeed\": \"6PYM714zxNRpmx7WRaCLNJyAreYx2BU5GkbCx5jF5QQNKeZExYqrNHzK8L\",\"ChainCode\": \"CarQU+owbi2iML7Fy5vf+6O0Lpc/V//NFkk7WLVkh70=\",\"Network\": \"Main\",\"CreationTime\": \"2019-07-18\"}";

      const string walletDirectoryName = "Wallets";
      const string walletFileName = "wallet_show_balances_test.json";
      var walletFilePath = Path.Combine(walletDirectoryName, walletFileName);

      Directory.CreateDirectory(walletDirectoryName);
      using (File.Create(walletFilePath))
      {
      }

      var args = new[]
      {
        $"wallet-file={walletFileName}",
      };

      File.WriteAllText(walletFilePath, walletContents);
      File.Exists(walletFilePath).Should().BeTrue();

      var password = new SecureString(); // Incorrect password.
      password.AppendChar('p');
      IBitcoinLibrary bitcoinLibrary = new BitcoinLibrary();

      try
      {
        // Act
        bitcoinLibrary.ShowBalances(args, password);

        // Should not get here - force a fail if we do
        bitcoinLibrary.Should().BeNull();
      }
      catch (IncorrectWalletPasswordException ex)
      {
        // Assert
        ex.Message.Should().Contain("Provided password is incorrect.");
      }

      // Clean up
      Directory.Delete(walletDirectoryName, true);
      File.Delete(walletFileName);
    }

    /// <summary>
    /// Tests the ProcessShowBalances method throws a custom exception when a given wallet is missing.
    /// </summary>
    [Fact]
    public void ProcessShowBalances_ShouldThrowACustomExceptionWhenAGivenWalletIsMissing()
    {
      // Arrange
      const string walletFileName = "wallet_show_balances_test.json";

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
      IBitcoinLibrary bitcoinLibrary = new BitcoinLibrary();

      try
      {
        // Act
        bitcoinLibrary.ShowBalances(args, password);

        // Should not get here - force a fail if we do
        bitcoinLibrary.Should().BeNull();
      }
      catch (WalletNotFoundException ex)
      {
        // Assert
        ex.Message.Should().Contain("Wallet not found: ");
      }
    }
  }
}
