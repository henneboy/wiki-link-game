# Wiki-link-game
## The game where you connect two wiki-pages by jumping through other pages

**Like jumping from [c-sharp](https://en.wikipedia.org/wiki/C_Sharp_(programming_language))**
**To [Visual Studio Code](https://en.wikipedia.org/wiki/Visual_Studio_Code)**

__It gives output like:__
>Reading: https://en.wikipedia.org/wiki/C_Sharp_(programming_language)
>Found page: https://en.wikipedia.org/wiki/roslyn_(compiler) from start-page: https://en.wikipedia.org/wiki/C_Sharp_(programming_language)
>Time elapsed(sec): 1,2567974, amount of tasks: 16
>Amount of pages visited: 1
>Average amount of pages per second: 0,7956731928312392

*It has 3 classes:*
- WikiController
- WikiGame
- WikiHTMLParser

### Features
- [x] It can find the other page
- [x] It uses the specified amount of tasks
- [x] It is fast
- [x] It uses lock to prevent errors from multithreading
- [x] It is perfect
