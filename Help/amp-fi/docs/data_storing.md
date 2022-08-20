# amp# tietojen tallennus
Ohjelma tallentaa kappaleiden tiedot ja tilastot paikalliseen [SQLite](https://sqlite.org/index.html) tietokantaa, joka sijaitsee JSON asetustiedostojen mukana kansiossa:

* *Linux*: `~/.local/share/VPKSoft/amp`
* *Windows*: `%LOCALAPPDATA%\VPKSoft\amp`
* *macOS*: `~/.local/share/VPKSoft/amp`

## Internet
Ohjelma käyttää internetiä seuraavissa tapauksissa:

1. Käyttäjä valitsee *Apu &rarr; Uuden version tarkistus* tarkastaakseen päivitykset ohjelmaan päävalikosta
2. Asetus *Tarkista päivitykset ohjelman käynnistyessä*, joka on oletuksena pois päältä on asetettu päälle käyttäjän toimesta ja ohjelma käynnistetään.

Molemmissa tapauksissa JSON-tiedosto ladataan muistiin ja versiotietoa verrataan nykyiseen ohjelmaversioon.