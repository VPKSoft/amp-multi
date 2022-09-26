# amp#-ohjelman pääikkuna

Pääikkuna koostuu valikosta, toiston ohjauspainikkeista, albumin valitsimesta, äänenvoimakkuuden-, pisteiden-, toiston sijainnin-asetuksesta, hakukentästä, kappalelistasta, äänen visualisoinnista ja tilapalkista.

![image](img/main_window1.png)

## Valikko

### Tiedosto-valikko
#### Lisää musiikkitiedostoja...
* **Lisää tiedostoja albumiin...**: Tämä toiminto avaa tiedostonvalintaikkunan, josta voi valita yhden tai useamman tiedoston lisättäväksi nykyiseen albumiin.

* **Lisää hakemiston sisältö albumiin...**: Tämä toiminto avaa kansionvalintaikkunan, mistä voi valita kansion josta tiedostoja lisätään rekursiivisesti nykyiseen albumiin.

#### Albumi
Tämä toiminto avaa dialogin, jolla [hallitaan albumeita](album.md) ohjelman tietokannassa.

#### Kappaleen tiedot
Avaa dialogin, jolla voi [näyttää ja muokata](track_info.md) valitun kappaleen metatietoja (IDvX, jne). Myös kappaleen kuvan voi asettaa. Näppäinoikotie: <kbd>F4</kbd>.

#### Lopeta
Lopettaa sovelluksen . Näppäinoikotie: <kbd>Ctrl</kbd>+<kbd>Q</kbd> tai <kbd>⌘</kbd>+<kbd>Q</kbd>

### Jono-valikko

#### Tallenna nykyinen jono
Avaa dialogin, johon voi antaa nimen nykyiselle jonolle tai vaihtoehtoiselle jonolle tallennettavaksi latausta ja uudelleen latausta varten. Näppäinoikotie: <kbd>Ctrl</kbd>+<kbd>S</kbd> tai <kbd>⌘</kbd>+<kbd>S</kbd>.

#### Tallennetut jonot
Avaa dialogin, jossa [tallennettuja jonoja](saved_queues_dialog.md) voi ladata, muokata ja poistaa. Näppäinoikotie: <kbd>F3</kbd>.

#### Tyhjennä jono
Tyhjentää jonon nykyisestä albumista. Näppäinoikotie: <kbd>Ctrl</kbd>+<kbd>D</kbd> tai <kbd>⌘</kbd>+<kbd>D</kbd>.

#### Sekoita jono
Arpoo jonon järjestyksen uudelleen. Jos kaksi tai useampi kappale on valittuna jonon kappaleista, ainoastaan valittujen kappaleiden järjestys arvotaan uudelleen. Näppäinoikotie: <kbd>F7</kbd>.

#### Varastoi nykyinen jono
Tallentaa nykyisen jonon väliaikaiseen varastoon samalla tyhjentäen nykyisen jonon. Toimintoa voi käyttää jos haluaa kuunnella jonon toiston välissä jotain muuta. Näppäinoikotie: <kbd>Ctrl</kbd>+<kbd>T</kbd> tai <kbd>⌘</kbd>+<kbd>T</kbd>.

#### Palauta jono varastosta
Palauttaa väliaikaiseen varastoon tallennetun jonon samalla kirjoittaen yli nykyisen jonon. Näppäinoikotie: <kbd>Ctrl</kbd>+<kbd>P</kbd> tai <kbd>⌘</kbd>+<kbd>P</kbd>.

### Työkalut-valikko

#### Asetukset
Avaa [asetukset-dialogin](settings.md) ohjelman asetusten muokkaamista varten.

#### Värien asetukset
Avaa [dialogin, missä ohjelmassa käytettyjä värejä voi vaihtaa](color_settings.md).

#### Ikonien asetukset
[Ikonien asetukset](icon_settings.md) -dialogissa käyttäjä voi valita SVG-kuvia ohjelmaan sisältyvien kuvakkeiden tilalle.

#### Päivitä kappaleiden metatiedot
Käy läpi kaikki kappaleet kaikista albumeista ja päivittää vastaavat tiedot ohjelmaan. Tämä työkalu on tarkoitettu sitä varten, jos kappaleiden metatietoja on muokattu muualla, kuin amp#-ohjelmassa, koska amp# lukee metadatatiedot ainoastaan kerran.

### Apu-valikko

#### tietoja
Näyttää dialogin, jossa on ohjelman versio ja lisenssitietoja.

#### Apu
Tämä apu-toiminto avataan selaimeen. Näppäinoikotie: <kbd>F1</kbd>.

#### Uuden version tarkistus
Tarkistaa, onko ohjelmasta saatavilla uutta versiota ja:
* Jos uusi versio on saatavilla, käyttäjälle näytetään dialogi, jossa on linkki uuden version lataukseen ja muutokset nykyiseen versioon nähden.
* Jos uutta versiota ei ole saatavilla, näytetään käyttäjälle ilmoitus siitä.

## Toiston ohjauksen työkalupalkki
Työkalupalkki sisältää yleiset toiminnot musiikin toiston hallintaa varten ja muutaman tila-asetuksen.

![image](img/toolbar1.png)

