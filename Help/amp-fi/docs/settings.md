# Asetukset-dialogi
Asetukset-dialogissa voi muokata ohjelman asetuksia, jotka on jaettu kolmeen eri kategoriaan.

## Yleiset asetukset
Yleiset asetukset sisältää yleisiä ja sekalaisia asetuksia.

**Hiljainen aika käytössä**
Tämä asetus sallii ohjelman joko tauottaa toisto tai vähentää äänenvoimakkuutta tietyllä aikavälillä. Esimerkiksi voit asettaa illasta myöhäiseen aamuun hiljaiseksi ajaksi säilyttääksesi rauhan naapurien kanssa.


**Kieli**
Käyttöliittymän kieli, katso [tällä hetkellä tuetut kielet](supported_languages.md) nähdäksesi tuettujen kielien listan. Tämän asetuksen vaihtaminen tarvitsee ohjelman manuaalisen uudelleenkäynnistyksen.

**Tarkista päivitykset ohjelman käynnistyessä**
Tämän asettaminen päällä laittaa ohjelman tarkistamaan uuden version aina käynnistyessään. Tänä asetus on oletuksena pois päältä.

**Pinottu jonon toisto**
Ohjelmain voi asettaa pinotun jonon toiston tilaa. Tässä tilassa jono ei lyhene kappaleita toistettaessa. Toistettu kappale siirretään jonon perälle ja määritetty prosenttimäärä kappaleita jonon lopusta arvotaan uuteen järjestykseen.

**Käytä kappaleen kuvaikkunaa**
Tämän asetuksen ollessa päällä pääikkunan oikeaan reunaan tulee kehystetty ikkuna, jossa on kappaleen kuva mikäli sellainen löytyy.

*Piilota albumin kuva automaattisesti* asetus piilottaa kappaleen kuvaikkunan jos toistettavalla kappaleella ei ole kuvaa.

**Näytä soittolistan sarakeotsikot**
Tämä asetus joko piilottaa tai näyttää sarakeotsikot pääikkunan kappalelistassa.

**Uudelleenyritysten määrä toiston epäonnistuessa**
Arvo, joka ilmaisee kuinka monta kertaa ohjelma yrittää toistaa toista kappaletta virheen sattuessa ennen toiston yrittämisen lopettamista.

**Aputiedostokansio**
Koska aputiedostot ovat erillisessä paketissa, tähän syötettään se polku, minne aputiedostot on purettu. Tämä tulisi asettaa purettuun `help_pack`-hakemistoon, jossa on useita alihakemistoja.

*Yleiset asetukset -välilehti*

![image](img/settings1.png)

## Muokattu arvonta
Muokattu tai puolueellinen tai painotettu arvonta toimii siten, että esimerkiksi paremman arvostelun saanut kappale arvotaan useammin, kuin huonomman saanut. Tässä voi ottaa huomioon kappaleen arvostelun, toistojen määrän, keskeytettyjen toistojen määrän, arvottujen toistojen määrän.

*Muokattu arvonta -välilehti*

![image](img/settings2.png)

## Kappaleiden nimeäminen
Kappaleiden nimeäminen vaikuttaa siihen, kuinka kappaleet näytetään kappalelistassa. *Kappaleen nimeämisen kaava* on tarkoitettu kappaleille, joita käyttäjä ei ole uudelleennimennyt. *Uudelleen nimettyjen kappaleiden nimeämisen kaava* on käytössä käyttäjän uudelleen nimeämissä kappaleissa.

*Nimen minimipituus* ja *Jos muodostetussa kappalenimessä ei ole yhtään kirjainta, käytä tiedostonimeä* -asetus vaikuttavat tilanteisiin, joissa kappaleen nimeämisen kaava tuottaa käyttökelvottoman nimen, esim. 'Track 01 - .'.

Sallitut kaavan osat on selitetty välilehdellä ja arvot voi aina palauttaa oletuksiin virheen sattuessa.

*Kappaleiden nimeäminen -välilehti*

![image](img/settings3.png)

## Äänen visualisointi

**Audion visualisointi**
*Näytä audiovisualisointi* -asetus määrittää, näytetäänkö äänen visualisointi kappalelistan alaosassa kappaletta toistettaessa.

**Audion visualisoinnin FFT-ikkunafunktio**
Tällä asetuksella FFT ([Nopea Fourier-muunnoksen](https://fi.wikipedia.org/wiki/Fourier-muunnos#FFT)) [ikkunafunktiota](https://en.wikipedia.org/wiki/Window_function) voidaan vaihtaa. Arvo kannattaa pitää kohdassa Hanning, jos et tiedä, mitä termit tarkoittavat. FFT vaikuttaa visualisoidun signaalin muotoon painottamalla eri taajuuskaistoja eri tavalla - näin ainakin allekirjoittanut (erittäin rajoittuneella tietämyksellä) ymmärtää asian.

**Visualisointi pylväsmuodossa**
Äänen visualisointi näytetään pylväinä, jos tämä asetus on päällä. Muussa tapauksessa visualisointi tehdään viivana.

**Visualisoi äänen tasot**
Ilmaisee, visualisoidaanko äänen tasot äänen visualisointialueella.

**Vaaka-suuntainen äänen tason visualisointi**
Arvo, millä ilmaistaan visualisoidaanko äänen voimakkuus vaaka- vai pystysuunnassa.

*Äänen voimakkuudet pystysuunnassa*

![image](img/audio_visualization1.png)

*Äänenvoimakkuudet vaaka-suunnassa*

![image](img/audio_visualization2.png)


*Äänen visualisointi -välilehti*

![image](img/settings4.png)

## Sekalaista
Tällä välilehdellä on sekalaiset asetukset jotka eivät suoraan kuulu mihinkään kategoriaan.

**Toimenpiteet jonon loputtua**

Ohjelmassa voi määritellä 2 toimenpidettä, jotka suoritetaan jonon loputtua.

Toimenpiteet ovat:

* Ei mitään
* Pysäytä toisto
* Palauta varastoitu jono
* Lopeta sovellus

Jos ensimmäistä toimenpidettä ei voida suorittaa, suoritetaan seuraava, muussa tapauksessa pysähdytään ensimmäiseen toimenpiteeseen ja arvioidaan tilanne uudelleen jonon loputtua.

**Varmuuskopioi sovelluksen tiedot**
Antaa käyttäjän varmuuskopioida SQLite-tietokanta ja asetustiedostot ohjelmasta yksittäiseen zip-tiedostoon. Varmuuskopiointi ei tarvitse sovelluksen uudelleen käynnistämistä, pelkkä toiston pysäyttäminen riittää.

Ennen varmuuskopioinnin suorittamista näytetään seuraava viesti:
*"Toisto pysäytetään varmuuskopiointia varten."* jota seuraa *"Varmuuskopiointi valmis."* viesti.

**Palauta sovelluksen tiedot**
Antaa käyttäjän palauttaa sovelluksen tiedot zip-tiedostosta. Zip-tiedoston sisältöä ei tarkasteta. Toisto pysäytetään varmuuskopiota palautettaessa ja sovellus sulkeutuu ja se pitää käynnistää uudelleen manuaalisesti.

Ennen varmuuskopion palauttamista näytetään seuraava viesti: *"Sovellus sammutetaan varmuuskopion palauttamisen jälkeen. Käynnistä sovellus uudelleen manuaalisesti."*. Kun varmuuskopion palautus on valmis näytetään vielä viesti: *"Varmuuskopioinnin palautus valmis."*.

*Sekalaista-välilehti*

![image](img/settings5.png)
