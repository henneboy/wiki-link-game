# Six degrees of wikipedia
Warning: This is my first hobby project...

Finds the shortest path between two wikipedia pages by only clicking the links (Breath First Search).

Inspired by: https://www.sixdegreesofwikipedia.com/

**Like jumping from [en.wikipedia.org/wiki/C_Sharp_(programming_language)](https://en.wikipedia.org/wiki/C_Sharp_(programming_language)) to [en.wikipedia.org/wiki/Visual_Studio_Code)](https://en.wikipedia.org/wiki/Visual_Studio_Code) through only clicking on links**

### It gives output like:

>Reading: https://en.wikipedia.org/wiki/C_Sharp_(programming_language)

>Found page: https://en.wikipedia.org/wiki/roslyn_(compiler) from start-page: https://en.wikipedia.org/wiki/C_Sharp_(programming_language)

>Time elapsed(sec): 1,2567974, amount of tasks: 16

>Amount of pages visited: 1

>Average amount of pages per second: 0,7956731928312392

It has 3 classes:
- WikiController
- WikiGame
- WikiHTMLParser

### Features
- [x] It can find the other page
- [x] It uses the specified amount of tasks
- [x] It is fast
- [x] It uses lock to prevent errors from multithreading
- [ ] It is reports the path it found
