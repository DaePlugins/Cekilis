using System.Collections.Generic;
using System.Linq;
using Rocket.API;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using SDG.Unturned;
using UnityEngine;

namespace DaeCekilis
{
    internal class KomutKaraListe : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Player;
        public string Name => "karaliste";
        public string Help => "Kara liste ile etkileşime geçer.";
        public string Syntax => "";
        public List<string> Aliases => new List<string>();
        public List<string> Permissions => new List<string>{ "dae.cekilis.karaliste" };

        public void Execute(IRocketPlayer komutuÇalıştıran, string[] parametreler)
        {
            if (parametreler.Length == 0)
            {
                var karaListedekiler = Çekiliş.Örnek.KaraListedekiler.Select(k => UnturnedPlayer.FromCSteamID(k).CharacterName).ToArray();
                if (karaListedekiler.Any())
                {
                    UnturnedChat.Say(komutuÇalıştıran, Çekiliş.Örnek.Translate("KaraListedekiler", string.Join(", ", karaListedekiler)));
                }
                else
                {
                    UnturnedChat.Say(komutuÇalıştıran, Çekiliş.Örnek.Translate("HatalıParametre"), Color.red);
                }

                return;
            }

            var parametre = parametreler[0].ToLower();
            if (parametre == "s")
            {
                if (Çekiliş.Örnek.Configuration.Instance.YetkililerKatılamaz)
                {
                    Çekiliş.Örnek.KaraListedekiler.RemoveAll(o => !UnturnedPlayer.FromCSteamID(o).HasPermission($"dae.cekilis.{Çekiliş.Örnek.Configuration.Instance.Yetkili}"));
                }
                else
                {
                    Çekiliş.Örnek.KaraListedekiler.Clear();
                }

                UnturnedChat.Say(komutuÇalıştıran, Çekiliş.Örnek.Translate("KaraListeSıfırlandı"));
                return;
            }

            var oyuncu = UnturnedPlayer.FromName(parametre);
            if (oyuncu == null)
            {
                UnturnedChat.Say(komutuÇalıştıran, Çekiliş.Örnek.Translate("OyuncuBulunamıyor"), Color.red);
                return;
            }

            if (Çekiliş.Örnek.KaraListedekiler.Contains(oyuncu.CSteamID))
            {
                Çekiliş.Örnek.KaraListedekiler.Remove(oyuncu.CSteamID);
                
                UnturnedChat.Say(oyuncu, Çekiliş.Örnek.Translate("YasağınKalktı"));
                UnturnedChat.Say(komutuÇalıştıran, Çekiliş.Örnek.Translate("YasağıKaldırdın"));
            }
            else
            {
                Çekiliş.Örnek.KaraListedekiler.Add(oyuncu.CSteamID);
                
                UnturnedChat.Say(oyuncu, Çekiliş.Örnek.Translate("Yasaklandın"), Color.red);
                UnturnedChat.Say(komutuÇalıştıran, Çekiliş.Örnek.Translate("Yasakladın"));
            }
        }
    }
}