// See https://aka.ms/new-console-template for more information

using Momir_Viginator_cs;

Console.WriteLine("This is a testing app that will retrieve a random card and output its relevant data");

OnlineScryfallFactory factory = new OnlineScryfallFactory();

var card = factory.makeRandom(3);

if (card != null)
{
    Console.WriteLine($"Name: {card.name}\n Oracle: {card.oracleText}\nflavour: {card.flavourText}\n{card.power}/{card.defense}");
}

