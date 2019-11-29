using Discord;
using Discord.Commands;
using MoreLinq.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SasnoBot.CommandModules
{
    public class TabletopUtilities : ModuleBase<SocketCommandContext>
    {
        [Command("dealnumbers"), Alias("ttdn", "deal")]
        public async Task DealNumbers(string range, params IUser[] users)
        {
            var startAndFinish = range.Split('-');
            var from = int.Parse(startAndFinish[0]);
            var to = int.Parse(startAndFinish[1]);
            var numbersToDeal = Enumerable.Range(from, to - from + 1).ToList();
            var countToDeal = (numbersToDeal.Count + users.Length - 1) / users.Length;

            var numbers = numbersToDeal.Shuffle().Batch(countToDeal).Reverse().ToList();

            for (int i = 0; i < users.Length; i++)
            {
                var user = users[i];
                var numbersForUser = numbers[i];
                await user.SendMessageAsync(numbersForUser.Aggregate("", (str, num) => $"{(str.Length == 0 ? "" : str + ", ")}{num}"));
            }
        }
    }
}