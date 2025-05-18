Komandos pavadinimas - „Pentagonas”
Projekto komandos nariai:
· Edgaras Miniotas IFIN-3/4 (edgarasminiotas@gmail.com) - Projekto vadovas, programuotojas ir dizaineris. Atsakingas už lygių kūrimą, jų veikimo logika bei projekto koordinavimą.
· Gerda Jurkutė IFIN-3/4 (gerjur@ktu.lt) - Projekto raštininkė, testuotoja ir dizainerė. Atliko žaidimo testavimą, pagrindinių veikėjų modeliavimą ir animacijos veikimą. Prisidėjo prie mechanikų veikimo.
· Emilis Skukauskas IFIN-3/4 (emisku1@ktu.lt) - Programuotojas, dizaineris. Prisidėjo prie žaidimo žaidimo vartotojo sąsajos kūrimo, mechanikų įgyvendinimo bei lygių funkcijų papildymo.
· Benediktas Šimas  IFIN-3/4 (benediktas.simas777@gmail.com) – Dizaineris, programuotojas. Rūpinosi, garso efektais ir foninės muzikos integracija, padėjo kurti UI elementus bei žaidimo lygius.


„ByteBound: The Code Quest“ – tai 2D nuotykių ir galvosūkių žaidimas, kuriame pagrindinis veikėjas keliauja per požeminį labirintą, sprendžia užduotis ir įveikia kliūtis pasitelkdamas programavimo logiką. Žaidimas skirtas ne tik smagiam laiko praleidimui, bet ir padeda žaidėjams įgyti ar įtvirtinti žinias apie C++ programavimo kalbą.
 
Funkcionalumas:
Kodo įvedimas: žaidėjas gali įvesti paprastą C++ stiliaus kodą, kuris aktyvuoja veiksmus (pvz., atrakina duris).
Interaktyvūs klausimai: Lygyje žaidėjas turi pasirinkti teisingus atsakymus į klausimus apie programavimo pagrindus.
Kodo sudėliojimas: Lygyje žaidėjas turi sudėlioti kodo fragmentus tinkama tvarka.
Garso efektai ir muzika: įdiegti garso takeliai ir efektai, kurie sustiprina žaidimo atmosferą.
Priešai: Kai kuriuose lygiuose yra funkcionalūs priešai trukdantys įveikti lygį.
UI (naudotojo sąsaja): sukurti lengvai suprantami meniu, mygtukai ir rezultatų lentelės.
Laiko rekordų sistema: įveikus visus lygius, žaidėjas gauna informacinę suvestinę su pasiekimais.
Naudotos technologijos:
Git ir GitHub – naudoti versijų kontrolei ir bendradarbiavimui komandoje. Git leido sekti visus kodo pakeitimus, o GitHub – patogiai talpinti projektą ir dirbti keliems žmonėms vienu metu.
Jira – naudota darbų planavimui ir komandiniam valdymui. Joje buvo kuriami sprintai, planuojami darbai, paskirstomos užduotys tarp komandos narių ir stebimas progreso įgyvendinimas.
C# – pagrindinė žaidimo programavimo kalba. Ji pasirinkta dėl puikios integracijos su Godot varikliu, taip pat dėl savo aiškios sintaksės, panašios į C++ struktūrą.
Godot Engine – tai atvirojo kodo žaidimų variklis, leidžiantis kurti 2D ir 3D žaidimus. Jis pasirinktas dėl savo lankstumo, geros dokumentacijos ir paprastumo naujokams.
C++ – Tai žaidime naudojama programavimo kalba kaip  klausimams bei kodo užduotims. Tai leidžia žaidėjams praktiškai susipažinti su C++ pagrindais smagiai žaidžiant.
Krita – piešimo programa, naudota žaidimo aplinkos, objektų iliustravimui. Leido kurti originalius 2D grafinius elementus.


