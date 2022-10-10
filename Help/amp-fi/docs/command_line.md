# Komentoriviparametrit
Sovellus tukee komentoriviparametrejä enimmäkseen varmuuskopiointia ja varmuuskopion palautusta varten.

Komentoriviparametrien aputekstin saa näkyviin käyttämällä `--help`-valitsinta.

```
amp.EtoForms 1.0.1.3
Copyright © VPKSoft 2022

  -p, --pid        Prosessitunniste (PID) jonka päättymistä
                   odottaa ennen sovelluksen käynnistämistä.

  -b, --backup     Tiedostonimi johon varmuuskopioida sovelluksen
                   tiedot ennen käynnistymistä.

  -r, --restore    Palauttaa ZIP-pakatun  varmuuskopioin sovelluksen
                   tietokansioon kirjoittaen yli olemassa olevat tiedostot.

  --help           Näytä tämä apunäkymä.

  --version        Näytä versiotiedot.
```