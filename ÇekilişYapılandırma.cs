using Rocket.API;

namespace DaeCekilis
{
    public class ÇekilişYapılandırma : IRocketPluginConfiguration
    {
        public byte ÇekilişİçinMinimumOyuncu { get; set; }

        public ushort GeriSayımSüresi { get; set; }

        public bool ÇekilişEsnasındaOyuncularıSustur { get; set; }

        public bool ÇekilişBitimindeSohbetiDoldur { get; set; }
        public byte ÇekilişBitimindeYollanacakMesajSayısı { get; set; }

        public bool BaşlatanKatılamaz { get; set; }
        
        public bool YetkililerKatılamaz { get; set; }
        public string Yetkili { get; set; }

        public bool KazananTekrarKatılamaz { get; set; }

        public void LoadDefaults()
        {
            ÇekilişİçinMinimumOyuncu = 2;

            GeriSayımSüresi = 10;

            ÇekilişEsnasındaOyuncularıSustur = true;

            ÇekilişBitimindeSohbetiDoldur = true;
            ÇekilişBitimindeYollanacakMesajSayısı = 5;
            
            BaşlatanKatılamaz = true;

            YetkililerKatılamaz = true;
            Yetkili = "Yetkili";

            KazananTekrarKatılamaz = false;
        }
    }
}