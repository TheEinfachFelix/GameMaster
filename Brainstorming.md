## Parts

- Dot2
- OBS
- audio
- Buzzer
- visual generator
- remote master controll

### DOT2

- via Websocet
- Drückt einfach executor und verschieb fader

### OBS
- https://github.com/GoaLitiuM/libobs-sharp

## TODOs

1. [x] greate logic idea
2. [x] implement basic logic
3. [x] implement buzzer
4. [x] implement audio
5. [x] implement dot2
6. [ ] implement remote
7. [ ] implement obs
8. [ ] implement visual generaor
9. [ ] Simultanios Audio Playing
10. [X] Rewite buzzer Handler with all the new feature
11. [X] Load Data fom Json

## Mini TODOs
- led numbering is offset
- clean / refactor Buzzer
- smart display of pcb valus
- create event to update binding


## Logic Idea

jedes level implementiert ein interface welches die logik übernimmt

#### Level interface
- [x] Buzzer Press (index as parameter)
- [x] setup
- [x] buzzer releas (index as parameter)


### Player
- [x] name
- [x] points

### Game
- [x] Add Player
- [x] Add LVLs

## Buzzer idea

- [X] interface
- [X] new text on serial chanel crates evetnt (Serial watcher class)
- [X] event calls button funtiontn
- [X] that parses and calls a button handler function