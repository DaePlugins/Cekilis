using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Rocket.API;
using Rocket.API.Collections;
using Rocket.Core.Plugins;
using Rocket.Unturned;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Events;
using Rocket.Unturned.Player;
using SDG.Unturned;
using Steamworks;
using UnityEngine;
using Random = System.Random;

namespace DaeCekilis
{
    public class Çekiliş : RocketPlugin<ÇekilişYapılandırma>
    {
        public static Çekiliş Örnek { get; private set; }

        private bool _çekilişVar;
        private byte _yollanacakMesajSayısı;

        public List<CSteamID> KaraListedekiler { get; set; } = new List<CSteamID>();

        protected override void Load()
        {
            Örnek = this;

            _yollanacakMesajSayısı = (byte)(Configuration.Instance.ÇekilişBitimindeSohbetiDoldur ? Configuration.Instance.ÇekilişBitimindeYollanacakMesajSayısı : 1);

            if (Configuration.Instance.YetkililerKatılamaz)
            {
                KaraListedekiler = Provider.clients
                    .Where(s => UnturnedPlayer.FromSteamPlayer(s).HasPermission($"dae.cekilis.{Configuration.Instance.Yetkili}"))
                    .Select(s => s.playerID.steamID)
                    .ToList();

                U.Events.OnPlayerConnected += OyuncuBağlandığında;
                U.Events.OnPlayerDisconnected += OyuncuAyrıldığında;
            }

            UnturnedPlayerEvents.OnPlayerChatted += SohbetKullanıldığında;
        }

        protected override void Unload()
        {
            Örnek = null;

            if (_çekilişVar)
            {
                StopAllCoroutines();
            }

            KaraListedekiler.Clear();

            if (Configuration.Instance.YetkililerKatılamaz)
            {
                U.Events.OnPlayerConnected -= OyuncuBağlandığında;
                U.Events.OnPlayerDisconnected -= OyuncuAyrıldığında;
            }

            UnturnedPlayerEvents.OnPlayerChatted -= SohbetKullanıldığında;
        }

        private void OyuncuBağlandığında(UnturnedPlayer oyuncu)
        {
            if (oyuncu.HasPermission($"dae.cekilis.{Configuration.Instance.Yetkili}"))
            {
                KaraListedekiler.Add(oyuncu.CSteamID);
            }
        }

        private void OyuncuAyrıldığında(UnturnedPlayer oyuncu)
        {
            if (oyuncu.HasPermission($"dae.cekilis.{Configuration.Instance.Yetkili}"))
            {
                KaraListedekiler.Remove(oyuncu.CSteamID);
            }
        }

        private void SohbetKullanıldığında(UnturnedPlayer oyuncu, ref Color renk, string mesaj, EChatMode sohbetModu, ref bool iptal)
        {
            if (_çekilişVar && Configuration.Instance.ÇekilişEsnasındaOyuncularıSustur)
            {
                iptal = true;
            }
        }

        public void ÇekilişiBaşlat(UnturnedPlayer komutuÇalıştıran)
        {
            if (_çekilişVar)
            {
                UnturnedChat.Say(komutuÇalıştıran, Translate("ZatenÇekilişVar"), Color.red);
                return;
            }
			
            if (Configuration.Instance.ÇekilişİçinMinimumOyuncu > Provider.clients.Count - KaraListedekiler.Count)
            {
                UnturnedChat.Say(komutuÇalıştıran, Translate("YetersizOyuncu", Configuration.Instance.ÇekilişİçinMinimumOyuncu - Provider.clients.Count - KaraListedekiler.Count), Color.red);
                return;
            }

            if (Configuration.Instance.BaşlatanKatılamaz)
            {
                if (!KaraListedekiler.Contains(komutuÇalıştıran.CSteamID))
                {
                    KaraListedekiler.Add(komutuÇalıştıran.CSteamID);
                }
            }

            StartCoroutine(Seç());
        }

        private IEnumerator Seç()
        {
            _çekilişVar = true;

            for (var kalanSüre = Configuration.Instance.GeriSayımSüresi; kalanSüre > 0; kalanSüre--)
            {
                UnturnedChat.Say(Translate("KalanSüre", kalanSüre));

                yield return new WaitForSeconds(1);
            }

            var çekilişeKatılabilecekler = Provider.clients.Where(s => !KaraListedekiler.Contains(s.playerID.steamID)).ToList();
            var kazanan = UnturnedPlayer.FromSteamPlayer(çekilişeKatılabilecekler[new Random().Next(0, çekilişeKatılabilecekler.Count)]);

            for (byte mesajSayısı = 0; mesajSayısı < _yollanacakMesajSayısı; mesajSayısı++)
            {
                UnturnedChat.Say(Translate("Kazanan", kazanan.CharacterName));
            }

            if (Configuration.Instance.KazananTekrarKatılamaz)
            {
                KaraListedekiler.Add(kazanan.CSteamID);
            }

            _çekilişVar = false;
        }

        public override TranslationList DefaultTranslations => new TranslationList
        {
            { "HatalıParametre", "Hatalı parametre." },
            { "OyuncuBulunamıyor", "Oyuncu bulunamıyor." },
            { "KaraListedekiler", "Kara Listedekiler: {0}" },
            { "KaraListeSıfırlandı", "Kara liste sıfırlandı." },
            { "Yasaklandın", "Artık çekilişlere katılamazsın." },
            { "Yasakladın", "{0} isimi oyuncu artık çekilişlere katılamaz." },
            { "YasağınKalktı", "Tekrar çekilişlere katılabilirsin." },
            { "YasağıKaldırdın", "{0} isimi oyuncu tekrar çekilişlere katılabilir." },
            { "ZatenÇekilişVar", "Şu anda başka bir çekiliş var." },
            { "YetersizOyuncu", "Çekilişin başlaması için yeterli oyuncu yok. Eksik oyuncu: {0}" },
            { "KalanSüre", "Çekiliş {0} saniye içerisinde sonuçlanacak!" },
            { "Kazanan", "Çekilişi {0} kazandı!" }
        };
    }
}