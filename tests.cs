using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;


[TestFixture]
public class ScrabbleTests
{
    private Scrabble scrabble;

    [OneTimeSetUp]
    public void Setup()
    {
        scrabble = new Scrabble();
        scrabble.gameSetup();
    }

    [Test, Order(1)]
    public void TestTileBagCount()
    {
        Assert.AreEqual(98, Scrabble.tileBag.Count(), "TileBag should contain 100 tiles initially");
    }

    [Test, Order(2)]
    public void TestPlayerSetup()
    {
        //Arrange
        scrabble.players.Clear();
        int expectedPlayerCount = 2;

        //Act
        scrabble.playerSetup(expectedPlayerCount);
        Assert.AreEqual(2, scrabble.players.Count, "There should be 2 players in the game");
    }

    [Test, Order(3)]
    public void TestValidWord()
    {
        //Arrange
        Scrabble.Player player = scrabble.players[0];
        string validWord = "APPLE";

        //Act
        bool isValidWord = player.ValidWord(validWord);

        //Assert
        Assert.IsTrue(isValidWord, "The word 'APPLE' should be a valid word");
    }

    [Test, Order(4)]
    public void TestInvalidWord()
    {
        //Arrange
        Scrabble.Player player = scrabble.players[0];
        string invalidWord = "XYZ";

        //Act
        bool isValidWord = player.ValidWord(invalidWord);

        //Assert
        Assert.IsFalse(isValidWord, "The word 'XYZ' should be an invalid word");
    }

    [Test, Order(5)]
    public void TestPlaceWord()
    {
        Scrabble.Player player = scrabble.players[0];
        player.hand['A'] = 1;
        player.hand['P'] = 2;
        player.hand['L'] = 1;
        player.hand['E'] = 1;

        //Act
        player.Place("APPLE");

        //Assert
        Assert.AreEqual(9, player.score, "Player score should be 9 after placing 'APPLE'.");
    }

    [Test, Order(6)]
    public void TestReplaceTiles()
    {
        //Arrange
        Scrabble.Player player = scrabble.players[0];
        player.hand['A'] = 1;
        player.hand['B'] = 2;
        player.hand['C'] = 1;
        player.hand['D'] = 3;
        int originalHandCount = player.hand.Count;
        
        //Act
        player.Replace("ABC");

        //Assert
        Assert.AreEqual(originalHandCount, player.handSize, "Player hand size should be 7 after replacing the tiles ABC.");
    }
}