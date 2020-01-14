# MasterMindGame
Prosty projekt programistyczny związany z grą Mastermind wykonany na potrzeby przedmiotu Matematyka dyskretna na WSEI Kraków.

## PL

### Klasa Game - model gry dostarczający API

`char[] POSSIBLY_LETTERS` - tablica literek (kolorów), z któych składać się może kod:
- r - red,
- y - yellow,
- g - green,
- b - blue
- c - cyan,
- m - magenta,
- p - purple (dodatkowy),
- o - orange (dodatkowy).

`int MIN_COLORS` - minimalna liczba kolorów, z których może składać się kod.

`int MAX_COLORS` - maksymalna liczba kolorów równa liczbie literek, z których składać się może kod.

`MIN_CODE_LENGTH` - minimalna długość kodu do odgadnięcia.
    
`MAX_CODE_LENGTH` - maksymalna długość kodu do odgadnięcia.
    
`int MAX_MOVES` - maksymalna liczba ruchów.

`string Code` - zwraca / ustawia kod do odgadnięcia.

`int CodeLength` - zwraca / ustawia długość kodu.

`string Letters` - zwraca / ustawia literki brane pod uwagę przy losowaniu kodu.

`int TotalMoves` - zwraca / ustawia liczbę ruchów.

`State GameState` - zwraca / ustawia stan gry.

`void Start()` - rozpoczyna grę - metoda powinna zostać użyta, gdy użytkownik wyrazi chęć rozpoczęcia gry.

`void Stop()` - kończy grę z niepowodzeniem - gracz poddał się lub nie udało mu się odgadnąć kodu w wyznaczonej liczbie ruchów.

`void Finish()` - kończy grę - użytkownik pomyślnie odgadnął kod.

`void SetLetters(int length)` - ustawia literki (na podstawie podanej długości), które będą brane pod uwagę podczas losowania kodu.

`bool CheckCodeLength(string code)` - sprawdza, czy długość podanego przez gracza kodu jest prawidłowa.

`int[] CheckCode(string code)` - sprawdza kod i zwraca tablicę zawierającą ilość liczb równą ustalonej długości kodu.

- Dla każdej literki (każdego indeksu) kodu zwraca:
- -1 - jeżeli literka na danej pozycji (danym indeksie) w ogóle nie znajduje się w podanym przez użytkownika kodzie,
-  0 - jeżeli literka znajduje się w podanym przez użytkownika kodzie, ale na innym miejscu,
-  1 - jeżeli literka znajduje się w podanym przez użytkownika kodzie na dokładnie tym samym miejscu.

`void SetRandomCode()` - ustawia losowy kod do odgadnięcia.

`bool SetUserCode(string code)` - ustawia kod podany przez gracza, który będzie mógł zostać odgadnięty przez innego gracza / komputer.

Zwraca:
- false, gdy długość kodu podanego przez gracza jest inna od tego, który obsługuje w konkretnej sytuacji gra.
- false również w sytuacji, gdy w podanym przez użytkownia kodzie znajdują się literki nieobsługiwane przez grę.
- zwraca true, gdy kod spełnia wszystkie wymagania.
Taka weryfikacja kodu pozwala uniknąć m.in. zapętlenia się gry.

### Enum State - pozwala określić stan gry
- `InProgres` - gra aktualnie trwa,
- `Over` - gra została zakończona i nie udało się odgadnąć kodu,
- `Finished` - gra została zakończona, ale gracz odgadł kod.

### Computer - klasa używana, gdy odgadywać kod ma "komputer".
`string GetRandomCode(string letters, int length)` - zwraca losowy kod o długości `length`, skłądający się z literek zawartych w ciągu znaków `letters`.

---

## ENG - a little simplified description

### Game class - model of game that provides API

`POSSIBLY_LETTERS` - letters from which the code may consist.

`MIN_COLORS` - minimum number of colors.

`MAX_COLORS` - maximum number of colors.

`MIN_CODE_LENGTH` - minimum code length.

`MAX_CODE_LENGTH` - maximum code length.

`MAX_MOVES` - maximum number of possibly moves.

`Code` - code to guess.

`CodeLength` - length of the code.

`Letters` - letters form which the code will consist off - default rygbcm.

`TotalMoves` - total moves made by player.

`GameState` - state of the game.

`void Start()` - starts the game, should be used at the beginning, as the first method.

`void Stop()` - stops the game, should be used, when the player wants to give up.

`void Finish()` - runs when the player guesses the code.

`void SetLetters(int length)` - set the letters from which the code will consist of (it takes `length` letters from `POSSIBLY_LETTERS`).

`bool CheckCodeLength(string code)` - checks if the lenght of code is equal to CodeLenth.

`int[] CheckCode(string code)` - checks code and return table of numbers (-1, 0 or 1):
- -1 - if the letter at specific index is not contained in the Code,
-  0 - if the letters at specific index is contained in the code, but on another position,
-  1 - if the letter is exactly on the same index.

`void SetRandomCode()` - sets random code.

`void SetUserCode(string code)` - sets code that can be guessed by other player or "computer" and it checks if the length is right and if the code consists of valid letters.

### State enum - helps to control the game
- `InProgress` - game is in profress.

- `Over` - game is over.

- `Finished` - game is finished - the code has been guessed.

### Computer - class that can be used, when the code has to be guessed by "computer"
`string GetRandomCode(string letters, int length)` - returns random code (`letters` - from which `letters` code must consist of, length - lenght of that code).

---

### Screenshots

1) GameCLI

<table>
   <tr>
      <td>
         <img src="/screenshots/screenshot-1.png" alt="screenshot-1.png"/>
      </td>
   </tr>
    <tr>
      <td>
         <img src="/screenshots/screenshot-2.png" alt="screenshot-2.png"/>
      </td>
   </tr>
</table>

2) GameWPF

<table>
   <tr>
      <td>
         <img src="/screenshots/screenshot-3.png" alt="screenshot-3.png"/>
      </td>
   </tr>
    <tr>
      <td>
         <img src="/screenshots/screenshot-4.png" alt="screenshot-4.png"/>
      </td>
   </tr>
    <tr>
      <td>
         <img src="/screenshots/screenshot-5.png" alt="screenshot-5.png"/>
      </td>
   </tr>
</table>