**Edellinen kappale -painike -** Toistaa edellisen kappaleen, jos historiassa on yhtään aiemmin toistettua kappaletta.

![image](img/gui/ic_fluent_previous_48_filled.png)

**Toista-/tauota toisto -painike -** Jatkaa toistoa tai tauottaa toiston.

![image](img/gui/play.png) ![image](img/gui/ic_fluent_pause_48_filled.png)

**Seuraava kappale -painike -** Toistaa seuraavan kappaleen. Seuraava kappale riippuu siitä, onko kappaleita jonossa, onko arvonta päällä ja onko jatkuva toisto päällä.

![image](img/gui/ic_fluent_next_48_filled.png)

**Näytä jono -painike -** Suodattaa kappalelistaan näkyviin ainoastaan jonossa olevat kappaleet. Jos mitään ei ole jonossa, lista jää tyhjäksi. Hakemalla tekstillä voi suodattaa jonotettujen kappaleiden listaa lisää.

![image](img/gui/queue_three_dots.png)

**Arvonta päällä -painike -** Vaihtaa arvonnan tilaa toistolle.

![image](img/gui/shuffle-random-svgrepo-com_modified.png)

**Jatkuva toisto päällä -painike -** Vaihtaa jatkuvan toiston tilaa.

![image](img/gui/repeat-svgrepo-com_modified.png)

**Pinottu jonon toisto päällä -painike -** Pinottu jonon toistotila varmistaa, että kappaleet eivät lopu jonosta. Toistetut kappaleet siirretään takaisin jonon loppuun ja ja tietty osuus jonon loppupään järjestyksestä arvotaan uudelleen.

![image](img/gui/stack_queue_three_dots.png)

## Albumin valinta
Albumin voi vaihtaa tästä pudotusvalikosta. Edellisen albumin mahdolliset muutokset tallentuvat automaattisesti, ennen albumin vaihtumista.

![image](img/album_selector1.png)

## Äänenvoimakkuus ja arvostelu
**Äänenvoimakkuus**
Äänenvoimakkuus vaikuttaa kaikkien kappaleiden toiston voimakkuuteen. Asetuksen muuttaminen ei vaikuta järjestelmän äänenvoimakkuuteen.

**Kappaleen äänenvoimakkuus**
Kappaleen äänenvoimakkuus vaikuttaa yksittäisen kappaleen toiston voimakkuuteen. Tämä arvo tallennetaan ohjelman sisäiseen tietokantaan.

**Arvostelu/pisteet**
Tällä toiminnolla voidaan antaa kappaleelle haluttu arvostelu. Tämä arvo tallennetaan ohjelman sisäiseen tietokantaan.

## Sijainnin säätö
Tästä voi säätää nykyisen kappaleen toiston sijaintia ajallisesti.

## Nykyisen kappaleen otsikko ja kesto
Kesto kertoo, kuinka paljon kappaleen toistoa on jäljellä. Kappaleen otsikkoa klikkaamalla voi keskittää kappalelistan toistettavaan kappaleeseen.

## Hakukenttä
Hakukentällä voi hakea kappaleita nykyisestä albumista. Haku kohdistuu kaikkiin kappaleen ominaisuuksiin:

- Tag, metatiedot

    * Vuosi
    * Albumi
    * Artisti
    * Tiedoston metatietojen sisältö sisältäen myös mahdolliset sanoitukset
    * Jne...

- Tiedostonimi

Keskitys siirtyy hakukenttään kun pääikkuna on aktiivinen ja tekstiä kirjoitetaan, joten hakukenttää ei tarvitse erillisesti valita näppäimistöllä tai hiirellä. **Riittää, kun alkaa kirjoittamaan!**

Käytä <kbd>Escape</kbd> näppäintä hakukentän tyhjentämiseen.

## Kappalelista
Kappalelista sisältää kaikki nykyisen albumin kappaleet. Siinä on myös sarakkeet jonotusnumerolle otsakkeella `J` ja vaihtoehtoiselle jonotusnumerolle otsakkeella `*`. Sarakkeiden järjestystä voi muuttaa ja niiden järjestys tallentuu automaattisesti ohjelman asetuksiin.

Kappaleita voi lisätä jonoon käyttäen <kbd>+</kbd> -näppäintä tai lisätä jonon alkuun <kbd>Ctrl</kbd>+<kbd>+</kbd> tai <kbd>⌘</kbd>+<kbd>+</kbd> -näppäinoikotiellä.

Kappaleita voi lisätä vaihtoehtoiseen jonoon käyttäen <kbd>\*</kbd> -näppäintä tai lisätä vaihtoehtoisen jonon alkuun <kbd>Ctrl</kbd>+<kbd>\*</kbd> tai <kbd>⌘</kbd>+<kbd>\*</kbd> -näppäinoikotiellä.

Vaihtoehtoisen jonon tarkoitus on sallia uuden jonon luonti tallennusta varten kuunneltaessa toista jonoa.

## Tilapaneeli
Tilapaneeli pääikkunan alareunassa kertoo jonotettujen kappaleiden ja suodatettujen kappaleiden määrän. Lisäksi mukana on valitun albumin kokonaiskappalemäärä.