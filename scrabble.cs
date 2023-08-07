using System.Collections.Generic;
using System;
using System.Security.Cryptography.X509Certificates;

public class Scrabble 
{

    public TileBag tileBag = new();

    public   List<Player> players = new();

    public   int numPasses = 0;

    public   Dictionary<char, Tile> letterTiles = new()
    {
        {'A', new Tile(9, 1)},
        {'B', new Tile(2, 3)},
        {'C', new Tile(2, 3)},
        {'D', new Tile(4, 2)},
        {'E', new Tile (12, 1)},
        {'F', new Tile(2, 4)},
        {'G', new Tile(3, 2)},
        {'H', new Tile(2, 4)},
        {'I', new Tile(9, 1)},
        {'J', new Tile(1, 8)},
        {'K', new Tile(1, 5)},
        {'L', new Tile(4, 1)},
        {'M', new Tile(2, 3)},
        {'N', new Tile(6, 1)},
        {'O', new Tile(8, 1)},
        {'P', new Tile(2, 3)},
        {'Q', new Tile(1, 10)},
        {'R', new Tile(6, 1)},
        {'S', new Tile(4, 1)},
        {'T', new Tile(6, 1)},
        {'U', new Tile(4, 1)},
        {'V', new Tile(2, 4)},
        {'W', new Tile(2, 4)},
        {'X', new Tile(1, 8)},
        {'Y', new Tile(2, 4)},
        {'Z', new Tile(1, 10)},
    };
    public class TileBag
    {
        public List<char> contents = new();

        public void Add(char letter, int amt=1)
        {
            for(int i = 0; i< amt; i++)
            {
                contents.Add(letter);
            }
        }

        public char Draw()
        {
            Random random = new Random();
            int index = random.Next(0, contents.Count);
            char val = contents[index];
            contents.RemoveAt(index);
            return val;
        }
        public bool IsEmpty()
        {
            if (contents.Count == 0)
            {
                return true;
            }
            return false;
        }
    }

    public class Tile
    {
        public int Quantity { get; set; }
        public int Score { get; set; }

        public Tile(int quantity, int score)
        {
            Quantity = quantity;
            Score = score;
        }
    }

    public class Player
    {
        public int playerId { get; set; }

        public Dictionary<char, int> hand = new();

        public int score = 0;

        public int handSize = 0;

        public bool TakeTurn()
        {
            List<char> tempHand = new();
            foreach (char c in hand.Keys)
            {
                for (int i = 0; i < hand[c]; i++)
                {
                    tempHand.Add(c);
                }
            }
            string joinedTiles = string.Join(" ", tempHand);
            Console.WriteLine($"Your letters are: {joinedTiles} ");
            Console.WriteLine($"Please select what you would like to do: Place a word (input: '1'), replace your tiles (input: '2'), or pass your turn (input: '3'). There are { tileBag.Count} tiles remaining in the bag.");
            string playerChoice = Console.ReadLine();
            if ( playerChoice == "1")
            {
                Console.WriteLine("You have chosen to place a word. Please input the word you would like to place: ");
                string playerWord = Console.ReadLine();
                Place(playerWord);
            }
            else if ( playerChoice == "2")
            {
                Console.WriteLine("You have chosen to replace your tiles. Please input the tiles you would like to replace: ");
                string playerReplace = Console.ReadLine();
                Replace(playerReplace);
            }
            else if ( playerChoice == "3")
            {
                return false;
            }
            else
            {
                TakeTurn();
            }
            return true;
        }
    
        public void Place(string playerWord)
        {
            string upperWord = playerWord.ToUpper();
            if(!ValidWord())
            {
                return;
            }
            foreach (char ch in upperWord)
            {
                score += GamePlay.letterTiles[ch].Score;
                if ( tileBag.IsEmpty())
                {
                    return;
                }
                char newLetter =  tileBag.Draw();
                hand[ch]--;
                if (!hand.ContainsKey(newLetter))
                {
                    hand[newLetter] = 1;
                }
                else
                {
                    hand[newLetter]++;
                }

            }
            Console.WriteLine($"Good job! Your current score is: {score}");
        }

        public void Replace(string playerReplace)
        {
            Dictionary<char, int> counts = new();
            if (tileBag.Count < playerReplace.Length)
            {
                Console.WriteLine($"You have input too many letters. There are {tileBag.Count} tiles left in the bag. Please try again.");
                TakeTurn();
                return;
            }
            foreach ( char letter in playerReplace )
            {
                if (!hand.ContainsKey(letter))
                {
                    Console.WriteLine("You have input a letter that is not in your hand. Please try again.");
                    TakeTurn();
                    return;
                }
                if (!counts.ContainsKey(letter))
                {
                    counts[letter] = 1;
                }
                else if (counts[letter] == hand[letter])
                {
                    Console.WriteLine("You do not have enough of this letter to replace at this time. Please try again.");
                    TakeTurn();
                    return;
                }
                else
                {
                    counts[letter]++;
                }
            }
            foreach (char letter in counts.Keys)
            {
                hand[letter] -= counts[letter];
                if (hand[letter] == 0 )
                {
                    hand.Remove(letter);
                }
            }
            for (int i = 0; i < playerReplace.Length; i++)
            {
                char newTile =  tileBag.Draw();
                if(!hand.ContainsKey(newTile))
                {
                    hand[newTile] = 1;
                }
                else
                {
                    hand[newTile]++;
                }
            }
            foreach (char letter in playerReplace)
            {
                 tileBag.Add(letter);
            }
        }

        public   bool ValidWord()
        {
            //would bring in API to use a dictionary to verify word
            return true;
        }
    }
    public void gameSetup()
    {
        foreach(char c in letterTiles.Keys)
        {
             tileBag.Add(c, letterTiles[c].Quantity);
        }
    }

    public void playerSetup()
    {
        Console.WriteLine("How many players will be joining this game? (Please only input a number)");
        string input = Console.ReadLine();
        int amtPlayers = Int32.Parse(input);
        for (int i = 1; i <= amtPlayers; i++)
        {
            Player player = new();
            player.playerId = i;
            while (player.handSize < 7) { 
                char tile =  tileBag.Draw();
                if (!player.hand.ContainsKey(tile))
                {
                    player.hand[tile] = 1;
                }
                else
                {
                    player.hand[tile]++;
                }
                player.handSize++;
                
            }
            players.Add(player);
            Console.WriteLine($"Player {player.playerId}");
        }
    }

    public void gamePlay()
    {
        bool gameOngoing = true;
        while ( gameOngoing )
        {
            foreach(Player player in players )
            {
                if (!player.TakeTurn())
                {
                    numPasses++;
                }
                else
                {
                    numPasses = 0;
                    if ( tileBag.IsEmpty() && player.handSize == 0)
                    {
                        gameOngoing = false;
                        break;
                    }

                }
                if (numPasses == (players.Count * 2))
                {
                    gameOngoing = false;
                }
            }
        }
        Console.WriteLine($"The winner is: /Insert winner here/");
        return;
    }
    public void Main()
    {
        gameSetup();
        playerSetup();
        gamePlay();
    }
}

}
