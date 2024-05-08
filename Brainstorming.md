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

1. greate logic idea
2. implement basic logic
3. implement buzzer
4. implement audio
5. implement dot2
6. implement remote
7. implement obs
8. implement visual generaor


## Logic Idea

jedes level implementiert ein interface welches die logik übernimmt

#### Level interface
- Buzzer Press (index as parameter)
- setup
- buzzer releas (index as parameter)


### Player
- name
- points

### master
- Add Player
- Add LVLs

## Buzzer idea

- interface
- new text on serial chanel crates evetnt (Serial watcher class)
- event calls button funtiontn
- that parses and calls a button handler function