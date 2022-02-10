# AuctionLand - Tim 7

Projekat iz predmeta Upravljanje razvojem informacionih sistema na četvrtoj godini studijskog programa Inženjerstvo informacionih sistema.

Projekat predstavlja Backend deo aplikacije kreirane na osnovu projektne specifikacije za vođenje evidencije o aukciji zemljišta.

## Mikroservisna arhitektura
![Arhitektura](https://i.ibb.co/M17CY2P/arhitektura.png)

Za izradu projekat korišćen je .NET Core 5.0.

## Delovi aplikacije
Aplikaciju čine 14 mikroservisa, Gateway i RabbitMQ.Consumer.
Aplikaciju je moguće koristiti i testirati putem Swagger-a za svaki mikroservis, Postman-a i sličnih alata.

Za pokretanje aplikacije potrebno je pokrenuti Gateway aplikaciju, i u folderu [RunSkripte](https://github.com/URIS-2021-2022/tim-7---auctionland/tree/main/RunSkripte) pokrenuti sve fajlove.

Za korišćenje RabbitMQ-a potrebno je podesiti sam RabbitMQ putem Docker-a:
> docker run -it --rm --name rabbitmq -p 5672:5672 -p 15672:15672 rabbitmq:3.9-management

Pristup je na adresi
> http://localhost:15672/

Za slanje mejlova potrebno je promeniti email i password u [RabbitMQ.Consumer](https://github.com/URIS-2021-2022/tim-7---auctionland/blob/main/RabbitMQ.Consumer/App.config) projektu i pokrenuti ga.

## Pristup aplikaciji
Aplikaciji se može pristupiti na adresi 
> http://localhost:44200/swagger/index.html

Na datoj adresi koristeći Gateway moguće je pristupiti svakom mikroservisu.
Za autentifikaciju potrebno je pristupiti KorisnikSistema mikroservisu i generisan token koristiti za ostale zahteve.

## Članovi tima
+ Bulaja Stefan - https://github.com/stefanbulaja
+ Stanić Gavrilo - https://github.com/GavriloS
+ Majkić Dragan - https://github.com/draganmajkic
+ Bajić Mladen - https://github.com/BajicMladen
+ Pekez Vuk - https://github.com/vukpekez
+ Atanackov Đorđe - https://github.com/AtanackovDjordje
+ Stošić Kristijan - https://github.com/KristijanStosic