![99be490d78fee2d63c53dd84267679ec](https://github.com/user-attachments/assets/551a5bc2-31e2-4277-9d81-8683360859d2)



Testavimas ir jo rezultatai (gali būti failas saugykloje)

Veiksmas 
Norimas rezultatas
Gautas rezultatas
Žaidėjo registracija
Žaidėjas susikuria vartotojo vardą ir slaptažodį, su kuriais gali prisijungti prie žaidimo.
Sėkmingai užsiregistravęs žaidėjas gali prisijungti prie žaidimo.
Prisijungimas
Žaidėjas gali prisijungti prie žaidimo naudodamas sukurtus duomenis, kurie yra saugomi.
Žaidėjas prisijungia prie žaidimo naudodamas registracijos metu sukurtus duomenis.
Naujo žaidimo paleidimas
Prasideda naujas žaidimas nuo įvadinio lygio.
Paspaudus „Naujas žaidimas“, paleidžiamas įvadinis lygis.
Nustatymų keitimas
Pakeitus nustatymus, jie yra išsaugomi.
Nustatymų lange galima pakeisti ir išsaugoti nustatymus.
Esamo žaidimo paleidimas
Tęsiamas ankstesnis žaidimas nuo išsaugoto lygio.
Paspaudus „Esamas žaidimas“, paleidžiamas paskutinis išsaugotas lygis.
Konsolės atidarymas
Paspaudus “`” mygtuką pirmame lygyje atidaromas konsolės langas
` mygtukas pirmame lygyje atidaro konsolės langą, kuriame galima atlikti užduotį.
Inventorius
Paspaudus „I“ mygtuką, visuose lygiuose žaidėjas gali pasiekti inventorių.
Naudodamas „I“ mygtuką, žaidėjas gali pasiekti inventorių ir juo naudotis.
Sąveika su objektais žaidime
Žaidėjas gali judinti nurodytus objektus naudodamas „B“ mygtuką.
Paspaudus „B“ mygtuką, žaidėjas gali judinti ar sąveikauti su nurodytais objektais.
Veikėjo judėjimas
Veikėjas juda žaidime naudojant „W“, „A“, „S“, „D“ mygtukus.
Paspaudus atitinkamą mygtuką, veikėjas juda į nurodytą pusę.
Žaidimo laimėjimas
Parodoma rekordų lentelė su 10 greičiausių laikų.
Įveikus paskutinį lygį, rodomas rezultatų langas su užfiksuotais greičiausiais laikais.



Trumpa naudotojo dokumentacija (gali būti failas saugykloje arba wiki puslapis)




Kaip įrašyti ir paleisti žaidimą
Atsidaryti https://github.com/EdgarasMin/Pentagonas/tree/tpAnimacija
Atsisiųsti failą iš <> Code (žalios spalvos mygtuko) 
bei įrašyti Godot programą iš interneto (geriausia 4.3 versiją)

Atsidrę Godot programą išsiarchyvuoti žaidimą. Toliau įkelti žaidimo failus kaip pavaizduota paveikslėlyje.












Belieka paspausti F5 mygtuką arba simbolį kitame paveikslėlyje ir žaidimas atsidarys.


Žaidimas yra linijinio tipo, tad norint pereiti į kitą lygį reikės įveikti dabartinį. Paleidus žaidimą atsidarys naujas langas, kuriame bus matomas žaidimo vaizdas. Naudotojas pradėjęs žaidimą turės prisijungti prie savo paskyros, arba sukurti nauja paskyra paspaudęs mygtuką “Registruotis”. 


(Kuriant naują paskyra vardas negali būti užimtas ir slaptažodis turi būti iš bent 8 simbolių). Prisijungus naudotojui bus rodomas pagrindinis meniu su keturiais mygtukų pasirinkimais: “Naujas žaidimas”, “Esamas žaidimas”, “Nustatymai”, “Ate ir iki”. 



Paspaudus: “Ate ir iki” programa užbaigs darbą, “Nustatymai” atidarys nustatymų langą, 


“Esamas Žaidimas” leis pratęsti kur buvo baigta ir “Naujas žaidimas” pradės naują žaidimą nuo įvadinio lygio, kuriame bus supažindinama su reikalingais mygtukais. (WASD judėjimui, I inventoriui, ` kodo rašymui, M žemėlapiui). 

Perėjus įvadinį lygį žaidėjui bus nuvesti į pirmą žaidimo lygį.

Pirmame lygyje naudotojas turės programavimo užduotį. (Atsakymas: kodo redagavimo lange pakeisti žodį “World” į “Dungeon”), tuomet atsidarys durys ir bus galima pereiti į antrą žaidimo lygį. Antras žaidimo lygis yra apie žaidėjo pasirinkimus, kur perskaičius klausimą reikės pasirinkti portalą su tinkamu atsakymu. 

(Teisingi atsakymai antrame lygyje yra: Java, Klasė – tai šablonas, apibrėžiantis duomenis ir elgseną. Objektas – tai klasės pavyzdys., C++, List<int> skaiciai = new List<int>();)
Teisingai pasirinkus visus portalus žaidėjas pateks į trečią lygį. Trečiame lygyje kovojant su priešu žaidėjui reikia suvedžioti signalus į visus brangakmenius, tuomet bus gauta kodavimo užduotis. (Kodavimo užduoties atsakymas x=7).


![image](https://github.com/user-attachments/assets/7061d9b4-05f5-4a3d-a2b5-c5d9a488f6ae)


Įveikus trečią lygį žaidėjas pateks į paskutinį, ketvirtą žaidimo lygį. Šiame lygyje žaidėjas bėgdamas nuo priešų turės sudėlioti kodą iš dalių randamų labirinte ir nulenkti viduryje esančią svirtį, kad atrakintų portalą ir baigtų žaidimą.





Baigus žaidimą bus matomi rekordai, per kiek laiko buvo įveiktas žaidimas. 
![image](https://github.com/user-attachments/assets/9ab7e6bf-f99c-49a2-b47f-87315c50bbca)

