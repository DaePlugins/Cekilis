using System.Collections.Generic;
using Rocket.API;
using Rocket.Unturned.Player;

namespace DaeCekilis
{
    internal class KomutÇekiliş : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Player;
        public string Name => "cekilis";
        public string Help => "Çevrimiçi oyuncuların arasından birisini seçer.";
        public string Syntax => "";
        public List<string> Aliases => new List<string>{ "cek" };
        public List<string> Permissions => new List<string>{ "dae.cekilis.cekilis" };

        public void Execute(IRocketPlayer komutuÇalıştıran, string[] parametreler) => Çekiliş.Örnek.ÇekilişiBaşlat((UnturnedPlayer)komutuÇalıştıran);
    }
}