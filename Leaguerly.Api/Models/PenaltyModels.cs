using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Leaguerly.Api.Models
{
    public enum PenaltyCardType
    {
        Yellow = 1,
        Red = 2
    }

    public class PenaltyCard
    {
        public string Description { get; set; }
        public string Title { get; set; }
        public int Points { get; set; }
        public PenaltyCardType Type { get; set; }
    }

    public class Booking
    {
        public int Id { get; set; }
        public int PlayerId { get; set; }
        public Player Player { get; set; }
        public int GameId { get; set; }
        public string MisconductCode { get; set; }
        public PenaltyCard PenaltyCard { get { return PenaltyCardDefinitions.Find(MisconductCode); } }
    }

    public static class PenaltyCardDefinitions
    {
        private static IDictionary<string, PenaltyCard> penaltyCards = new Dictionary<string, PenaltyCard> {
            { 
                "UB", new PenaltyCard {
                    Description = "UB : Unsporting Behavior",
                    Title = "Unsporting Behavior",
                    Points = 1,
                    Type = PenaltyCardType.Yellow
                }
            },
            { 
                "DI", new PenaltyCard {
                    Description = "DI : Dissent by word or action",
                    Title = "Dissent by word or action",
                    Points = 1,
                    Type = PenaltyCardType.Yellow
                }
            },
            { 
                "PI", new PenaltyCard {
                    Description = "PI : Persistent infringement of the rules",
                    Title = "Persistent infringement of the rules",
                    Points = 1,
                    Type = PenaltyCardType.Yellow
                }
            },
            { 
                "DR", new PenaltyCard {
                    Description = "DR : Delays the restart of play",
                    Title = "Delays the restart of play",
                    Points = 1,
                    Type = PenaltyCardType.Yellow
                }
            },
            { 
                "FD", new PenaltyCard {
                    Description = "FD : Fails to respect required distance at restart of play (corner or free kick)",
                    Title = "Fails to respect required distance at restart of play (corner or free kick)",
                    Points = 1,
                    Type = PenaltyCardType.Yellow
                }
            },
            { 
                "EN", new PenaltyCard {
                    Description = "EN : Enters or reenters play without permission of the referee",
                    Title = "Enters or reenters play without permission of the referee",
                    Points = 1,
                    Type = PenaltyCardType.Yellow
                }
            },
            { 
                "LE", new PenaltyCard {
                    Description = "LE : Deliberately leaves play without permissions of the referee",
                    Title = "Deliberately leaves play without permissions of the referee",
                    Points = 1,
                    Type = PenaltyCardType.Yellow
                }
            },
            { 
                "FP", new PenaltyCard {
                    Description = "FP : Seriously foul play",
                    Title = "Seriously foul play",
                    Points = 3,
                    Type = PenaltyCardType.Red
                }
            },
            { 
                "VC", new PenaltyCard {
                    Description = "VC : Violent conduct",
                    Title = "Violent conduct",
                    Points = 3,
                    Type = PenaltyCardType.Red
                }
            },
            { 
                "SP", new PenaltyCard {
                    Description = "SP : Spits at another player/official",
                    Title = "Spits at another player/official",
                    Points = 3,
                    Type = PenaltyCardType.Red
                }
            },
            { 
                "DH", new PenaltyCard {
                    Description = "DH : Deliberate hand ball to stop a goal",
                    Title = "Deliberate hand ball to stop a goal",
                    Points = 2,
                    Type = PenaltyCardType.Red
                }
            },
            { 
                "DF", new PenaltyCard {
                    Description = "DF : Deliberate foul to stop a goal scoring opportunity",
                    Title = "Deliberate foul to stop a goal scoring opportunity",
                    Points = 2,
                    Type = PenaltyCardType.Red
                }
            },
            { 
                "OL", new PenaltyCard {
                    Description = "OL : Offensive/insulting language or gestures",
                    Title = "Offensive/insulting language or gestures",
                    Points = 2,
                    Type = PenaltyCardType.Red
                }
            },
            { 
                "SC", new PenaltyCard {
                    Description = "SC : 2nd caution in the same match",
                    Title = "2nd caution in the same match",
                    Points = 0,
                    Type = PenaltyCardType.Red
                }
            },
        };

        public static IDictionary<string, PenaltyCard> FindAll() {
            return penaltyCards;
        }

        public static PenaltyCard Find(string key) {
            return penaltyCards[key];
        }
    }
}